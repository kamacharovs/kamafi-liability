using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace kamafi.liability.data.validators
{
    public class VehicleDtoValidator : LiabilityDtoValidator<VehicleDto>
    {
        public VehicleDtoValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            RuleSet(Constants.AddVehicleRuleSet, () => SetAddRules());
            RuleSet(Constants.UpdateVehicleRuleSet, () => SetUpdateRules());
        }

        public void SetAddRules()
        {
            RuleFor(x => x.DownPayment)
                .Must(CommonValidator.BeValidValue)
                .WithMessage(CommonValidator.ValueMessage)
                .When(x => x.DownPayment.HasValue);
        }

        public void SetUpdateRules()
        {
            RuleFor(x => x.DownPayment)
                .Must(CommonValidator.BeValidValue)
                .WithMessage(CommonValidator.ValueMessage)
                .When(x => x.DownPayment.HasValue);
        }
    }
}
