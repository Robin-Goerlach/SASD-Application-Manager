using Sasd.FinanceControl.Application.Documents;

namespace Sasd.FinanceControl.App.Views.Orders;

/// <summary>Selects an archived document to link to a purchase order.</summary>
public sealed class PurchaseOrderDocumentSelectionDialog : Form
{
    private readonly DataGridView _grid = new();

    public PurchaseOrderDocumentSelectionDialog(IReadOnlyList<DocumentListItem> documents)
    {
        ArgumentNullException.ThrowIfNull(documents);
        Text = "Bestelldokument verknüpfen";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(900, 500);
        MinimizeBox = false; MaximizeBox = false; ShowInTaskbar = false; AutoScaleMode = AutoScaleMode.Dpi;
        BuildUi();
        _grid.DataSource = documents.Select(d => new Row(d.Id, d.DocumentDate?.ToString("dd.MM.yyyy") ?? string.Empty, d.DocumentType.ToString(), d.OriginalFileName, d.Source ?? string.Empty)).ToList();
    }

    public Guid? SelectedDocumentId => _grid.CurrentRow?.DataBoundItem is Row row ? row.Id : null;

    private void BuildUi()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.AllowUserToDeleteRows = false; _grid.MultiSelect = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.AutoGenerateColumns = false; _grid.RowHeadersVisible = false;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Datum", DataPropertyName = nameof(Row.Date), Width = 90 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Typ", DataPropertyName = nameof(Row.Type), Width = 110 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Datei", DataPropertyName = nameof(Row.FileName), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 280 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quelle", DataPropertyName = nameof(Row.Source), Width = 180 });
        root.Controls.Add(_grid, 0, 0);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 10, 0, 0) };
        var ok = new Button { Text = "Verknüpfen", AutoSize = true, DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Abbrechen", AutoSize = true, DialogResult = DialogResult.Cancel };
        buttons.Controls.AddRange([ok, cancel]); root.Controls.Add(buttons, 0, 1); AcceptButton = ok; CancelButton = cancel; Controls.Add(root);
    }
    private sealed record Row(Guid Id, string Date, string Type, string FileName, string Source);
}
