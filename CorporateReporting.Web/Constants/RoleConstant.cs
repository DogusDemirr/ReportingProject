namespace CorporateReporting.Web.Constants
{
    public class RoleConstant
    {
        /// <summary>
        /// Admin role has full access to all features and settings of the application.
        /// </summary>
        public const string Admin = "Admin";
        /// <summary>
        /// Manager role has access to manage employees and view reports, but does not have full administrative privileges.
        /// </summary>
        public const string Manager = "Manager";
        /// <summary>
        /// Employee role has limited access, typically to view their own data and reports, but cannot manage other users or settings.
        /// </summary>
        public const string Employee = "Employee";
    }
}
