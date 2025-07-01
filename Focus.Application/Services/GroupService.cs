using Focus.Application.DTO.Common;
using Focus.Application.DTO.Group;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;
using Focus.Infra.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Focus.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public GroupService(IGroupRepository groupRepository, IUserGroupRepository userGroupRepository)
        {
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
        }

        private async Task<bool> IsUserAdminAsync(int userId, int groupId)
        {
            var userGroup = await _userGroupRepository.GetByIdAsync(userId, groupId);
            return userGroup != null && userGroup.IsAdmin;
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

        public async Task<PagedResultDto<GetGroupDto>> GetAllAsync(ISpecification<Group> filterSpec, int pageNumber, int pageSize)
        {
            var totalCount = await _groupRepository.CountAsync(filterSpec);
            var groups = await _groupRepository.ListAsync(filterSpec, pageNumber, pageSize);
            var groupDtos = groups.Select(GetGroupDto.FromGroup).ToList();
            return new PagedResultDto<GetGroupDto>(groupDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<GetGroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, int creatorId)
        {
            if (createGroupDto == null)
            {
                throw new ArgumentNullException(nameof(createGroupDto), "The group data cannot be null.");
            }
            var group = createGroupDto.ToGroup();
            await _groupRepository.AddAsync(group);

            var adminRelationship = new UserGroup
            {
                UserId = creatorId,
                GroupId = group.Id,
                IsAdmin = true,
                Status = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userGroupRepository.MakeRelationship(adminRelationship);
            return GetGroupDto.FromGroup(group);
        }

        public async Task UpdateAsync(int id, UpdateGroupDto groupDto, int requesterId)
        {
            if (!await IsUserAdminAsync(requesterId, id))
            {
                throw new UnauthorizedAccessException("Apenas administradores do grupo podem fazer alterações.");
            }

            var groupToUpdate = await _groupRepository.GetByIdAsync(id);
            if (groupToUpdate == null)
            {
                throw new KeyNotFoundException($"Group with ID {id} not found.");
            }
            
            groupDto.MapTo(groupToUpdate);
            await _groupRepository.UpdateAsync(id, groupToUpdate);
        }

        public async Task DeleteAsync(int id, int requesterId)
        {
            if (!await IsUserAdminAsync(requesterId, id))
            {
                throw new UnauthorizedAccessException("Apenas administradores podem deletar o grupo.");
            }

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
}