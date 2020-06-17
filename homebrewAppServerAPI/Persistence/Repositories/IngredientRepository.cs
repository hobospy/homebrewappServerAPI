using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class IngredientRepository : BaseRepository, IIngredientRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

#if USE_SQLITE
        public IngredientRepository(SqliteDbContext context) : base(context) { }
#else
        public IngredientRepository(AppDbContext context) : base(context) { }
#endif
        public async Task<Ingredient> AddAsync(Ingredient ingredient)
        {
            //TODO: Think this should be moved to the service layer
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            if (ingredient != null)
            {
                var newIngredient = new Ingredient();
                newIngredient.Name = ingredient.Name;
                newIngredient.Type = ingredient.Type;
                newIngredient.Amount = ingredient.Amount;
                newIngredient.Unit = ingredient.Unit;
                newIngredient.RecipeID = ingredient.RecipeID;

                log.Debug($"Adding {ingredient.Name} ingredient basic properties");

                _context.Ingredients.Add(newIngredient);
                _context.SaveChanges();
                await _context.Entry(newIngredient).GetDatabaseValuesAsync();
                _context.Entry<Ingredient>(newIngredient).State = EntityState.Detached;

                return (newIngredient);
            }
            return (null);
        }

        public async Task<Ingredient> FindByIdAsync(int id)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.ID == id);
        }

        public async Task<IEnumerable<Ingredient>> ListAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public void Remove(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
        }

        public void Update(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
        }
    }
}
