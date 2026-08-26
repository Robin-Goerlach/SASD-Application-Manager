using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SASD.Bewerbungsmanager.Domain.Entities;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;
using SASD.Bewerbungsmanager.Domain.Enums;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;

namespace SASD.Bewerbungsmanager.Infrastructure.Tests;

public sealed class SqlitePersistenceTests
{
    [Fact]
    public async Task MigrationAndApplicationHistory_RoundTripThroughSqlite()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<ApplicationTrackerDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var setup = new ApplicationTrackerDbContext(options))
        {
            await setup.Database.MigrateAsync();
        }

        var opportunity = new Opportunity
        {
            Id = Guid.NewGuid(),
            Title = "System Engineer Linux",
            DescriptionSnapshot = "Synthetische Stellenbeschreibung für einen Integrationstest.",
            Status = OpportunityStatus.Applied,
            FoundAtUtc = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow,
        };
        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            OpportunityId = opportunity.Id,
            StartedAtUtc = DateTimeOffset.UtcNow,
            Channel = ApplicationChannel.Portal,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow,
        };
        application.InitializeStage(ApplicationStage.Submitted, DateTimeOffset.UtcNow, "Testversand");

        await using (var write = new ApplicationTrackerDbContext(options))
        {
            write.Opportunities.Add(opportunity);
            write.Applications.Add(application);
            await write.SaveChangesAsync();
        }

        await using var read = new ApplicationTrackerDbContext(options);
        var loaded = await read.Applications.Include("_statusHistory").SingleAsync();

        Assert.Equal(ApplicationStage.Submitted, loaded.Stage);
        Assert.Single(loaded.StatusHistory);
        Assert.Equal("Testversand", loaded.StatusHistory.Single().Note);
    }
}
