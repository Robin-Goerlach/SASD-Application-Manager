using SASD.Bewerbungsmanager.Domain.Entities;
using SASD.Bewerbungsmanager.Domain.Enums;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;

namespace SASD.Bewerbungsmanager.Application.Abstractions;

/// <summary>
/// Defines the persistence operations needed by the early application layer. It is deliberately a
/// single pragmatic port instead of a generic repository hierarchy for every entity.
/// </summary>
public interface ITrackerDataStore
{
    Task<IReadOnlyList<Organization>> ListOrganizationsAsync(bool includeArchived, CancellationToken cancellationToken = default);
    Task<Organization?> GetOrganizationAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddOrganizationAsync(Organization organization, CancellationToken cancellationToken = default);
    Task UpdateOrganizationAsync(Organization organization, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Contact>> ListContactsAsync(bool includeArchived, CancellationToken cancellationToken = default);
    Task<Contact?> GetContactAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddContactAsync(Contact contact, CancellationToken cancellationToken = default);
    Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Opportunity>> ListOpportunitiesAsync(CancellationToken cancellationToken = default);
    Task<Opportunity?> GetOpportunityAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddOpportunityAsync(Opportunity opportunity, CancellationToken cancellationToken = default);
    Task UpdateOpportunityAsync(Opportunity opportunity, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SourceLink>> ListSourceLinksAsync(Guid opportunityId, CancellationToken cancellationToken = default);
    Task AddSourceLinkAsync(SourceLink sourceLink, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobApplication>> ListApplicationsAsync(CancellationToken cancellationToken = default);
    Task<JobApplication?> GetApplicationAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddApplicationAsync(JobApplication application, CancellationToken cancellationToken = default);
    Task ChangeApplicationStageAsync(Guid applicationId, ApplicationStage stage, DateTimeOffset changedAtUtc, string? note, CancellationToken cancellationToken = default);
}
