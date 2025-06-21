using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using Focus.Infra.Repositories;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<GetUserDto>?> GetAllAsync(ISpecification<User>? filterSpec = null)
    {
        var users = await _userRepository.ListAsync(filterSpec);
        var returnUsers = users.Select(GetUserDto.FromUser).ToList();
        return returnUsers;
    }
    
    public async Task<GetUserDto?> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        var returnUser = GetUserDto.FromUser(user);
        return returnUser;
    }

    public async Task Add(CreateUserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto), "User cannot be null.");
        }
        var user = userDto.ToUser();
        if (!await _userRepository.AddAsync(user))
        {
            throw new Exception("Failed to add user.");
        }
    }

    public async Task Update(int id, UpdateUserDto newUserDto)
    {
        var updateUser = await _userRepository.GetByIdAsync(id);
        if (newUserDto == null)
        {
            throw new ArgumentNullException(nameof(newUserDto), "User cannot be null.");
        }
        if (updateUser == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found on service.");
        }
        newUserDto.MapTo(updateUser);
        if (!await _userRepository.UpdateAsync())
        {
            throw new Exception($"Failed to update user with id {id}.");
        }
            
    }

    public async Task Delete(int id)
    {
        if (!await _userRepository.DeleteAsync(id))
        {
            throw new Exception($"Failed to delete user with id {id}.");
        }
    }
    
}