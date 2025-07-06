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
    Task<GetActivityDto> AddAsync(CreateActivityDto activityDto);
    Task Update(int id, UpdateActivityDto entity);
    Task UpdateAsync(int activityId, int requesterId, UpdateActivityDto newActivityDto);
    
    Task Delete(int id);
    Task DeleteAsync(int id, int requesterId);
    Task UpdateMedia(int activityId, int requesterId, string mediaUrl);
}