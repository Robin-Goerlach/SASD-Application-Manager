using SASD.Bewerbungsmanager.Domain.Enums;

namespace SASD.Bewerbungsmanager.WinForms.Presentation;

/// <summary>Centralizes concise German labels used by the WinForms presentation layer.</summary>
public static class DisplayText
{
    /// <summary>Returns a user-facing label for an opportunity status.</summary>
    public static string OpportunityStatus(OpportunityStatus status) => status switch
    {
        SASD.Bewerbungsmanager.Domain.Enums.OpportunityStatus.Identified => "Gefunden",
        SASD.Bewerbungsmanager.Domain.Enums.OpportunityStatus.Contacted => "Kontakt",
        SASD.Bewerbungsmanager.Domain.Enums.OpportunityStatus.ApplicationPlanned => "Bewerbung geplant",
        SASD.Bewerbungsmanager.Domain.Enums.OpportunityStatus.Applied => "Beworben",
        SASD.Bewerbungsmanager.Domain.Enums.OpportunityStatus.Interview => "Interview",
        SASD.Bewerbungsmanager.Domain.Enums.OpportunityStatus.Offer => "Angebot",
        SASD.Bewerbungsmanager.Domain.Enums.OpportunityStatus.Closed => "Abgeschlossen",
        _ => status.ToString(),
    };

    /// <summary>Returns a user-facing label for an application stage.</summary>
    public static string ApplicationStage(ApplicationStage stage) => stage switch
    {
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Draft => "Entwurf",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Submitted => "Versendet",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Screening => "Prüfung",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Interview => "Interview",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Offer => "Angebot",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Rejected => "Absage",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Withdrawn => "Zurückgezogen",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Hired => "Eingestellt",
        SASD.Bewerbungsmanager.Domain.Enums.ApplicationStage.Closed => "Abgeschlossen",
        _ => stage.ToString(),
    };

    /// <summary>Returns a user-facing label for an operational work-item kind.</summary>
    public static string WorkItemKind(WorkItemKind kind) => kind switch
    {
        SASD.Bewerbungsmanager.Domain.Enums.WorkItemKind.Action => "ACTION",
        SASD.Bewerbungsmanager.Domain.Enums.WorkItemKind.WaitingFor => "WAITING_FOR",
        _ => kind.ToString(),
    };

    /// <summary>Returns a user-facing label for a work-item lifecycle state.</summary>
    public static string WorkItemStatus(WorkItemStatus status) => status switch
    {
        SASD.Bewerbungsmanager.Domain.Enums.WorkItemStatus.Open => "Offen",
        SASD.Bewerbungsmanager.Domain.Enums.WorkItemStatus.Completed => "Erledigt",
        SASD.Bewerbungsmanager.Domain.Enums.WorkItemStatus.Cancelled => "Abgebrochen",
        _ => status.ToString(),
    };

    /// <summary>Returns a user-facing label for an activity type.</summary>
    public static string ActivityKind(ActivityKind kind) => kind switch
    {
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.Email => "E-Mail",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.PhoneCall => "Telefonat",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.LinkedIn => "LinkedIn",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.ApplicationSubmitted => "Bewerbung versendet",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.Interview => "Interview",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.Meeting => "Meeting",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.AuthorityAppointment => "Behördentermin",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.Note => "Notiz",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityKind.Other => "Sonstiges",
        _ => kind.ToString(),
    };

    /// <summary>Returns a user-facing label for an activity lifecycle state.</summary>
    public static string ActivityStatus(ActivityStatus status) => status switch
    {
        SASD.Bewerbungsmanager.Domain.Enums.ActivityStatus.Recorded => "Stattgefunden",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityStatus.Planned => "Geplant",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityStatus.Completed => "Erledigt",
        SASD.Bewerbungsmanager.Domain.Enums.ActivityStatus.Cancelled => "Abgesagt",
        _ => status.ToString(),
    };
}
