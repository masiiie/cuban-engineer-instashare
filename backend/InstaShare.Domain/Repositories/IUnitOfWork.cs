namespace InstaShare.Domain.Repositories;

public interface IUnitOfWork
{
    IFileRepository FileRepository {get;}

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}