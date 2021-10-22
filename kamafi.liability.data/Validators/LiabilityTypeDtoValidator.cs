using System;
using System.Net;

using Microsoft.EntityFrameworkCore;

using FluentValidation;

namespace kamafi.liability.data.validators
{
    public class LiabilityTypeDtoValidator : AbstractValidator<LiabilityTypeDto>
    {
        public LiabilityTypeDtoValidator()
        {
            SetRules();
        }

        public void SetRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500)
                .When(x => x.Description != null);

            RuleFor(x => x.DefaultInterest)
                .Must(CommonValidator.BeValidPercent)
                .When(x => x.DefaultInterest.HasValue);

            RuleFor(x => x.DefaultOriginalTerm)
                .Must(CommonValidator.BeValidTerm)
                .GreaterThanOrEqualTo(x => x.DefaultRemainingTerm)
                .When(x => x.DefaultOriginalTerm.HasValue);

            RuleFor(x => x.DefaultRemainingTerm)
                .Must(CommonValidator.BeValidTerm)
                .LessThanOrEqualTo(x => x.DefaultOriginalTerm)
                .When(x => x.DefaultRemainingTerm.HasValue);
        }
    }
}
