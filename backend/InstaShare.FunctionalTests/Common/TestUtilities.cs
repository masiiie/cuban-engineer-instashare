using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InstaShare.Infrastructure.Persistence;
using InstaShare.Domain.Entities.Files;
using System.Threading;
using System.Threading.Tasks;
using InstaShare.Domain.Entities.Files;


namespace InstaShare.FunctionalTests.Common;

public static class TestUtilities
{
    public static async Task<int> GetExistingFileIdAsync(ApplicationDbContext dbContext)
    {        
        var file = await dbContext.Files.FirstOrDefaultAsync();
        
        if (file == null)
        {
            // If no files exist in the database, create one
            var newFile = new InstaShareFile("Test File", FileStatus.OnlyInDbNoContent, 1024);
            dbContext.Files.Add(newFile);
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            await dbContext.SaveChangesAsync(cancellationToken);
            return (int)newFile.Id;
        }
        
        return (int)file.Id;
    }
}