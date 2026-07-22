namespace CorporateReporting.Web.Constants
{
    public class GeneralConstant
    {
        /// <summary>
        /// Email address is required error message.
        /// </summary>
        public const string ERROR_MESSAGE_EMAIL_REQUIRED = "E-posta adresi zorunludur";
        /// <summary>
        /// Email address is invalid error message.
        /// </summary>
        public const string ERROR_MESSAGE_EMAIL_INVALID = "Geçerli bir e-posta adresi giriniz";
        /// <summary>
        /// Password is required error message.
        /// </summary>
        public const string ERROR_MESSAGE_PASSWORD_REQUIRED = "Şifre zorunludur";
        /// <summary>
        /// Email address or password is invalid error message.
        /// </summary>
        public const string ERROR_MESSAGE_EMAIL_OR_PASSWORD_INVALID = "E-posta adresi veya şifre hatalı.";
        /// <summary>
        /// Schema name for the reportable tables in the database.
        /// </summary>
        public const string DEFAULT_SCHEMA_NAME = "dbo";
        /// <summary>
        /// Report request model data source required error message.
        /// </summary>
        public const string REPORT_REQUEST_MODEL_DATASOURCE_REQUIRED = "Lütfen bir veri kaynağı seçin";
        /// <summary>
        /// Report request model columns required error message.
        /// </summary>
        public const string REPORT_REQUEST_MODEL_COLUMNS_REQUIRED = "En az bir kolon seçmelisiniz";
    }
}
