using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class UserAccountDTOValidator: AbstractValidator<UserAccountDTO>
    {
        public UserAccountDTOValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(ua => ua.Id).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);

            RuleFor(ua => ua.FirstName).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);

            RuleFor(ua => ua.LastName).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
        }
    }
}
