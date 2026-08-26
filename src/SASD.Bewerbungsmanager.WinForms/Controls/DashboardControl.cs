using SASD.Bewerbungsmanager.Application.Models;
using SASD.Bewerbungsmanager.Application.Services;
using SASD.Bewerbungsmanager.WinForms.Presentation;

namespace SASD.Bewerbungsmanager.WinForms.Controls;

/// <summary>Displays the small Milestone-1 dashboard and leaves operational due-item logic for the next milestone.</summary>
public sealed class DashboardControl : UserControl
{
    private readonly DashboardService _service;
    private readonly UiExceptionPresenter _errors;
    private readonly Label _active = MetricLabel();
    private readonly Label _applications = MetricLabel();
    private readonly Label _interviews = MetricLabel();
    private readonly Label _offers = MetricLabel();

    /// <summary>Initializes the dashboard view.</summary>
    public DashboardControl(DashboardService service, UiExceptionPresenter errors)
    {
        _service = service;
        _errors = errors;
        BuildLayout();
        Load += async (_, _) => await RefreshAsync();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        root.Controls.Add(new Label
        {
            Text = "Heute / Übersicht",
            AutoSize = true,
            Font = new Font(Font.FontFamily, 18, FontStyle.Bold),
            Margin = new Padding(0, 0, 0, 16),
        }, 0, 0);

        var metrics = new TableLayoutPanel { AutoSize = true, ColumnCount = 4, Dock = DockStyle.Top };
        metrics.Controls.Add(MetricPanel("Aktive Stellen", _active), 0, 0);
        metrics.Controls.Add(MetricPanel("Bewerbungen", _applications), 1, 0);
        metrics.Controls.Add(MetricPanel("Interviews", _interviews), 2, 0);
        metrics.Controls.Add(MetricPanel("Angebote", _offers), 3, 0);
        root.Controls.Add(metrics, 0, 1);

        root.Controls.Add(new Label
        {
            Text = "Im nächsten Milestone wird diese Seite zum operativen Cockpit: fällige ACTIONs, WAITING_FOR, Termine und Suchprüfungen.",
            AutoSize = true,
            Margin = new Padding(0, 28, 0, 0),
        }, 0, 2);
        Controls.Add(root);
    }

    private async Task RefreshAsync()
    {
        try
        {
            var summary = await _service.GetSummaryAsync();
            Apply(summary);
        }
        catch (Exception ex)
        {
            _errors.Show(ex, this);
        }
    }

    private void Apply(DashboardSummary summary)
    {
        _active.Text = summary.ActiveOpportunities.ToString(System.Globalization.CultureInfo.CurrentCulture);
        _applications.Text = summary.Applications.ToString(System.Globalization.CultureInfo.CurrentCulture);
        _interviews.Text = summary.Interviews.ToString(System.Globalization.CultureInfo.CurrentCulture);
        _offers.Text = summary.Offers.ToString(System.Globalization.CultureInfo.CurrentCulture);
    }

    private static Label MetricLabel() => new()
    {
        AutoSize = true,
        Font = new Font(SystemFonts.DefaultFont.FontFamily, 20, FontStyle.Bold),
    };

    private static Control MetricPanel(string title, Label value)
    {
        var panel = new TableLayoutPanel { AutoSize = true, Padding = new Padding(12), Margin = new Padding(0, 0, 12, 0) };
        panel.Controls.Add(new Label { Text = title, AutoSize = true }, 0, 0);
        panel.Controls.Add(value, 0, 1);
        return panel;
    }
}
