using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class UserGroupRepository : IRepository<UserGroup>
{
    private readonly FocusDbContext _context;
    public UserGroupRepository(FocusDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserGroup?> GetByIdAsync(int id)
    {
        return await _context.UserGroups.FindAsync(id);
    }

    public async Task<IEnumerable<UserGroup>?> GetAllAsync()
    {
        return await _context.UserGroups.ToListAsync();
    }

    public async Task AddAsync(UserGroup entity)
    {
        
        await _context.UserGroups.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, UserGroup entity)
    {
        
        var existingUserGroup = await _context.UserGroups.FindAsync(entity.Id);
        
        if (existingUserGroup == null)
        {
            throw new KeyNotFoundException($"UserGroup with id {id} not found.");
        }
        
        _context.Entry(existingUserGroup).CurrentValues.SetValues(entity);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.UserGroups.FindAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"UserGroup with id {id} not found.");
        }
        
        _context.UserGroups.Remove(user);
        await _context.SaveChangesAsync();
    }
}