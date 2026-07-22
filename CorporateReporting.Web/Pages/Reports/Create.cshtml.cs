using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CorporateReporting.Web.Data;
using CorporateReporting.Web.Models;
using Microsoft.EntityFrameworkCore;
using CorporateReporting.Web.Services;
using CorporateReporting.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CorporateReporting.Web.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportQueryService _reportQueryService;
        public CreateModel(ApplicationDbContext context, IReportQueryService reportQueryService)
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
        /// <summary>
        /// TemplateInput for saving the report template
        /// </summary>
        public SaveReportTemplateInputModel TemplateInput { get; set; } = new();

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
        public async Task<IActionResult> OnPostSaveTemplateAsync(
                [Bind(Prefix = "TemplateInput")]
                SaveReportTemplateInputModel templateInput,
                CancellationToken cancellationToken)
        {
            await LoadFormDataAsync(
                Request.ReportableTableId,
                cancellationToken);

            if (Request.ReportableTableId <= 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Şablon kaydetmek için bir veri kaynağı seçmelisiniz.");

                return Page();
            }

            if (Request.SelectedColumnIds.Count == 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Şablon kaydetmek için en az bir kolon seçmelisiniz.");

                return Page();
            }

            if (string.IsNullOrWhiteSpace(templateInput.Name))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Şablon adı zorunludur.");

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

            var templateName = templateInput.Name.Trim();

            var exists = await _context.ReportTemplates.AnyAsync(x =>
                x.UserId == userId &&
                x.Name == templateName,
                cancellationToken);

            if (exists)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Bu isimde bir şablon zaten mevcut.");

                return Page();
            }

            var template = new ReportTemplate
            {
                Name = templateName,
                IsShared = templateInput.IsShared,
                UserId = userId,
                ReportableTableId = Request.ReportableTableId,
                ConfigurationJson = JsonSerializer.Serialize(Request)
            };

            _context.ReportTemplates.Add(template);

            await _context.SaveChangesAsync(cancellationToken);

            TempData["SuccessMessage"] =
                $"'{template.Name}' şablonu başarıyla kaydedildi.";

            return RedirectToPage(new
            {
                tableId = Request.ReportableTableId
            });
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
