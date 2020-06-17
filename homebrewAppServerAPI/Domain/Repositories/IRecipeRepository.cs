using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> ListAsync();
        Task<Recipe> AddAsync(Recipe recipe);
        Task<Recipe> FindByIdAsync(int id);
        Task<Recipe> Update(Recipe recipe);
        void Remove(Recipe recipe);
    }
}
