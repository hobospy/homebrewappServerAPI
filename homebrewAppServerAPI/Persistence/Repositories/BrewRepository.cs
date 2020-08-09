using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class BrewRepository : BaseRepository, IBrewRepository
    {
#if USE_SQLITE
        public BrewRepository(SqliteDbContext context) : base(context) { }
#else
        public BrewRepository(AppDbContext context) : base(context) { }
#endif

        public async Task<IEnumerable<Brew>> ListAsync()
        {
            return await _context.Brews
                                    .Include(p => p.Recipe)
                                    .Include(p => p.Recipe.WaterProfile)
                                    .Include(p => p.Recipe.Steps).ThenInclude(w => w.Ingredients)
                                    .Include(p => p.Recipe.Steps).ThenInclude(w => w.Timer)
                                    .Include(p => p.TastingNotes)
                                    .ToListAsync();
        }

        public async Task<Brew> AddAsync(Brew brew)
        {
            _context.Brews.Add(brew);
            _context.SaveChanges();

            await _context.Entry(brew).GetDatabaseValuesAsync();
            
            return brew;
        }

        public async Task<Brew> FindByIdAsync(int id)
        {
            return await _context.Brews
                                    .Include(p => p.Recipe)
                                    .Include(p => p.Recipe.WaterProfile)
                                    .Include(p => p.Recipe.Steps).ThenInclude(w => w.Ingredients)
                                    .Include(p => p.Recipe.Steps).ThenInclude(w => w.Timer)
                                    .Include(p => p.TastingNotes)
                                    .FirstOrDefaultAsync(brew => brew.ID == id);
        }

        public async Task<Brew> Update(Brew brew)
        {
            _context.Brews.Update(brew);
            await _context.SaveChangesAsync();

            return await _context.Brews
                                    .Include(p => p.Recipe)
                                    .Include(p => p.Recipe.WaterProfile)
                                    .Include(p => p.Recipe.Steps).ThenInclude(w => w.Ingredients)
                                    .Include(p => p.Recipe.Steps).ThenInclude(w => w.Timer)
                                    .Include(p => p.TastingNotes)
                                    .FirstOrDefaultAsync(brewToFind => brewToFind.ID == brew.ID);
        }

        public void Remove(Brew brew)
        {
            _context.Brews.Remove(brew);
        }
    }
}
