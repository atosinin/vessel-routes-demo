namespace DotNetApi.Helpers.Emails
{
    public interface IEmailService
    {
        void SendEmail(string recipientName, string recipientEmail, string subject, string content);
    }
}
