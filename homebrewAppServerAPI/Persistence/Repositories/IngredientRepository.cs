using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class IngredientRepository : BaseRepository, IIngredientRepository
    {
#if USE_SQLITE
        public IngredientRepository(SqliteDbContext context) : base(context) { }
#else
        public IngredientRepository(AppDbContext context) : base(context) { }
#endif

        public async Task<IEnumerable<Ingredient>> ListAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task AddAsync(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                var newIngredient = new Ingredient();
                newIngredient.Name = ingredient.Name;
                newIngredient.Type = ingredient.Type;
                newIngredient.Amount = ingredient.Amount;
                newIngredient.Unit = ingredient.Unit;
                newIngredient.RecipeID = ingredient.RecipeID;

                _context.Ingredients.Add(newIngredient);
                _context.SaveChanges();

                await _context.Entry(newIngredient).GetDatabaseValuesAsync();
            }
        }

        public async Task<Ingredient> FindByIdAsync(int id)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.ID == id);
        }

        public void Update(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
        }

        public void Remove(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
        }
    }
}
