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

    public ActivityService(IActivityRepository activityRepository, IMediaRepository mediaRepository)
    {
        _activityRepository = activityRepository;
        _mediaRepository = mediaRepository;
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

    public async Task Add(CreateActivityDto activityDto)
    {
        if (activityDto == null)
        {
            throw new ArgumentNullException(nameof(activityDto), "Activity cannot be null.");
        }

        var activityEntity = activityDto.ToActivity(activityDto.UserId, activityDto.GroupId);
        if (!await _activityRepository.AddAsync(activityEntity))
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

        newActivityDto.MapTo(activityToUpdate);

        await _activityRepository.UpdateAsync(id, activityToUpdate);
    }

    public async Task Delete(int id)
    {
        if (!await _activityRepository.DeleteAsync(id))
        {
            throw new KeyNotFoundException($"Activity with ID {id} not found.");
        }
    }

    public async Task UpdateMedia(int activityId, string mediaUrl, string? caption)
    {
        var activity = await _activityRepository.GetByIdAsync(activityId);
        if (activity == null)
        {
            throw new KeyNotFoundException($"Activity not found.");
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