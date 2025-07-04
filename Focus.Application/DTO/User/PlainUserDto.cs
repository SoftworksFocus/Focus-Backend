namespace Focus.Application.DTO.User;

public class PlainUserDto
{
    public string Username { get; set; } = null!;
    public string? Description { get; set; }
    public string? ProfilePictureUrl { get; set; }
}