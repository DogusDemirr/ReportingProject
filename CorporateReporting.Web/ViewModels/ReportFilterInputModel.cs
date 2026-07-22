namespace CorporateReporting.Web.ViewModels
{
    public class ReportFilterInputModel
    {
        /// <summary>
        /// ColumnId
        /// </summary>
        public int ColumnId { get; set; }
        /// <summary>
        /// Operator
        /// </summary>
        public string Operator { get; set; } = "Equals";
        /// <summary>
        /// Value
        /// </summary>
        public string? Value { get; set; }
        /// <summary>
        /// ValueTo
        /// </summary>
        public string? ValueTo { get; set; }
    }
}
