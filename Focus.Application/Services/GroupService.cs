using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class GroupService : IService<Group>
{
    private readonly GroupRepository _groupRepository;
    
    GroupService(GroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Group?> GetById(int id)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with {id} not found.");
        }
        
        return group;
    }

    public async Task<IEnumerable<Group>?> GetAll()
    {
        var groups =  await _groupRepository.GetAllAsync();

        if (groups == null)
        {
            throw new KeyNotFoundException($"Groups not found.");
        }
        
        return groups;
    }

    public async Task Add(Group group)
    {
        if (group == null)
        {
            throw new ArgumentNullException(nameof(group), "Group cannot be null.");
        }
        
        await _groupRepository.AddAsync(group);
    }

    public async Task Update(int id, Group group)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);

        if (existingGroup == null)
        {
            throw new ArgumentNullException(nameof(existingGroup), "Group cannot be null.");
        }

        await _groupRepository.UpdateAsync(id, existingGroup);
    }

    public async Task Delete(int id)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);

        if (existingGroup == null)
        {
            throw new ArgumentNullException(nameof(existingGroup), "Group cannot be null.");
        }

        await _groupRepository.DeleteAsync(id);
    }
    
    
}