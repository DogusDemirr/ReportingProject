using ClosedXML.Excel;
using CorporateReporting.Web.ViewModels;

namespace CorporateReporting.Web.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] CreateReportFile(
       ReportResultModel report,
       string reportTitle)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Rapor");

            var columnCount = report.Columns.Count;

            worksheet.Cell(1, 1).Value = reportTitle;
            worksheet.Range(1, 1, 1, columnCount).Merge();

            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.White;
            worksheet.Cell(1, 1).Style.Fill.BackgroundColor =
                XLColor.FromHtml("#173B82");

            worksheet.Cell(2, 1).Value =
                $"Oluşturulma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}";

            const int headerRow = 4;

            for (var columnIndex = 0;
                 columnIndex < report.Columns.Count;
                 columnIndex++)
            {
                var cell = worksheet.Cell(headerRow, columnIndex + 1);

                cell.Value = report.Columns[columnIndex].DisplayName;
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor =
                    XLColor.FromHtml("#386FFF");
                cell.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;
            }

            for (var rowIndex = 0; rowIndex < report.Rows.Count; rowIndex++)
            {
                var row = report.Rows[rowIndex];

                for (var columnIndex = 0;
                     columnIndex < report.Columns.Count;
                     columnIndex++)
                {
                    var column = report.Columns[columnIndex];
                    var cell = worksheet.Cell(
                        headerRow + rowIndex + 1,
                        columnIndex + 1);

                    row.TryGetValue(column.Name, out var value);

                    WriteCellValue(cell, value);
                }
            }

            var lastRow = headerRow + report.Rows.Count;

            var tableRange = worksheet.Range(
                headerRow,
                1,
                lastRow,
                columnCount);

            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            tableRange.Style.Border.OutsideBorderColor =
                XLColor.FromHtml("#D9E1F0");

            tableRange.SetAutoFilter();
            worksheet.SheetView.FreezeRows(headerRow);

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        private static void WriteCellValue(
            IXLCell cell,
            object? value)
        {
            switch (value)
            {
                case null:
                    cell.Value = string.Empty;
                    break;

                case DateTime date:
                    cell.Value = date;
                    cell.Style.DateFormat.Format = "dd.MM.yyyy";
                    break;

                case decimal decimalValue:
                    cell.Value = decimalValue;
                    cell.Style.NumberFormat.Format = "#,##0.00";
                    break;

                case int intValue:
                    cell.Value = intValue;
                    break;

                case long longValue:
                    cell.Value = longValue;
                    break;

                case double doubleValue:
                    cell.Value = doubleValue;
                    cell.Style.NumberFormat.Format = "#,##0.00";
                    break;

                default:
                    cell.Value = value.ToString();
                    break;
            }
        }
    }
}
