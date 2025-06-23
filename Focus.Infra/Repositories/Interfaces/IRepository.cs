namespace Focus.Infra.Repositories.Interfaces;

public interface IRepository <T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>?> GetAllAsync();
    Task<bool> AddAsync(T entity);
    Task UpdateAsync(int id,  T entity);
    Task<bool> DeleteAsync(int id);
    
}