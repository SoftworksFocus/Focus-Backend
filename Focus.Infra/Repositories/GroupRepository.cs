using Focus.Domain.Specifications;
using Focus.Infra.Common;
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

    public Task<List<Group>> ListAsync(ISpecification<Group>? spec)
    {
        return ApplySpecification(spec).ToListAsync();
    }

    public async Task<bool> AddAsync(Group group)
    {
        await _context.Groups.AddAsync(group);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync() =>   
        await _context.SaveChangesAsync() > 0;

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
    
    private IQueryable<Group> ApplySpecification(ISpecification<Group>? spec)
    {
        return SpecificationEvaluator<Group>.GetQuery(_context.Groups.AsQueryable(), spec);
    }
}