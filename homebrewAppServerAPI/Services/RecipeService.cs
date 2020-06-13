using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Domain.Services.Communication;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeStepRepository _recipeStepRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeService(IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository, IRecipeStepRepository recipeStepRepository, IUnitOfWork unitOfWork)
        {
            this._recipeRepository = recipeRepository;
            this._ingredientRepository = ingredientRepository;
            this._recipeStepRepository = recipeStepRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Recipe>> ListAsync()
        {
            return await _recipeRepository.ListAsync();
        }

        public async Task<RecipeResponse> GetAsync(int id)
        {
            var recipeDetail = await _recipeRepository.FindByIdAsync(id);

            if (recipeDetail == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a recipe with the ID: {id}");
            }

            return new RecipeResponse(recipeDetail);
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

        public async Task<RecipeResponse> UpdateAsync(int id, Recipe updatedRecipe)
        {
            var existingRecipe = await _recipeRepository.FindByIdAsync(id);

            if (existingRecipe == null)
            {
                return new RecipeResponse("Recipe not found.");
            }
            
            existingRecipe.Name = updatedRecipe.Name;
            existingRecipe.Description = updatedRecipe.Description;
            existingRecipe.Type = updatedRecipe.Type;
            existingRecipe.WaterProfileID = updatedRecipe.WaterProfileID;
            existingRecipe.ExpectedABV = updatedRecipe.ExpectedABV;

            // Remove any ingredient no longer included
            foreach (var existingIngredient in existingRecipe.Ingredients)
            {
                var foundIngredient = updatedRecipe.Ingredients.FirstOrDefault(i => i.ID == existingIngredient.ID);

                if (foundIngredient == null)
                {
                     _ingredientRepository.Remove(existingIngredient);
                }
            }

            foreach (var ingredient in updatedRecipe.Ingredients)
            {
                var existingIngredient = await _ingredientRepository.FindByIdAsync(ingredient.ID);

                if (existingIngredient != null)
                {
                    existingIngredient.Amount = ingredient.Amount;
                    existingIngredient.Name = ingredient.Name;
                    existingIngredient.Type = ingredient.Type;
                    existingIngredient.Unit = ingredient.Unit;

                    _ingredientRepository.Update(existingIngredient);
                    await _unitOfWork.CompleteAsync();
                }
                else
                {
                    // Got to add the ingredient before we can store it in the Recipe
                    ingredient.RecipeID = id;
                    var tempIng = await _ingredientRepository.AddAsync(ingredient);
                    ingredient.ID = tempIng.ID;
                }
            }

            // Remove any step no longer included
            foreach (var existingStep in existingRecipe.Steps)
            {
                var foundStep = updatedRecipe.Steps.FirstOrDefault(i => i.ID == existingStep.ID);

                if (foundStep == null)
                {
                    _recipeStepRepository.Remove(existingStep);
                }
            }

            foreach (var recipeStep in updatedRecipe.Steps)
            {
                var existingRecipeStep = await _recipeStepRepository.FindByIdAsync(recipeStep.ID);

                if (existingRecipeStep != null)
                {
                    existingRecipeStep.Description = recipeStep.Description;
                    existingRecipeStep.Timer = recipeStep.Timer;

                    _recipeStepRepository.Update(existingRecipeStep);
                    await _unitOfWork.CompleteAsync();
                }
                else
                {
                    // Got to add the ingredient before we can store it in the Recipe
                    recipeStep.RecipeID = id;
                    var tempRecipeStep = await _recipeStepRepository.AddAsync(recipeStep);
                    recipeStep.ID = tempRecipeStep.ID;
                }
            }

            try
            {
                var returnRecipe = await _recipeRepository.Update(existingRecipe);

                return new RecipeResponse(returnRecipe);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new RecipeResponse($"An error occured when updating the recipe: {ex.Message}");
            }
        }

        public async Task<RecipeResponse> PatchAsync(int id, JsonPatchDocument<Recipe> patch)
        {
            if (patch == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch recipe, patch information is null");
            }

            var existingRecipe = await _recipeRepository.FindByIdAsync(id);

            if (existingRecipe == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch recipe, can't find a recipe with ID: {id}");
            }

            patch.ApplyTo(existingRecipe);

            try
            {
                await _recipeRepository.Update(existingRecipe);

                return new RecipeResponse(existingRecipe);
            }
            catch (Exception ex)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when patching the recipe ({existingRecipe.Name}): {ex.Message}");
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
