using Focus.Application.DTO.Group;
using Focus.Domain.Entities;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Application.Services;

public class UserGroupService : IUserGroupService
{
     private readonly IUserGroupRepository _userGroupRepository;
    
     public UserGroupService(IUserGroupRepository userGroupRepository)
     {
         _userGroupRepository = userGroupRepository;
     }
     
     public async Task<IEnumerable<SummaryUserDto>?> GetAllMembersFromGroup(int groupId) 
     {
         var groupMembers = await _userGroupRepository.GetAllMembersAsync(groupId);
         var returnUsersGroups = groupMembers.Select(SummaryUserDto.FromUser).ToList();
         return returnUsersGroups;
     }
     
     public async Task<IEnumerable<SummaryGroupDto>?> GetAllGroupsFromUser(int userId) // Todo: change the logic of the others gets 
     {
         var userGroups = await _userGroupRepository.GetAllGroups(userId);
         var returnUsersGroups = userGroups.Select(SummaryGroupDto.FromGroup).ToList();
         return returnUsersGroups;
     }
     
     public async Task<UserGroup?> GetById(int userId, int groupId)
     {
         var activity = await _userGroupRepository.GetByIdAsync(userId, groupId);
         if (activity == null)
         {
             throw new KeyNotFoundException($"UserGroup not found");
         }
         
         return activity;
     }
    
     public async Task AddUserToGroupAsync(int userId, int groupId, bool isAdmin = false)
     {
        var memberInGroup = await _userGroupRepository.GetByIdAsync(userId, groupId);
        if (memberInGroup != null)
        {
            throw new ArgumentException($"User is already a member of group ."); // Todo: remove the id references in the messages
        }
        var relationship = new UserGroup
        {
            UserId = userId,
            GroupId = groupId,
            IsAdmin = isAdmin
        };
        await _userGroupRepository.MakeRelationship(relationship);
     }
    
     public async Task ToggleRoleAdmin(int userId, int groupId)
     {
         var existingUserGroup = await _userGroupRepository.GetByIdAsync(userId, groupId);
         
         if (existingUserGroup == null)
         {
             throw new ArgumentNullException(nameof(existingUserGroup), "User is not in this group.");
         }
         existingUserGroup.IsAdmin = !existingUserGroup.IsAdmin;
         await _userGroupRepository.UpdateAsync();
     }
    
     public async Task RemoveUserFromGroupAsync(int userId, int groupId)
     {
         await _userGroupRepository.DeleteAsync(userId, groupId);
     }
}