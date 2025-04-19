using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using MediatR;

namespace InstaShare.Application.Files.Queries;

public sealed record GetFileByIdQuery(long fileId) : IRequest<InstaShareFile>;

public sealed class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, InstaShareFile>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFileByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InstaShareFile> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.FileRepository.GetByIdAsync(request.fileId);
        return file;
    }
}