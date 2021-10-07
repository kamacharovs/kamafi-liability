using System;
using System.Linq.Expressions;
using System.Reflection;

using FluentValidation;
using FluentValidation.Internal;

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

    public class CamelCasePropertyNameResolver
    {
        public static string ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
        {
            return DefaultPropertyNameResolver(type, memberInfo, expression).ToCamelCase();
        }

        private static string DefaultPropertyNameResolver(Type type, MemberInfo memberInfo, LambdaExpression expression)
        {
            if (expression != null)
            {
                var chain = PropertyChain.FromExpression(expression);
                if (chain.Count > 0) return chain.ToString();
            }

            if (memberInfo != null)
            {
                return memberInfo.Name;
            }

            return null;
        }
    }
}
