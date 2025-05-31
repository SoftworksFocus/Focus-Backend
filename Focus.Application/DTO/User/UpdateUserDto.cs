namespace Focus.Application.DTO.User;

using Domain.Entities;

public class UpdateUserDto : PlainUserDto
{
    private int Id { get; set; }
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // May the user would like to change several groups at once
    // public IEnumerable<int>? GroupIds { get; set; }
    
    public User ToUser(int id) =>
        new User
        {
            Id = id,
            Username = Username ?? string.Empty,
            Email = Email ?? string.Empty,
            Description = Description,
            UpdatedAt = UpdatedAt,
        };
}