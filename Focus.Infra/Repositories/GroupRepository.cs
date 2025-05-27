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
        await _context.Groups.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Group entity)
    {
        var existingGroup = await _context.Groups.FindAsync(entity.Id);
        
        _context.Groups.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await  _context.Groups.FindAsync(id);
        
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
}