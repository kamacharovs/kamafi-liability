using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using AutoMapper;
using FluentValidation;

using kamafi.liability.data;
using kamafi.liability.services.handlers;

namespace kamafi.liability.services
{
    public abstract class BaseRepository<T, TDto>
        where T : Liability
        where TDto : LiabilityDto
    {
        private readonly ILogger<BaseRepository<T, TDto>> _logger;
        private readonly IValidator<TDto> _validator;
        private readonly IMapper _mapper;
        private readonly IAbstractHandler<T, TDto> _handler;
        private readonly LiabilityContext _context;

        [ExcludeFromCodeCoverage]
        public BaseRepository(
            ILogger<BaseRepository<T, TDto>> logger,
            IValidator<TDto> validator,
            IMapper mapper,
            IAbstractHandler<T, TDto> handler,
            LiabilityContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<LiabilityType> GetTypesQuery(bool asNoTracking = true)
        {
            var query = _context.LiabilityTypes
                .AsQueryable();

            return asNoTracking
                ? query.AsNoTracking()
                : query;
        }

        public IQueryable<T> GetQuery(bool asNoTracking = true)
        {
            var query = _context.Set<T>()
                .Include(x => x.Type)
                .AsQueryable();

            return asNoTracking
                ? query.AsNoTracking()
                : query;
        }

        public async Task<IEnumerable<ILiabilityType>> GetTypesAsync()
        {
            return await GetTypesQuery()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(bool asNoTracking = true)
        {
            return await GetQuery(asNoTracking)
                .ToListAsync();
        }

        public async Task<T> GetAsync(int id, bool asNoTracking = true)
        {
            return await GetQuery(asNoTracking)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new core.data.KamafiNotFoundException($"Liability with Id={id} was not found");
        }

        public async Task<ILiabilityType> AddAsync(LiabilityTypeDto dto)
        {
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

        public async Task<T> AddAsync(TDto dto)
        {
            dto = _handler.BeforeHandle(dto);

            var validatorResult = await _validator.ValidateAsync(dto, o => o.IncludeRuleSets(Constants.AddRuleSetMap[typeof(TDto).Name]));

            if (!validatorResult.IsValid) throw new ValidationException(validatorResult.Errors);

            var liability = _mapper.Map<T>(dto);

            liability = _handler.HandleAdd(liability);
            liability.UserId = (int)_context.Tenant.UserId;

            await _context.Set<T>().AddAsync(liability);
            await _context.SaveChangesAsync();

            _logger.LogInformation("{Tenant} | Created Liability={LiabilityType} with Id={LiabilityId}, PublicKey={LiabilityPublicKey} and UserId={LiabilityUserId}",
                _context.Tenant.Log,
                typeof(T).Name,
                liability.Id,
                liability.PublicKey,
                liability.UserId);

            return await GetAsync(liability.Id);
        }

        public async Task<T> UpdateAsync(
            int id,
            TDto dto)
        {
            var validatorResult = await _validator.ValidateAsync(dto, o => o.IncludeRuleSets(Constants.UpdateRuleSetMap[typeof(TDto).Name]));

            if (!validatorResult.IsValid) throw new ValidationException(validatorResult.Errors);

            var liability = await GetAsync(id, false);

            liability = _handler.HandleUpdate(dto, liability);
            liability = _mapper.Map(dto, liability);
            liability.UserId = (int)_context.Tenant.UserId;

            _context.Set<T>().Update(liability);
            await _context.SaveChangesAsync();

            _logger.LogInformation("{Tenant} | Updated Liability={LiabilityType} with Id={LiabilityId}, PublicKey={LiabilityPublicKey} and UserId={LiabilityUserId}",
                _context.Tenant.Log,
                typeof(T).Name,
                liability.Id,
                liability.PublicKey,
                liability.UserId);

            return await GetAsync(liability.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var liability = await _context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new core.data.KamafiNotFoundException($"Liability with Id={id} was not found");

            liability.IsDeleted = true;

            _context.Set<T>().Update(liability);
            await _context.SaveChangesAsync();

            _logger.LogInformation("{Tenant} | Soft Deleted Liability with Id={LiabilityId}",
                _context.Tenant.Log,
                id);
        }
    }
}
