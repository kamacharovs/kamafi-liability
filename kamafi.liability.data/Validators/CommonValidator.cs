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
        public const int MinimumTermValue = 0;
        public const int MaximumTermValue = 1200;

        public static string TypeNameMessage = "Unsupported TypeName";

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

        public static bool BeValidTerm(int? value)
        {
            return value.HasValue
                ? value >= MinimumTermValue && value <= MaximumTermValue
                : false;
        }
    }
}
