namespace Focus.Application.DTO.Group;
using Domain.Entities;
public class GetGroupDto : PlainGroupDto
{
    public int Id { get; set; }
    
    // public MemberCount {} //Todo: return member count in group
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public static GetGroupDto FromGroup(Group group)
    {
        if (group == null)
        {
            throw new ArgumentNullException(nameof(group), "Group cannot be null.");
        }

        return new GetGroupDto
        {
            Id = group.Id,
            ProfilePictureUrl = group.ProfilePictureUrl, 
            Name = group.Name,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            UpdatedAt = group.UpdatedAt
        };
    }
}