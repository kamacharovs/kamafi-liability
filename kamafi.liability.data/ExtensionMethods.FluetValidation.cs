using System;
using FluentValidation;

using kamafi.liability.data.validators;

namespace kamafi.liability.data
{
    public static partial class ExtensionMethods
    {
        public static IRuleBuilderOptions<T, TProperty> WithValueMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
        {
            return rule.WithMessage("Bad '{PropertyName}'. " 
                + $"Value must be between {CommonValidator.MinimumValue} and {CommonValidator.MaximumValue}. "
                + "Specified is '{PropertyValue}'.");
        }

        public static IRuleBuilderOptions<T, TProperty> WithPercentMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
        {
            return rule.WithMessage("Bad '{PropertyName}'. "
                + $"Percent must be between {CommonValidator.MinimumPercentValue} and {CommonValidator.MaximumPercentValue}. "
                + "Specified is '{PropertyValue}'.");
        }

        public static IRuleBuilderOptions<T, TProperty> WithTermMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
        {
            return rule.WithMessage("Bad '{PropertyName}'. "
                + $"Term must be between {CommonValidator.MinimumTermValue} and {CommonValidator.MaximumTermValue} months. "
                + "Specified is '{PropertyValue}'.");
        }
    }
}
