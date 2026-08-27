using System.Globalization;
using Sasd.FinanceControl.Application.Categories;
using Sasd.FinanceControl.Application.Orders;
using Sasd.FinanceControl.Application.Suppliers;
using Sasd.FinanceControl.Domain.Entities;

namespace Sasd.FinanceControl.App.Views.Orders;

/// <summary>Dialog for creating or editing one purchase order aggregate.</summary>
public sealed class PurchaseOrderEditDialog : Form
{
    private readonly PurchaseOrderDetails? _existing;
    private readonly IReadOnlyList<CategoryItem> _categories;
    private readonly List<PurchaseOrderLineRequest> _lines = [];
    private readonly TextBox _number = new() { ReadOnly = true };
    private readonly ComboBox _supplier = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox _supplierOrderNumber = new();
    private readonly DateTimePicker _orderDate = new() { Format = DateTimePickerFormat.Short };
    private readonly DateTimePicker _deliveryDate = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true };
    private readonly ComboBox _status = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox _currency = new() { Text = "EUR", MaxLength = 3 };
    private readonly TextBox _purpose = new() { Multiline = true, Height = 55 };
    private readonly TextBox _notes = new() { Multiline = true, Height = 65, ScrollBars = ScrollBars.Vertical };
    private readonly DataGridView _grid = new();
    private readonly Label _totals = new() { AutoSize = true };

    public PurchaseOrderEditDialog(IReadOnlyList<SupplierListItem> suppliers, IReadOnlyList<CategoryItem> categories, PurchaseOrderDetails? existing = null)
    {
        ArgumentNullException.ThrowIfNull(suppliers);
        ArgumentNullException.ThrowIfNull(categories);
        _existing = existing;
        _categories = categories;
        Text = existing is null ? "Bestellung anlegen" : "Bestellung bearbeiten";
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(1050, 720);
        MinimumSize = new Size(900, 620);
        AutoScaleMode = AutoScaleMode.Dpi;
        BuildUi();
        PopulateOptions(suppliers);
        if (existing is not null) LoadExisting(existing);
        RefreshGrid();
    }

    public CreatePurchaseOrderRequest? CreateRequest { get; private set; }
    public UpdatePurchaseOrderRequest? UpdateRequest { get; private set; }

    private void BuildUi()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 4 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var header = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, RowCount = 5 };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        AddPair(header, 0, 0, "SASD-Nr.", _number); AddPair(header, 2, 0, "Lieferant*", _supplier);
        AddPair(header, 0, 1, "Lieferanten-Nr.", _supplierOrderNumber); AddPair(header, 2, 1, "Bestelldatum*", _orderDate);
        AddPair(header, 0, 2, "Erwartete Lieferung", _deliveryDate); AddPair(header, 2, 2, "Status*", _status);
        AddPair(header, 0, 3, "Währung*", _currency); AddPair(header, 2, 3, "Geschäftszweck", _purpose);
        AddPair(header, 0, 4, "Notizen", _notes); header.SetColumnSpan(_notes, 3);
        root.Controls.Add(header, 0, 0);

        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Margin = new Padding(0, 10, 0, 6) };
        var add = new Button { Text = "Position hinzufügen …", AutoSize = true }; add.Click += (_, _) => AddLine();
        var edit = new Button { Text = "Position bearbeiten …", AutoSize = true }; edit.Click += (_, _) => EditLine();
        var remove = new Button { Text = "Position entfernen", AutoSize = true }; remove.Click += (_, _) => RemoveLine();
        toolbar.Controls.AddRange([add, edit, remove, _totals]);
        root.Controls.Add(toolbar, 0, 1);

        ConfigureGrid(); root.Controls.Add(_grid, 0, 2);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Margin = new Padding(0, 10, 0, 0) };
        var save = new Button { Text = "Speichern", AutoSize = true }; save.Click += (_, _) => Save();
        var cancel = new Button { Text = "Abbrechen", AutoSize = true, DialogResult = DialogResult.Cancel };
        buttons.Controls.AddRange([save, cancel]); root.Controls.Add(buttons, 0, 3);
        AcceptButton = save; CancelButton = cancel; Controls.Add(root);
    }

    private void ConfigureGrid()
    {
        _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.AllowUserToDeleteRows = false;
        _grid.MultiSelect = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.AutoGenerateColumns = false; _grid.RowHeadersVisible = false;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Pos.", DataPropertyName = nameof(LineRow.Position), Width = 55 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Artikel / Leistung", DataPropertyName = nameof(LineRow.Item), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 220 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kategorie", DataPropertyName = nameof(LineRow.Category), Width = 150 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Menge", DataPropertyName = nameof(LineRow.Quantity), Width = 80 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Netto", DataPropertyName = nameof(LineRow.Net), Width = 105 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Steuer", DataPropertyName = nameof(LineRow.Tax), Width = 105 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Brutto", DataPropertyName = nameof(LineRow.Gross), Width = 105 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Asset", DataPropertyName = nameof(LineRow.Asset), Width = 55 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Inventar", DataPropertyName = nameof(LineRow.Inventory), Width = 65 });
        _grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditLine(); };
    }

    private void PopulateOptions(IReadOnlyList<SupplierListItem> suppliers)
    {
        foreach (var item in suppliers) _supplier.Items.Add(new SupplierOption(item.Id, $"{item.SupplierNumber} – {item.SupplierName}"));
        if (_supplier.Items.Count > 0) _supplier.SelectedIndex = 0;
        _status.Items.AddRange([
            new StatusOption(PurchaseOrderStatus.Draft, "Entwurf"), new StatusOption(PurchaseOrderStatus.Ordered, "Bestellt"),
            new StatusOption(PurchaseOrderStatus.PartiallyReceived, "Teilweise geliefert"), new StatusOption(PurchaseOrderStatus.Received, "Geliefert"),
            new StatusOption(PurchaseOrderStatus.Cancelled, "Storniert"), new StatusOption(PurchaseOrderStatus.Closed, "Abgeschlossen")]);
        _status.SelectedIndex = 0;
    }

    private void LoadExisting(PurchaseOrderDetails existing)
    {
        _number.Text = existing.OrderNumber; SelectSupplier(existing.SupplierId); _supplierOrderNumber.Text = existing.SupplierOrderNumber ?? string.Empty;
        _orderDate.Value = existing.OrderDate.ToDateTime(TimeOnly.MinValue); _deliveryDate.Checked = existing.ExpectedDeliveryDate.HasValue;
        if (existing.ExpectedDeliveryDate is DateOnly d) _deliveryDate.Value = d.ToDateTime(TimeOnly.MinValue);
        _status.SelectedItem = _status.Items.Cast<StatusOption>().First(item => item.Status == existing.Status);
        _currency.Text = existing.CurrencyCode; _purpose.Text = existing.BusinessPurpose ?? string.Empty; _notes.Text = existing.Notes ?? string.Empty;
        _lines.AddRange(existing.Lines.Select(line => new PurchaseOrderLineRequest(line.Id, line.ItemName, line.Description, line.Quantity, line.Unit, line.UnitPriceNet, line.TaxRatePercent, line.CategoryId, line.AssetCandidate, line.InventoryCandidate)));
    }

    private void AddLine()
    {
        using var dialog = new PurchaseOrderLineDialog(_categories);
        if (dialog.ShowDialog(this) == DialogResult.OK && dialog.Result is not null) { _lines.Add(dialog.Result); RefreshGrid(_lines.Count - 1); }
    }
    private void EditLine()
    {
        if (_grid.CurrentRow?.DataBoundItem is not LineRow row) return;
        using var dialog = new PurchaseOrderLineDialog(_categories, _lines[row.Index]);
        if (dialog.ShowDialog(this) == DialogResult.OK && dialog.Result is not null) { _lines[row.Index] = dialog.Result; RefreshGrid(row.Index); }
    }
    private void RemoveLine()
    {
        if (_grid.CurrentRow?.DataBoundItem is not LineRow row) return;
        _lines.RemoveAt(row.Index); RefreshGrid(Math.Min(row.Index, _lines.Count - 1));
    }

    private void RefreshGrid(int selectIndex = -1)
    {
        var categories = _categories.ToDictionary(x => x.Id, x => x.Name);
        decimal net = 0m, tax = 0m, gross = 0m;
        var rows = new List<LineRow>();
        for (var i = 0; i < _lines.Count; i++)
        {
            var r = _lines[i];
            var line = r.Id is Guid id ? PurchaseOrderLine.Revise(id, i + 1, r.ItemName, r.Description, r.Quantity, r.Unit, r.UnitPriceNet, r.TaxRatePercent, r.CategoryId, r.AssetCandidate, r.InventoryCandidate)
                : PurchaseOrderLine.Create(i + 1, r.ItemName, r.Description, r.Quantity, r.Unit, r.UnitPriceNet, r.TaxRatePercent, r.CategoryId, r.AssetCandidate, r.InventoryCandidate);
            net += line.NetAmount; tax += line.TaxAmount; gross += line.GrossAmount;
            rows.Add(new LineRow(i, i + 1, line.ItemName, line.CategoryId is Guid cid && categories.TryGetValue(cid, out var name) ? name : string.Empty,
                $"{line.Quantity:G29} {line.Unit}".Trim(), line.NetAmount.ToString("N2"), line.TaxAmount.ToString("N2"), line.GrossAmount.ToString("N2"), line.AssetCandidate ? "Ja" : "", line.InventoryCandidate ? "Ja" : ""));
        }
        _grid.DataSource = rows; _grid.ClearSelection();
        if (selectIndex >= 0 && selectIndex < _grid.Rows.Count) { _grid.Rows[selectIndex].Selected = true; _grid.CurrentCell = _grid.Rows[selectIndex].Cells[0]; }
        _totals.Text = $"   Summe: Netto {net:N2} {_currency.Text.Trim().ToUpperInvariant()} | Steuer {tax:N2} | Brutto {gross:N2}";
    }

    private void Save()
    {
        try
        {
            var supplierId = (_supplier.SelectedItem as SupplierOption)?.Id ?? throw new ArgumentException("Bitte einen Lieferanten auswählen.");
            var status = (_status.SelectedItem as StatusOption)?.Status ?? throw new ArgumentException("Bitte einen Status auswählen.");
            var orderDate = DateOnly.FromDateTime(_orderDate.Value.Date);
            DateOnly? delivery = _deliveryDate.Checked ? DateOnly.FromDateTime(_deliveryDate.Value.Date) : null;
            var currency = _currency.Text.Trim().ToUpperInvariant();
            var validationLines = _lines.Select((r, i) => r.Id is Guid id ? PurchaseOrderLine.Revise(id, i + 1, r.ItemName, r.Description, r.Quantity, r.Unit, r.UnitPriceNet, r.TaxRatePercent, r.CategoryId, r.AssetCandidate, r.InventoryCandidate)
                : PurchaseOrderLine.Create(i + 1, r.ItemName, r.Description, r.Quantity, r.Unit, r.UnitPriceNet, r.TaxRatePercent, r.CategoryId, r.AssetCandidate, r.InventoryCandidate)).ToArray();
            _ = PurchaseOrder.Create("VALIDATION", supplierId, NullIfWhiteSpace(_supplierOrderNumber.Text), orderDate, delivery, status, currency, NullIfWhiteSpace(_purpose.Text), NullIfWhiteSpace(_notes.Text), validationLines);
            if (_existing is null)
                CreateRequest = new CreatePurchaseOrderRequest(supplierId, NullIfWhiteSpace(_supplierOrderNumber.Text), orderDate, delivery, status, currency, NullIfWhiteSpace(_purpose.Text), NullIfWhiteSpace(_notes.Text), _lines.ToArray());
            else
                UpdateRequest = new UpdatePurchaseOrderRequest(_existing.Id, supplierId, NullIfWhiteSpace(_supplierOrderNumber.Text), orderDate, delivery, status, currency, NullIfWhiteSpace(_purpose.Text), NullIfWhiteSpace(_notes.Text), _lines.ToArray());
            DialogResult = DialogResult.OK; Close();
        }
        catch (ArgumentException ex) { MessageBox.Show(this, ex.Message, "Bestellung", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private void SelectSupplier(Guid id)
    {
        for (var i = 0; i < _supplier.Items.Count; i++) if (_supplier.Items[i] is SupplierOption o && o.Id == id) { _supplier.SelectedIndex = i; return; }
    }
    private static string? NullIfWhiteSpace(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    private static void AddPair(TableLayoutPanel panel, int column, int row, string caption, Control control)
    {
        panel.Controls.Add(new Label { Text = caption + ":", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 7, 8, 4) }, column, row);
        control.Dock = DockStyle.Top; panel.Controls.Add(control, column + 1, row);
    }
    private sealed record SupplierOption(Guid Id, string Text) { public override string ToString() => Text; }
    private sealed record StatusOption(PurchaseOrderStatus Status, string Text) { public override string ToString() => Text; }
    private sealed record LineRow(int Index, int Position, string Item, string Category, string Quantity, string Net, string Tax, string Gross, string Asset, string Inventory);
}
