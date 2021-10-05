using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace kamafi.liability.data
{
    public static class ExtensionMethods
    {
        public static Loan EstimateMonthlyPayment(this Loan loan)
        {
            loan.EsimatedMonthlyPayment = EstimateMonthlyPayment(
                loan.Years,
                (double)loan.Value,
                (double)loan.Interest);

            return loan;
        }

        public static decimal EstimateMonthlyPayment(
            int? years,
            double value,
            double interest)
        {
            if (years.HasValue is false)
                throw new core.data.KamafiFriendlyException(HttpStatusCode.BadRequest,
                    $"Loan needs to have Years specified");

            var months = (double)(years * 12);
            var interestRate = (interest / 100) / 12;

            var payment = value *
                ((interestRate * Math.Pow(1 + interestRate, months)) /
                (Math.Pow(1 + interestRate, months) - 1));

            return (decimal)(Math.Round(payment, 3));
        }
    }
}
