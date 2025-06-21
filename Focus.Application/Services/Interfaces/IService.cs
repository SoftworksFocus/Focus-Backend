using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IService <TEntity, TEntityGetDto, TCreateEntityDto, TUpdateEntityDto> where TEntity : class
                                    where TEntityGetDto : class
                                    where TCreateEntityDto : class
                                    where TUpdateEntityDto : class

{
    Task<TEntityGetDto?> GetById(int id);
    Task<List<TEntityGetDto>?> GetAllAsync(ISpecification<TEntity>? filterSpec = null);
    Task Add(TCreateEntityDto entity);
    Task Update(int id, TUpdateEntityDto entity);
    Task Delete(int id);
}