using System.ComponentModel.DataAnnotations;

namespace Focus.Application.DTO.User;

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string NewPassword { get; set; } = null!;
}