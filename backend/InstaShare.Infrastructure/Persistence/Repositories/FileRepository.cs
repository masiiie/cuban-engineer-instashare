using InstaShare.Domain.Entities.Files;
using InstaShare.Domain.Repositories;
using InstaShare.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;


namespace InstaShare.Infrastucture.Persistence.Repositories;

public class FileRepository : IFileRepository
{
    ApplicationDbContext _dbContext;
    public FileRepository(ApplicationDbContext dbontext)
    {
        _dbContext = dbontext;
    }

    public void Create(InstaShareFile file)
    {
        _dbContext.Files.Add(file);
    }

    public void Update(InstaShareFile file)
    {
        _dbContext.Files.Update(file);
    }

    public void Remove(InstaShareFile file)
    {
        _dbContext.Files.Remove(file);
    }

    public async Task<IEnumerable<InstaShareFile>> GetAllAsync(bool IncludeRelation = false)
    {
        /*
        if (IncludeRelation)
        {
            return await _dbContext.Files.Include(x => x.User).ToListAsync();
        }
        */
        return await _dbContext.Files.ToListAsync();
    }

    public async Task<InstaShareFile> GetByIdAsync(long id, bool IncludeRelation = false)
    {
        /*
        if (IncludeRelation)
        {
            return await _dbContext.Files.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        */
        return await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == id);
    }   

    /*
    public async Task<IEnumerable<InstaShareFile>> GetAllUserFilesAsync(long userId, bool IncludeRelation = false)
    {
        if (IncludeRelation)
        {
            return await _dbContext.Files.Include(x => x.User).Where(x => x.UserId == userId).ToListAsync();
        }
        return await _dbContext.Files.Where(x => x.UserId == userId).ToListAsync();
    }
    */

    public async Task<IEnumerable<InstaShareFile>> GetAllUserFilesAsync(long userId, bool IncludeRelation = false)
    {
        throw new NotImplementedException("This method is not implemented yet.");
    }
}