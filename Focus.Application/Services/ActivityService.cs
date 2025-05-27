using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class ActivityService : IService<Activity>
{
    private readonly ActivityRepository _activityRepository;

    public ActivityService(ActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Activity?> GetById(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        if (activity == null)
        {
            throw new KeyNotFoundException($"Activity with {id} not found");
        }
        
        return activity;
    }

    public async Task<IEnumerable<Activity>?> GetAll() 
    {
        var activities = await _activityRepository.GetAllAsync();
        
        if (activities == null || !activities.Any())
        {
            throw new KeyNotFoundException("No activities found");
        }
        
        return activities;
    }

    public async Task Add(Activity activity)
    {
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        
        await _activityRepository.AddAsync(activity);
    }

    public async Task Update(int id, Activity activity)
    {
        
        var existingActivity = await _activityRepository.GetByIdAsync(id);
        
        if (existingActivity == null)
        {
            throw new ArgumentNullException(nameof(existingActivity), "Activity cannot be null.");
        }
        
        if (existingActivity == null)
        {
            throw new KeyNotFoundException($"Activity with ID {id} not found.");
        }
        
        await _activityRepository.UpdateAsync(existingActivity);
    }

    public async Task Delete(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        
        await _activityRepository.DeleteAsync(id);
    }
}