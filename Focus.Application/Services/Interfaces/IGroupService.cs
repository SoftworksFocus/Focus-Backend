using Focus.Application.DTO.Group;
using Focus.Domain.Entities;

namespace Focus.Application.Services.Interfaces;

public interface IGroupService : IService<Group, GetGroupDto, CreateGroupDto, UpdateGroupDto>
{
    Task UpdateProfilePicture(int groupId, string mediaUrl);
}