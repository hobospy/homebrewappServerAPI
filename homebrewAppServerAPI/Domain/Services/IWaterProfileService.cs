using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services
{
    public interface IWaterProfileService
    {
        Task<IEnumerable<WaterProfile>> ListAsync();
        Task<WaterProfileResponse> SaveAsync(WaterProfile waterProfile);
        Task<WaterProfileResponse> UpdateAsync(int id, WaterProfile waterProfile);
        Task<WaterProfileResponse> DeleteAsync(int id);
    }
}
