using DotNetApi.Config;
using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class ForgottenPasswordModelValidator: AbstractValidator<ForgottenPasswordModel>
    {
        public ForgottenPasswordModelValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(fp => fp.Email).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(fp => fp.Email).Length(ValidationConfig.EmailMinimalLength, ValidationConfig.EmailMaximalLength)
                .WithMessage(validationLocalizer["Invalid length"]);
            RuleFor(fp => fp.Email).EmailAddress()
                .WithMessage(validationLocalizer["Invalid email address"]);
        }
    }
}
