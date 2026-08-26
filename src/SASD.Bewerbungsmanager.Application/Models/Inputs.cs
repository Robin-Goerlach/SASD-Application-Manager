using SASD.Bewerbungsmanager.Domain.Enums;

namespace SASD.Bewerbungsmanager.Application.Models;

/// <summary>Input used to create or update an organization.</summary>
public sealed record OrganizationInput(string Name, OrganizationType Type, string? Website, string? Notes);

/// <summary>Input used to create or update a professional contact.</summary>
public sealed record ContactInput(Guid? OrganizationId, string FullName, string? Role, string? Email, string? Phone, string? LinkedInUrl, string? Notes);

/// <summary>Input used to create or update an opportunity.</summary>
public sealed record OpportunityInput(
    Guid? EmployerOrganizationId,
    Guid? IntermediaryOrganizationId,
    string Title,
    string DescriptionSnapshot,
    string? Location,
    string? RemoteText,
    string? SalaryText,
    OpportunityStatus Status,
    DateTimeOffset FoundAtUtc,
    DateTimeOffset? PublishedAtUtc,
    DateTimeOffset? DeadlineAtUtc);

/// <summary>Input used to attach an external source to an opportunity.</summary>
public sealed record SourceLinkInput(string Source, string Url, string? ExternalId);

/// <summary>Input used to create a concrete application.</summary>
public sealed record ApplicationInput(
    Guid OpportunityId,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset? SubmittedAtUtc,
    ApplicationStage Stage,
    ApplicationChannel Channel,
    string? SalaryExpectation);

/// <summary>Read model for the small operational dashboard included in Milestone 1.</summary>
public sealed record DashboardSummary(int ActiveOpportunities, int Applications, int Interviews, int Offers);
