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
                + $"{CommonValidator.ValueMessage}. "
                + "Specified is '{PropertyValue}'.");
        }

        public static IRuleBuilderOptions<T, TProperty> WithPercentMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
        {
            return rule.WithMessage("Bad '{PropertyName}'. "
                + $"{CommonValidator.PercentValueMessage}. "
                + "Specified is '{PropertyValue}'.");
        }

        public static IRuleBuilderOptions<T, TProperty> WithTermMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule)
        {
            return rule.WithMessage("Bad '{PropertyName}'. "
                + $"{CommonValidator.TermValueMessage}. "
                + "Specified is '{PropertyValue}'.");
        }
    }
}
