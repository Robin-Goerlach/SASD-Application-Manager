using Sasd.FinanceControl.App.Presentation;

namespace Sasd.FinanceControl.App.Views;

/// <summary>
/// Displays a truthful Milestone 9 dashboard without inventing accounting
/// figures that are not derived from persisted domain relations.
/// </summary>
public sealed class DashboardView : UserControl
{
    /// <summary>Initializes the dashboard.</summary>
    public DashboardView(PageDescriptor page, ShellStatus? status)
    {
        ArgumentNullException.ThrowIfNull(page);
        AutoScroll = true;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 0,
            Padding = new Padding(0),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        layout.Controls.Add(CreateHeading("Milestone 9 – Bestellungen & Beschaffung"));
        layout.Controls.Add(CreateParagraph(
            "Bestellungen und Bestellpositionen werden jetzt als eigene Beschaffungsobjekte geführt. " +
            "Lieferant, Geschäftszweck, Liefertermin, Kategorien und Asset-/Inventar-Kandidaten bleiben nachvollziehbar zusammen."));

        layout.Controls.Add(CreateGroup(
            "Bestellung → Rechnung → Zahlung",
            "Bestellungen können mit passenden Lieferantenrechnungen verknüpft werden. Die tatsächliche Zahlung bleibt weiterhin über die Rechnungsallokation " +
            "mit der unveränderlichen Banktransaktion verbunden; dadurch wird kein Geldbetrag doppelt verbucht."));

        layout.Controls.Add(CreateGroup(
            "Beschaffungsdetails",
            "Bestellpositionen besitzen stabile technische IDs, berechnen Netto/Steuer/Brutto deterministisch und können Kategorien sowie Asset-/Inventar-Kandidaten tragen."));

        layout.Controls.Add(CreateGroup(
            "Dokumente und Historie",
            "Archivierte Angebote, Bestellbestätigungen oder Lieferscheine können mit einer Bestellung verknüpft werden. Rechnungslinks werden bei Korrekturen storniert statt gelöscht."));

        layout.Controls.Add(CreateGroup(
            "Fachlicher Leitgrundsatz",
            "Der Kontoauszug bleibt der Single Point of Truth für tatsächlich geflossene Zahlungen. Rechnungen, Verträge und Allokationen erklären die Bankbewegung, " +
            "verändern sie aber niemals."));

        layout.Controls.Add(CreateGroup(
            "Als Nächstes",
            "Milestone 10 konzentriert sich auf Berichte und Monatskontrolle: offene Rechnungen, ungeklärte Zahlungen, Vertragsfristen und Kostenanalysen."));

        if (status is not null)
        {
            layout.Controls.Add(CreateGroup(
                "Lokale Laufzeitumgebung",
                $"Environment: {status.EnvironmentName}{Environment.NewLine}" +
                $"Daten: {status.DataDirectory}{Environment.NewLine}" +
                $"Logs: {status.LogDirectory}"));
        }

        Controls.Add(layout);
    }

    private Label CreateHeading(string text)
        => new()
        {
            AutoSize = true,
            Font = new Font(Font, FontStyle.Bold),
            Text = text,
            Margin = new Padding(0, 0, 0, 8),
        };

    private static Label CreateParagraph(string text)
        => new()
        {
            AutoSize = true,
            MaximumSize = new Size(850, 0),
            Text = text,
            Margin = new Padding(0, 0, 0, 18),
        };

    private static GroupBox CreateGroup(string title, string text)
    {
        var label = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            MaximumSize = new Size(800, 0),
            Text = text,
            Padding = new Padding(8),
        };

        var group = new GroupBox
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            Text = title,
            Padding = new Padding(8),
            Margin = new Padding(0, 0, 0, 14),
        };
        group.Controls.Add(label);
        return group;
    }
}
