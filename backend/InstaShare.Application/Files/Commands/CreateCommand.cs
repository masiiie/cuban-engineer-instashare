using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using MediatR;

namespace InstaShare.Application.Files.Commands;

public record CreateFileDto
{
    public required string name { get; init; }
    public required long size { get; init; }
    public required FileStatus status { get; init; }
}

public sealed record CreateFileCommand(string fileName, FileStatus fileStatus, long fileSize) : IRequest<InstaShareFile>;
public sealed class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, InstaShareFile>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InstaShareFile> Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        var fileEntity = new InstaShareFile(request.fileName, request.fileStatus, request.fileSize);
        _unitOfWork.FileRepository.Create(fileEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return fileEntity;
    }
}