namespace VerificationsService.Models.Common
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ValidAudience { get; set; }
        public string TokenIssuer { get; set; }
        public string Host { get; set; }
    }
}