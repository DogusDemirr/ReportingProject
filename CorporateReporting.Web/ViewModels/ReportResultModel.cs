namespace CorporateReporting.Web.ViewModels
{
    public class ReportResultModel
    {
        /// <summary>
        /// Column names for the report result. This property is required and must contain at least one string.
        /// </summary>
        public List<ReportResultColumnModel> Columns { get; set; } = [];
        /// <summary>
        /// Rows of data for the report result. Each row is represented as a dictionary where the key is the column name and the value is the corresponding data. This property is required and can contain zero or more rows.
        /// </summary>
        public List<Dictionary<string,object>> Rows { get; set; } = [];
        /// <summary>
        /// Total count of rows in the report result. This property is read-only and is calculated based on the number of rows.
        /// </summary>
        public int TotalCount => Rows.Count;
    }
    /// <summary>
    /// ReportResultColumnModel represents a column in the report result, including its name and display name. This class is used to define the structure of the columns in the report result.
    /// </summary>
    public class ReportResultColumnModel
    {
        /// <summary>
        /// Name of the column in the report result. This property is required and must be a non-null string.
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// DisplayName of the column in the report result. This property is required and must be a non-null string. It is used for display purposes in the user interface.
        /// </summary>
        public string DisplayName { get; set; } = null!;
        /// <summary>
        /// Datatype of the column in the report result. This property is required and must be a non-null string. It indicates the type of data contained in the column (e.g., string, int, datetime).
        /// </summary>
        public string DataType { get; set; } = null!;
    }
}
