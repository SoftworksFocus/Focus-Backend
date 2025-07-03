using Focus.Application.DTO.Common;
using Focus.Application.DTO.Group;
using Focus.Domain.Entities;
using Focus.Domain.Specifications;

namespace Focus.Application.Services.Interfaces;

public interface IGroupService // Todo: make use IService interface for consistency
{
    Task<GetGroupDto?> GetById(int id);
    Task<PagedResultDto<GetGroupDto>> GetAllAsync(ISpecification<Group> filterSpec, int pageNumber, int pageSize);
    Task<GetGroupDto> CreateGroupAsync(CreateGroupDto createGroupDto, int creatorId);
    Task UpdateAsync(int id, UpdateGroupDto groupDto, int requesterId);
    Task DeleteAsync(int id, int requesterId);
    Task UpdateProfilePicture(int groupId, string mediaUrl);
}