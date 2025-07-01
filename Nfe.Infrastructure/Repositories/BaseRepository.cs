using Microsoft.EntityFrameworkCore;
using Nfe.Infrastructure.Data;
using Nfe.Domain.Contracts.Repositories;

namespace Nfe.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly NfeDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(NfeDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }
    public virtual async Task<T> Update(T entity)
    {
        _dbSet.Update(entity);
        await SaveChangesAsync();
        return entity;
    }

    public virtual async Task Delete(Guid id)
    {
        var entity = await GetById(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
    }

    public virtual async Task<bool> Exists(Guid id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
