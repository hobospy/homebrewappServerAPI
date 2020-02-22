using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Services
{
    public class BrewService : IBrewService
    {
        private readonly IBrewRepository _brewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BrewService(IBrewRepository brewRepository, IUnitOfWork unitOfWork)
        {
            this._brewRepository = brewRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Brew>> ListAsync()
        {
            return await _brewRepository.ListAsync();
        }

        public async Task<SaveBrewResponse> SaveAsync(Brew brew)
        {
            try
            {
                await _brewRepository.AddAsync(brew);
                await _unitOfWork.CompleteAsync();

                return new SaveBrewResponse(brew);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveBrewResponse($"An error occurred when saving the brew: {ex.Message}");
            }
        }
    }
}
