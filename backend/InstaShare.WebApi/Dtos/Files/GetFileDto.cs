using InstaShare.Domain.Entities.Files;

namespace InstaShare.WebApi.Dtos;

public record GetFileDto
{
    public long Id { get; init; }
    public string Name { get; init; }
    public FileStatus Status { get; init; }
    public long Size { get; init; }
    public string? BlobUrl { get; init; }
    public DateTime Created { get; init; }
    public DateTime? LastModified { get; init; }

    public GetFileDto(InstaShareFile file)
    {
        Id = file.Id;
        Name = file.Name;
        Status = file.Status;
        Size = file.Size;
        BlobUrl = file.BlobUrl;
    }
}