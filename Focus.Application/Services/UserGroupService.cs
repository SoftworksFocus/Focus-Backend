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
    
     public async Task Add(UserGroup activity)
     {
         if (activity == null)
         {
             throw new ArgumentNullException(nameof(activity), "UserGroup cannot be null.");
         }
         
         await _userGroupRepository.AddAsync(activity);
     }
    
     public async Task Update(int userId, int groupId, UserGroup activity)
     {
         
         var existingUserGroup = await _userGroupRepository.GetByIdAsync(userId, groupId);
         
         if (existingUserGroup == null)
         {
             throw new ArgumentNullException(nameof(existingUserGroup), "UserGroup cannot be null.");
         }
         
         if (existingUserGroup == null)
         {
             throw new KeyNotFoundException($"UserGroup not found.");
         }
         await _userGroupRepository.UpdateAsync(userId, groupId, activity);
     }
    
     public async Task Delete(int userId, int groupId)
     {
         await _userGroupRepository.DeleteAsync(userId, groupId);
     }
}