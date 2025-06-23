namespace Focus.Application.DTO.User;
using Domain.Entities;

public class CreateUserDto : PlainUserDto
{
    public string Password { get; set; } = null!;

    public User ToUser() =>
        new User
        {
            Username = Username,
            Email = Email,
            Description = Description,
            Password = this.Password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
}