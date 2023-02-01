using DotNetApi.Config;
using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class ChangePasswordModelValidator: AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(cp => cp.Email).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(cp => cp.Email).Length(ValidationConfig.EmailMinimalLength, ValidationConfig.EmailMaximalLength)
                .WithMessage(validationLocalizer["Invalid length"]);
            RuleFor(cp => cp.Email).EmailAddress()
                .WithMessage(validationLocalizer["Invalid email address"]);

            RuleFor(cp => cp.Token).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);

            RuleFor(cp => cp.Password).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(cp => cp.Password).Length(ValidationConfig.PasswordMinimalLength, ValidationConfig.PasswordMaximalLength)
                .WithMessage(validationLocalizer["Invalid length"]);
            RuleFor(cp => cp.Password).Matches(ValidationConfig.PasswordRegularExpression)
                .WithMessage(validationLocalizer["Invalid password"]);

            RuleFor(cp => cp.ConfirmPassword).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(cp => cp.ConfirmPassword).Equal(cp => cp.Password)
                .WithMessage(validationLocalizer["Passwords do not match"]);
        }
    }
}
