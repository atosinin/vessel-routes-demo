using DotNetApi.Config;
using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class LoginModelValidator: AbstractValidator<LoginModel>
    {
        public LoginModelValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(lm => lm.Email).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(lm => lm.Email).Length(ValidationConfig.EmailMinimalLength, ValidationConfig.EmailMaximalLength)
                .WithMessage(validationLocalizer["Invalid length"]);
            RuleFor(lm => lm.Email).EmailAddress()
                .WithMessage(validationLocalizer["Invalid email address"]);

            RuleFor(lm => lm.Password).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(lm => lm.Password).Length(ValidationConfig.PasswordMinimalLength, ValidationConfig.PasswordMaximalLength)
                .WithMessage(validationLocalizer["Invalid length"]);
            RuleFor(lm => lm.Password).Matches(ValidationConfig.PasswordRegularExpression)
                .WithMessage(validationLocalizer["Invalid password"]);
        }
    }
}
