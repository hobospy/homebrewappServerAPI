using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class RecipeStepRepository : BaseRepository, IRecipeStepRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

#if USE_SQLITE
        public RecipeStepRepository(SqliteDbContext context) : base(context) { }
#else
        public RecipeStepRepository(AppDbContext context) : base(context) { }
#endif
        public async Task<RecipeStep> AddAsync(RecipeStep recipeStep)
        {
            //TODO: Think this should be moved to the service layer
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            if (recipeStep != null)
            {
                var newRecipeStep = new RecipeStep();
                newRecipeStep.Description = recipeStep.Description;
                newRecipeStep.Timer = recipeStep.Timer;
                newRecipeStep.RecipeID = recipeStep.RecipeID;

                log.Debug($"Adding {recipeStep.Description} recipe step basic properties");
                _context.RecipeSteps.Add(newRecipeStep);
                _context.SaveChanges();
                await _context.Entry(newRecipeStep).GetDatabaseValuesAsync();
                _context.Entry<RecipeStep>(newRecipeStep).State = EntityState.Detached;

                return (newRecipeStep);
            }
            return (null);
        }

        public async Task<RecipeStep> FindByIdAsync(int id)
        {
            return await _context.RecipeSteps
                                    .Include(p => p.Ingredients)
                                    .Include(p => p.Timer)
                                    .FirstOrDefaultAsync(recipeStep => recipeStep.ID == id);
        }

        public async Task<IEnumerable<RecipeStep>> ListAsync()
        {
            return await _context.RecipeSteps
                                    .Include(p => p.Ingredients)
                                    .Include(p => p.Timer)
                                    .ToListAsync();
        }

        public void Remove(RecipeStep recipeStep)
        {
            _context.RecipeSteps.Remove(recipeStep);
        }

        public async Task<RecipeStep> Update(RecipeStep recipeStep)
        {
            _context.RecipeSteps.Update(recipeStep);
            await _context.SaveChangesAsync();

            return await _context.RecipeSteps
                                    .Include(p => p.Ingredients)
                                    .Include(p => p.Timer)
                                    .FirstOrDefaultAsync(recipeStep => recipeStep.ID == recipeStep.ID);
        }
    }
}
