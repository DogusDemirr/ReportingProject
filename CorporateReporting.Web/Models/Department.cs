namespace CorporateReporting.Web.Models
{
    public class Department
    {
        /// <summary>
        /// Identifier of the department.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the department.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
