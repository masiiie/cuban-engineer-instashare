using Microsoft.Extensions.Configuration;
using InstaShare.Application.Services;

namespace InstaShare.Infrastructure.Services;

public class DevelopmentBlobStorageService : IBlobStorageService
{
    public DevelopmentBlobStorageService(IConfiguration configuration)
    {
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        // Simulate delay
        await Task.Delay(100);

        // Generate a unique name
        var uniqueFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{fileName}";
        
        // In development, just return a fake URL
        return $"http://fake-development-url/{uniqueFileName}";
    }

    public async Task DeleteFileAsync(string blobName)
    {
        // Simulate delay
        await Task.Delay(100);
    }

    public async Task<Stream> DownloadFileAsync(string blobUrl)
    {
        // Simulate delay
        await Task.Delay(100);
        
        // For development, return a simple memory stream with some test content
        var content = "This is a test file content from development storage";
        return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
    }
}