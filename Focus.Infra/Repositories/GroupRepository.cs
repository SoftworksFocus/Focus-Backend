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
        var groups =   await _context.Groups.ToListAsync();
        if (groups == null || groups.Count == 0)
        {
            throw new KeyNotFoundException($"Groups not found.");
        }
        return groups;
    }

    public async Task AddAsync(Group group)
    {
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Group newGroup)
    {
        var groupToUpdate = await _context.Groups.FindAsync(newGroup.Id);
        if (groupToUpdate == null)
        {
            throw new KeyNotFoundException($"Group with id {id} not found.");
        }

        _context.Groups.Update(newGroup);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var group = await  _context.Groups.FindAsync(id);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with id {id} not found.");
        }
        _context.Remove(group);
        await _context.SaveChangesAsync();
    }
}