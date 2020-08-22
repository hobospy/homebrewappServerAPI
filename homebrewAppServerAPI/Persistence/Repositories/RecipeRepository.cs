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
                                    .Include(p => p.Steps).ThenInclude(w => w.Ingredients)
                                    .Include(p => p.Steps).ThenInclude(w => w.Timer)
                                    .ToListAsync();
        }

        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            await _context.Entry(recipe).GetDatabaseValuesAsync();

            return recipe;
        }

        public async Task<Recipe> FindByIdAsync(int id)
        {
            return await _context.Recipes
                                    .Include(p => p.WaterProfile).ThenInclude(w => w.Additions)
                                    .Include(p => p.Steps).ThenInclude(w => w.Ingredients)
                                    .Include(p => p.Steps).ThenInclude(w => w.Timer)
                                    .FirstOrDefaultAsync(recipe => recipe.ID == id);
        }

        public async Task<Recipe> Update(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            _context.SaveChangesAsync();
            await _context.Entry(recipe).GetDatabaseValuesAsync();
            _context.Entry<Recipe>(recipe).State = EntityState.Detached;

            //var returnRecipe =  await _context.Recipes
            //                        .Include(p => p.WaterProfile).ThenInclude(w => w.Additions)
            //                        .Include(p => p.Steps).ThenInclude(w => w.Ingredients)
            //                        .Include(p => p.Steps).ThenInclude(w => w.Timer)
            //                        .FirstOrDefaultAsync(recipeToFind => recipeToFind.ID == recipe.ID);

            var returnRecipe = await FindByIdAsync(recipe.ID);

            return returnRecipe;
        }

        public void Remove(Recipe recipe)
        {
            _context.Recipes.Remove(recipe);
        }
    }
}
