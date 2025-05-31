namespace Focus.Application.DTO.Activity;
using Utils;
using Domain.Entities;

public class UpdateActivityDto : PlainActivityDto
{
    private int Id { get; set; }
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Activity ToActivity() =>
    new Activity
    {
        Id = Id,
        Title = Title,
        Description = Description ?? string.Empty,
        StartDate = Functions.ParseAndConvertToUtc(StartDate, nameof(StartDate)),
        EndDate = Functions.ParseAndConvertToUtc(EndDate, nameof(EndDate)),
        Status = Status,
        UpdatedAt = UpdatedAt
    };
}