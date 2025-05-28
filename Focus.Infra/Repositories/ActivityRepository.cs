using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class ActivityRepository : IRepository<Activity>
{
    private readonly FocusDbContext _context;

    public ActivityRepository(FocusDbContext context)
    {
        _context = context;
    }

    public async Task<Activity> GetByIdAsync(int id)
    {
        var  activity = await _context.Activities.FindAsync(id);
        return activity;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync()
    {
        var  activities = await _context.Activities.ToListAsync();
        return activities;
    }
    
    public async Task AddAsync(Activity entity)
    {
        await _context.Activities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(int id, Activity entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Activity entity)
    {
        var existinActivity = await _context.Activities.FindAsync(entity.Id);
        
        _context.Activities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await  _context.Activities.FindAsync(id);

        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
}