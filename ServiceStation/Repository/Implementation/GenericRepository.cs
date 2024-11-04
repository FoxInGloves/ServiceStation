using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceStation.Models.Entities.Abstraction;
using ServiceStation.Repository.Abstraction;

namespace ServiceStation.Repository.Implementation;

public class GenericRepository<TEntity>(ApplicationDatabaseContext context) : IGenericRepository<TEntity> 
    where TEntity : AbstractEntity
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task DeleteAsync(object id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete == null) return;
        await DeleteAsync(entityToDelete);
    }

    public async Task DeleteAsync(TEntity entityToDelete)
    {
        await Task.Run(() =>
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        });
    }

    public async Task UpdateAsync(TEntity entityToUpdate)
    {
        await Task.Run(() =>
        {
            _dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        });
    }
}