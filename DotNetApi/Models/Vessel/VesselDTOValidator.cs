using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class VesselDTOValidator: AbstractValidator<VesselDTO>
    {
        public VesselDTOValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(v => v.Name).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
        }
    }
}
