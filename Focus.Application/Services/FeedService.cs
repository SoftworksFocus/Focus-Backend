using Focus.Application.DTO.Activity;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using Focus.Infra.Repositories;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Application.Services;

public class FeedService : IFeedService
{
    private readonly ActivityRepository _activityRepository; 
    private readonly UserGroupRepository _userGroupRepository; // Todo: Refactor to use interface

    public FeedService(ActivityRepository activityRepository, UserGroupRepository userGroupRepository)
    {
        _activityRepository = activityRepository;
        _userGroupRepository = userGroupRepository;
    }

    public async Task<List<GetActivityDto>> GetFeedForUserAsync(int userId)
    {
        var groupsIds = _userGroupRepository.GetAllGroups(userId).Result.Select(g => g.Id).ToList();
        var feedSpec = new FeedActivitySpecification(userId, groupsIds);
        var filteredActivities = await _activityRepository.ListAsync(feedSpec);
        var returnActivities = filteredActivities.Select(GetActivityDto.FromActivity).ToList();
        return returnActivities;
    }

}