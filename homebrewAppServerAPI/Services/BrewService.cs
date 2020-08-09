using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Domain.Services.Communication;
using homebrewAppServerAPI.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Services
{
    public class BrewService : IBrewService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            log.Debug($"Called {Helper.GetCurrentMethod()} with brew {brew.Name}");

            try
            {
                var newBrew = new Brew();
                newBrew.Name = brew.Name;
                newBrew.RecipeID = brew.RecipeID;
                newBrew.BrewDate = brew.BrewDate;

                var storedBrew = await _brewRepository.AddAsync(newBrew);

                if (storedBrew != null)
                {
                    storedBrew = await _brewRepository.FindByIdAsync(storedBrew.ID);
                }

                return new BrewResponse(storedBrew);
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
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to update brew, can't find a brew with ID: {id}");
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
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when updating the brew ({existingBrew.Name}): {ex.Message}");
            }
        }

        public async Task<BrewResponse> PatchAsync(int id, JsonPatchDocument<Brew> patch)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with id {id}");

            if (patch == null)
            {
                var errorMsg = "Unable to patch brew, patch information is null";

                log.Debug(errorMsg);
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch brew, patch information is null");
            }

            var existingBrew = await _brewRepository.FindByIdAsync(id);

            if (existingBrew == null)
            {
                log.Debug($"Unable to find a brew with id {id}");
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch brew, can't find a brew with ID: {id}");
            }

            try
            {
                log.Debug($"Patching brew {existingBrew.Name} [{existingBrew.ID}]");
                patch.ApplyTo(existingBrew);
                var updatedBrew = await _brewRepository.Update(existingBrew);

                return new BrewResponse(updatedBrew);
            }
            catch (Exception ex)
            {
                log.Debug($"Error caught when updating the brew {existingBrew.Name}[{existingBrew.ID}] {Helper.GetCurrentMethod()}," +
                    $" {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "No inner exception")}");

                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when patching the brew ({existingBrew.Name}): {ex.Message}");
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
