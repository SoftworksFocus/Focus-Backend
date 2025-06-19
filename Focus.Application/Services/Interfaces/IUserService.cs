using Focus.Application.DTO.User;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IUserService
{
    Task<GetUserDto?> GetById(int id);
    Task<List<GetUserDto>?> GetAllAsync(ISpecification<User>? filterSpec = null);
    Task Add(CreateUserDto createUserDto);
    Task Update(int id, UpdateUserDto newUserDto);
    Task Delete(int id);
}