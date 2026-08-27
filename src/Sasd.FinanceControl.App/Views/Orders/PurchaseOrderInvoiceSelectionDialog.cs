using Sasd.FinanceControl.Application.Invoices;

namespace Sasd.FinanceControl.App.Views.Orders;

/// <summary>Selects a supplier invoice for a typed order/invoice relationship.</summary>
public sealed class PurchaseOrderInvoiceSelectionDialog : Form
{
    private readonly DataGridView _grid = new();
    private readonly TextBox _note = new() { Multiline = true, Height = 58, ScrollBars = ScrollBars.Vertical };

    public PurchaseOrderInvoiceSelectionDialog(IReadOnlyList<InvoiceListItem> invoices)
    {
        ArgumentNullException.ThrowIfNull(invoices);
        Text = "Rechnung mit Bestellung verknüpfen";
        StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(900, 520); MinimizeBox = false; MaximizeBox = false; ShowInTaskbar = false; AutoScaleMode = AutoScaleMode.Dpi;
        BuildUi();
        _grid.DataSource = invoices.Select(i => new Row(i.Id, i.InvoiceNumber, i.ExternalInvoiceNumber ?? string.Empty, i.InvoiceDate.ToString("dd.MM.yyyy"), $"{i.GrossAmount:N2} {i.CurrencyCode}")).ToList();
    }

    public Guid? SelectedInvoiceId => _grid.CurrentRow?.DataBoundItem is Row row ? row.Id : null;
    public string? Note => string.IsNullOrWhiteSpace(_note.Text) ? null : _note.Text.Trim();

    private void BuildUi()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 4 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(new Label { Text = "Es werden nur Rechnungen desselben Lieferanten und derselben Währung angeboten.", AutoSize = true, Margin = new Padding(0, 0, 0, 8) }, 0, 0);
        _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.AllowUserToDeleteRows = false; _grid.MultiSelect = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.AutoGenerateColumns = false; _grid.RowHeadersVisible = false;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SASD-Nr.", DataPropertyName = nameof(Row.Number), Width = 110 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Lieferanten-Nr.", DataPropertyName = nameof(Row.External), Width = 150 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Datum", DataPropertyName = nameof(Row.Date), Width = 95 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Brutto", DataPropertyName = nameof(Row.Gross), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        root.Controls.Add(_grid, 0, 1);
        var notePanel = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2 }; notePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80)); notePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        notePanel.Controls.Add(new Label { Text = "Notiz:", AutoSize = true, Margin = new Padding(0, 7, 8, 0) }, 0, 0); _note.Dock = DockStyle.Fill; notePanel.Controls.Add(_note, 1, 0); root.Controls.Add(notePanel, 0, 2);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Margin = new Padding(0, 10, 0, 0) };
        var ok = new Button { Text = "Verknüpfen", AutoSize = true, DialogResult = DialogResult.OK }; var cancel = new Button { Text = "Abbrechen", AutoSize = true, DialogResult = DialogResult.Cancel };
        buttons.Controls.AddRange([ok, cancel]); root.Controls.Add(buttons, 0, 3); AcceptButton = ok; CancelButton = cancel; Controls.Add(root);
    }
    private sealed record Row(Guid Id, string Number, string External, string Date, string Gross);
}
