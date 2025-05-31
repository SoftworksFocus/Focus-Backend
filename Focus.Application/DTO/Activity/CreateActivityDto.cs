namespace Focus.Application.DTO.Activity;
using Utils;
using Domain.Entities;

public class CreateActivityDto : PlainActivityDto
{
    public int UserId { get; set; }
    public int? GroupId { get; set; }
    
    public Activity ToActivity(int userId, int? groupId = null)
    {
        return new Activity
        {
            Title = Title,
            Description = Description ?? string.Empty,
            StartDate = Functions.ParseAndConvertToUtc(StartDate, nameof(StartDate)),
            EndDate = Functions.ParseAndConvertToUtc(EndDate, nameof(EndDate)),
            Status = Status,
            UserId = userId,
            GroupId = groupId
        };
    }
}