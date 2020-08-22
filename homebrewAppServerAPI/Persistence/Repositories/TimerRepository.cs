using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class TimerRepository : BaseRepository, ITimerRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

#if USE_SQLITE
        public TimerRepository(SqliteDbContext context) : base(context) { }
#else
        public IngredientRepository(AppDbContext context) : base(context) { }
#endif
        public async Task<Timer> AddAsync(Timer timer)
        {
            //TODO: Think this should be moved to the service layer
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            if (timer != null)
            {
                var newTimer = new Timer();
                newTimer.Duration = timer.Duration;
                newTimer.RecipeStepID = timer.RecipeStepID;
                newTimer.Type = timer.Type;

                log.Debug($"Adding {timer.Duration} timer basic properties");

                _context.Timers.Add(newTimer);
                _context.SaveChanges();
                await _context.Entry(newTimer).GetDatabaseValuesAsync();
                _context.Entry<Timer>(newTimer).State = EntityState.Detached;

                return (newTimer);
            }
            return (null);
        }

        public async Task<Timer> FindByIdAsync(int id)
        {
            return await _context.Timers.FirstOrDefaultAsync(timer => timer.ID == id);
        }

        public async Task<IEnumerable<Timer>> ListAsync()
        {
            return await _context.Timers.ToListAsync();
        }

        public void Remove(Timer timer)
        {
            _context.Timers.Remove(timer);
        }

        public void Update(Timer timer)
        {
            _context.Timers.Update(timer);
        }
    }
}
