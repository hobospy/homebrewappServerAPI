using homebrewAppServerAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services
{
    public interface IBrewService
    {
        Task<IEnumerable<Brew>> ListAsync();
    }
}
