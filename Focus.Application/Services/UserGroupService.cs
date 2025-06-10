using Focus.Application.DTO.Group;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;
using Focus.Application.DTO.User;

namespace Focus.Application.Services;

public class UserGroupService
{
     private readonly UserGroupRepository _userGroupRepository;
    
     public UserGroupService(UserGroupRepository userGroupRepository)
     {
         _userGroupRepository = userGroupRepository;
     }
     
     public async Task<IEnumerable<SummaryUserDto>?> GetAllMembersFromGroup(int groupId) 
     {
         var groupMembers = await _userGroupRepository.GetAllMembersAsync(groupId);
         
         if (groupMembers == null || !groupMembers.Any())
         {
             throw new KeyNotFoundException("No usersGroups found");
         }
         
         var returnUsersGroups = groupMembers.Select(SummaryUserDto.FromUser).ToList();
         
         return returnUsersGroups;
     }
     
     public async Task<IEnumerable<SummaryGroupDto>?> GetAllGroupsFromUser(int userId) 
     {
         var userGroups = await _userGroupRepository.GetAllGroups(userId);
         
         if (userGroups == null || !userGroups.Any())
         {
             throw new KeyNotFoundException("No usersGroups found");
         }
         
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
    
     public async Task UserJoinGroup(int userId, int groupId, bool isAdmin = false)
     {
        var memberInGroup = await _userGroupRepository.GetByIdAsync(userId, groupId);
        if (memberInGroup != null)
        {
            throw new ArgumentException($"User with id {userId} is already a member of group with id {groupId}."); // Todo: remove the id references in the messages
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
    
     public async Task Delete(int userId, int groupId)
     {
         await _userGroupRepository.DeleteAsync(userId, groupId);
     }
}