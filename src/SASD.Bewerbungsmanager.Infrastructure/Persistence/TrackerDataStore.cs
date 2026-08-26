using Microsoft.EntityFrameworkCore;
using SASD.Bewerbungsmanager.Application.Abstractions;
using SASD.Bewerbungsmanager.Domain.Entities;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;
using SASD.Bewerbungsmanager.Domain.Enums;

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of the early persistence port. Every operation creates and disposes a
/// short-lived DbContext; no context is shared with WinForms controls or across threads.
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
        return await context.Opportunities.AsNoTracking()
            .OrderByDescending(item => item.FoundAtUtc)
            .ThenBy(item => item.Title)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
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
        return await context.SourceLinks.AsNoTracking()
            .Where(item => item.OpportunityId == opportunityId)
            .OrderByDescending(item => item.CapturedAtUtc)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
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
        return await context.Applications.AsNoTracking()
            .Include("_statusHistory")
            .OrderByDescending(item => item.StartedAtUtc)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
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

        // Updating the aggregate and its history inside one DbContext/SaveChanges keeps the current
        // stage and audit trail transactionally consistent.
        application.ChangeStage(stage, changedAtUtc, note);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
