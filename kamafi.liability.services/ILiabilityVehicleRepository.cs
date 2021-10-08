using System;
using System.Threading.Tasks;

using kamafi.liability.data;

namespace kamafi.liability.services
{
    public interface IVehicleRepository
    {
        Task<Vehicle> AddAsync(VehicleDto dto);
        Task<Vehicle> UpdateAsync(int id, VehicleDto dto);
    }
}