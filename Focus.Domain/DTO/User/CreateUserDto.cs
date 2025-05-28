namespace Focus.Domain.DTO.User;
using Focus.Domain.Entities;

public class CreateUserDto : PlainUserDto
{
    public string Password { get; set; } = null!;

    public User ToUser() =>
        new User
        {
            Username = Username,
            Email = Email,
            Description = Description,
            Password = Password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
}