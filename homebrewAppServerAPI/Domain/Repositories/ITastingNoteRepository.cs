using homebrewAppServerAPI.Domain.Models;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface ITastingNoteRepository
    {
        Task<TastingNote> AddAsync(TastingNote tastingNote);
        Task<TastingNote> FindByIdAsync(int id);
        Task<TastingNote> Update(TastingNote tastingNote);
        void Remove(TastingNote tastingNote);
    }
}
