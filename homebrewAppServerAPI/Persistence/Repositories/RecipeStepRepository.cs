using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class RecipeStepRepository : BaseRepository, IRecipeStepRepository
    {
#if USE_SQLITE
        public RecipeStepRepository(SqliteDbContext context) : base(context) { }
#else
        public RecipeStepRepository(AppDbContext context) : base(context) { }
#endif
        public async Task<RecipeStep> AddAsync(RecipeStep recipeStep)
        {
            if (recipeStep != null)
            {
                var newRecipeStep = new RecipeStep();
                newRecipeStep.Description = recipeStep.Description;
                newRecipeStep.Timer = recipeStep.Timer;
                newRecipeStep.RecipeID = recipeStep.RecipeID;

                _context.RecipeSteps.Add(newRecipeStep);
                _context.SaveChanges();

                await _context.Entry(newRecipeStep).GetDatabaseValuesAsync();

                return (newRecipeStep);
            }
            return (null);
        }

        public async Task<RecipeStep> FindByIdAsync(int id)
        {
            return await _context.RecipeSteps.FirstOrDefaultAsync(recipeStep => recipeStep.ID == id);
        }

        public async Task<IEnumerable<RecipeStep>> ListAsync()
        {
            return await _context.RecipeSteps.ToListAsync();
        }

        public void Remove(RecipeStep recipeStep)
        {
            _context.RecipeSteps.Remove(recipeStep);
        }

        public void Update(RecipeStep recipeStep)
        {
            _context.RecipeSteps.Update(recipeStep);
        }
    }
}
