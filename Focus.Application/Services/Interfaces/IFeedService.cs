using Focus.Application.DTO.Activity;
using Focus.Domain.Entities;

namespace Focus.Application.Services.Interfaces;

public interface IFeedService
{
    Task<List<GetActivityDto>> GetFeedForUserAsync(int userId);
}