namespace Focus.Domain.DTO.User;

public class PlainUserDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Description { get; set; }
}