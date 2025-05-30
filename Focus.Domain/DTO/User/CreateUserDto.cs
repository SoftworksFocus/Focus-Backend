namespace Focus.Domain.DTO.User;
using Focus.Domain.Entities;

public class CreateUserDto : PlainUserDto
{
    private string Password { get; set; } = null!;

    public User ToUser() =>
        new User
        {
            Username = Username,
            Email = Email,
            Description = Description,
            // Password = Password,
            Password = "TestPassword", // Todo: This should be handled securely 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
}