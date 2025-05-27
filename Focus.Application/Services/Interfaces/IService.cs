namespace Focus.Application.Services.Interfaces;

public interface IService <T> where T : class
{
    Task<T?> GetById(int id);
    Task<IEnumerable<T>?> GetAll();
    Task Add(T entity);
    Task Update(int id, T entity);
    Task Delete(int id);
}