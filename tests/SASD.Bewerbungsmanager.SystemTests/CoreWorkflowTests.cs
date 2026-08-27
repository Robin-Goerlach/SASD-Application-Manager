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
            var activities = new ActivityService(store, clock);
            var workItems = new WorkItemService(store, clock);
            var searches = new SearchProfileService(store, clock);

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

            await workItems.CreateAsync(new WorkItemInput(
                opportunity.Id,
                application.Id,
                null,
                employer.Id,
                WorkItemKind.Action,
                "Interview vorbereiten",
                null,
                clock.UtcNow));
            await workItems.CreateAsync(new WorkItemInput(
                opportunity.Id,
                application.Id,
                null,
                employer.Id,
                WorkItemKind.WaitingFor,
                "Rückmeldung zum Termin",
                null,
                clock.UtcNow.AddDays(2)));
            await activities.CreateAsync(new ActivityInput(
                opportunity.Id,
                application.Id,
                null,
                employer.Id,
                ActivityKind.Interview,
                ActivityStatus.Planned,
                "Technisches Interview",
                null,
                null,
                clock.UtcNow.AddDays(1)));
            await searches.CreateAsync(new SearchProfileInput(
                "Linux Jobs",
                "Example Portal",
                "https://example.invalid/jobs",
                1,
                clock.UtcNow,
                true,
                null));

            // Regression for the real WinForms startup path: DashboardService immediately loads
            // and orders opportunities/applications. SQLite cannot order DateTimeOffset in SQL,
            // so this call used to throw as soon as the dashboard became visible.
            var dashboard = new DashboardService(store);
            var summary = await dashboard.GetSummaryAsync();

            Assert.Equal(1, summary.ActiveOpportunities);
            Assert.Equal(1, summary.Applications);

            var today = await new TodayService(store, clock).GetOverviewAsync();
            Assert.Single(today.DueActions);
            Assert.Single(today.WaitingFor);
            Assert.Single(today.UpcomingAppointments);
            Assert.Single(today.DueSearchProfiles);

            var contextText = await new ApplicationContextService(store, clock).BuildAsync(application.Id);
            Assert.Contains("Interview vorbereiten", contextText, StringComparison.Ordinal);
            Assert.Contains("Rückmeldung zum Termin", contextText, StringComparison.Ordinal);

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
        {
            // Microsoft.Data.Sqlite enables connection pooling by default. For this file-based
            // system test we deliberately disable pooling so disposing the DbContext also closes
            // the underlying file handle immediately. Otherwise Windows can still see the
            // temporary database as in use when the test cleanup deletes it.
            var connectionString = $"Data Source={databasePath};Pooling=False;Foreign Keys=True";
            var options = new DbContextOptionsBuilder<ApplicationTrackerDbContext>()
                .UseSqlite(connectionString)
                .Options;

            return new ApplicationTrackerDbContext(options);
        }

        public Task<ApplicationTrackerDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(CreateDbContext());
    }
}
