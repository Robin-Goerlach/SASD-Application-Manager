using Microsoft.EntityFrameworkCore;
using SASD.Bewerbungsmanager.Application.Abstractions;
using SASD.Bewerbungsmanager.Domain.Entities;
using SASD.Bewerbungsmanager.Domain.Enums;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;
using TrackerActivity = SASD.Bewerbungsmanager.Domain.Entities.Activity;
using TrackerDocument = SASD.Bewerbungsmanager.Domain.Entities.Document;

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of the pragmatic persistence port. Every operation obtains a short-lived
/// DbContext from the factory, so no context crosses UI operations or threads.
/// </summary>
public sealed class TrackerDataStore(IDbContextFactory<ApplicationTrackerDbContext> contextFactory) : ITrackerDataStore
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<Organization>> ListOrganizationsAsync(bool includeArchived, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var query = context.Organizations.AsNoTracking();
        if (!includeArchived)
        {
            query = query.Where(item => !item.IsArchived);
        }

        return await query.OrderBy(item => item.Name).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Organization?> GetOrganizationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.Organizations.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddOrganizationAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(organization);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Organizations.Add(organization);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateOrganizationAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(organization);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Organizations.Update(organization);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Contact>> ListContactsAsync(bool includeArchived, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var query = context.Contacts.AsNoTracking();
        if (!includeArchived)
        {
            query = query.Where(item => !item.IsArchived);
        }

        return await query.OrderBy(item => item.FullName).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Contact?> GetContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.Contacts.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddContactAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contact);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Contacts.Add(contact);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(contact);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Contacts.Update(contact);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Opportunity>> ListOpportunitiesAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var items = await context.Opportunities.AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Microsoft.EntityFrameworkCore.Sqlite can persist DateTimeOffset but cannot translate
        // ordering/comparison for it. The personal dataset is small, so deterministic in-memory
        // ordering is preferable to leaking provider-specific timestamp types into the domain.
        return items
            .OrderByDescending(item => item.FoundAtUtc)
            .ThenBy(item => item.Title)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<Opportunity?> GetOpportunityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.Opportunities.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddOpportunityAsync(Opportunity opportunity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(opportunity);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Opportunities.Add(opportunity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateOpportunityAsync(Opportunity opportunity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(opportunity);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Opportunities.Update(opportunity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<SourceLink>> ListSourceLinksAsync(Guid opportunityId, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var items = await context.SourceLinks.AsNoTracking()
            .Where(item => item.OpportunityId == opportunityId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return items.OrderByDescending(item => item.CapturedAtUtc).ToList();
    }

    /// <inheritdoc />
    public async Task AddSourceLinkAsync(SourceLink sourceLink, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sourceLink);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.SourceLinks.Add(sourceLink);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<JobApplication>> ListApplicationsAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var items = await context.Applications.AsNoTracking()
            .Include("_statusHistory")
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return items.OrderByDescending(item => item.StartedAtUtc).ToList();
    }

    /// <inheritdoc />
    public async Task<JobApplication?> GetApplicationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.Applications.AsNoTracking()
            .Include("_statusHistory")
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddApplicationAsync(JobApplication application, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Applications.Add(application);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateApplicationSubmissionAsync(
        Guid applicationId,
        DateTimeOffset? submittedAtUtc,
        ApplicationChannel channel,
        DateTimeOffset updatedAtUtc,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var application = await context.Applications
            .SingleOrDefaultAsync(item => item.Id == applicationId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new KeyNotFoundException("Die Bewerbung wurde nicht gefunden.");

        application.SubmittedAtUtc = submittedAtUtc;
        application.Channel = channel;
        application.UpdatedAtUtc = updatedAtUtc;
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task ChangeApplicationStageAsync(
        Guid applicationId,
        ApplicationStage stage,
        DateTimeOffset changedAtUtc,
        string? note,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var application = await context.Applications
            .Include("_statusHistory")
            .SingleOrDefaultAsync(item => item.Id == applicationId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new KeyNotFoundException("Die Bewerbung wurde nicht gefunden.");

        // Current stage and audit history are committed in one SaveChanges operation.
        application.ChangeStage(stage, changedAtUtc, note);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TrackerActivity>> ListActivitiesAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var items = await context.Activities.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        return items
            .OrderByDescending(item => item.OccurredAtUtc ?? item.ScheduledAtUtc ?? item.CreatedAtUtc)
            .ThenBy(item => item.Subject)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<TrackerActivity?> GetActivityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.Activities.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddActivityAsync(TrackerActivity activity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(activity);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Activities.Add(activity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateActivityAsync(TrackerActivity activity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(activity);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Activities.Update(activity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TrackerTask>> ListTasksAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var items = await context.Tasks.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        return items
            .OrderBy(item => item.Status)
            .ThenBy(item => item.DueAtUtc ?? DateTimeOffset.MaxValue)
            .ThenBy(item => item.Title)
            .ToList();
    }

    /// <inheritdoc />
    public async Task<TrackerTask?> GetTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.Tasks.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddTaskAsync(TrackerTask task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Tasks.Add(task);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateTaskAsync(TrackerTask task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Tasks.Update(task);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<SearchProfile>> ListSearchProfilesAsync(bool includeInactive, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var query = context.SearchProfiles.AsNoTracking();
        if (!includeInactive)
        {
            query = query.Where(item => item.IsActive);
        }

        var items = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        return items.OrderBy(item => item.NextCheckAtUtc).ThenBy(item => item.Name).ToList();
    }

    /// <inheritdoc />
    public async Task<SearchProfile?> GetSearchProfileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.SearchProfiles.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddSearchProfileAsync(SearchProfile profile, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(profile);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.SearchProfiles.Add(profile);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateSearchProfileAsync(SearchProfile profile, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(profile);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.SearchProfiles.Update(profile);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TrackerDocument>> ListDocumentsAsync(bool includeArchived, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var query = context.Documents.AsNoTracking();
        if (!includeArchived)
        {
            query = query.Where(item => !item.IsArchived);
        }

        return await query
            .OrderBy(item => item.Type)
            .ThenBy(item => item.Label)
            .ThenByDescending(item => item.Version)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TrackerDocument?> GetDocumentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        return await context.Documents.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddDocumentAsync(TrackerDocument document, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Documents.Add(document);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UpdateDocumentAsync(TrackerDocument document, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.Documents.Update(document);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ApplicationDocumentSnapshot>> ListApplicationDocumentSnapshotsAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        var items = await context.ApplicationDocumentSnapshots.AsNoTracking()
            .Where(item => item.ApplicationId == applicationId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return items.OrderBy(item => item.CapturedAtUtc).ThenBy(item => item.Type).ToList();
    }

    /// <inheritdoc />
    public async Task AddApplicationDocumentSnapshotAsync(
        ApplicationDocumentSnapshot snapshot,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        context.ApplicationDocumentSnapshots.Add(snapshot);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
