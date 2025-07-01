using Focus.Application.DTO.Group;
using Focus.Application.DTO.User;
using Focus.Domain.Entities;

namespace Focus.Application.Services.Interfaces
{
    public interface IUserGroupService
    {
        public Task<IEnumerable<SummaryUserDto>?> GetAllMembersFromGroup(int groupId);
        public Task<IEnumerable<SummaryGroupDto>?> GetAllGroupsFromUser(int userId);
        public Task<UserGroup?> GetById(int userId, int groupId);
        public Task AddUserToGroupAsync(int userId, int groupId, bool isAdmin = false);
        public Task ToggleRoleAdmin(int userId, int groupId, int requesterId);
        public Task RemoveUserFromGroupAsync(int userId, int groupId);
        public Task RemoveUserFromGroupAsync(int groupId, int userIdToRemove, int requesterId);

    }
}