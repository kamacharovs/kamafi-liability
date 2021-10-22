using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

using Microsoft.Extensions.Logging;

using AutoMapper;
using FluentValidation;

using kamafi.liability.data;
using kamafi.liability.services.handlers;

namespace kamafi.liability.services
{
    public class LiabilityRepository :
        BaseRepository<Liability, LiabilityDto>,
        ILiabilityRepository
    {
        private readonly ILogger<LiabilityRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<LiabilityTypeDto> _typeDtoValidator;
        private readonly LiabilityContext _context;

        [ExcludeFromCodeCoverage]
        public LiabilityRepository(
            ILogger<LiabilityRepository> logger,
            IValidator<LiabilityDto> validator,
            IValidator<LiabilityTypeDto> typeDtoValidator,
            IMapper mapper,
            IAbstractHandler<Liability, LiabilityDto> handler,
            LiabilityContext context)
            : base(logger, validator, mapper, handler, context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _typeDtoValidator = typeDtoValidator ?? throw new ArgumentNullException(nameof(typeDtoValidator));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public new async Task<IEnumerable<ILiabilityType>> GetTypesAsync()
        {
            return await base.GetTypesAsync();
        }

        public async Task<IEnumerable<ILiability>> GetAsync()
        {
            return await base.GetAsync();
        }

        public async Task<ILiability> GetAsync(int id)
        {
            return await base.GetAsync(id);
        }

        public async Task<ILiabilityType> AddAsync(LiabilityTypeDto dto)
        {
            await _typeDtoValidator.ValidateAndThrowAsync(dto);

            var liabilityTypes = await GetTypesAsync();
            if (liabilityTypes.Any(x => string.Equals(x.Name, dto.Name, StringComparison.InvariantCultureIgnoreCase) is true))
            {
                throw new core.data.KamafiFriendlyException(HttpStatusCode.BadRequest,
                    $"Liability type already exists. Please try again");
            }

            var liabilityType = _mapper.Map<LiabilityType>(dto);

            await _context.LiabilityTypes.AddAsync(liabilityType);
            await _context.SaveChangesAsync();

            _logger.LogInformation("{Tenant} | Created LiabilityType with Name={LiabilityTypeName} and PublicKey={LiabilityTypePublicKey}",
                _context.Tenant.Log,
                liabilityType.Name,
                liabilityType.PublicKey);

            return liabilityType;
        }

        public new async Task<ILiability> AddAsync(LiabilityDto dto)
        {
            dto.TypeName = LiabilityTypes.Base;

            return await base.AddAsync(dto);
        }

        public new async Task<ILiability> UpdateAsync(int id, LiabilityDto dto)
        {
            return await base.UpdateAsync(id, dto);
        }

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}