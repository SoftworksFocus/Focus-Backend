using Focus.Application.DTO.Activity;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IActivityService
{
    Task<GetActivityDto?> GetById(int id);
    Task<List<GetActivityDto>?> GetAllAsync(ISpecification<Activity>? filterSpec = null);
    
    Task Add(CreateActivityDto createUserDto);
    Task Update(int id, UpdateActivityDto newUserDto);
    Task Delete(int id);
}