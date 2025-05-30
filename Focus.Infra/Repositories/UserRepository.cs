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
    
    public async Task<IEnumerable<User>?> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User entity)
    {
        
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, User newUser)
    {
        var user = _context.Users.SingleAsync( x => x.Id == id);
        
        user.Result.Username = newUser.Username;
        user.Result.Description = newUser.Description;
        user.Result.Email = newUser.Email;
        user.Result.UpdatedAt = newUser.UpdatedAt;
        
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