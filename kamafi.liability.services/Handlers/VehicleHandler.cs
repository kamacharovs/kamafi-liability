using System;

using kamafi.liability.data;

namespace kamafi.liability.services.handlers
{
    public class VehicleHandler : AbstractHandler<Vehicle, VehicleDto>
    {
        protected override VehicleDto OnBeforeHandle(VehicleDto dto)
        {
            dto.TypeName = LiabilityTypes.Vehicle;

            return dto;
        }

        protected override Vehicle OnHandle(Vehicle liability)
        {
            return liability;
        }

        protected override Vehicle OnHandleUpdate(VehicleDto dto, Vehicle liability)
        {
            return liability;
        }
    }
}
