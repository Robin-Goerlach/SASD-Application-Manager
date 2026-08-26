using Microsoft.Extensions.DependencyInjection;
using SASD.Bewerbungsmanager.WinForms.Controls;

namespace SASD.Bewerbungsmanager.WinForms.Forms;

/// <summary>
/// Main application shell. Milestone 1 uses a stable Windows business-application layout with
/// navigation on the left and one working area on the right; individual controls own no business logic.
/// </summary>
public sealed class MainForm : Form
{
    private readonly IServiceProvider _services;
    private readonly Panel _contentPanel = new() { Dock = DockStyle.Fill, Padding = new Padding(12) };
    private Control? _currentView;

    /// <summary>Initializes the main application window.</summary>
    public MainForm(IServiceProvider services)
    {
        _services = services;
        Text = "SASD Bewerbungsmanager — Milestone 1";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1100, 700);
        Size = new Size(1280, 800);
        AutoScaleMode = AutoScaleMode.Dpi;

        var navigation = BuildNavigation();
        Controls.Add(_contentPanel);
        Controls.Add(navigation);
        Shown += (_, _) => ShowView<DashboardControl>();
    }

    private Control BuildNavigation()
    {
        var navigation = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 190,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(10, 16, 10, 10),
        };

        var title = new Label
        {
            Text = "SASD\nBewerbungsmanager",
            AutoSize = false,
            Width = 165,
            Height = 56,
            Font = new Font(Font.FontFamily, 11, FontStyle.Bold),
        };
        navigation.Controls.Add(title);
        navigation.Controls.Add(NavButton("Heute / Übersicht", () => ShowView<DashboardControl>()));
        navigation.Controls.Add(NavButton("Organisationen", () => ShowView<OrganizationsControl>()));
        navigation.Controls.Add(NavButton("Kontakte", () => ShowView<ContactsControl>()));
        navigation.Controls.Add(NavButton("Stellen", () => ShowView<OpportunitiesControl>()));
        navigation.Controls.Add(NavButton("Bewerbungen", () => ShowView<ApplicationsControl>()));
        return navigation;
    }

    private Button NavButton(string text, Action action)
    {
        var button = new Button
        {
            Text = text,
            Width = 165,
            Height = 42,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 0, 0, 6),
        };
        button.Click += (_, _) => action();
        return button;
    }

    private void ShowView<TControl>() where TControl : Control
    {
        var next = ActivatorUtilities.CreateInstance<TControl>(_services);
        next.Dock = DockStyle.Fill;

        _contentPanel.SuspendLayout();
        try
        {
            _contentPanel.Controls.Clear();
            _currentView?.Dispose();
            _currentView = next;
            _contentPanel.Controls.Add(next);
        }
        finally
        {
            _contentPanel.ResumeLayout();
        }
    }
}
