using Focus.Application.DTO.Activity;

namespace Focus.Application.Services.Interfaces;

public interface IActivityService
{
    Task<GetActivityDto?> GetById(int id);
    Task<IEnumerable<GetActivityDto>?> GetAll();
    Task Add(CreateActivityDto createUserDto);
    Task Update(int id, UpdateActivityDto newUserDto);
    Task Delete(int id);
}