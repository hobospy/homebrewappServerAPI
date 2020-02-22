using System.Threading.Tasks;

namespace homebrewAppServerAPI.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
