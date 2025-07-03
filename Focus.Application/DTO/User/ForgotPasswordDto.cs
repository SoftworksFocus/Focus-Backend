using System.ComponentModel.DataAnnotations;

namespace Focus.Application.DTO.User;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}