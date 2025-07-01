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
    Task Delete(int id);
    Task UpdateProfilePicture(int userId, string mediaUrl);
}