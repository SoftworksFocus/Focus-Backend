using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class UserGroupService
{
     private readonly UserGroupRepository _userGroupRepository;
    
     public UserGroupService(UserGroupRepository userGroupRepository)
     {
         _userGroupRepository = userGroupRepository;
     }
     
     public async Task<IEnumerable<UserGroup>?> GetAll() 
     {
         var usersGroups = await _userGroupRepository.GetAllAsync();
         
         if (usersGroups == null || !usersGroups.Any())
         {
             throw new KeyNotFoundException("No usersGroups found");
         }
         
         return usersGroups;
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