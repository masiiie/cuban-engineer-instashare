using InstaShare.Domain.Entities.Files;

namespace InstaShare.WebApi.Dtos;

public record CreateFileDto
{
    public required string name { get; init; }
    public required long size { get; init; }
    public required FileStatus status { get; init; }
}