using System.ComponentModel.DataAnnotations;

namespace CorporateReporting.Web.ViewModels
{
    public class SaveReportTemplateInputModel
    {
        /// <summary>
        /// ErrorMessage
        /// </summary>
        [Required(ErrorMessage = "Şablon adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Şablon adı en fazla 150 karakter olabilir.")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// IsShared
        /// </summary>
        public bool IsShared { get; set; }
    }
}
