using Sasd.FinanceControl.App.Configuration;
using Sasd.FinanceControl.App.Presentation;
using Sasd.FinanceControl.App.Views;

namespace Sasd.FinanceControl.App.Forms;

/// <summary>
/// Main WinForms shell for SASD Finance Control.
/// </summary>
/// <remarks>
/// The form contains UI construction and event forwarding only. Navigation
/// decisions live in <see cref="MainPresenter"/>; financial use cases will live
/// in Application/Domain services in later milestones.
/// </remarks>
public sealed class MainForm : Form, IMainView
{
    private readonly FinanceControlOptions _options;
    private readonly IPageViewFactory _pageViewFactory;
    private readonly Panel _contentPanel;
    private readonly Label _pageTitleLabel;
    private readonly ToolStripStatusLabel _environmentStatusLabel;
    private readonly ToolStripStatusLabel _versionStatusLabel;
    private readonly ToolStripStatusLabel _logStatusLabel;
    private ShellStatus? _shellStatus;

    /// <summary>Initializes the application shell.</summary>
    public MainForm(FinanceControlOptions options, IPageViewFactory pageViewFactory)
    {
        _options = options;
        _pageViewFactory = pageViewFactory;

        Text = options.ApplicationName;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(960, 640);
        ClientSize = new Size(1180, 760);
        AutoScaleMode = AutoScaleMode.Dpi;

        var rootLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
        };
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var navigationPanel = CreateNavigationPanel();
        rootLayout.Controls.Add(navigationPanel, 0, 0);

        var mainArea = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Margin = Padding.Empty,
            Padding = new Padding(20),
        };
        mainArea.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        mainArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _pageTitleLabel = new Label
        {
            AutoSize = true,
            Font = new Font(Font, FontStyle.Bold),
            Padding = new Padding(0, 0, 0, 12),
            Text = "Dashboard",
        };
        mainArea.Controls.Add(_pageTitleLabel, 0, 0);

        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
        };
        mainArea.Controls.Add(_contentPanel, 0, 1);
        rootLayout.Controls.Add(mainArea, 1, 0);

        var statusStrip = new StatusStrip
        {
            SizingGrip = false,
        };
        _environmentStatusLabel = new ToolStripStatusLabel("Environment: -");
        _versionStatusLabel = new ToolStripStatusLabel("Version: -");
        _logStatusLabel = new ToolStripStatusLabel("Log: -")
        {
            Spring = true,
            TextAlign = ContentAlignment.MiddleRight,
        };
        statusStrip.Items.AddRange(
        [
            _environmentStatusLabel,
            new ToolStripStatusLabel("|"),
            _versionStatusLabel,
            _logStatusLabel,
        ]);
        rootLayout.Controls.Add(statusStrip, 0, 1);
        rootLayout.SetColumnSpan(statusStrip, 2);

        Controls.Add(rootLayout);
    }

    /// <inheritdoc />
    public event EventHandler<NavigationRequestedEventArgs>? NavigationRequested;

    /// <inheritdoc />
    public void ShowPage(PageDescriptor page)
    {
        ArgumentNullException.ThrowIfNull(page);

        _pageTitleLabel.Text = page.Title;

        // Controls are replaced rather than hidden so later modules can own
        // their lifecycle cleanly. This shell currently creates only lightweight
        // views and placeholders.
        foreach (Control control in _contentPanel.Controls.Cast<Control>().ToArray())
        {
            control.Dispose();
        }

        _contentPanel.Controls.Clear();

        var nextView = _pageViewFactory.Create(page, _shellStatus);

        nextView.Dock = DockStyle.Fill;
        _contentPanel.Controls.Add(nextView);
    }

    /// <inheritdoc />
    public void SetShellStatus(ShellStatus status)
    {
        ArgumentNullException.ThrowIfNull(status);
        _shellStatus = status;

        _environmentStatusLabel.Text = $"Environment: {status.EnvironmentName}";
        _versionStatusLabel.Text = $"Version: {status.ApplicationVersion}";
        _logStatusLabel.Text = $"Logs: {status.LogDirectory}";
    }

    private Control CreateNavigationPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 14,
            Padding = new Padding(12),
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        for (var index = 0; index < 12; index++)
        {
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        }

        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var brandLabel = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            Height = 64,
            Text = _options.ApplicationName + Environment.NewLine + "Milestone 9",
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font(Font, FontStyle.Bold),
        };
        panel.Controls.Add(brandLabel);

        AddNavigationButton(panel, "Dashboard", NavigationTarget.Dashboard);
        AddNavigationButton(panel, "Lieferanten", NavigationTarget.Suppliers);
        AddNavigationButton(panel, "Kategorien", NavigationTarget.Categories);
        AddNavigationButton(panel, "Banking", NavigationTarget.Banking);
        AddNavigationButton(panel, "Zahlungszuordnung", NavigationTarget.Payments);
        AddNavigationButton(panel, "Zahlungsabgleich", NavigationTarget.Reconciliation);
        AddNavigationButton(panel, "Verträge & Abos", NavigationTarget.Contracts);
        AddNavigationButton(panel, "Bestellungen", NavigationTarget.Orders);
        AddNavigationButton(panel, "Rechnungen", NavigationTarget.Invoices);
        AddNavigationButton(panel, "Dokumente", NavigationTarget.Documents);
        AddNavigationButton(panel, "Berichte", NavigationTarget.Reports);
        AddNavigationButton(panel, "Einstellungen", NavigationTarget.Settings);

        // The final percent row declared above absorbs unused vertical space
        // and keeps the navigation stable when the form is resized.
        return panel;
    }

    private void AddNavigationButton(
        TableLayoutPanel panel,
        string text,
        NavigationTarget target)
    {
        var button = new Button
        {
            AutoSize = false,
            Dock = DockStyle.Top,
            Height = 42,
            Text = text,
            TextAlign = ContentAlignment.MiddleLeft,
            Tag = target,
            Margin = new Padding(0, 2, 0, 2),
        };

        button.Click += OnNavigationButtonClick;
        panel.Controls.Add(button);
    }

    private void OnNavigationButtonClick(object? sender, EventArgs e)
    {
        if (sender is not Button { Tag: NavigationTarget target })
        {
            return;
        }

        NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(target));
    }
}
