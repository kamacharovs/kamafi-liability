using System;

using Microsoft.EntityFrameworkCore;

using FluentValidation;

namespace kamafi.liability.data.validators
{
    public class LiabilityTypeValidator : AbstractValidator<string>
    {
        private readonly LiabilityContext _context;

        public LiabilityTypeValidator(LiabilityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleSet(Constants.AddRuleSet, () => { SetTypeRules(); });
            RuleSet(Constants.AddVehicleRuleSet, () => { SetTypeRules(); });
            RuleSet(Constants.AddLoanRuleSet, () => { SetTypeRules(); });

            RuleSet(Constants.UpdateRuleSet, () => { SetTypeRules(); });
            RuleSet(Constants.UpdateVehicleRuleSet, () => { SetTypeRules(); });
            RuleSet(Constants.UpdateLoanRuleSet, () => { SetTypeRules(); });
        }

        public void SetTypeRules()
        {
            RuleFor(assetType => assetType)
                .NotEmpty()
                .MaximumLength(100)
                .MustAsync(async (type, cancellation) =>
                {
                    return await _context.LiabilityTypes
                        .AnyAsync(x => x.Name == type, cancellation);
                })
                .WithMessage(CommonValidator.TypeNameMessage);
        }
    }
}
