using DotNetApi.Config;
using DotNetApi.Models;
using DotNetApi.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace DotNetApi.Helpers.Emails
{
    public interface IEmailHelpers
    {
        void SendConfirmationLinkToNewUser(UserAccount account, string confirmationToken);

        void SendChangePasswordLink(UserAccount account, string changePasswordToken);
    }

    public class EmailHelpers : IEmailHelpers
    {
        private readonly IEmailService _emailService;
        private readonly IUrlHelper _urlHelper;
        private readonly FrontendSettings _frontendSettings;
        private readonly IStringLocalizer<EmailsLocalizer> _emailsLocalizer;

        public EmailHelpers(
            IEmailService emailService,
            IUrlHelper urlHelper,
            IOptions<FrontendSettings> frontendOptions,
            IStringLocalizer<EmailsLocalizer> emailsLocalizer)
        {
            _emailService = emailService;
            _urlHelper = urlHelper;
            _frontendSettings = frontendOptions.Value;
            _emailsLocalizer = emailsLocalizer;
        }

        public void SendConfirmationLinkToNewUser(UserAccount account, string confirmationToken)
        {
            string sendTo = string.Concat(account.FirstName, " ", account.LastName);
            // email confirmation link on backend API
            string confirmationLink = _urlHelper.Action(
                "ConfirmInvitation", 
                "Account", 
                new { email = account.Email, token = confirmationToken }, 
                "https"
            )!;
            string subject = _emailsLocalizer["ConfirmationEmailSubject"];
            string content = _emailsLocalizer["ConfirmationEmailSubject {0} {1}", sendTo, confirmationLink];
            _emailService.SendEmail(
                sendTo,
                account.Email,
                subject,
                content
            );
        }

        public void SendChangePasswordLink(UserAccount account, string changePasswordToken)
        {
            string sendTo = string.Concat(account.FirstName, " ", account.LastName);
            // change password link on frontend SPA
            string changePasswordLink = string.Concat(
                _frontendSettings.ChangePasswordUrl, 
                $"?email={account.Email}&token={changePasswordToken}"
            );
            string subject = _emailsLocalizer["ChangePasswordEmailSubject"];
            string content = _emailsLocalizer["ChangePasswordEmailSubject {0} {1}", sendTo, changePasswordLink];
            _emailService.SendEmail(
                sendTo,
                account.Email,
                subject,
                content
            );
        }
    }
}
