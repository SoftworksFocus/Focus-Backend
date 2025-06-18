using Focus.Infra.Repositories.Interfaces;
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

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync() =>   
         await _context.SaveChangesAsync() > 0;
    
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }
}