using Focus.Domain.Specifications;

namespace Focus.Infra.Repositories.Interfaces;

public interface IRepository <T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> ListAsync(ISpecification<T> spec);
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync();
    Task<bool> DeleteAsync(int id);
    
}