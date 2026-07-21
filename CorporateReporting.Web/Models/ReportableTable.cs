using CorporateReporting.Web.Constants;

namespace CorporateReporting.Web.Models
{
    public class ReportableTable
    {
        /// <summary>
        /// Identifier of the reportable table.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Schema name of the reportable table in the database.
        /// </summary>
        public string SchemaName { get; set; } = GeneralConstant.DEFAULT_SCHEMA_NAME;
        /// <summary>
        /// TableName of the reportable table in the database.
        /// </summary>
        public string TableName { get; set; } = null!;
        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// IsActive indicates whether the reportable table is active or not.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// CreatedAt indicates the date and time when the reportable table was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// DepartmentId indicates the identifier of the department associated with the reportable table. It is nullable, meaning that a reportable table may not be associated with any department.
        /// </summary>
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Department represents the navigation property to the associated Department entity. It is nullable, meaning that a reportable table may not be associated with any department.
        /// </summary>
        public Department? Department { get; set; }
    }
}
