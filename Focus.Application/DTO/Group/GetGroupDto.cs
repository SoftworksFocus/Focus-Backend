namespace Focus.Application.DTO.Group;

public class GetGroupDto : PlainGroupDto
{
    public int Id { get; set; }
    
    // public MemberCount {} //Todo: return member count in group
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public static GetGroupDto FromGroup(Domain.Entities.Group group)
    {
        if (group == null)
        {
            throw new ArgumentNullException(nameof(group), "Group cannot be null.");
        }

        return new GetGroupDto
        {
            Name = group.Name,
            Description = group.Description,
            CreatedAt = group.CreatedAt,
            UpdatedAt = group.UpdatedAt
        };
    }
}