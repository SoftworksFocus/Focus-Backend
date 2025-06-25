using Focus.Domain.Specifications;

namespace Focus.Infra.Repositories.Interfaces;

public interface IRepository <T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> ListAsync(ISpecification<T> spec);
    Task<bool> AddAsync(T entity);
    Task UpdateAsync(int id,  T entity);
    Task<bool> DeleteAsync(int id);
    Task<T?> GetFirstOrDefaultAsync(ISpecification<T> spec);
}