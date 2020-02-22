using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeService(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            this._recipeRepository = recipeRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Recipe>> ListAsync()
        {
            return await _recipeRepository.ListAsync();
        }

        public async Task<RecipeResponse> SaveAsync(Recipe recipe)
        {
            try
            {
                await _recipeRepository.AddAsync(recipe);
                await _unitOfWork.CompleteAsync();

                return new RecipeResponse(recipe);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new RecipeResponse($"An error occurred when saving the recipe: {ex.Message}");
            }
        }

        public async Task<RecipeResponse> UpdateAsync(int id, Recipe recipe)
        {
            var existingRecipe = await _recipeRepository.FindByIdAsync(id);

            if (existingRecipe == null)
            {
                return new RecipeResponse("Recipe not found.");
            }

            existingRecipe.Name = recipe.Name;

            try
            {
                _recipeRepository.Update(existingRecipe);
                await _unitOfWork.CompleteAsync();

                return new RecipeResponse(existingRecipe);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new RecipeResponse($"An error occured when updating the recipe: {ex.Message}");
            }
        }

        public async Task<RecipeResponse> DeleteAsync(int id)
        {
            var existingRecipe = await _recipeRepository.FindByIdAsync(id);

            if (existingRecipe == null)
            {
                return new RecipeResponse("Recipe not found");
            }

            try
            {
                _recipeRepository.Remove(existingRecipe);
                await _unitOfWork.CompleteAsync();

                return new RecipeResponse(existingRecipe);
            }
            catch (Exception ex)
            {
                // Do some logging
                return new RecipeResponse($"An error occured when deleting the recipe: {ex.Message}");
            }
        }
    }
}
