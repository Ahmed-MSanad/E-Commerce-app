using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(StoreDbContext _dbContext) {
            dbContext = _dbContext;
            dbSet = dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool isTrackable = false) => isTrackable ? await dbSet.ToListAsync() : await dbSet.AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetAsync(TKey id) => await dbSet.FindAsync(id);

        public async Task AddAsync(TEntity entity) => await dbSet.AddAsync(entity);

        public void Delete(TEntity entity) => dbSet.Remove(entity);

        public void Update(TEntity entity) => dbSet.Update(entity);

        public async Task<IEnumerable<TEntity?>> GetAllAsync(Specification<TEntity> specification) 
            => await ApplySpecification(specification).ToListAsync();
        public async Task<TEntity?> GetAsync(Specification<TEntity> specification) 
            => await ApplySpecification(specification).FirstOrDefaultAsync();
        private IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification) 
            => SpecificationEvaluator.GetQuery<TEntity>(dbContext.Set<TEntity>(), specification);
    }
}