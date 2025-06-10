namespace Focus.Application.DTO.Activity;
using Utils;
using Domain.Entities;

public class UpdateActivityDto : PlainActivityDto
{
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void MapTo(Activity updateActivity)
    {
        updateActivity.Title = Title;
        updateActivity.Description = Description;
        updateActivity.StartDate = Functions.ParseAndConvertToUtc(StartDate, nameof(StartDate)); //send this to service
        updateActivity.EndDate = Functions.ParseAndConvertToUtc(EndDate, nameof(EndDate));
        updateActivity.Status = Status;
        updateActivity.UpdatedAt = UpdatedAt;
    }
}