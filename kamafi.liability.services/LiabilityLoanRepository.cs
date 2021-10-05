using System;
using System.Threading.Tasks;
using System.Net;

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
        private readonly IValidator<LoanDto> _validator;
        private readonly IMapper _mapper;

        public LoanRepository(
            ILogger<LoanRepository> logger,
            IValidator<LoanDto> validator,
            IMapper mapper,        
            LiabilityContext context)
            : base(logger, validator, mapper, context)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public new async Task<Loan> AddAsync(LoanDto dto)
        {
            dto.TypeName = LiabilityTypes.Loan;

            var validatorResult = await _validator.ValidateAsync(dto, 
                o => o.IncludeRuleSets(Constants.AddLoanRuleSet));

            if (!validatorResult.IsValid) 
                throw new ValidationException(validatorResult.Errors);

            var loan = _mapper.Map<Loan>(dto);
            loan.EstimateMonthlyPayment();

            return await base.AddAsync(loan);
        }

        public new async Task<Loan> UpdateAsync(int id, LoanDto dto)
        {
            return await base.UpdateAsync(id, dto);
        }
    }
}