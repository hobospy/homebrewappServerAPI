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
        Task<SaveBrewResponse> SaveAsync(Brew brew);
    }
}
