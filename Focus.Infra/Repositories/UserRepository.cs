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
        
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        
        return users;
    }

    public async Task AddAsync(User entity)
    {
        
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, User entity)
    {
        
        var existingUser = await _context.Users.FindAsync(entity.Id);
        
        _context.Entry(existingUser).CurrentValues.SetValues(entity);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}