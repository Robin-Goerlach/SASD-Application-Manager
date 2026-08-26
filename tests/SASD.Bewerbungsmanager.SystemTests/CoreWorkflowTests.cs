using Xunit;
using Microsoft.EntityFrameworkCore;
using SASD.Bewerbungsmanager.Application.Abstractions;
using SASD.Bewerbungsmanager.Application.Models;
using SASD.Bewerbungsmanager.Application.Services;
using SASD.Bewerbungsmanager.Domain.Enums;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;

namespace SASD.Bewerbungsmanager.SystemTests;

public sealed class CoreWorkflowTests
{
    [Fact]
    public async Task OrganizationOpportunityApplicationStatusHistory_WorksAsOneCoreFlow()
    {
        var path = Path.Combine(Path.GetTempPath(), $"sasd-bewerbungsmanager-{Guid.NewGuid():N}.db");
        try
        {
            var factory = new TestDbContextFactory(path);
            await using (var context = await factory.CreateDbContextAsync())
            {
                await context.Database.MigrateAsync();
            }

            var clock = new FixedClock(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
            var store = new TrackerDataStore(factory);
            var organizations = new OrganizationService(store, clock);
            var opportunities = new OpportunityService(store, clock);
            var applications = new ApplicationService(store, clock);

            var employer = await organizations.CreateAsync(new OrganizationInput("Example Health IT GmbH", OrganizationType.Employer, null, null));
            var opportunity = await opportunities.CreateAsync(new OpportunityInput(
                employer.Id,
                null,
                "System Engineer Linux",
                "Verantwortung für eine synthetische Linux-Serverlandschaft.",
                "Beispielstadt",
                "Hybrid",
                null,
                OpportunityStatus.ApplicationPlanned,
                clock.UtcNow,
                null,
                null));
            var application = await applications.CreateAsync(new ApplicationInput(
                opportunity.Id,
                clock.UtcNow,
                null,
                ApplicationStage.Draft,
                ApplicationChannel.Email,
                "65–70 k€"));

            await applications.ChangeStageAsync(application.Id, ApplicationStage.Submitted, "Unterlagen versendet");
            var persisted = await store.GetApplicationAsync(application.Id);

            Assert.NotNull(persisted);
            var actual = persisted!;
            Assert.Equal(ApplicationStage.Submitted, actual.Stage);
            Assert.Equal(2, actual.StatusHistory.Count);
        }
        finally
        {
            TryDelete(path);
            TryDelete(path + "-shm");
            TryDelete(path + "-wal");
        }
    }

    private static void TryDelete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class TestDbContextFactory(string databasePath) : IDbContextFactory<ApplicationTrackerDbContext>
    {
        public ApplicationTrackerDbContext CreateDbContext()
            => new(new DbContextOptionsBuilder<ApplicationTrackerDbContext>().UseSqlite($"Data Source={databasePath}").Options);

        public Task<ApplicationTrackerDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(CreateDbContext());
    }
}
