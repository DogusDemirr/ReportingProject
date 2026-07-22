using CorporateReporting.Web.Data;
using CorporateReporting.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Text;

namespace CorporateReporting.Web.Services
{
    public class ReportQueryService : IReportQueryService
    {
        private readonly ApplicationDbContext _context;

        public ReportQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportResultModel> ExecuteAsync(
       ReportRequestModel request,
       int userId,
       int departmentId,
       string roleName,
       CancellationToken cancellationToken = default)
        {
            _ = userId;

            var reportableTable = await _context.ReportableTables
                .SingleOrDefaultAsync(x =>
                    x.Id == request.ReportableTableId &&
                    x.IsActive,
                    cancellationToken);

            if (reportableTable is null)
            {
                throw new InvalidOperationException(
                    "Seçilen veri kaynağı bulunamadı veya kullanıma kapalı.");
            }

            var isAdmin = string.Equals(
                roleName,
                "Admin",
                StringComparison.OrdinalIgnoreCase);

            if (!isAdmin &&
                reportableTable.DepartmentId is not null &&
                reportableTable.DepartmentId != departmentId)
            {
                throw new UnauthorizedAccessException(
                    "Bu veri kaynağına erişim yetkiniz yok.");
            }

            var allowedColumns = await _context.ReportableColumns
                .Where(x => x.ReportableTableId == reportableTable.Id)
                .ToListAsync(cancellationToken);

            var selectedColumns = allowedColumns
                .Where(x =>
                    x.IsVisible &&
                    request.SelectedColumnIds.Contains(x.Id))
                .OrderBy(x => x.DisplayOrder)
                .ToList();

            if (selectedColumns.Count == 0)
            {
                throw new InvalidOperationException(
                    "En az bir geçerli kolon seçmelisiniz.");
            }

            var sql = new StringBuilder();
            sql.Append("SELECT TOP (1000) ");
            sql.AppendJoin(", ",
                selectedColumns.Select(x => QuoteIdentifier(x.ColumnName)));
            sql.Append(" FROM ");
            sql.Append(QuoteIdentifier(reportableTable.SchemaName));
            sql.Append(".");
            sql.Append(QuoteIdentifier(reportableTable.TableName));

            var whereParts = new List<string>();
            var parameters = new List<(string Name, object Value)>();
            var parameterIndex = 0;

            // Admin dışındaki kullanıcılar yalnızca kendi departman satırlarını görür.
            if (!isAdmin)
            {
                var parameterName = $"@p{parameterIndex++}";

                whereParts.Add(
                    $"{QuoteIdentifier("DepartmentId")} = {parameterName}");

                parameters.Add((parameterName, departmentId));
            }

            foreach (var filter in request.Filters)
            {
                var column = allowedColumns.SingleOrDefault(x =>
                    x.Id == filter.ColumnId &&
                    x.IsFilterable);

                if (column is null || string.IsNullOrWhiteSpace(filter.Value))
                {
                    continue;
                }

                var columnName = QuoteIdentifier(column.ColumnName);

                switch (filter.Operator)
                {
                    case "Equals":
                        {
                            var parameterName = $"@p{parameterIndex++}";

                            whereParts.Add($"{columnName} = {parameterName}");

                            parameters.Add((
                                parameterName,
                                ConvertValue(filter.Value, column.DataType)));

                            break;
                        }

                    case "Contains":
                        {
                            if (!string.Equals(
                                    column.DataType,
                                    "string",
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidOperationException(
                                    "'İçerir' filtresi sadece metin alanlarında kullanılabilir.");
                            }

                            var parameterName = $"@p{parameterIndex++}";

                            whereParts.Add($"{columnName} LIKE {parameterName}");
                            parameters.Add((parameterName, $"%{filter.Value}%"));

                            break;
                        }

                    case "GreaterThan":
                        {
                            var parameterName = $"@p{parameterIndex++}";

                            whereParts.Add($"{columnName} > {parameterName}");

                            parameters.Add((
                                parameterName,
                                ConvertValue(filter.Value, column.DataType)));

                            break;
                        }

                    case "LessThan":
                        {
                            var parameterName = $"@p{parameterIndex++}";

                            whereParts.Add($"{columnName} < {parameterName}");

                            parameters.Add((
                                parameterName,
                                ConvertValue(filter.Value, column.DataType)));

                            break;
                        }

                    case "Between":
                        {
                            if (string.IsNullOrWhiteSpace(filter.ValueTo))
                            {
                                throw new InvalidOperationException(
                                    "Aralık filtresi için bitiş değeri zorunludur.");
                            }

                            var fromParameter = $"@p{parameterIndex++}";
                            var toParameter = $"@p{parameterIndex++}";

                            whereParts.Add(
                                $"{columnName} BETWEEN {fromParameter} AND {toParameter}");

                            parameters.Add((
                                fromParameter,
                                ConvertValue(filter.Value, column.DataType)));

                            parameters.Add((
                                toParameter,
                                ConvertValue(filter.ValueTo, column.DataType)));

                            break;
                        }

                    default:
                        throw new InvalidOperationException(
                            "Geçersiz filtre operatörü.");
                }
            }

            if (whereParts.Count > 0)
            {
                sql.Append(" WHERE ");
                sql.AppendJoin(" AND ", whereParts);
            }

            if (request.SortColumnId is not null)
            {
                var sortColumn = allowedColumns.SingleOrDefault(x =>
                    x.Id == request.SortColumnId &&
                    x.IsSortable);

                if (sortColumn is not null)
                {
                    var direction = string.Equals(
                        request.SortDirection,
                        "DESC",
                        StringComparison.OrdinalIgnoreCase)
                        ? "DESC"
                        : "ASC";

                    sql.Append(" ORDER BY ");
                    sql.Append(QuoteIdentifier(sortColumn.ColumnName));
                    sql.Append(" ");
                    sql.Append(direction);
                }
            }

            var result = new ReportResultModel
            {
                Columns = selectedColumns
                    .Select(x => new ReportResultColumnModel
                    {
                        Name = x.ColumnName,
                        DisplayName = x.DisplayName,
                        DataType = x.DataType
                    })
                    .ToList()
            };

            var connection = _context.Database.GetDbConnection();
            var connectionWasClosed = connection.State == ConnectionState.Closed;

            try
            {
                if (connectionWasClosed)
                {
                    await connection.OpenAsync(cancellationToken);
                }

                await using var command = connection.CreateCommand();
                command.CommandText = sql.ToString();
                command.CommandType = CommandType.Text;

                foreach (var item in parameters)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = item.Name;
                    parameter.Value = item.Value;

                    command.Parameters.Add(parameter);
                }

                await using var reader = await command.ExecuteReaderAsync(
                    cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    var row = new Dictionary<string, object?>();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var value = await reader.IsDBNullAsync(i, cancellationToken)
                            ? null
                            : reader.GetValue(i);

                        row[reader.GetName(i)] = value;
                    }

                    result.Rows.Add(row);
                }
            }
            finally
            {
                if (connectionWasClosed)
                {
                    await connection.CloseAsync();
                }
            }

            return result;
        }

        private static string QuoteIdentifier(string identifier)
        {
            return $"[{identifier.Replace("]", "]]")}]";
        }

        private static object ConvertValue(string value, string dataType)
        {
            switch (dataType.ToLowerInvariant())
            {
                case "int":
                    return int.Parse(
                        value,
                        NumberStyles.Integer,
                        CultureInfo.InvariantCulture);

                case "decimal":
                    if (decimal.TryParse(
                            value,
                            NumberStyles.Number,
                            CultureInfo.InvariantCulture,
                            out var invariantDecimal))
                    {
                        return invariantDecimal;
                    }

                    return decimal.Parse(
                        value,
                        NumberStyles.Number,
                        CultureInfo.GetCultureInfo("tr-TR"));

                case "datetime":
                    return DateTime.Parse(
                        value,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal);

                default:
                    return value;
            }
        }
    }
}
