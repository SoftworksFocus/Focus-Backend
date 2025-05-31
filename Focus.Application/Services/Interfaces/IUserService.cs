using Focus.Application.DTO.User;

namespace Focus.Application.Services.Interfaces;

public interface IUserService
{
    Task<GetUserDto> GetById(int id);
    Task<IEnumerable<GetUserDto>?> GetAll();
    Task Add(CreateUserDto createUserDto);
    Task Update(int id, UpdateUserDto newUserDto);
    Task Delete(int id);
}