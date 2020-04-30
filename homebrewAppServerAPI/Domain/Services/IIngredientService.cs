using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services.Communication;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> ListAsync();
        Task<IngredientResponse> GetAsync(int id);
        Task<IngredientResponse> SaveAsync(Ingredient ingredient);
        Task<IngredientResponse> UpdateAsync(int id, Ingredient ingredient);
        Task<IngredientResponse> PatchAsync(int id, JsonPatchDocument<Ingredient> patch);
        Task<IngredientResponse> DeleteAsync(int id);
    }
}
