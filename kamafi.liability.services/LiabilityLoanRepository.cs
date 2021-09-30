using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using AutoMapper;
using FluentValidation;

using kamafi.liability.data;

namespace kamafi.liability.services
{
    public class LoanRepository : 
        BaseRepository<Loan, LoanDto>,
        ILoanRepository
    {
        public LoanRepository(
            ILogger<LoanRepository> logger,
            IValidator<LoanDto> validator,
            IMapper mapper,        
            LiabilityContext context)
            : base(logger, validator, mapper, context)
        { }

        public new async Task<Loan> AddAsync(LoanDto dto)
        {
            dto.TypeName = LiabilityTypes.Loan;

            return await base.AddAsync(dto);
        }
    }
}