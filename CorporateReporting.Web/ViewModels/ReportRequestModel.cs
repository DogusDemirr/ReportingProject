using CorporateReporting.Web.Constants;
using System.ComponentModel.DataAnnotations;

namespace CorporateReporting.Web.ViewModels
{
    public class ReportRequestModel
    {
        /// <summary>
        /// Reportable table ID for the report request. This property is required and must be a positive integer.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = GeneralConstant.REPORT_REQUEST_MODEL_DATASOURCE_REQUIRED)]
        public int ReportableTableId { get; set; }
        /// <summary>
        /// Selected column IDs for the report request. This property is required and must contain at least one positive integer.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = GeneralConstant.REPORT_REQUEST_MODEL_COLUMNS_REQUIRED)]
        public List<int> SelectedColumnIds { get; set; } = [];
        /// <summary>
        /// Filters for the report request. This property is optional and can contain zero or more filter objects.
        /// </summary>
        public List<ReportFilterInputModel> Filters { get; set; } = [];
        /// <summary>
        /// Group by column ID for the report request. This property is optional and can be null or a positive integer.
        /// </summary>
        public int? GroupByColumnId { get; set; }
        /// <summary>
        /// Sort column ID for the report request. This property is optional and can be null or a positive integer.
        /// </summary>
        public int? SortColumnId { get; set; }
        /// <summary>
        /// Sort direction for the report request. This property is optional and can be either "ASC" or "DESC". The default value is "ASC".
        /// </summary>
        public string SortDirection { get; set; } = "ASC";
    }
}
