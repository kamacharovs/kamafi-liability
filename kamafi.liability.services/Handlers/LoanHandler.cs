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
            return liability;
        }
    }
}
