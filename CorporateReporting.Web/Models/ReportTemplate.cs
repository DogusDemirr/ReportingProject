namespace CorporateReporting.Web.Models
{
    public class ReportTemplate
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = null!;

        // ReportRequestModel nesnesi JSON olarak saklanacak.
        public string ConfigurationJson { get; set; } = null!;
        /// <summary>
        /// IsShared
        /// </summary>
        public bool IsShared { get; set; }
        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
        /// <summary>
        /// ReportableTableId
        /// </summary>
        public int ReportableTableId { get; set; }
        /// <summary>
        /// ReportableTable
        /// </summary>
        public ReportableTable ReportableTable { get; set; } = null!;
    }
}
