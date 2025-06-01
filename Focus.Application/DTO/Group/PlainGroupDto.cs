namespace Focus.Application.DTO.Group;

public class PlainGroupDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public int MemberCount { get; set; }
}