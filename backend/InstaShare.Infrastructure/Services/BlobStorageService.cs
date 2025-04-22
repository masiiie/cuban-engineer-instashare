using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using InstaShare.Application.Services;

namespace InstaShare.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzureStorage");
        var containerName = configuration["BlobContainerName"] ?? "instashare-files";
        
        // Create BlobServiceClient
        var blobServiceClient = new BlobServiceClient(connectionString);
        
        // Get container client and create container if it doesn't exist
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        // Generate a unique name to avoid conflicts
        var uniqueFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{fileName}";
        var blobClient = _containerClient.GetBlobClient(uniqueFileName);
        
        await blobClient.UploadAsync(fileStream, true);
        return blobClient.Uri.ToString();
    }

    public async Task DeleteFileAsync(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}