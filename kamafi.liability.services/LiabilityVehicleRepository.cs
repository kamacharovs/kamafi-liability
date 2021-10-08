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
    public class VehicleRepository : 
        BaseRepository<Vehicle, VehicleDto>,
        IVehicleRepository
    {
        [ExcludeFromCodeCoverage]
        public VehicleRepository(
            ILogger<VehicleRepository> logger,
            IValidator<VehicleDto> validator,
            IMapper mapper,
            IAbstractHandler<Vehicle, VehicleDto> handler,
            LiabilityContext context)
            : base(logger, validator, mapper, handler, context)
        { }

        public new async Task<Vehicle> AddAsync(VehicleDto dto)
        {
            return await base.AddAsync(dto);
        }

        public new async Task<Vehicle> UpdateAsync(int id, VehicleDto dto)
        {
            return await base.UpdateAsync(id, dto);
        }
    }
}