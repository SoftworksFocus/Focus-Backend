using Focus.Application.DTO.Group;
using Focus.Application.Services.Interfaces;
using Focus.Infra.Repositories;

namespace Focus.Application.Services;

public class GroupService : IGroupService
{
    private readonly GroupRepository _groupRepository;
    
    public GroupService(GroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<GetGroupDto?> GetById(int id)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with {id} not found.");
        }
        return GetGroupDto.FromGroup(group);
    }

    public async Task<IEnumerable<GetGroupDto>?> GetAll()
    {
        var groups =  await _groupRepository.GetAllAsync();

        if (groups == null || !groups.Any())
        {
            throw new KeyNotFoundException($"Groups not found.");
        }
        var groupsDto = groups.Select(GetGroupDto.FromGroup);
        return groupsDto;
    }

    public async Task Add(CreateGroupDto createGroupDto)
    {
        if (createGroupDto == null)
        {
            throw new ArgumentNullException(nameof(createGroupDto), "Group cannot be null.");
        }
        var group = createGroupDto.ToGroup();
        if (!await _groupRepository.AddAsync(group))
        {
            throw new Exception("Failed to add group.");
        }
    }

    public async Task Update(int id, UpdateGroupDto newGroupDto)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);
        if (existingGroup == null)
        {
            throw new ArgumentNullException($"Group with {id} not found.");
        }
        if (newGroupDto == null)
        {
            throw new ArgumentNullException(nameof(newGroupDto), "Group cannot be null.");
        }
        newGroupDto.MapTo(existingGroup);
        if(!await _groupRepository.UpdateAsync())
        {
            throw new Exception($"Group with {id} not Updated.");
        }
    }

    public async Task Delete(int id)
    {
        if (!await _groupRepository.DeleteAsync(id))
        {
            throw new Exception($"Group with {id} not Deleted.");
        }
    }
}