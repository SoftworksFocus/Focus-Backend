using Focus.Application.DTO.Activity;
using Focus.Application.DTO.Feed;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;
using Focus.Domain.Entities;
using Focus.Infra.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Focus.Application.Services
{
    public class FeedService : IFeedService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public FeedService(IActivityRepository activityRepository, IUserGroupRepository userGroupRepository)
        {
            _activityRepository = activityRepository;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<FeedResultDto> GetFeedForUserAsync(int userId, DateTime? cursor, int pageSize)
        {
            var groups = await _userGroupRepository.GetAllGroups(userId);
            var groupIds = groups.Select(g => g.Id).ToList();
            
            var feedSpec = new FeedActivitySpecification(userId, groupIds, cursor);

            var activities = await _activityRepository.ListAsync(feedSpec, pageSize);

            DateTime? nextCursor = null;
            if (activities.Count == pageSize && activities.Any())
            {
                nextCursor = activities.Last().CreatedAt;
            }

            var activityDtos = activities.Select(GetActivityDto.FromActivity).ToList();

            return new FeedResultDto
            {
                Activities = activityDtos,
                NextCursor = nextCursor
            };
        }
    }
}