using Microsoft.Extensions.Configuration;
using InstaShare.Application.Services;

namespace InstaShare.Infrastructure.Services;

public class DevelopmentBlobStorageService : IBlobStorageService
{
    private readonly string _localStoragePath;

    public DevelopmentBlobStorageService(IConfiguration configuration)
    {
        //_localStoragePath = configuration["DevelopmentStorage:Path"] ?? Path.Combine(Directory.GetCurrentDirectory(), "dev-storage");
        //Directory.CreateDirectory(_localStoragePath);
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
}