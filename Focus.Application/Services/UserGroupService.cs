using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class UserGroupService
{
    // private readonly UserGroupRepository _userGroupRepository;
    //
    // public UserGroupService(UserGroupRepository userGroupRepository)
    // {
    //     _userGroupRepository = userGroupRepository;
    // }
    //
    // public async Task<UserGroup?> GetById(int id)
    // {
    //     var activity = await _userGroupRepository.GetByIdAsync(id);
    //     if (activity == null)
    //     {
    //         throw new KeyNotFoundException($"UserGroup with {id} not found");
    //     }
    //     
    //     return activity;
    // }
    //
    // public async Task<IEnumerable<UserGroup>?> GetAll() 
    // {
    //     var activities = await _userGroupRepository.
    //     
    //     if (activities == null || !activities.Any())
    //     {
    //         throw new KeyNotFoundException("No activities found");
    //     }
    //     
    //     return activities;
    // }
    //
    // public async Task Add(UserGroup activity)
    // {
    //     if (activity == null)
    //     {
    //         throw new ArgumentNullException(nameof(activity), "UserGroup cannot be null.");
    //     }
    //     
    //     await _userGroupRepository.AddAsync(activity);
    // }
    //
    // public async Task Update(int id, UserGroup activity)
    // {
    //     
    //     var existingUserGroup = await _userGroupRepository.GetByIdAsync(id);
    //     
    //     if (existingUserGroup == null)
    //     {
    //         throw new ArgumentNullException(nameof(existingUserGroup), "UserGroup cannot be null.");
    //     }
    //     
    //     if (existingUserGroup == null)
    //     {
    //         throw new KeyNotFoundException($"UserGroup with ID {id} not found.");
    //     }
    //     
    //     await _userGroupRepository.UpdateAsync(existingUserGroup);
    // }
    //
    // public async Task Delete(int id)
    // {
    //     var activity = await _userGroupRepository.GetByIdAsync(id);
    //     
    //     if (activity == null)
    //     {
    //         throw new ArgumentNullException(nameof(activity), "UserGroup cannot be null.");
    //     }
    //     
    //     await _userGroupRepository.DeleteAsync(id);
    //}
}