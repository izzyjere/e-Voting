using ICTAZEVoting.Shared.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IUnitOfWork<TKey> : IDisposable
    {
        IRepository<T, TKey> Repository<T>() where T : class, IEntity<TKey>;
        Task<int> Commit(CancellationToken cancellationToken);
        Task RollBack();
    }
}
