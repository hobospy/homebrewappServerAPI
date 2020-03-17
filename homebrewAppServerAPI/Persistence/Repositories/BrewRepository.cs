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
        public BrewRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Brew>> ListAsync()
        {
            return await _context.Brews.Include(p => p.Recipe).ToListAsync();
        }

        public async Task AddAsync(Brew brew)
        {
            await _context.Brews.AddAsync(brew);
        }

        public async Task<Brew> FindByIdAsync(int id)
        {
            return await _context.Brews.Include(p => p.Recipe).FirstOrDefaultAsync(brew => brew.ID == id);
        }

        public void Update(Brew brew)
        {
            _context.Brews.Update(brew);
        }

        public void Remove(Brew brew)
        {
            _context.Brews.Remove(brew);
        }
    }
}
