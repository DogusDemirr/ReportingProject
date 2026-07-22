namespace CorporateReporting.Web.ViewModels
{
    public class ReportResultModel
    {
        /// <summary>
        /// Column names for the report result. This property is required and must contain at least one string.
        /// </summary>
        public List<string> Columns { get; set; } = [];
        /// <summary>
        /// Rows of data for the report result. Each row is represented as a dictionary where the key is the column name and the value is the corresponding data. This property is required and can contain zero or more rows.
        /// </summary>
        public List<Dictionary<string,object>> Rows { get; set; } = [];
        /// <summary>
        /// Total count of rows in the report result. This property is read-only and is calculated based on the number of rows.
        /// </summary>
        public int TotalCount => Rows.Count;
    }
}
