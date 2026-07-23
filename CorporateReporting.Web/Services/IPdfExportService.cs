using CorporateReporting.Web.ViewModels;

namespace CorporateReporting.Web.Services
{
    public interface IPdfExportService
    {
        /// <summary>
        /// CreateReportFile generates a PDF report file based on the provided ReportResultModel, report title, and creator information. It returns the generated PDF as a byte array.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="reportTitle"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        byte[] CreateReportFile(
            ReportResultModel report,
            string reportTitle,
            string createdBy
            );
    }
}
