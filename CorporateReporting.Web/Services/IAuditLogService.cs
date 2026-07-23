namespace CorporateReporting.Web.Services
{
    public interface IAuditLogService
    {
        /// <summary>
        /// LogAsync logs an action performed by a user with optional details.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        /// <param name="details"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task LogAsync(int userId, string action, string? details = null, CancellationToken cancellationToken = default);
    }
}
