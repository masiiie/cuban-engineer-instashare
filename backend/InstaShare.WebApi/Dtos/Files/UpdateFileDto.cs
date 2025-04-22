using InstaShare.Domain.Entities.Files;

namespace InstaShare.WebApi.Dtos;

public record UpdateFileDto
{
    public string? name { get; init; }
    public long? size { get; init; }
    public FileStatus? status { get; init; }
    public string? blobUrl { get; init; }
}