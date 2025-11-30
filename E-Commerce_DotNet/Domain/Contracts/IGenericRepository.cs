using Domain.Models;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool isTrackable = false);
        Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> specification);
        Task<TEntity?> GetAsync(TKey id);
        Task<TEntity?> GetAsync(Specification<TEntity> specification);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> CountAsync(Specification<TEntity> specification);
    }
}
