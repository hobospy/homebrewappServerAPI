using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface IRecipeStepRepository
    {
        Task<IEnumerable<RecipeStep>> ListAsync();
        Task<RecipeStep> AddAsync(RecipeStep recipeStep);
        Task<RecipeStep> FindByIdAsync(int id);
        Task<RecipeStep> Update(RecipeStep recipeStep);
        void Remove(RecipeStep recipeStep);
    }
}
