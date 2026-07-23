namespace CorporateReporting.Web.Models
{
    public class AuditLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Details
        /// </summary>
        public string? Details { get; set; }
        /// <summary>
        /// IpAddress
        /// </summary>
        public string? IpAddress { get; set; }
        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
