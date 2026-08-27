using System.Globalization;
using Sasd.FinanceControl.Application.Categories;
using Sasd.FinanceControl.Application.Orders;
using Sasd.FinanceControl.Domain.Entities;

namespace Sasd.FinanceControl.App.Views.Orders;

/// <summary>Editor for one purchase-order line.</summary>
public sealed class PurchaseOrderLineDialog : Form
{
    private readonly Guid? _lineId;
    private readonly TextBox _itemName = new();
    private readonly TextBox _description = new() { Multiline = true, Height = 56 };
    private readonly TextBox _quantity = new() { Text = "1" };
    private readonly TextBox _unit = new() { Text = "Stück" };
    private readonly TextBox _unitPrice = new();
    private readonly TextBox _taxRate = new() { Text = "19" };
    private readonly ComboBox _category = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly CheckBox _asset = new() { Text = "Asset-Kandidat", AutoSize = true };
    private readonly CheckBox _inventory = new() { Text = "Inventar-Kandidat", AutoSize = true };
    private readonly Label _preview = new() { AutoSize = true };

    public PurchaseOrderLineDialog(IReadOnlyList<CategoryItem> categories, PurchaseOrderLineRequest? existing = null)
    {
        ArgumentNullException.ThrowIfNull(categories);
        _lineId = existing?.Id;
        Text = existing is null ? "Bestellposition hinzufügen" : "Bestellposition bearbeiten";
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(640, 485);
        AutoScaleMode = AutoScaleMode.Dpi;
        BuildUi(categories, existing?.CategoryId);
        if (existing is not null) LoadExisting(existing);
        UpdatePreview();
    }

    public PurchaseOrderLineRequest? Result { get; private set; }

    private void BuildUi(IReadOnlyList<CategoryItem> categories, Guid? existingCategoryId)
    {
        _category.Items.Add(new CategoryOption(null, "– keine Kategorie –"));
        foreach (var item in categories.Where(item => item.IsActive || item.Id == existingCategoryId).OrderBy(item => item.Name))
            _category.Items.Add(new CategoryOption(item.Id, item.IsActive ? item.Name : item.Name + " (inaktiv)"));
        _category.SelectedIndex = 0;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 11 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        AddRow(root, 0, "Artikel / Leistung*", _itemName);
        AddRow(root, 1, "Beschreibung", _description);
        AddRow(root, 2, "Menge*", _quantity);
        AddRow(root, 3, "Einheit", _unit);
        AddRow(root, 4, "Einzelpreis netto*", _unitPrice);
        AddRow(root, 5, "USt. %*", _taxRate);
        AddRow(root, 6, "Kategorie", _category);

        var flags = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        flags.Controls.AddRange([_asset, _inventory]);
        AddRow(root, 7, "Kennzeichnung", flags);
        AddRow(root, 8, "Vorschau", _preview);

        foreach (var box in new[] { _quantity, _unitPrice, _taxRate }) box.TextChanged += (_, _) => UpdatePreview();

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft };
        var save = new Button { Text = "Übernehmen", AutoSize = true };
        save.Click += (_, _) => Save();
        var cancel = new Button { Text = "Abbrechen", AutoSize = true, DialogResult = DialogResult.Cancel };
        buttons.Controls.AddRange([save, cancel]);
        root.Controls.Add(buttons, 0, 10);
        root.SetColumnSpan(buttons, 2);
        AcceptButton = save;
        CancelButton = cancel;
        Controls.Add(root);
    }

    private void LoadExisting(PurchaseOrderLineRequest existing)
    {
        _itemName.Text = existing.ItemName;
        _description.Text = existing.Description ?? string.Empty;
        _quantity.Text = existing.Quantity.ToString("G29", CultureInfo.CurrentCulture);
        _unit.Text = existing.Unit ?? string.Empty;
        _unitPrice.Text = existing.UnitPriceNet.ToString("G29", CultureInfo.CurrentCulture);
        _taxRate.Text = existing.TaxRatePercent.ToString("G29", CultureInfo.CurrentCulture);
        _asset.Checked = existing.AssetCandidate;
        _inventory.Checked = existing.InventoryCandidate;
        for (var i = 0; i < _category.Items.Count; i++)
        {
            if (_category.Items[i] is CategoryOption option && option.Id == existing.CategoryId)
            {
                _category.SelectedIndex = i;
                break;
            }
        }
    }

    private void Save()
    {
        try
        {
            var quantity = ParseDecimal(_quantity.Text, "Menge");
            var unitPrice = ParseDecimal(_unitPrice.Text, "Einzelpreis netto");
            var taxRate = ParseDecimal(_taxRate.Text, "USt.-Satz");
            var categoryId = (_category.SelectedItem as CategoryOption)?.Id;
            var line = _lineId is Guid id
                ? PurchaseOrderLine.Revise(id, 1, _itemName.Text, NullIfWhiteSpace(_description.Text), quantity, NullIfWhiteSpace(_unit.Text), unitPrice, taxRate, categoryId, _asset.Checked, _inventory.Checked)
                : PurchaseOrderLine.Create(1, _itemName.Text, NullIfWhiteSpace(_description.Text), quantity, NullIfWhiteSpace(_unit.Text), unitPrice, taxRate, categoryId, _asset.Checked, _inventory.Checked);
            Result = new PurchaseOrderLineRequest(_lineId, line.ItemName, line.Description, line.Quantity, line.Unit, line.UnitPriceNet, line.TaxRatePercent, line.CategoryId, line.AssetCandidate, line.InventoryCandidate);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(this, ex.Message, "Bestellposition", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void UpdatePreview()
    {
        if (!TryParseDecimal(_quantity.Text, out var q) || !TryParseDecimal(_unitPrice.Text, out var p) || !TryParseDecimal(_taxRate.Text, out var t)) { _preview.Text = "–"; return; }
        try
        {
            var line = PurchaseOrderLine.Create(1, "Vorschau", null, q, null, p, t, null, false, false);
            _preview.Text = $"Netto {line.NetAmount:N2}  |  Steuer {line.TaxAmount:N2}  |  Brutto {line.GrossAmount:N2}";
        }
        catch (ArgumentException) { _preview.Text = "–"; }
    }

    private static decimal ParseDecimal(string value, string field)
        => TryParseDecimal(value, out var result) ? result : throw new ArgumentException($"{field} ist keine gültige Zahl.");
    private static bool TryParseDecimal(string value, out decimal result)
        => decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result)
            || decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
    private static string? NullIfWhiteSpace(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    private static void AddRow(TableLayoutPanel panel, int row, string caption, Control control)
    {
        panel.Controls.Add(new Label { Text = caption, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 7, 8, 4) }, 0, row);
        control.Dock = DockStyle.Top;
        panel.Controls.Add(control, 1, row);
    }
    private sealed record CategoryOption(Guid? Id, string Text) { public override string ToString() => Text; }
}
