using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public LiabilityRepository(
            ILogger<LiabilityRepository> logger,
            IValidator<LiabilityDto> validator,
            IMapper mapper,
            IAbstractHandler<Liability, LiabilityDto> handler,
            LiabilityContext context)
            : base(logger, validator, mapper, handler, context)
        { }

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

        public new async Task<ILiabilityType> AddAsync(LiabilityTypeDto dto)
        {
            return await base.AddAsync(dto);
        }

        public new async Task<Liability> AddAsync(LiabilityDto dto)
        {
            dto.TypeName = LiabilityTypes.Base;

            return await base.AddAsync(dto);
        }

        public new async Task<Liability> UpdateAsync(int id, LiabilityDto dto)
        {
            return await base.UpdateAsync(id, dto);
        }

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}