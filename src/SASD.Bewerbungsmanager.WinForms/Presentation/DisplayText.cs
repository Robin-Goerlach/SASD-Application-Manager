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
}
