using System.Collections.Concurrent;
using Domain.Contracts;
using Domain.Models;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext dbContext;
        private ConcurrentDictionary<string, object> Repositories;

        public UnitOfWork(StoreDbContext _dbContext)
        {
            Repositories = new ConcurrentDictionary<string, object>();
            dbContext = _dbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
            => (IGenericRepository<TEntity, TKey>)Repositories.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepository<TEntity, TKey>(dbContext));

        public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();
    }
}
