using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class UserService : IService<User>
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>?> GetAll()
    {
        var users = await  _userRepository.GetAllAsync();
        
        if (users == null || !users.Any())
        {
            throw new KeyNotFoundException("No users found.");
        }
        
        return users;
    }
    
    public async Task<User?> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        return user;
    }

    public async Task Add(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        await _userRepository.AddAsync(user);
    }

    public async Task Update(int id, User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }
        
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found on service.");
        }

        await _userRepository.UpdateAsync(id, user);

    }

    public async Task Delete(int id)
    {
        await _userRepository.DeleteAsync(id);
    }
    
}