using Sasd.FinanceControl.Application.Categories;
using Sasd.FinanceControl.Application.Common;
using Sasd.FinanceControl.Application.Documents;
using Sasd.FinanceControl.Application.Invoices;
using Sasd.FinanceControl.Application.Suppliers;
using Sasd.FinanceControl.Application.Time;
using Sasd.FinanceControl.Domain.Entities;

namespace Sasd.FinanceControl.Application.Orders;

/// <summary>
/// Coordinates purchase-order use cases independently from WinForms and SQLite.
/// </summary>
public sealed class PurchaseOrderService
{
    private readonly IPurchaseOrderRepository _orders;
    private readonly ISupplierRepository _suppliers;
    private readonly ICategoryRepository _categories;
    private readonly IInvoiceRepository _invoices;
    private readonly IDocumentRepository _documents;
    private readonly IApplicationClock _clock;

    /// <summary>Initializes the purchase-order application service.</summary>
    public PurchaseOrderService(
        IPurchaseOrderRepository orders,
        ISupplierRepository suppliers,
        ICategoryRepository categories,
        IInvoiceRepository invoices,
        IDocumentRepository documents,
        IApplicationClock clock)
    {
        _orders = orders;
        _suppliers = suppliers;
        _categories = categories;
        _invoices = invoices;
        _documents = documents;
        _clock = clock;
    }

    /// <summary>Creates a purchase order for an active supplier.</summary>
    public async Task<PurchaseOrderDetails> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        await RequireSupplierAsync(request.SupplierId, requireActive: true, cancellationToken: cancellationToken).ConfigureAwait(false);
        await ValidateNewLineCategoriesAsync(request.Lines, cancellationToken).ConfigureAwait(false);

        var number = await _orders.ReserveNextOrderNumberAsync(cancellationToken).ConfigureAwait(false);
        var order = PurchaseOrder.Create(
            number,
            request.SupplierId,
            request.SupplierOrderNumber,
            request.OrderDate,
            request.ExpectedDeliveryDate,
            request.Status,
            request.CurrencyCode,
            request.BusinessPurpose,
            request.Notes,
            BuildNewLines(request.Lines));

        await _orders.AddAsync(order, cancellationToken).ConfigureAwait(false);
        return await GetAsync(order.Id, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Updates editable order data while preserving existing line identifiers.</summary>
    public async Task<PurchaseOrderDetails> UpdateAsync(UpdatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var order = await RequireOrderAsync(request.Id, cancellationToken).ConfigureAwait(false);
        var supplierChanged = order.SupplierId != request.SupplierId;
        await RequireSupplierAsync(request.SupplierId, requireActive: supplierChanged, cancellationToken: cancellationToken).ConfigureAwait(false);
        await ValidateUpdatedLineCategoriesAsync(order, request.Lines, cancellationToken).ConfigureAwait(false);

        order.Update(
            request.SupplierId,
            request.SupplierOrderNumber,
            request.OrderDate,
            request.ExpectedDeliveryDate,
            request.Status,
            request.CurrencyCode,
            request.BusinessPurpose,
            request.Notes,
            BuildUpdatedLines(order, request.Lines));

        await _orders.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
        return await GetAsync(order.Id, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Gets one order including supplier, categories, documents and linked invoices.</summary>
    public async Task<PurchaseOrderDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await RequireOrderAsync(id, cancellationToken).ConfigureAwait(false);
        var supplier = await RequireSupplierAsync(order.SupplierId, requireActive: false, cancellationToken: cancellationToken).ConfigureAwait(false);
        var categories = (await _categories.GetAllAsync(includeInactive: true, cancellationToken).ConfigureAwait(false))
            .ToDictionary(category => category.Id);

        var documentLinks = await _documents.GetLinksByTargetAsync(DocumentLinkTargetType.PurchaseOrder, order.Id, cancellationToken).ConfigureAwait(false);
        var documents = new List<PurchaseOrderDocumentItem>(documentLinks.Count);
        foreach (var link in documentLinks)
        {
            var document = await _documents.GetByIdAsync(link.DocumentId, cancellationToken).ConfigureAwait(false);
            if (document is not null)
            {
                documents.Add(new PurchaseOrderDocumentItem(document.Id, document.DocumentType, document.OriginalFileName, document.DocumentDate, document.Sha256Hash));
            }
        }

        var invoiceLinks = await BuildInvoiceLinksAsync(order.Id, includeVoided: true, cancellationToken).ConfigureAwait(false);
        return ToDetails(order, supplier, categories, documents, invoiceLinks);
    }

    /// <summary>Searches purchase orders and resolves supplier names.</summary>
    public async Task<IReadOnlyList<PurchaseOrderListItem>> SearchAsync(PurchaseOrderSearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        var orders = await _orders.SearchAsync(criteria, cancellationToken).ConfigureAwait(false);
        var suppliers = new Dictionary<Guid, Supplier>();
        var result = new List<PurchaseOrderListItem>(orders.Count);
        foreach (var order in orders)
        {
            if (!suppliers.TryGetValue(order.SupplierId, out var supplier))
            {
                supplier = await RequireSupplierAsync(order.SupplierId, requireActive: false, cancellationToken: cancellationToken).ConfigureAwait(false);
                suppliers[order.SupplierId] = supplier;
            }
            result.Add(ToListItem(order, supplier));
        }
        return result;
    }

    /// <summary>Links an archived document to a purchase order without duplicating the file.</summary>
    public async Task<bool> LinkDocumentAsync(Guid orderId, Guid documentId, CancellationToken cancellationToken = default)
    {
        _ = await RequireOrderAsync(orderId, cancellationToken).ConfigureAwait(false);
        var document = await _documents.GetByIdAsync(documentId, cancellationToken).ConfigureAwait(false)
            ?? throw new DocumentNotFoundException("Das ausgewählte Dokument wurde nicht gefunden.");
        return await _documents.AddLinkIfMissingAsync(
            DocumentLink.Create(document.Id, DocumentLinkTargetType.PurchaseOrder, orderId, _clock.UtcNow),
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns invoices that can be linked to an order. Supplier and currency must match.
    /// </summary>
    public async Task<IReadOnlyList<InvoiceListItem>> GetInvoiceCandidatesAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await RequireOrderAsync(orderId, cancellationToken).ConfigureAwait(false);
        var invoices = await _invoices.SearchAsync(new InvoiceSearchCriteria(null, order.SupplierId, null, IncludeCancelled: false), cancellationToken).ConfigureAwait(false);
        var supplier = await RequireSupplierAsync(order.SupplierId, requireActive: false, cancellationToken: cancellationToken).ConfigureAwait(false);
        var result = new List<InvoiceListItem>();
        foreach (var invoice in invoices.Where(invoice => string.Equals(invoice.CurrencyCode, order.CurrencyCode, StringComparison.OrdinalIgnoreCase)))
        {
            result.Add(new InvoiceListItem(
                invoice.Id,
                invoice.InvoiceNumber,
                invoice.SupplierId,
                supplier.SupplierNumber,
                supplier.SupplierName,
                invoice.ExternalInvoiceNumber,
                invoice.InvoiceDate,
                invoice.DueDate,
                invoice.Status,
                invoice.CurrencyCode,
                invoice.NetAmount,
                invoice.TaxAmount,
                invoice.GrossAmount,
                invoice.Lines.Count));
        }
        return result;
    }

    /// <summary>Creates an append/void relationship between an order and an invoice.</summary>
    public async Task<PurchaseOrderInvoiceLinkItem> LinkInvoiceAsync(LinkOrderInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var order = await RequireOrderAsync(request.OrderId, cancellationToken).ConfigureAwait(false);
        if (order.Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Eine stornierte Bestellung kann nicht neu mit einer Rechnung verknüpft werden.");

        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false)
            ?? throw new MasterDataNotFoundException("Die ausgewählte Rechnung wurde nicht gefunden.");
        if (invoice.Status == InvoiceStatus.Cancelled)
            throw new InvalidOperationException("Eine stornierte Rechnung kann nicht mit einer Bestellung verknüpft werden.");
        if (invoice.SupplierId != order.SupplierId)
            throw new InvalidOperationException("Bestellung und Rechnung müssen denselben Lieferanten haben.");
        if (!string.Equals(invoice.CurrencyCode, order.CurrencyCode, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Bestellung und Rechnung müssen dieselbe Währung verwenden.");
        if (await _orders.ActiveInvoiceLinkExistsAsync(order.Id, invoice.Id, cancellationToken).ConfigureAwait(false))
            throw new DuplicateMasterDataException("Diese Rechnung ist bereits aktiv mit der Bestellung verknüpft.");

        var link = PurchaseOrderInvoiceLink.Create(order.Id, invoice.Id, request.Note, _clock.UtcNow);
        await _orders.AddInvoiceLinkAsync(link, cancellationToken).ConfigureAwait(false);
        return ToInvoiceLinkItem(link, invoice);
    }

    /// <summary>Voids an order/invoice link instead of deleting historical reconciliation evidence.</summary>
    public async Task VoidInvoiceLinkAsync(Guid linkId, string reason, CancellationToken cancellationToken = default)
    {
        var link = await _orders.GetInvoiceLinkAsync(linkId, cancellationToken).ConfigureAwait(false)
            ?? throw new MasterDataNotFoundException("Die Bestell-/Rechnungsverknüpfung wurde nicht gefunden.");
        link.Void(_clock.UtcNow, reason);
        await _orders.UpdateInvoiceLinkAsync(link, cancellationToken).ConfigureAwait(false);
    }

    private async Task<IReadOnlyList<PurchaseOrderInvoiceLinkItem>> BuildInvoiceLinksAsync(Guid orderId, bool includeVoided, CancellationToken cancellationToken)
    {
        var links = await _orders.GetInvoiceLinksAsync(orderId, includeVoided, cancellationToken).ConfigureAwait(false);
        var result = new List<PurchaseOrderInvoiceLinkItem>(links.Count);
        foreach (var link in links)
        {
            var invoice = await _invoices.GetByIdAsync(link.InvoiceId, cancellationToken).ConfigureAwait(false);
            if (invoice is not null) result.Add(ToInvoiceLinkItem(link, invoice));
        }
        return result;
    }

    private static PurchaseOrderInvoiceLinkItem ToInvoiceLinkItem(PurchaseOrderInvoiceLink link, Invoice invoice)
        => new(link.Id, invoice.Id, invoice.InvoiceNumber, invoice.ExternalInvoiceNumber, invoice.InvoiceDate, invoice.GrossAmount, invoice.CurrencyCode, link.IsVoided, link.CreatedAtUtc, link.VoidedAtUtc, link.VoidReason, link.Note);

    private static PurchaseOrderListItem ToListItem(PurchaseOrder order, Supplier supplier)
        => new(order.Id, order.OrderNumber, order.SupplierId, supplier.SupplierNumber, supplier.SupplierName, order.SupplierOrderNumber, order.OrderDate, order.ExpectedDeliveryDate, order.Status, order.CurrencyCode, order.BusinessPurpose, order.NetAmount, order.TaxAmount, order.GrossAmount, order.Lines.Count);

    private static PurchaseOrderDetails ToDetails(PurchaseOrder order, Supplier supplier, IReadOnlyDictionary<Guid, Category> categories, IReadOnlyList<PurchaseOrderDocumentItem> documents, IReadOnlyList<PurchaseOrderInvoiceLinkItem> invoiceLinks)
        => new(
            order.Id, order.OrderNumber, order.SupplierId, supplier.SupplierNumber, supplier.SupplierName,
            order.SupplierOrderNumber, order.OrderDate, order.ExpectedDeliveryDate, order.Status, order.CurrencyCode,
            order.BusinessPurpose, order.Notes, order.NetAmount, order.TaxAmount, order.GrossAmount,
            order.Lines.Select(line => new PurchaseOrderLineDetails(
                line.Id, line.Position, line.ItemName, line.Description, line.Quantity, line.Unit,
                line.UnitPriceNet, line.TaxRatePercent, line.CategoryId,
                line.CategoryId is Guid categoryId && categories.TryGetValue(categoryId, out var category) ? category.Name : null,
                line.AssetCandidate, line.InventoryCandidate, line.NetAmount, line.TaxAmount, line.GrossAmount)).ToArray(),
            documents, invoiceLinks);

    private async Task<Supplier> RequireSupplierAsync(Guid id, bool requireActive, CancellationToken cancellationToken)
    {
        var supplier = await _suppliers.GetByIdAsync(id, cancellationToken).ConfigureAwait(false)
            ?? throw new MasterDataNotFoundException("Der Lieferant wurde nicht gefunden.");
        if (requireActive && !supplier.IsActive) throw new InvalidOperationException("Für neue Zuordnungen muss der Lieferant aktiv sein.");
        return supplier;
    }

    private async Task<PurchaseOrder> RequireOrderAsync(Guid id, CancellationToken cancellationToken)
        => await _orders.GetByIdAsync(id, cancellationToken).ConfigureAwait(false)
            ?? throw new MasterDataNotFoundException("Die Bestellung wurde nicht gefunden.");

    private async Task ValidateNewLineCategoriesAsync(IReadOnlyList<PurchaseOrderLineRequest> requests, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(requests);
        foreach (var categoryId in requests.Where(request => request.CategoryId.HasValue).Select(request => request.CategoryId!.Value).Distinct())
            await RequireActiveCategoryAsync(categoryId, cancellationToken).ConfigureAwait(false);
    }

    private async Task ValidateUpdatedLineCategoriesAsync(PurchaseOrder order, IReadOnlyList<PurchaseOrderLineRequest> requests, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(requests);
        var existing = order.Lines.ToDictionary(line => line.Id);
        foreach (var request in requests.Where(request => request.CategoryId.HasValue))
        {
            var categoryId = request.CategoryId!.Value;
            var unchanged = request.Id is Guid lineId && existing.TryGetValue(lineId, out var oldLine) && oldLine.CategoryId == categoryId;
            if (!unchanged) await RequireActiveCategoryAsync(categoryId, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task RequireActiveCategoryAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken).ConfigureAwait(false)
            ?? throw new MasterDataNotFoundException("Die ausgewählte Kategorie wurde nicht gefunden.");
        if (!category.IsActive) throw new InvalidOperationException("Neue Bestellpositionen dürfen nur aktive Kategorien verwenden.");
    }

    private static IReadOnlyList<PurchaseOrderLine> BuildNewLines(IReadOnlyList<PurchaseOrderLineRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);
        var result = new List<PurchaseOrderLine>(requests.Count);
        for (var index = 0; index < requests.Count; index++)
        {
            var request = requests[index];
            if (request.Id is not null) throw new ArgumentException("New order lines must not already have an identifier.", nameof(requests));
            result.Add(PurchaseOrderLine.Create(index + 1, request.ItemName, request.Description, request.Quantity, request.Unit, request.UnitPriceNet, request.TaxRatePercent, request.CategoryId, request.AssetCandidate, request.InventoryCandidate));
        }
        return result;
    }

    private static IReadOnlyList<PurchaseOrderLine> BuildUpdatedLines(PurchaseOrder order, IReadOnlyList<PurchaseOrderLineRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);
        var existingIds = order.Lines.Select(line => line.Id).ToHashSet();
        var suppliedIds = requests.Where(request => request.Id.HasValue).Select(request => request.Id!.Value).ToArray();
        if (suppliedIds.Distinct().Count() != suppliedIds.Length) throw new ArgumentException("An order line cannot occur more than once in the update.", nameof(requests));
        if (suppliedIds.Any(id => !existingIds.Contains(id))) throw new ArgumentException("The update contains an order-line id that does not belong to this order.", nameof(requests));

        var result = new List<PurchaseOrderLine>(requests.Count);
        for (var index = 0; index < requests.Count; index++)
        {
            var request = requests[index];
            result.Add(request.Id is Guid id
                ? PurchaseOrderLine.Revise(id, index + 1, request.ItemName, request.Description, request.Quantity, request.Unit, request.UnitPriceNet, request.TaxRatePercent, request.CategoryId, request.AssetCandidate, request.InventoryCandidate)
                : PurchaseOrderLine.Create(index + 1, request.ItemName, request.Description, request.Quantity, request.Unit, request.UnitPriceNet, request.TaxRatePercent, request.CategoryId, request.AssetCandidate, request.InventoryCandidate));
        }
        return result;
    }
}
