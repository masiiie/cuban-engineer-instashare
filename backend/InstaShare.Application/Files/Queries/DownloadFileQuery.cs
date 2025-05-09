using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using InstaShare.Application.Services;
using InstaShare.Application.CustomExceptions;
using MediatR;

namespace InstaShare.Application.Files.Queries;

public record DownloadFileResult(Stream Content, string FileName, string ContentType);
public sealed record DownloadFileQuery(long FileId) : IRequest<DownloadFileResult>;

public sealed class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, DownloadFileResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public DownloadFileQueryHandler(IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task<DownloadFileResult> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.FileRepository.GetByIdAsync(request.FileId);
        if (file == null)
        {
            throw new NotFoundException($"File with ID {request.FileId} not found.");
        }

        if (string.IsNullOrEmpty(file.BlobUrl))
        {
            throw new InvalidOperationException("File has no associated blob URL.");
        }

        var stream = await _blobStorageService.DownloadFileAsync(file.BlobUrl);
        
        // Infer content type from file extension
        var contentType = GetContentType(file.Name);
        
        return new DownloadFileResult(stream, file.Name, contentType);
    }

    private string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}