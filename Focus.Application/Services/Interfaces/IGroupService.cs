using Focus.Application.DTO.Group;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IGroupService
{
    Task<GetGroupDto?> GetById(int id);
    Task<List<GetGroupDto>?> GetAllAsync(ISpecification<Group>? filterSpec = null);
    Task Add(CreateGroupDto entity);
    Task Update(int id, UpdateGroupDto entity, int requesterId);
    Task Delete(int id, int requesterId);
    Task<GetGroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, int creatorId);
}