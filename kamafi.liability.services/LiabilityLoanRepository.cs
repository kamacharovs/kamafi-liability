using System;
using System.Threading.Tasks;
using System.Net;

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
        private readonly IValidator<LoanDto> _validator;
        private readonly IMapper _mapper;

        public LoanRepository(
            ILogger<LoanRepository> logger,
            IValidator<LoanDto> validator,
            IMapper mapper,
            IAbstractHandler<Loan, LoanDto> handler,
            LiabilityContext context)
            : base(logger, validator, mapper, handler, context)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

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