using Focus.Application.DTO.Activity;
using Focus.Application.DTO.Common;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Application.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly IUserRepository _userRepository;

    public ActivityService(IActivityRepository activityRepository, IMediaRepository mediaRepository, IUserRepository userRepository)
    {
        _activityRepository = activityRepository;
        _mediaRepository = mediaRepository;
        _userRepository = userRepository;
    }

    public async Task<GetActivityDto?> GetById(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        if (activity == null)
        {
            throw new KeyNotFoundException($"Activity with {id} not found");
        }

        return GetActivityDto.FromActivity(activity);
    }

    public async Task<PagedResultDto<GetActivityDto>> GetAllAsync(ISpecification<Activity> filterSpec, int pageNumber,
        int pageSize)
    {
        var totalCount = await _activityRepository.CountAsync(filterSpec);
        var activities = await _activityRepository.ListAsync(filterSpec, pageNumber, pageSize);
        var returnActivities = activities.Select(GetActivityDto.FromActivity).ToList();
        return new PagedResultDto<GetActivityDto>(returnActivities, totalCount, pageNumber, pageSize);
    }

    public Task Add(CreateActivityDto activityDto)
    {
        throw new NotImplementedException();
    }

    public async Task<GetActivityDto> AddAsync(CreateActivityDto activityDto)
    {
        if (activityDto == null)
        {
            throw new ArgumentNullException(nameof(activityDto), "Activity cannot be null.");
        }

        var activityEntity = activityDto.ToActivity(activityDto.UserId, activityDto.GroupId);
        var addedActivity = await _activityRepository.AddActivityAsync(activityEntity);
        if (addedActivity == null)
        {
            throw new Exception("Failed to add activity.");
        }
        return GetActivityDto.FromActivity(addedActivity);
    }

    public async Task Update(int id, UpdateActivityDto newActivityDto)
    {
        var activityToUpdate = await _activityRepository.GetByIdAsync(id);

        if (activityToUpdate == null)
        {
            throw new KeyNotFoundException($"Activity with ID {id} not found.");
        }

        newActivityDto.MapTo(activityToUpdate);

        await _activityRepository.UpdateAsync(id, activityToUpdate);
    }
    
    public async Task UpdateAsync(int activityId, int requesterId,  UpdateActivityDto newActivityDto)
    {
        if (!await IsSameUser(requesterId, activityId))
        {
            throw new UnauthorizedAccessException("Only owner can update this activity.");
        }
        var activityToUpdate = await _activityRepository.GetByIdAsync(activityId);
        if (activityToUpdate == null)
        {
            throw new KeyNotFoundException($"Activity not found.");
        }
        newActivityDto.MapTo(activityToUpdate);
        await _activityRepository.UpdateAsync(activityId, activityToUpdate);
    }

    public async Task Delete(int id)
    {
        if (!await _activityRepository.DeleteAsync(id))
        {
            throw new KeyNotFoundException($"Activity not found.");
        }
    }
    
    public async Task DeleteAsync(int activityId, int requesterId)
    {
        var activityToDelete = await _activityRepository.GetByIdAsync(activityId);
        if (activityToDelete != null)
        {
            if (!await IsSameUser(requesterId, activityToDelete.UserId))
            {
                throw new UnauthorizedAccessException("Only owner can delete this activity.");
            }
            if (!await _activityRepository.DeleteAsync(activityId))
            {
                throw new KeyNotFoundException($"Activity not found.");
            }
        }
        else
        {
            throw new KeyNotFoundException($"Activity not found.");
        }
    }

    private async Task<bool> IsSameUser(int requesterId, int userId)
    {
        var existingUser = await _userRepository.GetByIdAsync(userId);
        return existingUser != null && existingUser.Id == requesterId;
    }

    public async Task UpdateMedia(int activityId, int requesterId, string mediaUrl, string? caption)
    {
        var activity = await _activityRepository.GetByIdAsync(activityId);
        if (activity == null)
        {
            throw new KeyNotFoundException("Activity not found.");
        }
        if (!await IsSameUser(requesterId, activity.UserId))
        {
            throw new UnauthorizedAccessException("Only owner can update media for this activity.");
        }
        var newMedia = new Media
        {
            Url = mediaUrl,
            Caption = caption,
            ActivityId = activityId,
            DisplayOrder = (activity.Media?.Count ?? 0) + 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _mediaRepository.AddAsync(newMedia);
        await _activityRepository.UpdateAsync(activityId, activity);
    }
}