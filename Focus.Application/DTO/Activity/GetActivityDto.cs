namespace Focus.Application.DTO.Activity;
using User;
using Domain.Entities;
using DTO.User;

public class GetActivityDto : PlainActivityDto
{
    public int Id { get; set; }
    public SummaryUserDto User { get; set; } = null!;
    public Group? Group { get; set; }
    
    public static GetActivityDto FromActivity(Activity activity)
    {
        return new GetActivityDto
        {
            Id = activity.Id,
            Title = activity.Title,
            Description = activity.Description,
            StartDate = activity.StartDate.ToString("g"),
            EndDate = activity.EndDate.ToString("g"),
            Status = activity.Status,
            User = SummaryUserDto.FromUser(activity.User),
            Group = activity.Group
        };
    }
}