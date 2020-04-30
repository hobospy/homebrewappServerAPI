using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> ListAsync();
        Task AddAsync(Ingredient ingredient);
        Task<Ingredient> FindByIdAsync(int id);
        void Update(Ingredient ingredient);
        void Remove(Ingredient ingredient);
    }
}
