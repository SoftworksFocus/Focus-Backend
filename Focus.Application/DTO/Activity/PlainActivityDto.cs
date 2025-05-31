namespace Focus.Application.DTO.Activity;

public class PlainActivityDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public bool Status { get; set; } = true;
}