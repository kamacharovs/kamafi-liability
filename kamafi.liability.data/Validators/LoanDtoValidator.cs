using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace kamafi.liability.data.validators
{
    public class LoanDtoValidator : LiabilityDtoValidator<LoanDto>
    {
        public LoanDtoValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            RuleSet(Constants.AddVehicleRuleSet, () => SetAddRules());
            RuleSet(Constants.UpdateVehicleRuleSet, () => SetUpdateRules());
        }

        public void SetAddRules()
        {
            RuleFor(x => x.LoanType)
                .NotEmpty();

            RuleFor(x => x.Interest)
                .Must(CommonValidator.BeValidPercent)
                .WithMessage(CommonValidator.PercentValueMessage)
                .When(x => x.Interest.HasValue);
        }

        public void SetUpdateRules()
        {
            RuleFor(x => x.LoanType)
                .Length(100)
                .When(x => !string.IsNullOrWhiteSpace(x.LoanType));

            RuleFor(x => x.Interest)
                .Must(CommonValidator.BeValidPercent)
                .WithMessage(CommonValidator.PercentValueMessage)
                .When(x => x.Interest.HasValue);
        }
    }
}
