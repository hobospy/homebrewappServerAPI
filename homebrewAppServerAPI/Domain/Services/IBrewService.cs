using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services
{
    public interface IBrewService
    {
        Task<IEnumerable<Brew>> ListAsync();
        Task<Brew> GetAsync(int id);
        Task<BrewResponse> SaveAsync(Brew brew);
        Task<BrewResponse> UpdateAsync(int id, Brew brew);
        Task<BrewResponse> DeleteAsync(int id);
    }
}
