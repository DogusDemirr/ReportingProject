using CorporateReporting.Web.Data;
using CorporateReporting.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CorporateReporting.Web.Pages.Reports
{
    public class TemplatesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TemplatesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ReportTemplate> Templates { get; private set; } = [];

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var departmentId = await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => x.DepartmentId)
                .SingleAsync(cancellationToken);

            var isAdmin = User.IsInRole("Admin");

            var query = _context.ReportTemplates
                .Include(x => x.User)
                .Include(x => x.ReportableTable)
                .AsNoTracking();

            if (!isAdmin)
            {
                query = query.Where(x =>
                    x.UserId == userId ||
                    (x.IsShared &&
                     x.User.DepartmentId == departmentId));
            }

            Templates = await query
                .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
