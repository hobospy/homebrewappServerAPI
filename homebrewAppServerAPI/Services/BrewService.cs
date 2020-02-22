using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Services
{
    public class BrewService : IBrewService
    {
        private readonly IBrewRepository _brewRepository;

        public BrewService(IBrewRepository brewRepository)
        {
            this._brewRepository = brewRepository;
        }

        public async Task<IEnumerable<Brew>> ListAsync()
        {
            return await _brewRepository.ListAsync();
        }
    }
}
