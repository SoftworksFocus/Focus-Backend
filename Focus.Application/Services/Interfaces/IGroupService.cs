using Focus.Application.DTO.Group;

namespace Focus.Application.Services.Interfaces;

public interface IGroupService
{
    Task<GetGroupDto?> GetById(int id);
    Task<IEnumerable<GetGroupDto>?> GetAll();
    Task Add(CreateGroupDto createGroupDto);
    Task Update(int id, UpdateGroupDto newGroupDto);
    Task Delete(int id);
}