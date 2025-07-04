namespace Focus.Application.DTO.User;

using Domain.Entities;

public class UpdateUserDto : PlainUserDto
{
    private int Id { get; set; }
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // May the user would like to change several groups at once
    // public IEnumerable<int>? GroupIds { get; set; }

    public void MapTo(User user)
    {
        user.Username = Username;
        user.Description = Description;
        user.UpdatedAt = UpdatedAt;
        // user.GroupIds = GroupIds;
    }
        
}