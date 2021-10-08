using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using kamafi.liability.data;

namespace kamafi.liability.services
{
    public interface ILiabilityRepository
    {
        Task<IEnumerable<ILiabilityType>> GetTypesAsync();
        Task<IEnumerable<ILiability>> GetAsync();
        Task<ILiability> GetAsync(int id);
        Task<ILiabilityType> AddAsync(LiabilityTypeDto dto);
        Task<Liability> AddAsync(LiabilityDto dto);
        Task<Liability> UpdateAsync(int id, LiabilityDto dto);
        Task DeleteAsync(int id);
    }
}