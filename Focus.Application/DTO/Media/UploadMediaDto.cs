using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Focus.Application.DTO.Media;

public class UploadMediaDto
{
    [Required]
    public IFormFile File { get; set; } = null!;
        
    public string? Caption { get; set; }
}