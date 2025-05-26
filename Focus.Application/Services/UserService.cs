using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;

    UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        return user;
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await  _userRepository.GetAllAsync();
        
        if (users == null || !users.Any())
        {
            throw new KeyNotFoundException("No users found.");
        }
        
        
        return users;
    }

    public async Task<User> AddAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        await _userRepository.AddAsync(user);
        
        return user;
    }

    public async Task<User> UpdateAsync(int id, User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id);
        
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }
        
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {user.Id} not found.");
        }

        await _userRepository.UpdateAsync(id, user);

        return user;

    }

    public async Task<User> DeleteAsync(int id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        
        await _userRepository.DeleteAsync(existingUser.Id);
        return existingUser;
    }
    
    
}