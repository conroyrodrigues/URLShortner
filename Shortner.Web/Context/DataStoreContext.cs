using Microsoft.EntityFrameworkCore;
using Shortner.Web.Context.Models;

namespace Shortner.Web.Context
{
    public class DataStoreContext: DbContext
    {
        public DataStoreContext(DbContextOptions<DataStoreContext> options) : base(options) { }

        public DbSet<UrlShortener> UrlShorteners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Enforce Unique Constraint
            modelBuilder.Entity<UrlShortener>()
                .HasIndex(u => u.OriginalUrl)
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Example: Enforce uniqueness of OriginalUrl in memory
            foreach (var entity in ChangeTracker.Entries<UrlShortener>())
            {
                if (entity.State == EntityState.Added || entity.State == EntityState.Modified)
                {
                    var shortUrl = entity.Entity.OriginalUrl;
                    if (UrlShorteners.Any(u => u.OriginalUrl == shortUrl))
                    {
                        throw new DbUpdateException("The Original Url must be unique.");
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
