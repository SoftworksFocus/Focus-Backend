using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class ActivityService
{
    private readonly ActivityRepository _activityRepository;

    ActivityService(ActivityRepository activityRepository)
    {
        activityRepository = activityRepository;
    }

    public async Task<Activity> GetAsyncById(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        if (activity == null)
        {
            throw new KeyNotFoundException($"Activity with {id} not found");
        }
        
        return activity;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync()
    {
        var activities = await _activityRepository.GetAllAsync();
        
        if (activities == null || !activities.Any())
        {
            throw new KeyNotFoundException("No activities found");
        }
        
        return activities;
    }

    public async Task<Activity> AddAsync(Activity activity)
    {
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        
        await _activityRepository.AddAsync(activity);
        
        return activity;
    }

    public async Task<Activity> UpdateAsync(Activity activity)
    {
        
        var existingActivity = await _activityRepository.GetByIdAsync(activity.Id);
        
        if (existingActivity == null)
        {
            throw new ArgumentNullException(nameof(existingActivity), "Activity cannot be null.");
        }
        
        if (existingActivity == null)
        {
            throw new KeyNotFoundException($"Activity with {existingActivity.Id} not found.");
        }
        
        await _activityRepository.UpdateAsync(existingActivity);
        return existingActivity;
    }

    public async Task<Activity> DeleteAsync(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        
        await _activityRepository.DeleteAsync(id);
        return activity;
    }
}