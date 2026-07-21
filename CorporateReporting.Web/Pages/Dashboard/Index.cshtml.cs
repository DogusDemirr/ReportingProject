using CorporateReporting.Web.Constants;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CorporateReporting.Web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        public string FullName { get; private set; } = string.Empty;
        public string RoleName { get; private set; } = string.Empty;
        public void OnGet()
        {
            FullName = User.Identity?.Name ?? "Kullanıcı";
            RoleName = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? RoleConstant.Employee;
        }
    }
}
