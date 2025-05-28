using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class UserGroupRepository
{
    private readonly FocusDbContext _context;
    public UserGroupRepository(FocusDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserGroup?> GetByIdAsync(int userId, int groupId)
    {
        return await _context.UserGroups.FindAsync(userId, groupId);
    }

    public async Task<IEnumerable<UserGroup>?> GetAllAsync()
    {
        return await _context.UserGroups.ToListAsync();
    }

    public async Task AddAsync(UserGroup userGroup)
    {
        await _context.UserGroups.AddAsync(userGroup);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int userId, int groupId, UserGroup newUserGroup)
    {
        var userGroupToUpdate = await _context.UserGroups.FindAsync(newUserGroup.UserId, newUserGroup.GroupId);
        
        if (userGroupToUpdate == null)
        {
            throw new KeyNotFoundException($"UserGroup not found.");
        }
        
        _context.Entry(userGroupToUpdate).CurrentValues.SetValues(newUserGroup);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId, int groupId)
    {
        var user = await _context.UserGroups.FindAsync(userId, groupId);
        if (user == null)
        {
            throw new KeyNotFoundException($"UserGroup not found.");
        }
        _context.UserGroups.Remove(user);
        await _context.SaveChangesAsync();
    }
}