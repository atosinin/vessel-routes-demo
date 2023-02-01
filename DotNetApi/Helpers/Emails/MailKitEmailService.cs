using DotNetApi.Config;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DotNetApi.Helpers.Emails
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public MailKitEmailService(
            IOptions<EmailSettings> emailSettingsOptions)
        {
            _emailSettings = emailSettingsOptions.Value;
        }

        public void SendEmail(string recipientName, string recipientEmail, string subject, string content)
        {
            EmailMessage message = MakeMessageWithoutSender(recipientName, recipientEmail, subject, content);
            string senderEmail = _emailSettings.ApiSenderEmail;
            message.FromAddresses.Add(new EmailAddress()
            {
                RecipientName = _emailSettings.ApiSenderName,
                RecipientEmail = senderEmail
            });
            Send(message, senderEmail);
        }

        private EmailMessage MakeMessageWithoutSender(string recipientName, string recipientEmail, string subject, string content)
        {
            EmailMessage message = new EmailMessage();
            message.ToAddresses.Add(new EmailAddress()
            {
                RecipientName = recipientName,
                RecipientEmail = recipientEmail
            });
            message.Subject = subject;
            message.Content = content;
            return message;
        }

        // Do NOT await email sent
        private void Send(EmailMessage emailMessage, string sender)
        {
            Task notAwaited = SendAsync(emailMessage, sender)
                .ContinueWith(
                    task => Serilog.Log.Error(
                        task.Exception,
                        "EmailServices.Send task failed in background at {Time} for email to '{Recipients}' with subject '{Subject}' and content '{Content}'",
                        DateTime.UtcNow,
                        emailMessage.ToAddresses.Select(to => to.RecipientEmail).DefaultIfEmpty("").Aggregate((a, b) => string.Concat(a, ", ", b)),
                        emailMessage.Subject,
                        emailMessage.Content
                    ),
                    TaskContinuationOptions.OnlyOnFaulted
                );
        }

        private async Task SendAsync(EmailMessage emailMessage, string sender)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.RecipientName, x.RecipientEmail)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.RecipientName, x.RecipientEmail)));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };
            // Make sure to use SmtpClient from Mailkit and not ASP.NET Core
            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                emailClient.ServerCertificateValidationCallback = MySslCertificateValidationCallback;
                await emailClient.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                // Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(sender, _emailSettings.ApiSenderPassword);
                await emailClient.SendAsync(message);
                await emailClient.DisconnectAsync(true);
            }
        }

        static bool MySslCertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            // If there are no errors, then everything went smoothly.
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            // Note: MailKit will always pass the host name string as the `sender` argument.
            var host = (string)sender;
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != 0)
            {
                // This means that the remote certificate is unavailable. Notify the user and return false.
                Serilog.Log.Error("The SSL certificate was not available for {0}", host);
                return false;
            }
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
            {
                // This means that the server's SSL certificate did not match the host name that we are trying to connect to.
                var certificate2 = certificate as X509Certificate2;
                var cn = certificate2 != null ? certificate2.GetNameInfo(X509NameType.SimpleName, false) : certificate!.Subject;
                Serilog.Log.Error("The Common Name for the SSL certificate did not match {0}. Instead, it was {1}.", host, cn);
                return false;
            }
            // The only other errors left are chain errors.
            // The first element's certificate will be the server's SSL certificate (and will match the `certificate` argument)
            // while the last element in the chain will typically either be the Root Certificate Authority's certificate -or- it
            // will be a non-authoritative self-signed certificate that the server admin created.
            string errorMessages = "The SSL certificate for the server could not be validated for the following reasons:";
            foreach (var element in chain!.ChainElements)
            {
                // Each element in the chain will have its own status list. If the status list is empty, it means that the
                // certificate itself did not contain any errors.
                if (element.ChainElementStatus.Length == 0)
                    continue;
                errorMessages += "+ " + element.Certificate.Subject;
                foreach (var error in element.ChainElementStatus)
                {
                    // `error.StatusInformation` contains a human-readable error string while `error.Status` is the corresponding enum value.
                    errorMessages += "-> " + error.StatusInformation;
                }
            }
            Serilog.Log.Error(errorMessages);
            return false;
        }
    }
}
