namespace CorporateReporting.Web.Models
{
    public class ReportableColumn
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ReportableTableId
        /// </summary>
        public int ReportableTableId { get; set; }

        /// <summary>
        /// Reporttable
        /// </summary>
        public ReportableTable ReportableTable { get; set; } = null!;
        /// <summary>
        /// ColumnName
        /// </summary>
        public string ColumnName { get; set; } = null!;
        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; set; } = null!;
        /// <summary>
        /// DataType
        /// </summary>
        public string DataType { get; set; } = null!;
        /// <summary>
        /// IsVisible
        /// </summary>
        public bool IsVisible { get; set; } = true;
        /// <summary>
        /// IsFilterable
        /// </summary>
        public bool IsFilterable { get; set; } = true;
        /// <summary>
        /// IsGroupable
        /// </summary>
        public bool IsGroupable { get; set; } = true;
        /// <summary>
        /// IsSortable
        /// </summary>
        public bool IsSortable { get; set; } = true;
        /// <summary>
        /// DisplayOrder
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
