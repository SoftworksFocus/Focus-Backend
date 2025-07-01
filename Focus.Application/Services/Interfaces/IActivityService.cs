using Focus.Application.DTO.Activity;
using Focus.Application.DTO.Common;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IActivityService
{
    Task<GetActivityDto?> GetById(int id);
    Task<PagedResultDto<GetActivityDto>> GetAllAsync(ISpecification<Activity> filterSpec, int pageNumber, int pageSize);
    Task Add(CreateActivityDto entity);
    Task Update(int id, UpdateActivityDto entity);
    Task Delete(int id);
    Task UpdateMedia(int activityId, string mediaUrl, string? caption);

}