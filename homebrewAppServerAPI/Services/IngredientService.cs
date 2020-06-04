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
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IngredientService(IIngredientRepository ingredientRepository, IUnitOfWork unitOfWork)
        {
            this._ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Ingredient>> ListAsync()
        {
            return await _ingredientRepository.ListAsync();
        }

        public async Task<IngredientResponse> GetAsync(int id)
        {
            var ingredientDetail = await _ingredientRepository.FindByIdAsync(id);

            if (ingredientDetail == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a ingredient with the ID: {id}");
            }

            return new IngredientResponse(ingredientDetail);
        }

        public async Task<IngredientResponse> SaveAsync(Ingredient ingredient)
        {
            try
            {
                IngredientResponse returnValue;

                if (ingredient != null)
                {
                    await _ingredientRepository.AddAsync(ingredient);
                    await _unitOfWork.CompleteAsync();

                    returnValue = new IngredientResponse(ingredient);
                }
                else
                {
                    throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Cannot save a null ingredient");
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                var msg = "An error occurred when saving the ingredient";
                msg += ingredient != null ? $" ({ingredient.Name}: {ex.Message}" : $": {ex.Message}";
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", msg);
            }
        }

        public async Task<IngredientResponse> UpdateAsync(int id, Ingredient ingredient)
        {
            var existingIngredient = await _ingredientRepository.FindByIdAsync(id);

            if (existingIngredient == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to update ingredient, can't find a ingredient with ID: {id}");
            }

            existingIngredient.Name = ingredient.Name;

            try
            {
                _ingredientRepository.Update(existingIngredient);
                await _unitOfWork.CompleteAsync();

                return new IngredientResponse(existingIngredient);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when updating the ingredient ({existingIngredient.Name}): {ex.Message}");
            }
        }

        public async Task<IngredientResponse> PatchAsync(int id, JsonPatchDocument<Ingredient> patch)
        {
            if (patch == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch ingredient, patch information is null");
            }

            var existingIngredient = await _ingredientRepository.FindByIdAsync(id);

            if (existingIngredient == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch ingredient, can't find an ingredient with ID: {id}");
            }

            patch.ApplyTo(existingIngredient);

            try
            {
                _ingredientRepository.Update(existingIngredient);
                await _unitOfWork.CompleteAsync();

                return new IngredientResponse(existingIngredient);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when patching the ingredient ({existingIngredient.Name}): {ex.Message}");
            }
        }

        public async Task<IngredientResponse> DeleteAsync(int id)
        {
            var existingIngredient = await _ingredientRepository.FindByIdAsync(id);

            if (existingIngredient == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to delete ingredient, can't find an ingredient with ID: {id}");
            }

            try
            {
                _ingredientRepository.Remove(existingIngredient);
                await _unitOfWork.CompleteAsync();

                return new IngredientResponse(existingIngredient);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when deleting the ingredient: {ex.Message}");
            }
        }
    }
}
