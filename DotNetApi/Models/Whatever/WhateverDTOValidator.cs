using DotNetApi.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DotNetApi.Models
{
    public class WhateverDTOValidator: AbstractValidator<WhateverDTO>
    {
        public WhateverDTOValidator(IStringLocalizer<ValidationLocalizer> validationLocalizer)
        {
            RuleFor(w => w.Name).NotEmpty()
                .WithMessage(validationLocalizer["Required"]);
        }
    }
}
