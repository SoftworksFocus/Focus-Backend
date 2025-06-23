using Focus.Application.DTO.Activity;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using Focus.Infra.Repositories;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Application.Services;

public class ActivityService : IActivityService {
    private readonly IActivityRepository _activityRepository;

    public ActivityService(IActivityRepository activityRepository)
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

    public async Task<List<GetActivityDto>?> GetAllAsync(ISpecification<Activity>? filterSpec = null)
    {
        var activities = await _activityRepository.ListAsync(filterSpec);
        var returnActivities = activities.Select(GetActivityDto.FromActivity).ToList();
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