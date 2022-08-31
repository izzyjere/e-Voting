
using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Interfaces;

using ICTAZEVoting.Core.Data.Contexts;

using Microsoft.EntityFrameworkCore;

using System.Reflection;

namespace ICTAZEVoting.Core.Data.Repositories
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class, IEntity<TKey>
    {
        readonly SystemDbContext context;
        public Repository(SystemDbContext _context)
        {
            context = _context;
        }
        public Task<bool> Add(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                return Task.FromResult(true);
            }
            catch
            {

                return Task.FromResult(false);
            }
        }

        public Task<bool> Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
                return Task.FromResult(true);
            }
            catch 
            {

                return Task.FromResult(false);
            }
           
        }

        public IQueryable<T> Entities(bool eager = true)
        {
            var query = context.Set<T>().AsQueryable();
            var navigationProperties = new List<PropertyInfo>();
            if (eager)
            {
                context.Model.GetEntityTypes().Select(entity => entity.GetNavigations()).ToList().ForEach(navigations =>
                {
                    navigations.ToList().ForEach(navigationProperty =>
                    {
                        navigationProperties.AddRange(typeof(T).GetProperties().Where(x => x.PropertyType == navigationProperty.PropertyInfo.PropertyType).ToList());
                    });
                });
                foreach (var navigationProperty in navigationProperties)
                {
                    query = query.Include(navigationProperty.Name);
                }
            }

            return query;
        }

        public async Task<T> Get(TKey id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public Task<bool> Update(T entity)
        {
            try
            {
                context.Set<T>().Update(entity);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
