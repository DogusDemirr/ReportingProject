using CorporateReporting.Web.Data;
using CorporateReporting.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CorporateReporting.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AuditLogModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AuditLogModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? ActionFilter { get; set; }

        public List<AuditLog> Logs { get; private set; } = [];

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var query = _context.AuditLogs
                .Include(x => x.User)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(ActionFilter))
            {
                query = query.Where(x => x.Action == ActionFilter);
            }

            Logs = await query
                .OrderByDescending(x => x.CreatedAt)
                .Take(200)
                .ToListAsync(cancellationToken);
        }
    }
}
