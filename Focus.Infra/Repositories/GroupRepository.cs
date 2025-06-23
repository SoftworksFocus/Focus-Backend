using Focus.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class GroupRepository : IRepository<Group>
{
    private readonly FocusDbContext _context;

    public GroupRepository(FocusDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> GetByIdAsync(int id)
    {
        var group =  await _context.Groups.FindAsync(id);
        return group;
    }

    public async Task<IEnumerable<Group>?> GetAllAsync()
    {
        var groups = await _context.Groups.ToListAsync();
        return groups;
    }

    public async Task<bool> AddAsync(Group group)
    {
        await _context.Groups.AddAsync(group);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task UpdateAsync(int id, Group newGroup)
    {
        var groupToUpdate = await _context.Groups.FindAsync(id);
        if (groupToUpdate == null)
        {
            throw new KeyNotFoundException($"Group with id {id} not found.");
        }

        groupToUpdate.Name = newGroup.Name;
        groupToUpdate.Description = newGroup.Description;
        groupToUpdate.UpdatedAt = newGroup.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with id {id} not found.");
        }
        _context.Remove(group);
        return await _context.SaveChangesAsync() > 0;
    }
}