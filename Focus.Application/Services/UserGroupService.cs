using Focus.Application.DTO.Group;
using Focus.Domain.Entities;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Infra.Repositories.Interfaces;
using System.Security.Claims;

namespace Focus.Application.Services;

public class UserGroupService : IUserGroupService
{
     private readonly IUserGroupRepository _userGroupRepository;
    
     public UserGroupService(IUserGroupRepository userGroupRepository)
     {
         _userGroupRepository = userGroupRepository;
     }

    private async Task<bool> IsUserAdminAsync(int userId, int groupId)
    {
        var userGroup = await _userGroupRepository.GetByIdAsync(userId, groupId);
        return userGroup != null && userGroup.IsAdmin;
    }
     
     public async Task<IEnumerable<SummaryUserDto>?> GetAllMembersFromGroup(int groupId) 
     {
         var groupMembers = await _userGroupRepository.GetAllMembersAsync(groupId);
         var returnUsersGroups = groupMembers.Select(SummaryUserDto.FromUser).ToList();
         return returnUsersGroups;
     }
     
     public async Task<IEnumerable<SummaryGroupDto>?> GetAllGroupsFromUser(int userId) 
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
            throw new ArgumentException($"User is already a member of group .");
        }
        var relationship = new UserGroup
        {
            UserId = userId,
            GroupId = groupId,
            IsAdmin = isAdmin
        };
        await _userGroupRepository.MakeRelationship(relationship);
     }
    
     public async Task ToggleRoleAdmin(int userId, int groupId, int requesterId)
     {
        if (!await IsUserAdminAsync(requesterId, groupId))
        {
            throw new UnauthorizedAccessException("Apenas administradores do grupo podem alterar cargos.");
        }

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

     public async Task RemoveUserFromGroupAsync(int groupId, int userIdToRemove, int requesterId)
     {
         if (!await IsUserAdminAsync(requesterId, groupId))
         {
             throw new UnauthorizedAccessException("Only group admins can remove members.");
         }

         if (userIdToRemove == requesterId)
         {
             throw new InvalidOperationException("You cannot remove yourself with this function. Please use the 'Leave Group' feature.");
         }

         var memberToRemove = await _userGroupRepository.GetByIdAsync(userIdToRemove, groupId);
         if (memberToRemove is null)
         {
             throw new KeyNotFoundException("The user to be removed is not a member of this group.");
         }

         await _userGroupRepository.DeleteAsync(userIdToRemove, groupId);
     }
}