using Focus.Infra.Repositories.Interfaces;
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

    public async Task<Activity?> GetByIdAsync(int id)
    {
        var  activity = await _context.Activities
            .Include( a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
        return activity;
    }

    public async Task<IEnumerable<Activity>?> GetAllAsync()
    {
        var  activities = await _context.Activities
            .Include(u => u.User)
            .ToListAsync();
        return activities;
    }
    
    public async Task<bool> AddAsync(Activity activity)
    {
        await _context.Activities.AddAsync(activity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync() =>   
        await _context.SaveChangesAsync() > 0;

    public async Task<bool> DeleteAsync(int id)
    {
        var activityToDelete = await  _context.Activities.FindAsync(id);
        if (activityToDelete == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        _context.Remove(activityToDelete);
        return await _context.SaveChangesAsync() > 0;
    }
}