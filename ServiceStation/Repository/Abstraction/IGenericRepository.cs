using System.Linq.Expressions;
using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Repository.Abstraction;

public interface IGenericRepository<TEntity> where TEntity : AbstractEntity
{
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

    Task<TEntity?> GetByIdAsync(object id);

    Task CreateAsync(TEntity entity);

    Task DeleteByIdAsync(object id);

    void Delete(TEntity entityToDelete);
    
    void DeleteRange(IEnumerable<TEntity> entitiesToDelete);

    Task UpdateAsync(TEntity entityToUpdate);
}