using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Domain.Services.Communication;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Services
{
    public class RecipeStepService : IRecipeStepService
    {
        private readonly IRecipeStepRepository _recipeStepRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeStepService(IRecipeStepRepository recipeStepRepository, IUnitOfWork unitOfWork)
        {
            this._recipeStepRepository = recipeStepRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RecipeStep>> ListAsync()
        {
            return await _recipeStepRepository.ListAsync();
        }

        public async Task<RecipeStepResponse> GetAsync(int id)
        {
            var recipeStepDetail = await _recipeStepRepository.FindByIdAsync(id);

            if (recipeStepDetail == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a recipe step with the ID: {id}");
            }

            return new RecipeStepResponse(recipeStepDetail);
        }

        public async Task<RecipeStepResponse> SaveAsync(RecipeStep recipeStep)
        {
            try
            {
                RecipeStepResponse returnValue;

                if (recipeStep != null)
                {
                    await _recipeStepRepository.AddAsync(recipeStep);
                    await _unitOfWork.CompleteAsync();

                    returnValue = new RecipeStepResponse(recipeStep);
                }
                else
                {
                    throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Cannot save a null recipe step");
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                var msg = "An error occurred when saving the recipe step";
                msg += recipeStep != null ? $" ({recipeStep.Description}: {ex.Message}" : $": {ex.Message}";
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", msg);
            }
        }

        public async Task<RecipeStepResponse> UpdateAsync(int id, RecipeStep recipeStep)
        {
            var existingRecipeStep = await _recipeStepRepository.FindByIdAsync(id);

            if (existingRecipeStep == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to update recipe step, can't find a recipe step with ID: {id}");
            }

            existingRecipeStep.Description = recipeStep.Description;
            existingRecipeStep.Timer = recipeStep.Timer;

            try
            {
                _recipeStepRepository.Update(existingRecipeStep);
                await _unitOfWork.CompleteAsync();

                return new RecipeStepResponse(existingRecipeStep);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when updating the recipe step ({existingRecipeStep.Description}): {ex.Message}");
            }
        }

        public async Task<RecipeStepResponse> PatchAsync(int id, JsonPatchDocument<RecipeStep> patch)
        {
            if (patch == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch recipe step, patch information is null");
            }

            var existingRecipeStep = await _recipeStepRepository.FindByIdAsync(id);

            if (existingRecipeStep == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch recipe step, can't find an recipe step with ID: {id}");
            }

            patch.ApplyTo(existingRecipeStep);

            try
            {
                _recipeStepRepository.Update(existingRecipeStep);
                await _unitOfWork.CompleteAsync();

                return new RecipeStepResponse(existingRecipeStep);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when patching the recipe step ({existingRecipeStep.Description}): {ex.Message}");
            }
        }

        public async Task<RecipeStepResponse> DeleteAsync(int id)
        {
            var existingRecipeStep = await _recipeStepRepository.FindByIdAsync(id);

            if (existingRecipeStep == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to delete recipe step, can't find a recipe step with ID: {id}");
            }

            try
            {
                _recipeStepRepository.Remove(existingRecipeStep);
                await _unitOfWork.CompleteAsync();

                return new RecipeStepResponse(existingRecipeStep);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when deleting the recipe step: {ex.Message}");
            }
        }
    }
}
