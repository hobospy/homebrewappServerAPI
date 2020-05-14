using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class RecipeRepository : BaseRepository, IRecipeRepository
    {
#if USE_SQLITE
        public RecipeRepository(SqliteDbContext context) : base(context) { }
#else
        public RecipeRepository(AppDbContext context) : base(context) { }
#endif

        public async Task<IEnumerable<Recipe>> ListAsync()
        {
            return await _context.Recipes
                                    .Include(p => p.WaterProfile).ThenInclude(w => w.Additions)
                                    .Include(p => p.Ingredients)
                                    .ToListAsync();
        }

        public async Task AddAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
        }

        public async Task<Recipe> FindByIdAsync(int id)
        {
            return await _context.Recipes
                                    .Include(p => p.WaterProfile).ThenInclude(w => w.Additions)
                                    .Include(p => p.Ingredients)
                                    .FirstOrDefaultAsync(recipe => recipe.ID == id);
        }

        public void Update(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
        }

        public void Remove(Recipe recipe)
        {
            _context.Recipes.Remove(recipe);
        }
    }
}
