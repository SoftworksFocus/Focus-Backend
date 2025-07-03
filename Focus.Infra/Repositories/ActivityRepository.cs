using Focus.Domain.Specifications;
using Focus.Infra.Common;
using Focus.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class ActivityRepository : IActivityRepository
{
    private readonly FocusDbContext _context;

    public ActivityRepository(FocusDbContext context)
    {
        _context = context;
    }

    public async Task<Activity?> GetByIdAsync(int activityId)
    {
        var  activity = await _context.Activities
            .Include( a => a.User)
            .Include( a => a.Group)
            .Include( a => a.Media)
            .FirstOrDefaultAsync(a => a.Id == activityId);
        return activity;
    }

    public async Task<List<Activity>> ListAsync(ISpecification<Activity> spec, int pageNumber, int pageSize)
    {
        var query = ApplySpecification(spec);
        return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    
    public async Task<List<Activity>> ListAsync(ISpecification<Activity> spec, int take)
    {
        var query = ApplySpecification(spec);
        return await query.Take(take).ToListAsync();
    }

    public async Task<int> CountAsync(ISpecification<Activity> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    public Task<bool> AddAsync(Activity entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Activity?> AddActivityAsync(Activity activity)
    {
        var activityadded = await _context.Activities.AddAsync(activity);
        await _context.SaveChangesAsync();
        var newActivity = await GetByIdAsync(activityadded.Entity.Id);
        return newActivity;
    }

    public async Task UpdateAsync(int id, Activity newActivity)
    {
        var activityToUpdate = await _context.Activities.FindAsync(id);
    
        if (activityToUpdate == null)
        {
            throw new KeyNotFoundException($"Activity with id {id} not found.");
        }
    
        activityToUpdate.Title = newActivity.Title;
        activityToUpdate.Description = newActivity.Description;
        activityToUpdate.StartDate = newActivity.StartDate;
        activityToUpdate.EndDate = newActivity.EndDate;
        activityToUpdate.Status = newActivity.Status;
        activityToUpdate.UpdatedAt = newActivity.UpdatedAt;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int activityId)
    {
        var activityToDelete = await  _context.Activities.FindAsync(activityId);
        if (activityToDelete == null)
        {
            throw new KeyNotFoundException($"User not found.");
        }
        _context.Remove(activityToDelete);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<Activity?> GetFirstOrDefaultAsync(ISpecification<Activity> spec)
    {
        throw new NotImplementedException();
    }

    private IQueryable<Activity> ApplySpecification(ISpecification<Activity>? spec)
    {
        return SpecificationEvaluator<Activity>.GetQuery(_context.Activities.AsQueryable(), spec);
    }
}