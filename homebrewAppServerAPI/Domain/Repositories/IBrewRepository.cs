using homebrewAppServerAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface IBrewRepository
    {
        Task<IEnumerable<Brew>> ListAsync();
        Task<Brew> AddAsync(Brew brew);
        Task<Brew> FindByIdAsync(int id);
        Task<Brew> Update(Brew brew);
        void Remove(Brew brew);
    }
}
