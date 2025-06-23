using Focus.Application.DTO.Activity;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class ActivityService : IActivityService
{
    private readonly ActivityRepository _activityRepository;

    public ActivityService(ActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<GetActivityDto?> GetById(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        if (activity == null)
        {
            throw new KeyNotFoundException($"Activity with {id} not found");
        }
        var returnActivity = GetActivityDto.FromActivity(activity);
        return returnActivity;
    }

    public async Task<IEnumerable<GetActivityDto>?> GetAll() 
    {
        var activities = await _activityRepository.GetAllAsync();
        if (activities == null || !activities.Any())
        {
            throw new KeyNotFoundException("No activities found");
        }
        var returnActivities = activities.Select(GetActivityDto.FromActivity);
        Console.WriteLine(returnActivities);
        
        return returnActivities;
    }

    public async Task Add(CreateActivityDto activity)
    {
        if (activity == null)
        {
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        
        var activityEntity = activity.ToActivity(activity.UserId, activity.GroupId);
        if(!await _activityRepository.AddAsync(activityEntity))
        {
            throw new Exception("Failed to add activity.");
        }
    }

    public async Task Update(int id, UpdateActivityDto newActivityDto)
    {
        var activityToUpdate = await _activityRepository.GetByIdAsync(id);
    
        if (activityToUpdate == null)
        {
            throw new KeyNotFoundException($"Activity with ID {id} not found.");
        }
    
        var newActivity = newActivityDto.ToActivity();
    
        activityToUpdate.Title = newActivity.Title;
        activityToUpdate.Description = newActivity.Description;
        activityToUpdate.StartDate = newActivity.StartDate;
        activityToUpdate.EndDate = newActivity.EndDate;
        activityToUpdate.Status = newActivity.Status;
    
        await _activityRepository.UpdateAsync(id, activityToUpdate);
    }

    public async Task Delete(int id)
    {
        if(!await _activityRepository.DeleteAsync(id))
        {
            throw new KeyNotFoundException($"Activity with ID {id} not found.");
        }
    }
}