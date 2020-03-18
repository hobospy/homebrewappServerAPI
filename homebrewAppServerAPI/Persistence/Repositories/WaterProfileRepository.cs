using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class WaterProfileRepository : BaseRepository, IWaterProfileRepository
    {
        public WaterProfileRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<WaterProfile>> ListAsync()
        {
            return await _context.WaterProfiles.ToListAsync();
        }

        public async Task AddAsync(WaterProfile waterProfile)
        {
            await _context.WaterProfiles.AddAsync(waterProfile);
        }

        public async Task<WaterProfile> FindByIdAsync(int id)
        {
            return await _context.WaterProfiles.FirstOrDefaultAsync(waterProfile => waterProfile.ID == id);
        }

        public void Update(WaterProfile waterProfile)
        {
            _context.WaterProfiles.Update(waterProfile);
        }

        public void Remove(WaterProfile waterProfile)
        {
            _context.WaterProfiles.Remove(waterProfile);
        }
    }
}
