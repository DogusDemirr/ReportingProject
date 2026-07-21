namespace CorporateReporting.Web.Models
{
    public class ApplicationUser
    {
        /// <summary>
        /// Identifier for the application user.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Full name of the application user.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Email address of the application user, used for authentication and communication purposes.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Password hash for the application user, used for authentication purposes.
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// Is the application user currently active? This property indicates whether the user account is active and can be used for authentication and authorization.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Creation timestamp for the application user. This property indicates when the user account was created in the system.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// RoleId 
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Role of the application user. This property represents the role assigned to the user, which determines their permissions and access levels within the application.
        /// </summary>
        public Role Role { get; set; }
        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// Department
        /// </summary>
        public Department Department { get; set; }
    }
}
