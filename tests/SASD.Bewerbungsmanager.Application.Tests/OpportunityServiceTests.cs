using Xunit;
using SASD.Bewerbungsmanager.Application.Abstractions;
using SASD.Bewerbungsmanager.Application.Exceptions;
using SASD.Bewerbungsmanager.Application.Models;
using SASD.Bewerbungsmanager.Application.Services;
using SASD.Bewerbungsmanager.Domain.Entities;
using SASD.Bewerbungsmanager.Domain.Enums;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;

namespace SASD.Bewerbungsmanager.Application.Tests;

public sealed class OpportunityServiceTests
{
    [Fact]
    public async Task CreateAsync_PreservesRoleDescriptionAsSnapshot()
    {
        var store = new MemoryStore();
        var employer = new Organization { Id = Guid.NewGuid(), Name = "Example Health IT GmbH" };
        store.Organizations.Add(employer);
        var clock = new FixedClock(new DateTimeOffset(2026, 8, 26, 7, 0, 0, TimeSpan.Zero));
        var service = new OpportunityService(store, clock);

        var created = await service.CreateAsync(new OpportunityInput(
            employer.Id,
            null,
            "System Engineer Linux",
            "Betrieb und Weiterentwicklung einer Linux-Plattform.",
            "Beispielstadt",
            "Hybrid",
            "60–70 k€",
            OpportunityStatus.Identified,
            clock.UtcNow,
            null,
            null));

        Assert.Equal("Betrieb und Weiterentwicklung einer Linux-Plattform.", created.DescriptionSnapshot);
        Assert.Single(store.Opportunities);
    }

    [Fact]
    public async Task CreateAsync_WhenEmployerAndIntermediaryAreSame_RejectsInput()
    {
        var store = new MemoryStore();
        var organization = new Organization { Id = Guid.NewGuid(), Name = "Example Recruiting GmbH" };
        store.Organizations.Add(organization);
        var service = new OpportunityService(store, new FixedClock(DateTimeOffset.UtcNow));

        var input = new OpportunityInput(
            organization.Id,
            organization.Id,
            "Platform Engineer",
            "Synthetische Rollenbeschreibung",
            null,
            null,
            null,
            OpportunityStatus.Identified,
            DateTimeOffset.UtcNow,
            null,
            null);

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(input));
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }

    private sealed class MemoryStore : ITrackerDataStore
    {
        public List<Organization> Organizations { get; } = [];
        public List<Contact> Contacts { get; } = [];
        public List<Opportunity> Opportunities { get; } = [];
        public List<SourceLink> SourceLinks { get; } = [];
        public List<JobApplication> Applications { get; } = [];

        public Task<IReadOnlyList<Organization>> ListOrganizationsAsync(bool includeArchived, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<Organization>>(Organizations.Where(item => includeArchived || !item.IsArchived).ToList());

        public Task<Organization?> GetOrganizationAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(Organizations.SingleOrDefault(item => item.Id == id));

        public Task AddOrganizationAsync(Organization organization, CancellationToken cancellationToken = default)
        {
            Organizations.Add(organization);
            return Task.CompletedTask;
        }

        public Task UpdateOrganizationAsync(Organization organization, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<IReadOnlyList<Contact>> ListContactsAsync(bool includeArchived, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<Contact>>(Contacts.Where(item => includeArchived || !item.IsArchived).ToList());

        public Task<Contact?> GetContactAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(Contacts.SingleOrDefault(item => item.Id == id));

        public Task AddContactAsync(Contact contact, CancellationToken cancellationToken = default)
        {
            Contacts.Add(contact);
            return Task.CompletedTask;
        }

        public Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<IReadOnlyList<Opportunity>> ListOpportunitiesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<Opportunity>>(Opportunities.ToList());

        public Task<Opportunity?> GetOpportunityAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(Opportunities.SingleOrDefault(item => item.Id == id));

        public Task AddOpportunityAsync(Opportunity opportunity, CancellationToken cancellationToken = default)
        {
            Opportunities.Add(opportunity);
            return Task.CompletedTask;
        }

        public Task UpdateOpportunityAsync(Opportunity opportunity, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<IReadOnlyList<SourceLink>> ListSourceLinksAsync(Guid opportunityId, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<SourceLink>>(SourceLinks.Where(item => item.OpportunityId == opportunityId).ToList());

        public Task AddSourceLinkAsync(SourceLink sourceLink, CancellationToken cancellationToken = default)
        {
            SourceLinks.Add(sourceLink);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<JobApplication>> ListApplicationsAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<JobApplication>>(Applications.ToList());

        public Task<JobApplication?> GetApplicationAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(Applications.SingleOrDefault(item => item.Id == id));

        public Task AddApplicationAsync(JobApplication application, CancellationToken cancellationToken = default)
        {
            Applications.Add(application);
            return Task.CompletedTask;
        }

        public Task ChangeApplicationStageAsync(Guid applicationId, ApplicationStage stage, DateTimeOffset changedAtUtc, string? note, CancellationToken cancellationToken = default)
        {
            var application = Applications.Single(item => item.Id == applicationId);
            application.ChangeStage(stage, changedAtUtc, note);
            return Task.CompletedTask;
        }
    }
}
