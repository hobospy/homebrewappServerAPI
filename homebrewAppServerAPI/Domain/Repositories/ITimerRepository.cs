using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface ITimerRepository
    {
        Task<IEnumerable<Timer>> ListAsync();
        Task<Timer> AddAsync(Timer timer);
        Task<Timer> FindByIdAsync(int id);
        void Update(Timer timer);
        void Remove(Timer timer);
    }
}
