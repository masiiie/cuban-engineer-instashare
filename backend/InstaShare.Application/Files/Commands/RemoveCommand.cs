using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using InstaShare.Application.CustomExceptions;
using MediatR;

namespace InstaShare.Application.Files.Commands;

public sealed record RemoveFileCommand(long fileId) : IRequest<InstaShareFile>;

public sealed class RemoveFileCommandHandler : IRequestHandler<RemoveFileCommand, InstaShareFile>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InstaShareFile> Handle(RemoveFileCommand request, CancellationToken cancellationToken)
    {
        var fileEntity = await _unitOfWork.FileRepository.GetByIdAsync(request.fileId);
        if (fileEntity == null)
        {
            throw new NotFoundException($"File with ID {request.fileId} not found.");
        }

        _unitOfWork.FileRepository.Remove(fileEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return fileEntity;
    }
}