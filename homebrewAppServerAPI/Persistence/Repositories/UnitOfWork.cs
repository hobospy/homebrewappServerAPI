using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Persistence.Contexts;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
#if USE_SQLITE
        private readonly SqliteDbContext _context;

        public UnitOfWork(SqliteDbContext context)
        {
            _context = context;
        }
#else
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
#endif

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
