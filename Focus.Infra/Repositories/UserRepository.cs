using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class UserRepository : IRepository<User>
{
    private readonly FocusDbContext _context;
    public UserRepository(FocusDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        
        if (users == null || !users.Any())
        {
            throw new KeyNotFoundException("No users found.");
        }
        
        return users;
    }

    public async Task AddAsync(User entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "User cannot be null.");
        }
        
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "User cannot be null.");
        }
        
        var existingUser = await _context.Users.FindAsync(entity.Id);
        
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {entity.Id} not found.");
        }
        
        _context.Entry(existingUser).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}