using System;

using kamafi.liability.data;

namespace kamafi.liability.services.handlers
{
    public class LoanHandler : AbstractHandler<Loan, LoanDto>
    {
        protected override LoanDto OnBeforeHandle(LoanDto dto)
        {
            dto.TypeName = LiabilityTypes.Loan;

            return dto;
        }

        protected override Loan OnHandle(Loan liability)
        {
            return liability.EstimateMonthlyPayment();
        }

        protected override Loan OnHandleUpdate(LoanDto dto, Loan liability)
        {
            // Re-calculate EstimatedMonthlyPayments based on differences
            if (dto.Years.HasValue && dto.Years != liability.Years
                || dto.Value.HasValue && dto.Value != liability.Value
                || dto.Interest.HasValue && dto.Interest != liability.Interest)
            {
                liability.EsimatedMonthlyPayment = ExtensionMethods.EstimateMonthlyPayment(
                    dto.Years.HasValue ? dto.Years : liability.Years,
                    dto.Value.HasValue ? (double)dto.Value : (double)liability.Value,
                    dto.Interest.HasValue ? (double)dto.Interest : (double)liability.Interest);
            }

            return liability;
        }
    }
}
