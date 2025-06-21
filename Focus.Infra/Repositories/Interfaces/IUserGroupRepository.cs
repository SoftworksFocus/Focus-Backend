using Focus.Domain.Entities;

namespace Focus.Infra.Repositories.Interfaces;

public interface IUserGroupRepository
{
    public Task<UserGroup?> GetByIdAsync(int userId, int groupId);
    public Task<IEnumerable<UserGroup>?> GetAllAsync();
    public Task<IEnumerable<Group>> GetAllGroups(int userId);
    public Task MakeRelationship(UserGroup relationship);
    public Task UpdateAsync();
    public Task DeleteAsync(int userId, int groupId);
    public Task<IEnumerable<User>?> GetAllMembersAsync(int groupId);
}