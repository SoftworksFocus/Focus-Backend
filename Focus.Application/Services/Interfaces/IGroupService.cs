using Focus.Application.DTO.Group;
using Focus.Domain.Entities;

namespace Focus.Application.Services.Interfaces;

public interface IGroupService
{
    Task<GetGroupDto?> GetById(int id);
    Task<List<GetGroupDto>?> GetAllAsync(ISpecification<Group>? filterSpec = null);
    Task<GetGroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, int creatorId);
    Task UpdateAsync(int id, UpdateGroupDto groupDto, int requesterId);
    Task DeleteAsync(int id, int requesterId);
    Task UpdateProfilePicture(int groupId, string mediaUrl);
}