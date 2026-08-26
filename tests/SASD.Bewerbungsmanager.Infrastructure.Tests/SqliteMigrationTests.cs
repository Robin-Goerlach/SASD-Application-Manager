using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SASD.Bewerbungsmanager.Infrastructure.Paths;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;

namespace SASD.Bewerbungsmanager.Infrastructure.Tests;

public sealed class SqliteMigrationTests
{
    [Fact]
    public void Initial_migration_creates_a_writable_real_sqlite_database()
    {
        var root = Path.Combine(Path.GetTempPath(), "SASD-Bewerbungsmanager-tests", Guid.NewGuid().ToString("N"));

        try
        {
            var paths = new ApplicationPaths(root);
            paths.EnsureDirectoriesExist();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(paths.ConnectionString)
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Database.Migrate();
                dbContext.SystemMetadata.Add(new SystemMetadataRecord
                {
                    Key = "IntegrationTest",
                    Value = "OK",
                    UpdatedAtUtc = DateTime.UtcNow,
                });
                dbContext.SaveChanges();
            }

            using (var dbContext = new ApplicationDbContext(options))
            {
                var record = dbContext.SystemMetadata.Single(item => item.Key == "IntegrationTest");
                Assert.Equal("OK", record.Value);
            }
        }
        finally
        {
            SqliteConnection.ClearAllPools();

            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }
}
