using DotNetApi.Config;
using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class RegisterModelValidator: AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(rm => rm.Email).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(rm => rm.Email).Length(ValidationConfig.EmailMinimalLength, ValidationConfig.EmailMaximalLength)
                .WithMessage(validationLocalizer["Invalid length"]);
            RuleFor(rm => rm.Email).EmailAddress()
                .WithMessage(validationLocalizer["Invalid email address"]);

            RuleFor(rm => rm.FirstName).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);

            RuleFor(rm => rm.LastName).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);

            RuleFor(rm => rm.HasAcceptedTerms).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(rm => rm.HasAcceptedTerms).Equal(true)
                .WithMessage(validationLocalizer["Must accept terms"]);

            RuleFor(rm => rm.Password).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(rm => rm.Password).Length(ValidationConfig.PasswordMinimalLength, ValidationConfig.PasswordMaximalLength)
                .WithMessage(validationLocalizer["Invalid length"]);
            RuleFor(rm => rm.Password).Matches(ValidationConfig.PasswordRegularExpression)
                .WithMessage(validationLocalizer["Invalid password"]);

            RuleFor(rm => rm.ConfirmPassword).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(rm => rm.ConfirmPassword).Equal(rm => rm.Password)
                .WithMessage(validationLocalizer["Passwords do not match"]);
        }
    }
}
