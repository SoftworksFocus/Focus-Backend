using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<GetUserDto>?> GetAll()
    {
        var users = await  _userRepository.GetAllAsync();
        
        if (users == null || !users.Any())
        {
            throw new KeyNotFoundException("No users found.");
        }
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
        user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        if (!await _userRepository.AddAsync(user))
        {
            throw new Exception("Failed to add user.");
        }
    }

    public async Task Update(int id, UpdateUserDto newUserDto)
    {
        if (newUserDto == null)
        {
            throw new ArgumentNullException(nameof(newUserDto), "User DTO cannot be null.");
        }

        var userToUpdate = await _userRepository.GetByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found on service.");
        }

        userToUpdate.Username = newUserDto.Username;
        userToUpdate.Email = newUserDto.Email;
        userToUpdate.Description = newUserDto.Description;

        await _userRepository.UpdateAsync(id, userToUpdate);
    }

    public async Task Delete(int id)
    {
        if (!await _userRepository.DeleteAsync(id))
        {
            throw new Exception($"Failed to delete user with id {id}.");
        }
    }
    
}