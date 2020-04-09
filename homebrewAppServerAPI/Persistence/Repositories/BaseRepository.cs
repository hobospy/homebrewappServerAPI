using homebrewAppServerAPI.Persistence.Contexts;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public abstract class BaseRepository
    {
#if USE_SQLITE
        protected readonly SqliteDbContext _context;

        public BaseRepository(SqliteDbContext context)
        {
            _context = context;
        }
#else
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
#endif
    }
}
