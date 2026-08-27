using Microsoft.Extensions.Logging;
using Sasd.FinanceControl.Application.Categories;
using Sasd.FinanceControl.Application.Documents;
using Sasd.FinanceControl.Application.Orders;
using Sasd.FinanceControl.Application.Reconciliation;
using Sasd.FinanceControl.Application.Suppliers;
using Sasd.FinanceControl.App.Services;
using Sasd.FinanceControl.App.Views.Reconciliation;
using Sasd.FinanceControl.Domain.Entities;

namespace Sasd.FinanceControl.App.Views.Orders;

/// <summary>WinForms workbench for purchase orders and their typed evidence links.</summary>
public sealed class PurchaseOrderManagementView : UserControl
{
    private readonly PurchaseOrderService _orders;
    private readonly SupplierService _suppliers;
    private readonly ReconciliationService _reconciliation;
    private readonly CategoryService _categories;
    private readonly DocumentArchiveService _documents;
    private readonly IUserNotificationService _notifications;
    private readonly ILogger<PurchaseOrderManagementView> _logger;
    private readonly CancellationTokenSource _lifetime = new();
    private readonly TextBox _search = new();
    private readonly ComboBox _status = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly CheckBox _includeClosed = new() { Text = "Stornierte/abgeschlossene anzeigen", AutoSize = true };
    private readonly DataGridView _orderGrid = new();
    private readonly DataGridView _lineGrid = new();
    private readonly DataGridView _invoiceGrid = new();
    private readonly DataGridView _documentGrid = new();

    public PurchaseOrderManagementView(PurchaseOrderService orders, ReconciliationService reconciliation, SupplierService suppliers, CategoryService categories, DocumentArchiveService documents, IUserNotificationService notifications, ILogger<PurchaseOrderManagementView> logger)
    {
        _orders = orders; _reconciliation = reconciliation; _suppliers = suppliers; _categories = categories; _documents = documents; _notifications = notifications; _logger = logger;
        Dock = DockStyle.Fill; AutoScaleMode = AutoScaleMode.Dpi;
        BuildUi();
        Disposed += (_, _) => _lifetime.Cancel();
        _ = LoadAsync();
    }

    private void BuildUi()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 1, RowCount = 3 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        var filters = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        filters.Controls.Add(new Label { Text = "Suche:", AutoSize = true, Margin = new Padding(0, 7, 4, 0) }); _search.Width = 230; filters.Controls.Add(_search);
        filters.Controls.Add(new Label { Text = "Status:", AutoSize = true, Margin = new Padding(12, 7, 4, 0) });
        _status.Items.AddRange([new StatusOption(null, "Alle"), new StatusOption(PurchaseOrderStatus.Draft, "Entwurf"), new StatusOption(PurchaseOrderStatus.Ordered, "Bestellt"), new StatusOption(PurchaseOrderStatus.PartiallyReceived, "Teilweise geliefert"), new StatusOption(PurchaseOrderStatus.Received, "Geliefert"), new StatusOption(PurchaseOrderStatus.Cancelled, "Storniert"), new StatusOption(PurchaseOrderStatus.Closed, "Abgeschlossen")]);
        _status.SelectedIndex = 0; filters.Controls.Add(_status); filters.Controls.Add(_includeClosed);
        var refresh = new Button { Text = "Aktualisieren", AutoSize = true }; refresh.Click += async (_, _) => await LoadAsync(); filters.Controls.Add(refresh); root.Controls.Add(filters, 0, 0);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Margin = new Padding(0, 6, 0, 6) };
        var create = new Button { Text = "Neu …", AutoSize = true }; create.Click += async (_, _) => await CreateAsync();
        var edit = new Button { Text = "Bearbeiten …", AutoSize = true }; edit.Click += async (_, _) => await EditAsync();
        var linkDoc = new Button { Text = "Dokument verknüpfen …", AutoSize = true }; linkDoc.Click += async (_, _) => await LinkDocumentAsync();
        var linkInvoice = new Button { Text = "Rechnung verknüpfen …", AutoSize = true }; linkInvoice.Click += async (_, _) => await LinkInvoiceAsync();
        var voidInvoice = new Button { Text = "Rechnungslink stornieren …", AutoSize = true }; voidInvoice.Click += async (_, _) => await VoidInvoiceLinkAsync();
        actions.Controls.AddRange([create, edit, linkDoc, linkInvoice, voidInvoice]); root.Controls.Add(actions, 0, 1);

        var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, Size = new Size(1000, 600) };
        // Splitter limits are assigned only after a realistic initial size exists.
        // WinForms otherwise may reject SplitterDistance during construction.
        split.Panel1MinSize = 180;
        split.Panel2MinSize = 180;
        split.SplitterDistance = 300;
        ConfigureOrderGrid(); split.Panel1.Controls.Add(_orderGrid);
        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(CreateTab("Positionen", _lineGrid, ConfigureLineGrid)); tabs.TabPages.Add(CreateTab("Verknüpfte Rechnungen", _invoiceGrid, ConfigureInvoiceGrid)); tabs.TabPages.Add(CreateTab("Dokumente", _documentGrid, ConfigureDocumentGrid)); split.Panel2.Controls.Add(tabs);
        root.Controls.Add(split, 0, 2); Controls.Add(root);
        _orderGrid.SelectionChanged += async (_, _) => await LoadDetailsAsync();
        _orderGrid.CellDoubleClick += async (_, e) => { if (e.RowIndex >= 0) await EditAsync(); };
        _search.KeyDown += async (_, e) => { if (e.KeyCode == Keys.Enter) await LoadAsync(); };
    }

    private static TabPage CreateTab(string title, DataGridView grid, Action configure)
    {
        var page = new TabPage(title); configure(); page.Controls.Add(grid); return page;
    }

    private void ConfigureOrderGrid()
    {
        ConfigureGrid(_orderGrid);
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SASD-Nr.", DataPropertyName = nameof(OrderRow.Number), Width = 110 });
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Lieferant", DataPropertyName = nameof(OrderRow.Supplier), Width = 220 });
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Lieferanten-Nr.", DataPropertyName = nameof(OrderRow.SupplierNumber), Width = 130 });
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Datum", DataPropertyName = nameof(OrderRow.Date), Width = 90 });
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Lieferung", DataPropertyName = nameof(OrderRow.Delivery), Width = 90 });
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Status", DataPropertyName = nameof(OrderRow.Status), Width = 120 });
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Brutto", DataPropertyName = nameof(OrderRow.Gross), Width = 120 });
        _orderGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Geschäftszweck", DataPropertyName = nameof(OrderRow.Purpose), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 200 });
    }
    private void ConfigureLineGrid()
    {
        ConfigureGrid(_lineGrid);
        _lineGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Pos.", DataPropertyName = nameof(LineRow.Position), Width = 55 });
        _lineGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Artikel / Leistung", DataPropertyName = nameof(LineRow.Item), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 220 });
        _lineGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kategorie", DataPropertyName = nameof(LineRow.Category), Width = 160 });
        _lineGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Netto", DataPropertyName = nameof(LineRow.Net), Width = 110 });
        _lineGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Brutto", DataPropertyName = nameof(LineRow.Gross), Width = 110 });
        _lineGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kennz.", DataPropertyName = nameof(LineRow.Flags), Width = 110 });
    }
    private void ConfigureInvoiceGrid()
    {
        ConfigureGrid(_invoiceGrid);
        _invoiceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SASD-Nr.", DataPropertyName = nameof(InvoiceLinkRow.Number), Width = 110 });
        _invoiceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Lieferanten-Nr.", DataPropertyName = nameof(InvoiceLinkRow.External), Width = 140 });
        _invoiceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Datum", DataPropertyName = nameof(InvoiceLinkRow.Date), Width = 90 });
        _invoiceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Brutto", DataPropertyName = nameof(InvoiceLinkRow.Gross), Width = 120 });
        _invoiceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Link", DataPropertyName = nameof(InvoiceLinkRow.State), Width = 90 });
        _invoiceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Zahlung", DataPropertyName = nameof(InvoiceLinkRow.Payment), Width = 150 });
        _invoiceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Notiz", DataPropertyName = nameof(InvoiceLinkRow.Note), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 160 });
    }
    private void ConfigureDocumentGrid()
    {
        ConfigureGrid(_documentGrid);
        _documentGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Typ", DataPropertyName = nameof(DocumentRow.Type), Width = 110 });
        _documentGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Datum", DataPropertyName = nameof(DocumentRow.Date), Width = 90 });
        _documentGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Datei", DataPropertyName = nameof(DocumentRow.File), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
    }
    private static void ConfigureGrid(DataGridView grid)
    {
        grid.Dock = DockStyle.Fill; grid.ReadOnly = true; grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.MultiSelect = false; grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.AutoGenerateColumns = false; grid.RowHeadersVisible = false;
    }

    private async Task LoadAsync(Guid? selectId = null)
    {
        try
        {
            SetBusy(true);
            var items = await _orders.SearchAsync(new PurchaseOrderSearchCriteria(_search.Text, null, (_status.SelectedItem as StatusOption)?.Status, _includeClosed.Checked), _lifetime.Token);
            _orderGrid.DataSource = items.Select(o => new OrderRow(o.Id, o.OrderNumber, $"{o.SupplierNumber} – {o.SupplierName}", o.SupplierOrderNumber ?? string.Empty, o.OrderDate.ToString("dd.MM.yyyy"), o.ExpectedDeliveryDate?.ToString("dd.MM.yyyy") ?? string.Empty, StatusText(o.Status), $"{o.GrossAmount:N2} {o.CurrencyCode}", o.BusinessPurpose ?? string.Empty)).ToList();
            _orderGrid.ClearSelection();
            if (selectId is Guid id)
            {
                foreach (DataGridViewRow row in _orderGrid.Rows) if (row.DataBoundItem is OrderRow item && item.Id == id) { row.Selected = true; _orderGrid.CurrentCell = row.Cells[0]; break; }
            }
        }
        catch (OperationCanceledException) when (_lifetime.IsCancellationRequested) { }
        catch (Exception ex) { ReportError("Bestellungen konnten nicht geladen werden.", ex); }
        finally { SetBusy(false); }
    }

    private async Task LoadDetailsAsync()
    {
        if (_orderGrid.CurrentRow?.DataBoundItem is not OrderRow row) { _lineGrid.DataSource = null; _invoiceGrid.DataSource = null; _documentGrid.DataSource = null; return; }
        try
        {
            var d = await _orders.GetAsync(row.Id, _lifetime.Token);
            _lineGrid.DataSource = d.Lines.Select(l => new LineRow(l.Position, l.ItemName, l.CategoryName ?? string.Empty, $"{l.NetAmount:N2} {d.CurrencyCode}", $"{l.GrossAmount:N2} {d.CurrencyCode}", (l.AssetCandidate ? "Asset " : "") + (l.InventoryCandidate ? "Inventar" : ""))).ToList();
            var invoiceRows = new List<InvoiceLinkRow>(d.InvoiceLinks.Count);
            foreach (var link in d.InvoiceLinks)
            {
                var payment = await _reconciliation.GetInvoicePaymentSummaryAsync(link.InvoiceId, _lifetime.Token);
                var paymentText = payment.CoverageStatus switch
                {
                    InvoicePaymentCoverageStatus.Open => $"Offen {payment.OutstandingAmount:N2}",
                    InvoicePaymentCoverageStatus.PartiallyPaid => $"Teilbezahlt {payment.PaidAmount:N2}",
                    InvoicePaymentCoverageStatus.Paid => "Bezahlt",
                    _ => payment.CoverageStatus.ToString(),
                };
                invoiceRows.Add(new InvoiceLinkRow(link.Id, link.InvoiceNumber, link.ExternalInvoiceNumber ?? string.Empty, link.InvoiceDate.ToString("dd.MM.yyyy"), $"{link.GrossAmount:N2} {link.CurrencyCode}", link.IsVoided ? "Storniert" : "Aktiv", paymentText, link.Note ?? string.Empty));
            }
            _invoiceGrid.DataSource = invoiceRows;
            _documentGrid.DataSource = d.Documents.Select(doc => new DocumentRow(doc.DocumentType.ToString(), doc.DocumentDate?.ToString("dd.MM.yyyy") ?? string.Empty, doc.OriginalFileName)).ToList();
        }
        catch (OperationCanceledException) when (_lifetime.IsCancellationRequested) { }
        catch (Exception ex) { _logger.LogError(ex, "Bestelldetails konnten nicht geladen werden."); }
    }

    private async Task CreateAsync()
    {
        try
        {
            var suppliers = await _suppliers.SearchAsync(null, false, _lifetime.Token); if (suppliers.Count == 0) { _notifications.ShowError("Bestellungen", "Bitte zuerst einen aktiven Lieferanten anlegen."); return; }
            var categories = await _categories.GetAllAsync(true, _lifetime.Token);
            using var dialog = new PurchaseOrderEditDialog(suppliers, categories);
            if (dialog.ShowDialog(FindForm()) != DialogResult.OK || dialog.CreateRequest is null) return;
            var created = await _orders.CreateAsync(dialog.CreateRequest, _lifetime.Token); await LoadAsync(created.Id);
        }
        catch (OperationCanceledException) when (_lifetime.IsCancellationRequested) { }
        catch (Exception ex) { ReportError("Die Bestellung konnte nicht angelegt werden.", ex); }
    }

    private async Task EditAsync()
    {
        if (_orderGrid.CurrentRow?.DataBoundItem is not OrderRow row) return;
        try
        {
            var details = await _orders.GetAsync(row.Id, _lifetime.Token);
            var suppliers = await _suppliers.SearchAsync(null, true, _lifetime.Token);
            var allowed = suppliers.Where(s => s.IsActive || s.Id == details.SupplierId).ToArray();
            var categories = await _categories.GetAllAsync(true, _lifetime.Token);
            using var dialog = new PurchaseOrderEditDialog(allowed, categories, details);
            if (dialog.ShowDialog(FindForm()) != DialogResult.OK || dialog.UpdateRequest is null) return;
            var updated = await _orders.UpdateAsync(dialog.UpdateRequest, _lifetime.Token); await LoadAsync(updated.Id);
        }
        catch (OperationCanceledException) when (_lifetime.IsCancellationRequested) { }
        catch (Exception ex) { ReportError("Die Bestellung konnte nicht bearbeitet werden.", ex); }
    }

    private async Task LinkDocumentAsync()
    {
        if (_orderGrid.CurrentRow?.DataBoundItem is not OrderRow row) { _notifications.ShowError("Bestellungen", "Bitte zuerst eine Bestellung auswählen."); return; }
        try
        {
            var docs = await _documents.SearchAsync(new DocumentSearchCriteria(null, null, null), _lifetime.Token); if (docs.Count == 0) { _notifications.ShowInformation("Bestellungen", "Im Dokumentenarchiv sind noch keine Dokumente vorhanden."); return; }
            using var dialog = new PurchaseOrderDocumentSelectionDialog(docs);
            if (dialog.ShowDialog(FindForm()) != DialogResult.OK || dialog.SelectedDocumentId is not Guid documentId) return;
            var created = await _orders.LinkDocumentAsync(row.Id, documentId, _lifetime.Token);
            _notifications.ShowInformation("Bestellungen", created ? "Das Dokument wurde verknüpft." : "Diese Dokumentverknüpfung existiert bereits."); await LoadDetailsAsync();
        }
        catch (Exception ex) { ReportError("Das Dokument konnte nicht verknüpft werden.", ex); }
    }

    private async Task LinkInvoiceAsync()
    {
        if (_orderGrid.CurrentRow?.DataBoundItem is not OrderRow row) { _notifications.ShowError("Bestellungen", "Bitte zuerst eine Bestellung auswählen."); return; }
        try
        {
            var candidates = await _orders.GetInvoiceCandidatesAsync(row.Id, _lifetime.Token); if (candidates.Count == 0) { _notifications.ShowInformation("Bestellungen", "Keine passende Rechnung desselben Lieferanten und derselben Währung gefunden."); return; }
            using var dialog = new PurchaseOrderInvoiceSelectionDialog(candidates);
            if (dialog.ShowDialog(FindForm()) != DialogResult.OK || dialog.SelectedInvoiceId is not Guid invoiceId) return;
            await _orders.LinkInvoiceAsync(new LinkOrderInvoiceRequest(row.Id, invoiceId, dialog.Note), _lifetime.Token); await LoadDetailsAsync();
        }
        catch (Exception ex) { ReportError("Die Rechnung konnte nicht verknüpft werden.", ex); }
    }

    private async Task VoidInvoiceLinkAsync()
    {
        if (_invoiceGrid.CurrentRow?.DataBoundItem is not InvoiceLinkRow row)
        {
            _notifications.ShowError("Bestellungen", "Bitte einen aktiven Rechnungslink auswählen.");
            return;
        }
        if (row.State == "Storniert")
        {
            _notifications.ShowError("Bestellungen", "Der ausgewählte Rechnungslink ist bereits storniert.");
            return;
        }
        using var dialog = new VoidReasonDialog("Bestell-/Rechnungsverknüpfung stornieren");
        if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;
        try { await _orders.VoidInvoiceLinkAsync(row.Id, dialog.Reason, _lifetime.Token); await LoadDetailsAsync(); }
        catch (Exception ex) { ReportError("Die Rechnungsverknüpfung konnte nicht storniert werden.", ex); }
    }

    private void SetBusy(bool busy) { UseWaitCursor = busy; _orderGrid.Enabled = !busy; }
    private void ReportError(string message, Exception ex) { _logger.LogError(ex, "{Message}", message); _notifications.ShowError("Bestellungen", message + Environment.NewLine + ex.Message); }
    private static string StatusText(PurchaseOrderStatus status) => status switch { PurchaseOrderStatus.Draft => "Entwurf", PurchaseOrderStatus.Ordered => "Bestellt", PurchaseOrderStatus.PartiallyReceived => "Teilweise geliefert", PurchaseOrderStatus.Received => "Geliefert", PurchaseOrderStatus.Cancelled => "Storniert", PurchaseOrderStatus.Closed => "Abgeschlossen", _ => status.ToString() };
    private sealed record StatusOption(PurchaseOrderStatus? Status, string Text) { public override string ToString() => Text; }
    private sealed record OrderRow(Guid Id, string Number, string Supplier, string SupplierNumber, string Date, string Delivery, string Status, string Gross, string Purpose);
    private sealed record LineRow(int Position, string Item, string Category, string Net, string Gross, string Flags);
    private sealed record InvoiceLinkRow(Guid Id, string Number, string External, string Date, string Gross, string State, string Payment, string Note);
    private sealed record DocumentRow(string Type, string Date, string File);
}
