using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using InstaShare.Application.CustomExceptions;
using MediatR;

namespace InstaShare.Application.Files.Commands;

public sealed record UpdateFileCommand(long fileId, string? filename, FileStatus? fileStatus, long? fileSize, string? blobUrl) : IRequest<InstaShareFile>;

public sealed class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand, InstaShareFile>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InstaShareFile> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
        var fileEntity = await _unitOfWork.FileRepository.GetByIdAsync(request.fileId);
        if(fileEntity == null)
        {
            throw new NotFoundException($"File with ID {request.fileId} not found");
        }

        if(request.filename is not null)
        {
            fileEntity.SetName(request.filename);
        }

        if(request.fileStatus is not null)
        {
            fileEntity.SetStatus(request.fileStatus.Value);
        }

        if(request.fileSize is not null)
        {
            fileEntity.SetSize(request.fileSize.Value);
        }

        if(request.blobUrl is not null)
        {
            fileEntity.SetBlobUrl(request.blobUrl);
        }

        _unitOfWork.FileRepository.Update(fileEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return fileEntity;
    }
}