using MediatR;
using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using InstaShare.Application.Services;

namespace InstaShare.Application.Files.Commands;

public record UploadFileCommand(Stream FileStream, string FileName, string ContentType, long Size) : IRequest<InstaShareFile>;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, InstaShareFile>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public UploadFileCommandHandler(IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task<InstaShareFile> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        string newName = request.FileName.Replace(" ", "_").ToLower();
        // Upload to blob storage
        var blobUrl = await _blobStorageService.UploadFileAsync(request.FileStream, newName);

        // Create file entity
        var file = new InstaShareFile(
            request.FileName,
            FileStatus.Uploaded,
            request.Size,
            blobUrl
        );

        // Save to database
        await _unitOfWork.FileRepository.CreateAsync(file);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return file;
    }
}