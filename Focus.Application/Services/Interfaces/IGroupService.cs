using Focus.Application.DTO.Group;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IGroupService
{
    Task<GetGroupDto?> GetById(int id);
    Task<List<GetGroupDto>?> GetAllAsync(ISpecification<Group>? filterSpec = null);
    Task Add(CreateGroupDto createGroupDto);
    Task Update(int id, UpdateGroupDto newGroupDto);
    Task Delete(int id);
}