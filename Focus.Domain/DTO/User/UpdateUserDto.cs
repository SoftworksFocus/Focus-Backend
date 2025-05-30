namespace Focus.Domain.DTO.User;

using Focus.Domain.Entities;

public class UpdateUserDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // May the user would like to change several groups at once
    // public IEnumerable<int>? GroupIds { get; set; }
    
    public User ToUser() =>
        new User
        {
            Username = Username ?? string.Empty,
            Email = Email ?? string.Empty,
            Description = Description,
            UpdatedAt = UpdatedAt,
        };
    
    public static UpdateUserDto FromUser(User user) =>
        new UpdateUserDto
        {
            Username = user.Username,
            Email = user.Email,
            Description = user.Description,
            UpdatedAt = user.UpdatedAt
        };
    
}