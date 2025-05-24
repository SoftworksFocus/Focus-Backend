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

    public async Task<Group> GetByIdAsync(int id)
    {
        var group =  await _context.Groups.FindAsync(id);

        if (group == null)
        {
            throw new KeyNotFoundException($"Group with {id} not found.");
        }

        return group;
    }

    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        var groups =   await _context.Groups.ToListAsync();

        if (groups == null || !groups.Any())
        {
            throw new KeyNotFoundException($"Groups not found.");
        }
        
        return groups;
    }

    public async Task AddAsync(Group entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Group cannot be null.");
        }
        
        await _context.Groups.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Group entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Group cannot be null.");
        }
        
        var existinGroup = await _context.Groups.FindAsync(entity.Id);
        
        if (existinGroup == null)
        {
            throw new KeyNotFoundException($"Group with {entity.Id} not found.");
        }
        
        _context.Groups.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await  _context.Groups.FindAsync(id);
        
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Group cannot be null.");
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
}