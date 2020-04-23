using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> ListAsync();
        Task<RecipeResponse> GetAsync(int id);
        Task<RecipeResponse> SaveAsync(Recipe recipe);
        Task<RecipeResponse> UpdateAsync(int id, Recipe recipe);
        Task<RecipeResponse> DeleteAsync(int id);
    }
}
