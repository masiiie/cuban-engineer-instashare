using Microsoft.EntityFrameworkCore;
using InstaShare.Domain.Entities.Files;

namespace InstaShare.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
        public DbSet<InstaShareFile> Files => Set<InstaShareFile>();
    }
}