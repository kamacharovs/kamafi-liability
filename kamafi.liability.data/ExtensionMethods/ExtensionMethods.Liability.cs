using System;
using System.Net;

namespace kamafi.liability.data
{
    public static partial class ExtensionMethods
    {
        public static T EstimateMonthlyPayment<T>(this T liability)
            where T : Liability
        {
            liability.MonthlyPaymentEstimate = CalculatePayment(
                liability.RemainingTerm,
                (double)liability.Value,
                (double)liability.Interest);

            return liability;
        }

        public static decimal CalculatePayment(
            int? months,
            double value,
            double interest)
        {
            if (months.HasValue is false)
                throw new core.data.KamafiFriendlyException(HttpStatusCode.BadRequest,
                    $"Loan needs to have Years specified");

            var monthsDouble = (double)months;
            var interestRate = (interest / 100) / 12;

            var payment = value *
                ((interestRate * Math.Pow(1 + interestRate, monthsDouble)) /
                (Math.Pow(1 + interestRate, monthsDouble) - 1));

            return (decimal)(Math.Round(payment, 3));
        }
    }
}
