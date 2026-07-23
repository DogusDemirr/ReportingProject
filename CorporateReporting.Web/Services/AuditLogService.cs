using CorporateReporting.Web.Data;
using CorporateReporting.Web.Models;

namespace CorporateReporting.Web.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(
      int userId,
      string action,
      string? details = null,
      CancellationToken cancellationToken = default)
        {
            var ipAddress = _httpContextAccessor
                .HttpContext?
                .Connection
                .RemoteIpAddress?
                .ToString();

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                Details = details,
                IpAddress = ipAddress
            };

            _context.AuditLogs.Add(auditLog);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

