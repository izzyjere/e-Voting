using ICTAZEVoting.Shared.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IRepository<T, TKey> where T : class, IEntity<TKey>
    {
        IQueryable<T> Entities(bool eager = true);
        Task<List<T>> GetAll();
        Task<T> Get(TKey id);
        Task<bool> Delete(T entity);
        Task<bool> Update(T entity);
        Task<bool> Add(T entity);

    }
}
