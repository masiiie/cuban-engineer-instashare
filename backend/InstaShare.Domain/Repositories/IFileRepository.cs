using InstaShare.Domain.Entities.Files;

namespace InstaShare.Domain.Repositories;

public interface IFileRepository : ICrudBaseRepository<InstaShareFile>
{
    Task<IEnumerable<InstaShareFile>> GetAllUserFilesAsync(long userId, bool IncludeRelation = false);
}