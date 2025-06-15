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

    public async Task<IEnumerable<Group>> GetAllGroups(int userId)
    {
        if(await _context.Users.FindAsync(userId) == null)
        {
            throw new KeyNotFoundException($"User not found.");
        }
        var userGroups = await _context.UserGroups
            .Include(ug => ug.Group)
            .Where(ug => ug.UserId == userId)
            .ToListAsync();

        var groups = userGroups.Select(ug => ug.Group).ToList();
        return groups;
    }
    
    public async Task MakeRelationship(UserGroup relationship)
    {
        await _context.UserGroups.AddAsync(relationship);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId, int groupId)
    {
        var relationship = await _context.UserGroups.FindAsync(userId, groupId);
        if (relationship == null)
        {
            throw new KeyNotFoundException($"User not in group.");
        }
        _context.UserGroups.Remove(relationship);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>?> GetAllMembersAsync(int groupId)
    {
        if (await _context.Groups.FindAsync(groupId) == null)
        {
            throw new KeyNotFoundException($"Group not found.");
        }
        
        var groupMembers = await _context.UserGroups
            .Include(ug => ug.User)
            .Where(ug => ug.GroupId == groupId)
            .ToListAsync();
        var users = groupMembers.Select(ug => ug.User).ToList();
        return users;
    }

}