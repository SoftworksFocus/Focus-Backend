using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Focus.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Focus.Application.Services;

public class AzureBlobUploadService : IMediaUploadService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "media";
    
    public AzureBlobUploadService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MediaStorage");
        _blobServiceClient = new BlobServiceClient(connectionString);
    }
    
    public async Task<string> UploadMediaAsync(IFormFile file)
    {
        if (file.Length == 0)
            return "File is empty";
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        var blobName = $"{Guid.NewGuid()}-{file.FileName}";
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        }
        return blobClient.Uri.ToString();
    }

}