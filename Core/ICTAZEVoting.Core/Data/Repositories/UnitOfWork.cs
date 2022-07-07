using ICTAZEvoting.Shared.Interfaces;
using static System.Console;
using System.Collections;
using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEvoting.Shared.Contracts;

namespace ICTAZEVoting.Core.Data.Repositories
{
    public class UnitOfWork<TKey> : IUnitOfWork<TKey>
    {

        bool disposed;
        readonly SystemDbContext context;
        Hashtable repositories;
        public UnitOfWork(SystemDbContext _context)
        {
            context = _context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            try
            {
                return await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                WriteLine(e.Message + e.StackTrace);
                return 0;
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T, TKey> Repository<T>() where T : class, IEntity<TKey>
        {
            if (repositories == null)
                repositories = new Hashtable();
            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repoType = typeof(Repository<,>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T), typeof(TKey)), context);
                repositories.Add(type, repoInstance);
            }
            return (IRepository<T, TKey>)repositories[type];
        }

        public Task RollBack()
        {
            context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    context.Dispose();
                }
            }
            //disposed unmanaged resources
            disposed = true;
        }
    }

}
