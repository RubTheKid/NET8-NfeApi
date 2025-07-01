namespace Nfe.Domain.Contracts.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task<bool> Exists(Guid id);

    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task Delete(Guid id);
    Task<int> SaveChangesAsync();
}