namespace Focus.Application.DTO.Group;
using Focus.Domain.Entities;
public class SummaryGroupDto : PlainGroupDto
{
    public static SummaryGroupDto FromGroup(Group group) =>
        new SummaryGroupDto
        {
            Name = group.Name,
            Description = group.Description,
        };
}