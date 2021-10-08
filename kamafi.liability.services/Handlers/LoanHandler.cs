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

        protected override Loan OnHandleAdd(Loan liability)
        {
            return liability;
        }

        protected override Loan OnHandleUpdate(LoanDto dto, Loan liability)
        {
            return liability;
        }
    }
}
