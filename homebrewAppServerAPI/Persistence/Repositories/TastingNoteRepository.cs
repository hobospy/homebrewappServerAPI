using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Persistence.Repositories
{
    public class TastingNoteRepository : BaseRepository, ITastingNoteRepository
    {
#if USE_SQLITE
        public TastingNoteRepository(SqliteDbContext context) : base(context) { }
#else
        public TastingNoteRepository(AppDbContext context) : base(context) { }
#endif

        public async Task<TastingNote> AddAsync(TastingNote tastingNote)
        {
            _context.TastingNotes.Add(tastingNote);
            _context.SaveChanges();

            await _context.Entry(tastingNote).GetDatabaseValuesAsync();

            return tastingNote;
        }

        public async Task<TastingNote> FindByIdAsync(int id)
        {
            return await _context.TastingNotes
                                    .FirstOrDefaultAsync(tastingNote => tastingNote.ID == id);
        }

        public async Task<TastingNote> Update(TastingNote tastingNote)
        {
            _context.TastingNotes.Update(tastingNote);
            await _context.SaveChangesAsync();

            return await _context.TastingNotes
                                    .FirstOrDefaultAsync(tastingNoteToFind => tastingNoteToFind.ID == tastingNote.ID);
        }

        public void Remove(TastingNote tastingNote)
        {
            _context.TastingNotes.Remove(tastingNote);
        }
    }
}
