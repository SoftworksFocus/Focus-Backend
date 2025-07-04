using Focus.Application.DTO.Common;
using Focus.Application.DTO.User;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IUserService
{
    Task<GetUserDto?> GetById(int id);
    Task<PagedResultDto<GetUserDto>> GetAllAsync(ISpecification<User> filterSpec, int pageNumber, int pageSize);
    Task Add(CreateUserDto entity);
    Task Update(int id, UpdateUserDto entity);
    Task UpdateAsync(int userId, int requesterId, UpdateUserDto newUserDto);
    Task Delete(int userId);
    Task DeleteAsync(int userId, int requesterId);
    
    Task UpdateProfilePicture(int userId, int requesterId, string mediaUrl);

}