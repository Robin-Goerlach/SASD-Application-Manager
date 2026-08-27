namespace Sasd.FinanceControl.App.Presentation;

/// <summary>
/// Centralizes the labels and roadmap status for shell navigation.
/// </summary>
/// <remarks>
/// This is presentation metadata, not business logic. Deferred modules stay
/// visible as roadmap placeholders while Milestone 9 adds purchase orders and typed order/invoice traceability.
/// </remarks>
public static class NavigationCatalog
{
    private static readonly IReadOnlyDictionary<NavigationTarget, PageDescriptor> Pages =
        new Dictionary<NavigationTarget, PageDescriptor>
        {
            [NavigationTarget.Dashboard] = new(
                NavigationTarget.Dashboard,
                "Dashboard",
                "Technischer Startpunkt und Überblick über den aktuellen Entwicklungsstand.",
                "Milestone 9",
                IsImplemented: true),
            [NavigationTarget.Suppliers] = new(
                NavigationTarget.Suppliers,
                "Lieferanten",
                "Persistente Lieferantenstammdaten mit Suche, Bearbeitung und Aktivstatus.",
                "Phase 2 – Stammdaten",
                IsImplemented: true),
            [NavigationTarget.Categories] = new(
                NavigationTarget.Categories,
                "Kategorien",
                "Hierarchische Finanz- und Kostenkategorien mit Zyklenschutz.",
                "Phase 2 – Stammdaten",
                IsImplemented: true),
            [NavigationTarget.Documents] = new(
                NavigationTarget.Documents,
                "Dokumente",
                "Inhaltsadressierte, unveränderliche Ablage mit SHA-256, Integritätsprüfung und Verknüpfungen.",
                "Phase 3 – Dokumentenarchiv",
                IsImplemented: true),
            [NavigationTarget.Banking] = new(
                NavigationTarget.Banking,
                "Banking",
                "Bankkonten, CSV-Import und manuelle, nach Abschluss unveränderliche Kontoauszüge.",
                "Phase 4 – Banking",
                IsImplemented: true),
            [NavigationTarget.Payments] = new(
                NavigationTarget.Payments,
                "Zahlungszuordnung",
                "Unveränderliche Bankbewegungen fachlich mit Lieferanten, Kategorien und Klärungsstatus erklären.",
                "Phase 5 – Zahlungszuordnung",
                IsImplemented: true),
            [NavigationTarget.Reconciliation] = new(
                NavigationTarget.Reconciliation,
                "Zahlungsabgleich",
                "Rechnungen und Verträge mit Bankbewegungen abgleichen sowie Rechnungspositionen Projekten/Kostenstellen zuordnen.",
                "Phase 8 – Zahlungsabgleich & Kostenallokation",
                IsImplemented: true),
            [NavigationTarget.Contracts] = new(
                NavigationTarget.Contracts,
                "Verträge & Abos",
                "Vertragslaufzeiten, Abonnements, Kündigungsfristen, Dokumente und erwartete Zahlungen.",
                "Phase 6 – Verträge & Abos",
                IsImplemented: true),
            [NavigationTarget.Invoices] = new(
                NavigationTarget.Invoices,
                "Rechnungen",
                "Eingangsrechnungen mit stabilen Positionen, Lieferantenbezug und Dokumentverknüpfungen.",
                "Phase 7 – Rechnungen",
                IsImplemented: true),
            [NavigationTarget.Orders] = new(
                NavigationTarget.Orders,
                "Bestellungen",
                "Beschaffung mit stabilen Positionen, Lieferanten-, Kategorie-, Dokument- und Rechnungsbezug.",
                "Phase 9 – Bestellungen",
                IsImplemented: true),
            [NavigationTarget.Reports] = new(
                NavigationTarget.Reports,
                "Berichte",
                "Fixkosten-, Lieferanten- und Kostenanalysen.",
                "Phase 10 – Berichte & Monatskontrolle",
                IsImplemented: false),
            [NavigationTarget.Settings] = new(
                NavigationTarget.Settings,
                "Einstellungen",
                "Anwendungs- und spätere fachliche Konfiguration.",
                "spätere Ausbaustufe",
                IsImplemented: false),
        };

    /// <summary>Gets the descriptor for a navigation target.</summary>
    public static PageDescriptor Get(NavigationTarget target)
    {
        if (Pages.TryGetValue(target, out var descriptor))
        {
            return descriptor;
        }

        throw new ArgumentOutOfRangeException(nameof(target), target, "Unknown navigation target.");
    }
}
