using Focus.Domain.Specifications;
using Focus.Infra.Common;
using Focus.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class GroupRepository : IGroupRepository
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

    public async Task<List<Group>> ListAsync(ISpecification<Group> spec, int pageNumber, int pageSize)
    {
        var query = ApplySpecification(spec);
        return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<Group> spec)
    {
        return await ApplySpecification(spec).CountAsync();
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

    public Task<Group?> GetFirstOrDefaultAsync(ISpecification<Group> spec)
    {
        throw new NotImplementedException();
    }

    private IQueryable<Group> ApplySpecification(ISpecification<Group>? spec)
    {
        return SpecificationEvaluator<Group>.GetQuery(_context.Groups.AsQueryable(), spec);
    }
}