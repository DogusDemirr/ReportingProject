using CorporateReporting.Web.ViewModels;

namespace CorporateReporting.Web.Services
{
    public interface IExcelExportService
    {
        /// <summary>
        /// Creates an Excel file from the given report result and title.
        /// </summary>
        /// <param name="reportResult"></param>
        /// <param name="reportTitle"></param>
        /// <returns></returns>
        byte[] CreateReportFile(
            ReportResultModel reportResult,
            string reportTitle
            );
    }
}
