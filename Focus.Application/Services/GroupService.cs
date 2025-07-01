using Focus.Application.DTO.Group;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using Focus.Infra.Repositories;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Application.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    
    public GroupService(IGroupRepository groupRepository)
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

    public async Task<List<GetGroupDto>?> GetAllAsync(ISpecification<Group>? filterSpec = null)
    {
        var groups = await _groupRepository.ListAsync(filterSpec);
        return groups.Select(GetGroupDto.FromGroup).ToList();
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

    public async Task Update(int id, UpdateGroupDto groupDto)
    {
        var groupToUpdate = await _groupRepository.GetByIdAsync(id);

        if (groupToUpdate == null)
        {
            throw new KeyNotFoundException($"Group with ID {id} not found.");
        }

        groupToUpdate.Name = groupDto.Name;
        groupToUpdate.Description = groupDto.Description;
        groupToUpdate.UpdatedAt = DateTime.UtcNow;

        await _groupRepository.UpdateAsync(id, groupToUpdate);
    }

    public async Task Delete(int id)
    {
        if (!await _groupRepository.DeleteAsync(id))
        {
            throw new Exception($"Group with {id} not Deleted.");
        }
    }

    public async Task UpdateProfilePicture(int groupId, string mediaUrl)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group not found.");
        }
        group.ProfilePictureUrl = mediaUrl;
        group.UpdatedAt = DateTime.UtcNow;
        await _groupRepository.UpdateAsync(groupId, group);
    }
}