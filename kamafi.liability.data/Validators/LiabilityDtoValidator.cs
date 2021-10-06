using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace kamafi.liability.data.validators
{
    public class LiabilityDtoValidator<T> : AbstractValidator<T>
        where T : LiabilityDto
    {
        public LiabilityDtoValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            RuleSet(Constants.AddRuleSet, () => { SetAddBaseRules(); });
            RuleSet(Constants.AddVehicleRuleSet, () => { SetAddBaseRules(); });
            RuleSet(Constants.AddLoanRuleSet, () => { SetAddBaseRules(); });

            RuleSet(Constants.UpdateRuleSet, () => { SetUpdateBaseRules(); });
            RuleSet(Constants.UpdateVehicleRuleSet, () => { SetUpdateBaseRules(); });
            RuleSet(Constants.UpdateLoanRuleSet, () => { SetUpdateBaseRules(); });
        }

        public void SetAddBaseRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.TypeName)
                .NotEmpty();

            RuleFor(x => x.Value)
                .Must(CommonValidator.BeValidValue)
                .WithValueMessage();

            RuleFor(x => x.MonthlyPayment)
                .Must(CommonValidator.BeValidValue)
                .WithValueMessage();

            RuleFor(x => x.OriginalTerm)
                .Must(CommonValidator.BeValidTerm)
                .WithTermMessage();

            RuleFor(x => x.RemainingTerm)
                .Must(CommonValidator.BeValidTerm)
                .WithTermMessage();

            RuleFor(x => x.Interest)
                .Must(CommonValidator.BeValidPercent)
                .WithPercentMessage();

            RuleFor(x => x.AdditionalPayments)
                .Must(CommonValidator.BeValidValue)
                .WithValueMessage()
                .When(x => x.AdditionalPayments.HasValue);
        }

        public void SetUpdateBaseRules()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(x => x.Name != null);

            RuleFor(x => x.TypeName)
                .NotEmpty()
                .When(x => x.TypeName != null);

            RuleFor(x => x.Value)
                .Must(CommonValidator.BeValidValue)
                .WithValueMessage()
                .When(x => x.Value.HasValue);

            RuleFor(x => x.MonthlyPayment)
                .Must(CommonValidator.BeValidValue)
                .WithValueMessage()
                .When(x => x.MonthlyPayment.HasValue);

            RuleFor(x => x.OriginalTerm)
                .Must(CommonValidator.BeValidTerm)
                .WithTermMessage()
                .When(x => x.OriginalTerm.HasValue);

            RuleFor(x => x.RemainingTerm)
                .Must(CommonValidator.BeValidTerm)
                .WithTermMessage()
                .When(x => x.RemainingTerm.HasValue);

            RuleFor(x => x.Interest)
                .Must(CommonValidator.BeValidPercent)
                .WithPercentMessage()
                .When(x => x.Interest.HasValue);

            RuleFor(x => x.AdditionalPayments)
                .Must(CommonValidator.BeValidValue)
                .WithValueMessage()
                .When(x => x.AdditionalPayments.HasValue);
        }
    }
}
