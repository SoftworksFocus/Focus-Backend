namespace Focus.Application.Services.Interfaces;

public interface IService <T> where T : class
// Todo: refactor Iservice to user 3 generic types, this will allow us to use the same interface for different entities
{
    Task<T?> GetById(int id);
    Task<IEnumerable<T>?> GetAll();
    Task Add(T entity);
    Task Update(int id, T entity);
    Task Delete(int id);
}