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
                .WithMessage(CommonValidator.ValueMessage)
                .When(x => x.Value.HasValue);
        }

        public void SetUpdateBaseRules()
        {
            var exemptTypes = new List<string>
            {
                nameof(LoanDto),
                nameof(VehicleDto)
            };

            RuleFor(x => x)
                .Must(x =>
                {
                    var type = x.GetType().Name;

                    var areAllNull =
                        x.Name is null
                        && x.TypeName is null
                        && !x.Value.HasValue
                        && !exemptTypes.Contains(type);

                    return !areAllNull;
                })
                .WithMessage(CommonValidator.LiabilityUpdateMessage);

            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(x => x.Name != null);

            RuleFor(x => x.TypeName)
                .NotEmpty()
                .When(x => x.TypeName != null);

            RuleFor(x => x.Value)
                .Must(CommonValidator.BeValidValue)
                .WithMessage(CommonValidator.ValueMessage)
                .When(x => x.Value.HasValue);
        }
    }
}
