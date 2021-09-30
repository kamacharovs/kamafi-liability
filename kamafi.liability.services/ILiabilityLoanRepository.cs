using System;
using System.Threading.Tasks;

using kamafi.liability.data;

namespace kamafi.liability.services
{
    public interface ILoanRepository
    {
        Task<Loan> AddAsync(LoanDto dto);
    }
}