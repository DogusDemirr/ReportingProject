using CorporateReporting.Web.ViewModels;

namespace CorporateReporting.Web.Services
{
    public interface IReportQueryService
    {
        /// <summary>
        /// Executes a report query based on the provided request model, user ID, department ID, and role name.
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="userId"></param>
        /// <param name="departmentId"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportResultModel> ExecuteAsync(
            ReportRequestModel requestModel,
            int userId,
            int departmentId,
            string roleName,
            CancellationToken cancellationToken = default
            );
    }
}
