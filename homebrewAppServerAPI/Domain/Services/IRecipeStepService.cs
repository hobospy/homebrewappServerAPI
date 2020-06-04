using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services.Communication;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services
{
    public interface IRecipeStepService
    {
        Task<IEnumerable<RecipeStep>> ListAsync();
        Task<RecipeStepResponse> GetAsync(int id);
        Task<RecipeStepResponse> SaveAsync(RecipeStep recipeStep);
        Task<RecipeStepResponse> UpdateAsync(int id, RecipeStep recipeStep);
        Task<RecipeStepResponse> PatchAsync(int id, JsonPatchDocument<RecipeStep> patch);
        Task<RecipeStepResponse> DeleteAsync(int id);
    }
}
