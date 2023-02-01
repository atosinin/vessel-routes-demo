namespace DotNetApi.Config
{
    public class EmailSettings
    {
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string ApiSenderName { get; set; } = string.Empty;
        public string ApiSenderEmail { get; set; } = string.Empty;
        public string ApiSenderPassword { get; set; } = string.Empty; // Store in user-secrets or key vault
    }
}
