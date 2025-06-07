namespace Focus.Application.DTO.Group;
using Domain.Entities;

public class UpdateGroupDto : PlainGroupDto
{
    public void MapTo(Group updateGroup)
    {
        updateGroup.Name = Name;
        updateGroup.Description = Description;
    }
}