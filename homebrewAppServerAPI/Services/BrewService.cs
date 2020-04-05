using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<BrewResponse> GetAsync(int id)
        {
            var brewDetail = await _brewRepository.FindByIdAsync(id);

            if (brewDetail == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a brew with the ID: {id}");
            }

            return new BrewResponse(brewDetail);
        }

        public async Task<BrewResponse> SaveAsync(Brew brew)
        {
            try
            {
                BrewResponse returnValue;

                if (brew != null)
                {
                    await _brewRepository.AddAsync(brew);
                    await _unitOfWork.CompleteAsync();

                    returnValue = new BrewResponse(brew);
                }
                else
                {
                    throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Cannot save a null brew");
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                var msg = "An error occurred when saving the brew";
                msg += brew != null ? $" ({brew.Name}: {ex.Message}" : $": {ex.Message}";
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", msg);
            }
        }

        public async Task<BrewResponse> UpdateAsync(int id, Brew brew)
        {
            var existingBrew = await _brewRepository.FindByIdAsync(id);

            if (existingBrew == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to update berw, can't find a brew with ID: {id}");
            }

            existingBrew.Name = brew.Name;

            try
            {
                _brewRepository.Update(existingBrew);
                await _unitOfWork.CompleteAsync();

                return new BrewResponse(existingBrew);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when updating the brew ({brew.Name}): {ex.Message}");
            }
        }

        public async Task<BrewResponse> DeleteAsync(int id)
        {
            var existingBrew = await _brewRepository.FindByIdAsync(id);

            if (existingBrew == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to delete brew, can't find a brew with ID: {id}");
            }

            try
            {
                _brewRepository.Remove(existingBrew);
                await _unitOfWork.CompleteAsync();

                return new BrewResponse(existingBrew);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when deleting the brew: {ex.Message}");
            }
        }
    }
}
