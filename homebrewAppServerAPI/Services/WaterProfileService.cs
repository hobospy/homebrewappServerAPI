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
    public class WaterProfileService : IWaterProfileService
    {
        private readonly IWaterProfileRepository _waterProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WaterProfileService(IWaterProfileRepository waterProfileRepository, IUnitOfWork unitOfWork)
        {
            this._waterProfileRepository = waterProfileRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WaterProfile>> ListAsync()
        {
            return await _waterProfileRepository.ListAsync();
        }

        public async Task<WaterProfileResponse> SaveAsync(WaterProfile waterProfile)
        {
            try
            {
                await _waterProfileRepository.AddAsync(waterProfile);
                await _unitOfWork.CompleteAsync();

                return new WaterProfileResponse(waterProfile);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new WaterProfileResponse($"An error occurred when saving the water profile: {ex.Message}");
            }
        }

        public async Task<WaterProfileResponse> UpdateAsync(int id, WaterProfile waterProfile)
        {
            var existingWaterProfile = await _waterProfileRepository.FindByIdAsync(id);

            if (existingWaterProfile == null)
            {
                return new WaterProfileResponse("Water profile not found.");
            }

            existingWaterProfile.Name = waterProfile.Name;

            try
            {
                _waterProfileRepository.Update(existingWaterProfile);
                await _unitOfWork.CompleteAsync();

                return new WaterProfileResponse(existingWaterProfile);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new WaterProfileResponse($"An error occured when updating the water profile: {ex.Message}");
            }
        }

        public async Task<WaterProfileResponse> DeleteAsync(int id)
        {
            var existingWaterProfile = await _waterProfileRepository.FindByIdAsync(id);

            if (existingWaterProfile == null)
            {
                return new WaterProfileResponse("Water profile not found");
            }

            try
            {
                _waterProfileRepository.Remove(existingWaterProfile);
                await _unitOfWork.CompleteAsync();

                return new WaterProfileResponse(existingWaterProfile);
            }
            catch (Exception ex)
            {
                // Do some logging
                return new WaterProfileResponse($"An error occured when deleting the water profile: {ex.Message}");
            }
        }
    }
}
