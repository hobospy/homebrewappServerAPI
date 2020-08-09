using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services.Communication;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Services
{
    public interface ITastingNoteService
    {
        Task<TastingNoteResponse> SaveAsync(TastingNote tastingNote);
        Task<TastingNoteResponse> UpdateAsync(int id, TastingNote tastingNote);
        Task<TastingNoteResponse> PatchAsync(int id, JsonPatchDocument<TastingNote> patch);
        Task<TastingNoteResponse> DeleteAsync(int id);
    }
}
