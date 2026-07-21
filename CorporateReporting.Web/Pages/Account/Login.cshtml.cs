using CorporateReporting.Web.Constants;
using CorporateReporting.Web.Data;
using CorporateReporting.Web.Models;
using CorporateReporting.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CorporateReporting.Web.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public LoginModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public void OnGet()
        {
            // Handle GET request logic if needed
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _dbContext.Users
           .Include(x => x.Role)
           .SingleOrDefaultAsync(x =>
               x.Email == Input.Email &&
               x.IsActive);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty,
                    GeneralConstant.ERROR_MESSAGE_EMAIL_OR_PASSWORD_INVALID);

                return Page();
            }

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var verifyResult = passwordHasher.VerifyHashedPassword(
                 user,
                 user.PasswordHash,
                 Input.Password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty,
                    GeneralConstant.ERROR_MESSAGE_EMAIL_OR_PASSWORD_INVALID);

                return Page();
            }
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.Name),
            new("DepartmentId", user.DepartmentId.ToString())
        };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = Input.RememberMe,
                    AllowRefresh = true
                });

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Dashboard/Index");
        }
    }
}
