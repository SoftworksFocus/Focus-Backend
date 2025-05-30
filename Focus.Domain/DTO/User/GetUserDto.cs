namespace Focus.Domain.DTO.User;
using Focus.Domain.Entities;
public class GetUserDto : PlainUserDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<Group> Groups { get; set; } = null!;
    public IEnumerable<Activity> Activities { get; set; } = null!;
    
    public static GetUserDto FromUser(User user) =>
        new GetUserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Description = user.Description,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Groups = user.Groups?.Select(ug => ug.Group) ?? new List<Group>(),
            Activities = user.Activities ?? new List<Activity>()
        };
}