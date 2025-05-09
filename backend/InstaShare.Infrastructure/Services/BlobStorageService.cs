using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using InstaShare.Application.Services;

namespace InstaShare.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(IConfiguration configuration)
    {
        var useAzuriteStr = configuration["StorageConfig:UseAzurite"];
        bool useAzurite = false;
        bool.TryParse(useAzuriteStr, out useAzurite);
    
        var connectionString = useAzurite ? configuration.GetConnectionString("AzuriteConnection") : configuration.GetConnectionString("AzureBlobStorage");
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

    public async Task<Stream> DownloadFileAsync(string blobUrl)
    {
        // Extract blob name from URL
        var uri = new Uri(blobUrl);
        var blobName = uri.Segments[^1];
        
        var blobClient = _containerClient.GetBlobClient(blobName);
        var downloadInfo = await blobClient.DownloadAsync();
        return downloadInfo.Value.Content;
    }
}