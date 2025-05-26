using Focus.Infra.Repositories;
using Focus.Domain.Entities;

namespace Focus.Application.Services;

public class GroupService
{
    private readonly GroupRepository _groupRepository;
    
    GroupService(GroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Group> GetByIdAsync(int id)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with {id} not found.");
        }
        
        return group;
    }

    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        var groups =  await _groupRepository.GetAllAsync();

        if (groups == null)
        {
            throw new KeyNotFoundException($"Groups not found.");
        }
        
        return groups;
    }

    public async Task<Group> AddAsync(Group group)
    {
        if (group == null)
        {
            throw new ArgumentNullException(nameof(group), "Group cannot be null.");
        }
        
        await _groupRepository.AddAsync(group);
        return group;
    }

    public async Task<Group> UpdateAsync(int id, Group group)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);

        if (existingGroup == null)
        {
            throw new ArgumentNullException(nameof(existingGroup), "Group cannot be null.");
        }

        await _groupRepository.UpdateAsync(id, existingGroup);
        return  existingGroup;
    }

    public async Task<Group> DeleteAsync(int id)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);

        if (existingGroup == null)
        {
            throw new ArgumentNullException(nameof(existingGroup), "Group cannot be null.");
        }

        await _groupRepository.DeleteAsync(id);
        return existingGroup;
    }
    
    
}