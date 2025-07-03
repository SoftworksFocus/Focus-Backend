using Focus.Application.DTO.Activity;
using Focus.Application.DTO.Feed;
using Focus.Domain.Entities;

namespace Focus.Application.Services.Interfaces;

public interface IFeedService
{
    Task<FeedResultDto> GetFeedForUserAsync(int userId, DateTime? cursor, int pageSize);
}