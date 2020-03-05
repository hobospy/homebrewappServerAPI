﻿using homebrewAppServerAPI.Domain.Models;
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

        public async Task<Brew> GetAsync(int id)
        {
            return await _brewRepository.FindByIdAsync(id);
        }

        public async Task<BrewResponse> SaveAsync(Brew brew)
        {
            try
            {
                await _brewRepository.AddAsync(brew);
                await _unitOfWork.CompleteAsync();

                return new BrewResponse(brew);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new BrewResponse($"An error occurred when saving the brew: {ex.Message}");
            }
        }

        public async Task<BrewResponse> UpdateAsync(int id, Brew brew)
        {
            var existingBrew = await _brewRepository.FindByIdAsync(id);

            if (existingBrew == null)
            {
                return new BrewResponse("Brew not found.");
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
                // Do some logging stuff
                return new BrewResponse($"An error occured when updating the brew: {ex.Message}");
            }
        }

        public async Task<BrewResponse> DeleteAsync(int id)
        {
            var existingBrew = await _brewRepository.FindByIdAsync(id);

            if (existingBrew == null)
            {
                return new BrewResponse("Brew not found");
            }

            try
            {
                _brewRepository.Remove(existingBrew);
                await _unitOfWork.CompleteAsync();

                return new BrewResponse(existingBrew);
            }
            catch (Exception ex)
            {
                // Do some logging
                return new BrewResponse($"An error occured when deleting the brew: {ex.Message}");
            }
        }
    }
}
