using CorporateReporting.Web.Data;
using CorporateReporting.Web.Models;
using CorporateReporting.Web.Services;
using CorporateReporting.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CorporateReporting.Web.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportQueryService _reportQueryService;
        public CreateModel(ApplicationDbContext context,IReportQueryService reportQueryService)
        {
            _context = context;
            _reportQueryService = reportQueryService;
        }
        /// <summary>
        /// TableId
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int? TableId { get; set; }
        /// <summary>
        /// Request model for creating a report
        /// </summary>
        [BindProperty]
        public ReportRequestModel Request { get; set; } = new();
        /// <summary>
        /// DataSources
        /// </summary>
        public List<ReportableTable> DataSources { get; private set; } = [];
        /// <summary>
        /// AvailableColumns
        /// </summary>
        public List<ReportableColumn> AvailableColumns { get; private set; } = [];
        /// <summary>
        /// Result of the report query
        /// </summary>
        public ReportResultModel? Result { get; private set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            await LoadFormDataAsync(TableId, cancellationToken);

            if (TableId is not null)
            {
                Request.ReportableTableId = TableId.Value;
            }
        }
        public async Task<IActionResult> OnPostAsync(
     CancellationToken cancellationToken)
        {
            await LoadFormDataAsync(
                Request.ReportableTableId,
                cancellationToken);

            if (Request.ReportableTableId <= 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Lütfen bir veri kaynağı seçin.");

                return Page();
            }

            if (Request.SelectedColumnIds.Count == 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "En az bir kolon seçmelisiniz.");

                return Page();
            }

            var userIdText = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdText, out var userId))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Oturum bilginiz geçersiz. Lütfen tekrar giriş yapın.");

                return Page();
            }

            var departmentId = await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => x.DepartmentId)
                .SingleAsync(cancellationToken);

            var roleName = User.FindFirstValue(ClaimTypes.Role) ?? "Employee";

            try
            {
                Result = await _reportQueryService.ExecuteAsync(
                    Request,
                    userId,
                    departmentId,
                    roleName,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    $"Rapor oluşturulamadı: {exception.Message}");
            }

            return Page();
        }

        private async Task LoadFormDataAsync(
            int? selectedTableId,
            CancellationToken cancellationToken)
        {
            var userId = int.Parse(
      User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var departmentId = await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => x.DepartmentId)
                .SingleAsync(cancellationToken);

            var isAdmin = User.IsInRole("Admin");

            var dataSourceQuery = _context.ReportableTables
                .Where(x => x.IsActive);

            if (!isAdmin)
            {
                dataSourceQuery = dataSourceQuery.Where(x =>
                    x.DepartmentId == null ||
                    x.DepartmentId == departmentId);
            }

            DataSources = await dataSourceQuery
                .OrderBy(x => x.DisplayName)
                .ToListAsync(cancellationToken);

            if (selectedTableId is null)
            {
                return;
            }

            AvailableColumns = await _context.ReportableColumns
                .Where(x =>
                    x.ReportableTableId == selectedTableId &&
                    x.IsVisible)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

    }
}
