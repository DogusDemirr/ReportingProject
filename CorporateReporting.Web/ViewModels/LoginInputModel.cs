using CorporateReporting.Web.Constants;
using System.ComponentModel.DataAnnotations;

namespace CorporateReporting.Web.ViewModels
{
    public class LoginInputModel
    {
        /// <summary>
        /// Email address of the user.
        /// </summary>
        [Required(ErrorMessage = GeneralConstant.ERROR_MESSAGE_EMAIL_REQUIRED)]
        [EmailAddress(ErrorMessage = GeneralConstant.ERROR_MESSAGE_EMAIL_INVALID)]
        public string Email { get; set; } = null;
        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required(ErrorMessage = GeneralConstant.ERROR_MESSAGE_PASSWORD_REQUIRED)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null;
        /// <summary>
        /// Remember me option for the user.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
