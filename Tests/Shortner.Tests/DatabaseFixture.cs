using Microsoft.EntityFrameworkCore;
using Shortner.Web.Context.Models;
using Shortner.Web.Context;

namespace Shortner.Tests
{
    public class DatabaseFixture: IDisposable
    {
        public DataStoreContext Context { get; private set; }
        public DatabaseFixture() 
        {
            var options = new DbContextOptionsBuilder<DataStoreContext>()
                .UseInMemoryDatabase(databaseName: "TestDataStore")
                .Options;

            Context = new DataStoreContext(options);

            // Seed some data
            Context.UrlShorteners.AddRange(
                new UrlShortener { UrlShortenerId = 1, ShortUrl = "https://short.ly/abc123", OriginalUrl = "https://example.com", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new UrlShortener { UrlShortenerId = 2, ShortUrl = "https://short.ly/xyz456", OriginalUrl = "https://another.com", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now }
            );

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
