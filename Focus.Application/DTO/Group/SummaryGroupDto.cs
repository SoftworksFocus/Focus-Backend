namespace Focus.Application.DTO.Group;
using Focus.Domain.Entities;
public class SummaryGroupDto : PlainGroupDto
{
    public static SummaryGroupDto FromGroup(Group group) =>
        new SummaryGroupDto
        {
            Id = group.Id,
            ProfilePictureUrl = group.ProfilePictureUrl,
            Name = group.Name,
            Description = group.Description,
        };
}