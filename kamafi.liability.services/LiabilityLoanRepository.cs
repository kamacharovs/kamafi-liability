using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

using AutoMapper;
using FluentValidation;

using kamafi.liability.data;
using kamafi.liability.services.handlers;

namespace kamafi.liability.services
{
    public class LoanRepository : 
        BaseRepository<Loan, LoanDto>,
        ILoanRepository
    {
        [ExcludeFromCodeCoverage]
        public LoanRepository(
            ILogger<LoanRepository> logger,
            IValidator<LoanDto> validator,
            IMapper mapper,
            IAbstractHandler<Loan, LoanDto> handler,
            LiabilityContext context)
            : base(logger, validator, mapper, handler, context)
        { }

        public new async Task<Loan> AddAsync(LoanDto dto)
        {
            return await base.AddAsync(dto);
        }

        public new async Task<Loan> UpdateAsync(int id, LoanDto dto)
        {
            return await base.UpdateAsync(id, dto);
        }
    }
}