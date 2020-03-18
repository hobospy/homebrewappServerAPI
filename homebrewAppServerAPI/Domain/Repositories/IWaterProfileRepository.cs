using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface IWaterProfileRepository
    {
        Task<IEnumerable<WaterProfile>> ListAsync();
        Task AddAsync(WaterProfile waterProfile);
        Task<WaterProfile> FindByIdAsync(int id);
        void Update(WaterProfile waterProfile);
        void Remove(WaterProfile waterProfile);
    }
}
