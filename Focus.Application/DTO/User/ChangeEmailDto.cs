using System.ComponentModel.DataAnnotations;

namespace Focus.Application.DTO.User;

public class ChangeEmailRequestDto
{
    [Required]
    [EmailAddress]
    public string NewEmail { get; set; }
}