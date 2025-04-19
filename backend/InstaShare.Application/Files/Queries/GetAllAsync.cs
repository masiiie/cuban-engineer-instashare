using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using MediatR;

namespace InstaShare.Application.Files.Queries;

public sealed record GetAllFilesQuery() : IRequest<IEnumerable<InstaShareFile>>;

public sealed class GetAllFilesQueryHandler : IRequestHandler<GetAllFilesQuery, IEnumerable<InstaShareFile>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllFilesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<InstaShareFile>> Handle(GetAllFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await _unitOfWork.FileRepository.GetAllAsync();
        return files;
    }
}