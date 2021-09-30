using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using kamafi.liability.data;

namespace kamafi.liability.services
{
    public interface ILiabilityRepository
    {
        Task<IEnumerable<Liability>> GetAsync();
        Task<Liability> AddAsync(LiabilityDto dto);
        Task<Liability> UpdateAsync(int id, LiabilityDto dto);
        Task DeleteAsync(int id);
    }
}