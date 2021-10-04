using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kamafi.liability.data.validators
{
    public static class CommonValidator
    {
        public const decimal MinimumValue = 0M;
        public const decimal MaximumValue = 99999999M;
        public const decimal MinimumPercentValue = 0M;
        public const decimal MaximumPercentValue = 100M;

        public static string ValueMessage = $"Value must be between {MinimumValue} and {MaximumValue}";
        public static string PercentValueMessage = $"Percent must be between {MinimumPercentValue} and {MaximumPercentValue}";
        public static string TypeNameMessage = "Unsupported TypeName";
        public static string LiabilityUpdateMessage = $"You must specify at least one field to update. '{nameof(LiabilityDto.Name)}', '{nameof(LiabilityDto.TypeName)}' or '{nameof(LiabilityDto.Value)}'";

        public static string GenerateValueMessage(string field)
        {
            return $"{field}. Value must be between {MinimumValue} and {MaximumValue}";
        }

        public static string GeneratePercentValueMessage(string field)
        {
            return $"{field}. Percent must be between {MinimumPercentValue} and {MaximumPercentValue}";
        }

        public static bool BeValidValue(decimal? value)
        {
            return value.HasValue
                ? value >= MinimumValue && value <= MaximumValue
                : false;
        }

        public static bool BeValidPercent(decimal? value)
        {
            return value.HasValue
                ? (decimal)value > 0 && (decimal)value <= 100
                : false;
        }
    }
}
