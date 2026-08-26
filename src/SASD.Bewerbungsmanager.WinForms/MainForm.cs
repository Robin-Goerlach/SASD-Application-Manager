using System.Drawing;
using SASD.Bewerbungsmanager.WinForms.Presentation.MainShell;

namespace SASD.Bewerbungsmanager.WinForms;

/// <summary>
/// Main Windows Forms shell. It intentionally contains no business logic.
/// </summary>
public sealed class MainForm : Form, IMainShellView
{
    private readonly ToolStripStatusLabel _statusLabel = new("Bereit");

    /// <summary>
    /// Initializes the M0 shell and attaches its presenter.
    /// </summary>
    /// <param name="presenter">Shell presenter.</param>
    public MainForm(MainShellPresenter presenter)
    {
        ArgumentNullException.ThrowIfNull(presenter);

        Text = "SASD Bewerbungsmanager";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1_180, 720);
        ClientSize = new Size(1_360, 820);
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        Controls.Add(BuildShell());

        presenter.Attach(this);
        Load += (_, _) => presenter.OnLoaded();
    }

    /// <inheritdoc />
    public void SetStatus(string text)
    {
        _statusLabel.Text = text;
    }

    private Control BuildShell()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = SystemColors.ControlLightLight,
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        root.Controls.Add(BuildNavigation(), 0, 0);
        root.Controls.Add(BuildMainArea(), 1, 0);
        return root;
    }

    private static Control BuildNavigation()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(10, 18, 10, 10),
            BackColor = Color.FromArgb(246, 247, 249),
        };

        panel.Controls.Add(new Label
        {
            Text = "SASD",
            Font = new Font("Segoe UI Semibold", 12F),
            AutoSize = true,
            Margin = new Padding(8, 0, 0, 14),
        });

        var entries = new[]
        {
            "Dashboard", "Bewerbungen", "Stellen", "Unternehmen", "Kontakte", "Aufgaben",
            "Interviews", "Dokumente", "Aktivitäten", "Kalender", "Berichte", "Einstellungen",
        };

        foreach (var entry in entries)
        {
            panel.Controls.Add(new Button
            {
                Text = entry,
                Width = 174,
                Height = 36,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = entry == "Dashboard" ? Color.FromArgb(225, 238, 252) : Color.Transparent,
                FlatAppearance = { BorderSize = 0 },
                Margin = new Padding(0, 1, 0, 1),
            });
        }

        return panel;
    }

    private Control BuildMainArea()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(18, 12, 18, 8),
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

        layout.Controls.Add(BuildHeader(), 0, 0);
        layout.Controls.Add(BuildToolbar(), 0, 1);
        layout.Controls.Add(BuildMetrics(), 0, 2);
        layout.Controls.Add(BuildDashboardPlaceholder(), 0, 3);

        var statusStrip = new StatusStrip { Dock = DockStyle.Fill, SizingGrip = false };
        statusStrip.Items.Add(_statusLabel);
        statusStrip.Items.Add(new ToolStripStatusLabel { Spring = true });
        statusStrip.Items.Add(new ToolStripStatusLabel("Version 0.0.1 – M0"));
        layout.Controls.Add(statusStrip, 0, 4);

        return layout;
    }

    private static Control BuildHeader()
    {
        var header = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        header.Controls.Add(new Label
        {
            Text = "Dashboard",
            Font = new Font("Segoe UI Semibold", 18F),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
        }, 0, 0);
        header.Controls.Add(new TextBox
        {
            PlaceholderText = "Suchen (Strg+F) – ab M4",
            Dock = DockStyle.Fill,
            Margin = new Padding(30, 9, 0, 9),
        }, 1, 0);
        return header;
    }

    private static Control BuildToolbar()
    {
        var strip = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };

        foreach (var caption in new[] { "Neu", "Bearbeiten", "Aktivität erfassen", "Dokument anhängen", "Backup" })
        {
            strip.Controls.Add(new Button
            {
                Text = caption,
                AutoSize = true,
                Height = 32,
                Enabled = false,
                Margin = new Padding(0, 5, 8, 5),
            });
        }

        return strip;
    }

    private static Control BuildMetrics()
    {
        var metrics = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, Padding = new Padding(0, 4, 0, 8) };
        for (var index = 0; index < 4; index++)
        {
            metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }

        var cards = new[]
        {
            ("Aktive Bewerbungen", "–"),
            ("Interviews", "–"),
            ("Rückmeldungen offen", "–"),
            ("Zusagen / Angebote", "–"),
        };

        for (var index = 0; index < cards.Length; index++)
        {
            metrics.Controls.Add(BuildMetricCard(cards[index].Item1, cards[index].Item2), index, 0);
        }

        return metrics;
    }

    private static Control BuildMetricCard(string title, string value)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(14),
            BackColor = Color.White,
        };
        card.Controls.Add(new Label
        {
            Text = $"{title}\r\n{value}",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 11F),
            TextAlign = ContentAlignment.MiddleLeft,
        });
        return card;
    }

    private static Control BuildDashboardPlaceholder()
    {
        var group = new GroupBox
        {
            Text = "Bewerbungspipeline",
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
        };

        group.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = SystemColors.GrayText,
            Font = new Font("Segoe UI", 11F),
            Text = "M0 Architecture Skeleton\r\n\r\n" +
                   "Die Shell orientiert sich am GUI-Konzept in docs/images/dashboard-concept.png.\r\n" +
                   "Produktive Bewerbungsdaten und Workflows beginnen vertikal ab M1.",
        });
        return group;
    }
}
