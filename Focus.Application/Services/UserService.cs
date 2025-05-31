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
        await _userRepository.AddAsync(user);
    }

    public async Task Update(int id, UpdateUserDto newUserDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            throw new ArgumentNullException(nameof(newUserDto), "User cannot be null.");
        }
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found on service.");
        }
        var newUser = newUserDto.ToUser(id);
        await _userRepository.UpdateAsync(id, newUser);

    }

    public async Task Delete(int id)
    {
        await _userRepository.DeleteAsync(id);
    }
    
}