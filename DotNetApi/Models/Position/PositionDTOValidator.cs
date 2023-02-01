using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class PositionDTOValidator: AbstractValidator<PositionDTO>
    {
        public PositionDTOValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(p => p.X).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(p => p.Y).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
            RuleFor(p => p.Timestamp).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
        }
    }
}
