using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using AutoMapper;
using FluentValidation;

using kamafi.liability.data;

namespace kamafi.liability.services
{
    public class VehicleRepository : 
        BaseRepository<Vehicle, VehicleDto>,
        IVehicleRepository
    {
        public VehicleRepository(
            ILogger<VehicleRepository> logger,
            IValidator<VehicleDto> validator,
            IMapper mapper,        
            LiabilityContext context)
            : base(logger, validator, mapper, context)
        { }

        public new async Task<Vehicle> AddAsync(VehicleDto dto)
        {
            dto.TypeName = LiabilityTypes.Vehicle;

            return await base.AddAsync(dto);
        }
    }
}