using Focus.Application.DTO.Group;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IGroupService : IService<Group, GetGroupDto, CreateGroupDto, UpdateGroupDto>
{
    Task<GetGroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, int creatorId);
    Task UpdateProfilePicture(int groupId, string mediaUrl);
}