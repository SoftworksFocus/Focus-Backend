using Microsoft.AspNetCore.Http;

namespace Focus.Application.Services.Interfaces;

public interface IMediaUploadService
{
    Task<string> UploadMediaAsync(IFormFile file);

}