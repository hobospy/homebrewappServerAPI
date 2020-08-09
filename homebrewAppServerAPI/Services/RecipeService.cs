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
    public class RecipeService : IRecipeService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            log.Debug($"Called {Helper.GetCurrentMethod()} with ID {id}");

            var recipeDetail = await _recipeRepository.FindByIdAsync(id);

            if (recipeDetail == null)
            {
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to find a recipe with the ID: {id}");
            }

            log.Debug($"Returning detail for {recipeDetail.Name} [{recipeDetail.ID}]");
            return new RecipeResponse(recipeDetail);
        }

        public async Task<RecipeResponse> SaveAsync(Recipe recipe)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with recipe {recipe.Name}");

            try
            {
                var newRecipe = new Recipe();
                newRecipe.Name = recipe.Name;
                newRecipe.Description = recipe.Description;
                newRecipe.ExpectedABV = recipe.ExpectedABV;
                newRecipe.Favourite = recipe.Favourite;
                newRecipe.Type = recipe.Type;
                newRecipe.WaterProfileID = recipe.WaterProfileID;

                var storedRecipe = await _recipeRepository.AddAsync(newRecipe);

                // Add all the linked items to their respective repostiories
                //foreach (var ingredient in recipe.Ingredients)
                //{
                //    var newIngredient = new Ingredient();
                //    newIngredient.Amount = ingredient.Amount;
                //    newIngredient.Name = ingredient.Name;
                //    newIngredient.RecipeID = storedRecipe.ID;
                //    newIngredient.Type = ingredient.Type;
                //    newIngredient.Unit = ingredient.Unit;

                //    var tempIngredient = await _ingredientRepository.AddAsync(newIngredient);
                //    newIngredient.ID = tempIngredient.ID;

                //    log.Debug($"Adding {newIngredient.Name} step to recipe {recipe.Name}");
                //    storedRecipe.Ingredients.Add(newIngredient);
                //}

                foreach (var step in recipe.Steps)
                {
                    var newStep = new RecipeStep();
                    newStep.RecipeID = storedRecipe.ID;
                    newStep.Description = step.Description;
                    newStep.Timer = step.Timer;

                    foreach (var ingredient in step.Ingredients)
                    {
                        var newIngredient = new Ingredient();
                        newIngredient.Amount = ingredient.Amount;
                        newIngredient.Name = ingredient.Name;
                        newIngredient.RecipeStepID = step.ID;
                        newIngredient.Type = ingredient.Type;
                        newIngredient.Unit = ingredient.Unit;

                        var tempIngredient = await _ingredientRepository.AddAsync(newIngredient);
                        newIngredient.ID = tempIngredient.ID;

                        log.Debug($"Adding {newIngredient.Name} step to recipe {recipe.Name} under step {newStep.Description}");
                        //storedRecipe.Ingredients.Add(newIngredient);
                        newStep.Ingredients.Add(newIngredient);
                    }

                    var tempRecipeStep = await _recipeStepRepository.AddAsync(newStep);
                    newStep.ID = tempRecipeStep.ID;

                    log.Debug($"Adding {newStep.Description} step to recipe {recipe.Name}");
                    storedRecipe.Steps.Add(newStep);
                }

                // return list of recipes
                return new RecipeResponse(storedRecipe);
            }
            catch (Exception ex)
            {
                log.Debug($"Error caught when saving the recipe {recipe.Name}[{recipe.ID}] {Helper.GetCurrentMethod()}," + 
                    $" {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "No inner exception")}");

                return new RecipeResponse($"An error occurred when saving the recipe: {ex.Message}");
            }
        }

        public async Task<RecipeResponse> UpdateAsync(int id, Recipe updatedRecipe)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with id {id} and updated recipe {updatedRecipe.Name}");

            var existingRecipe = await _recipeRepository.FindByIdAsync(id);

            if (existingRecipe == null)
            {
                log.Debug($"Unable to find a recipe with id {id}");
                return new RecipeResponse("Recipe not found.");
            }
            
            existingRecipe.Name = updatedRecipe.Name;
            existingRecipe.Description = updatedRecipe.Description;
            existingRecipe.Type = updatedRecipe.Type;
            existingRecipe.WaterProfileID = updatedRecipe.WaterProfileID;
            existingRecipe.ExpectedABV = updatedRecipe.ExpectedABV;

            //// Remove any ingredient no longer included
            //foreach (var existingIngredient in existingRecipe.Ingredients)
            //{
            //    var foundIngredient = updatedRecipe.Ingredients.FirstOrDefault(i => i.ID == existingIngredient.ID);

            //    if (foundIngredient == null)
            //    {
            //        log.Debug($"Removing ingredient with ID {existingIngredient.ID}");
            //        _ingredientRepository.Remove(existingIngredient);
            //    }
            //}

            //foreach (var ingredient in updatedRecipe.Ingredients)
            //{
            //    var existingIngredient = await _ingredientRepository.FindByIdAsync(ingredient.ID);

            //    if (existingIngredient != null)
            //    {
            //        existingIngredient.Amount = ingredient.Amount;
            //        existingIngredient.Name = ingredient.Name;
            //        existingIngredient.Type = ingredient.Type;
            //        existingIngredient.Unit = ingredient.Unit;

            //        log.Debug($"Ensuring the existing ingredient with ID {existingIngredient.ID}, matches details entered");
            //        _ingredientRepository.Update(existingIngredient);
            //        await _unitOfWork.CompleteAsync();
            //    }
            //    else
            //    {
            //        // Got to add the ingredient before we can store it in the Recipe
            //        log.Debug($"Adding new ingredient {ingredient.Name}");
            //        ingredient.RecipeID = id;
            //        var tempIng = await _ingredientRepository.AddAsync(ingredient);
            //        ingredient.ID = tempIng.ID;
            //    }
            //}

            // Remove any step no longer included
            foreach (var existingStep in existingRecipe.Steps)
            {
                var foundStep = updatedRecipe.Steps.FirstOrDefault(i => i.ID == existingStep.ID);

                if (foundStep == null)
                {
                    log.Debug($"Removing step with ID {existingStep.ID}");
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

                    // Remove any ingredient no longer included
                    foreach (var existingIngredient in existingRecipeStep.Ingredients)
                    {
                        var foundIngredient = recipeStep.Ingredients.FirstOrDefault(i => i.ID == existingIngredient.ID);

                        if (foundIngredient == null)
                        {
                            log.Debug($"Removing ingredient with ID {existingIngredient.ID}");
                            _ingredientRepository.Remove(existingIngredient);
                        }
                    }

                    foreach (var ingredient in recipeStep.Ingredients)
                    {
                        var existingIngredient = await _ingredientRepository.FindByIdAsync(ingredient.ID);

                        if (existingIngredient != null)
                        {
                            existingIngredient.Amount = ingredient.Amount;
                            existingIngredient.Name = ingredient.Name;
                            existingIngredient.Type = ingredient.Type;
                            existingIngredient.Unit = ingredient.Unit;

                            log.Debug($"Ensuring the existing ingredient with ID {existingIngredient.ID}, matches details entered");
                            _ingredientRepository.Update(existingIngredient);
                            await _unitOfWork.CompleteAsync();
                        }
                        else
                        {
                            // Got to add the ingredient before we can store it in the Recipe
                            log.Debug($"Adding new ingredient {ingredient.Name}");
                            ingredient.RecipeStepID = recipeStep.ID;
                            var tempIng = await _ingredientRepository.AddAsync(ingredient);
                            ingredient.ID = tempIng.ID;
                        }
                    }

                    log.Debug($"Ensuring the existing step with ID {existingRecipeStep.ID}, matches details entered");
                    _recipeStepRepository.Update(existingRecipeStep);
                    await _unitOfWork.CompleteAsync();
                }
                else
                {
                    // Got to add the ingredient before we can store it in the Recipe
                    log.Debug($"Adding new step {recipeStep.Description}");
                    recipeStep.RecipeID = id;
                    var tempRecipeStep = await _recipeStepRepository.AddAsync(recipeStep);
                    recipeStep.ID = tempRecipeStep.ID;
                }
            }

            try
            {
                log.Debug($"Updating recipe {existingRecipe.Name} [{existingRecipe.ID}]");
                var returnRecipe = await _recipeRepository.Update(existingRecipe);

                return new RecipeResponse(returnRecipe);
            }
            catch (Exception ex)
            {
                log.Debug($"Error caught when updating the recipe {existingRecipe.Name}[{existingRecipe.ID}] {Helper.GetCurrentMethod()}," +
                    $" {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "No inner exception")}");

                return new RecipeResponse($"An error occured when updating the recipe: {ex.Message}");
            }
        }

        public async Task<RecipeResponse> PatchAsync(int id, JsonPatchDocument<Recipe> patch)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with id {id}");

            if (patch == null)
            {
                var errorMsg = "Unable to patch recipe, patch information is null";

                log.Debug(errorMsg);
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", errorMsg);
            }

            var existingRecipe = await _recipeRepository.FindByIdAsync(id);

            if (existingRecipe == null)
            {
                log.Debug($"Unable to find a recipe with id {id}");
                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"Unable to patch recipe, can't find a recipe with ID: {id}");
            }

            try
            {
                log.Debug($"Patching recipe {existingRecipe.Name} [{existingRecipe.ID}]");
                patch.ApplyTo(existingRecipe);
                var updatedRecipe  = await _recipeRepository.Update(existingRecipe);

                return new RecipeResponse(updatedRecipe);
            }
            catch (Exception ex)
            {
                log.Debug($"Error caught when updating the recipe {existingRecipe.Name}[{existingRecipe.ID}] {Helper.GetCurrentMethod()}," +
                    $" {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "No inner exception")}");

                throw new homebrewAPIException(HttpStatusCode.BadRequest, "0", $"An error occurred when patching the recipe ({existingRecipe.Name}): {ex.Message}");
            }
        }

        public async Task<RecipeResponse> DeleteAsync(int id)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()} with id {id}");

            var existingRecipe = await _recipeRepository.FindByIdAsync(id);

            if (existingRecipe == null)
            {
                log.Debug($"Unable to find a recipe with id {id}");
                return new RecipeResponse("Recipe not found");
            }

            try
            {
                log.Debug($"Removing recipe {existingRecipe.Name} [{existingRecipe.ID}]");
                _recipeRepository.Remove(existingRecipe);
                await _unitOfWork.CompleteAsync();

                return new RecipeResponse(existingRecipe);
            }
            catch (Exception ex)
            {
                log.Debug($"Error caught when updating the recipe {existingRecipe.Name}[{existingRecipe.ID}] {Helper.GetCurrentMethod()}," +
                    $" {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "No inner exception")}");

                return new RecipeResponse($"An error occured when deleting the recipe: {ex.Message}");
            }
        }
    }
}
