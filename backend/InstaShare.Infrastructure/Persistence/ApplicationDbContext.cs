using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using InstaShare.Domain.Entities.Common;
using InstaShare.Domain.Entities.Files;

namespace InstaShare.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
        public DbSet<InstaShareFile> Files => Set<InstaShareFile>();

        public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableBase && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AuditableBase)entityEntry.Entity).LastModified = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                ((AuditableBase)entityEntry.Entity).Created = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}