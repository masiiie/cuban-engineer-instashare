namespace InstaShare.Application.Services;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string blobName);
}