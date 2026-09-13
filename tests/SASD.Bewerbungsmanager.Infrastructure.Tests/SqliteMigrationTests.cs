using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;

namespace SASD.Bewerbungsmanager.Infrastructure.Tests;

/// <summary>Verifies the complete productive migration chain against a real SQLite database.</summary>
public sealed class SqliteMigrationTests
{
    private static readonly string[] ExpectedMigrations =
    [
        "202608260001_InitialMilestone1",
        "202608260002_OperationalMvp",
        "202608270003_CommunicationIntegration",
        "202608270004_JobSearchAdapters",
        "202608270005_AssistantWorkspace",
    ];

    [Fact]
    public async Task Productive_migration_chain_creates_the_complete_current_schema()
    {
        var path = Path.Combine(Path.GetTempPath(), $"sasd-migrations-{Guid.NewGuid():N}.db");

        try
        {
            var options = new DbContextOptionsBuilder<ApplicationTrackerDbContext>()
                .UseSqlite($"Data Source={path};Pooling=False;Foreign Keys=True")
                .Options;

            await using var dbContext = new ApplicationTrackerDbContext(options);
            await dbContext.Database.MigrateAsync();

            var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
            Assert.Equal(ExpectedMigrations, appliedMigrations);
            Assert.Empty(await dbContext.Database.GetPendingMigrationsAsync());

            var expectedTables = new[]
            {
                "activities",
                "application_document_snapshots",
                "application_status_history",
                "applications",
                "assistant_sessions",
                "communication_messages",
                "contacts",
                "documents",
                "job_leads",
                "opportunities",
                "organizations",
                "search_profiles",
                "source_links",
                "work_items",
            };

            var actualTables = await ReadApplicationTablesAsync(dbContext);
            Assert.Equal(expectedTables, actualTables);
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    private static async Task<IReadOnlyList<string>> ReadApplicationTablesAsync(
        ApplicationTrackerDbContext dbContext)
    {
        var connection = dbContext.Database.GetDbConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT name FROM sqlite_master " +
            "WHERE type = 'table' AND name NOT LIKE 'sqlite_%' AND name <> '__EFMigrationsHistory' " +
            "ORDER BY name;";

        var tables = new List<string>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tables.Add(reader.GetString(0));
        }

        return tables;
    }
}
