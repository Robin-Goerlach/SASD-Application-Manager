using Sasd.FinanceControl.Application.Categories;
using Sasd.FinanceControl.Application.Documents;
using Sasd.FinanceControl.Application.Invoices;
using Sasd.FinanceControl.Application.Orders;
using Sasd.FinanceControl.Application.Suppliers;
using Sasd.FinanceControl.Application.Time;
using Sasd.FinanceControl.Domain.Entities;
using Xunit;

namespace Sasd.FinanceControl.Application.Tests;

public sealed class PurchaseOrderServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 27, 6, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task CreateAsync_ActiveSupplierAndCategory_CreatesStableOrder()
    {
        var supplier = Supplier.Create("SUP-000001", "Example GmbH", "Hardware");
        var category = Category.Create("Hardware");
        var repo = new FakeOrderRepository();
        var service = CreateService(repo, supplier, category);

        var result = await service.CreateAsync(new CreatePurchaseOrderRequest(
            supplier.Id, "EXT-1", new DateOnly(2026, 8, 27), null, PurchaseOrderStatus.Ordered,
            "EUR", "Test", null,
            [new PurchaseOrderLineRequest(null, "Router", null, 1m, "Stück", 100m, 19m, category.Id, true, true)]));

        Assert.Equal("PO-000001", result.OrderNumber);
        Assert.Equal(119m, result.GrossAmount);
        Assert.Equal(category.Id, result.Lines[0].CategoryId);
        Assert.NotNull(repo.Saved);
    }

    [Fact]
    public async Task CreateAsync_InactiveCategory_IsRejected()
    {
        var supplier = Supplier.Create("SUP-000001", "Example GmbH", "Hardware");
        var category = Category.Create("Old");
        category.Deactivate();
        var repo = new FakeOrderRepository();
        var service = CreateService(repo, supplier, category);

        var action = () => service.CreateAsync(new CreatePurchaseOrderRequest(
            supplier.Id, null, new DateOnly(2026, 8, 27), null, PurchaseOrderStatus.Draft,
            "EUR", null, null,
            [new PurchaseOrderLineRequest(null, "Router", null, 1m, null, 100m, 19m, category.Id, false, false)]));

        await Assert.ThrowsAsync<InvalidOperationException>(action);
    }

    [Fact]
    public async Task LinkInvoiceAsync_DifferentSupplier_IsRejected()
    {
        var supplier = Supplier.Create("SUP-000001", "Order Vendor", "Hardware");
        var other = Supplier.Create("SUP-000002", "Other Vendor", "Hardware");
        var order = PurchaseOrder.Create("PO-000001", supplier.Id, null, new DateOnly(2026, 8, 27), null, PurchaseOrderStatus.Ordered, "EUR", null, null,
            [PurchaseOrderLine.Create(1, "Router", null, 1m, null, 100m, 19m, null, false, false)]);
        var invoice = Invoice.Create("INV-000001", other.Id, "EXT", new DateOnly(2026, 8, 27), null, null, null, InvoiceStatus.Open, "EUR", null,
            [InvoiceLine.Create(1, "Router", 1m, null, 100m, 19m)]);
        var repo = new FakeOrderRepository(order);
        var service = new PurchaseOrderService(repo, new FakeSupplierRepository(supplier, other), new FakeCategoryRepository(), new FakeInvoiceRepository(invoice), new FakeDocumentRepository(), new FakeClock(Now));

        var action = () => service.LinkInvoiceAsync(new LinkOrderInvoiceRequest(order.Id, invoice.Id, null));

        await Assert.ThrowsAsync<InvalidOperationException>(action);
    }

    private static PurchaseOrderService CreateService(FakeOrderRepository orders, Supplier supplier, Category category)
        => new(orders, new FakeSupplierRepository(supplier), new FakeCategoryRepository(category), new FakeInvoiceRepository(), new FakeDocumentRepository(), new FakeClock(Now));

    private sealed class FakeOrderRepository : IPurchaseOrderRepository
    {
        private readonly Dictionary<Guid, PurchaseOrder> _orders = [];
        private readonly Dictionary<Guid, PurchaseOrderInvoiceLink> _links = [];
        public FakeOrderRepository(params PurchaseOrder[] orders) { foreach (var order in orders) _orders[order.Id] = order; }
        public PurchaseOrder? Saved { get; private set; }
        public Task<string> ReserveNextOrderNumberAsync(CancellationToken cancellationToken = default) => Task.FromResult("PO-000001");
        public Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default) { Saved = order; _orders[order.Id] = order; return Task.CompletedTask; }
        public Task UpdateAsync(PurchaseOrder order, CancellationToken cancellationToken = default) { Saved = order; _orders[order.Id] = order; return Task.CompletedTask; }
        public Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(_orders.GetValueOrDefault(id));
        public Task<IReadOnlyList<PurchaseOrder>> SearchAsync(PurchaseOrderSearchCriteria criteria, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<PurchaseOrder>>(_orders.Values.ToArray());
        public Task AddInvoiceLinkAsync(PurchaseOrderInvoiceLink link, CancellationToken cancellationToken = default) { _links[link.Id] = link; return Task.CompletedTask; }
        public Task UpdateInvoiceLinkAsync(PurchaseOrderInvoiceLink link, CancellationToken cancellationToken = default) { _links[link.Id] = link; return Task.CompletedTask; }
        public Task<PurchaseOrderInvoiceLink?> GetInvoiceLinkAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(_links.GetValueOrDefault(id));
        public Task<IReadOnlyList<PurchaseOrderInvoiceLink>> GetInvoiceLinksAsync(Guid orderId, bool includeVoided, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<PurchaseOrderInvoiceLink>>(_links.Values.Where(x => x.PurchaseOrderId == orderId && (includeVoided || !x.IsVoided)).ToArray());
        public Task<bool> ActiveInvoiceLinkExistsAsync(Guid orderId, Guid invoiceId, CancellationToken cancellationToken = default) => Task.FromResult(_links.Values.Any(x => x.PurchaseOrderId == orderId && x.InvoiceId == invoiceId && !x.IsVoided));
    }

    private sealed class FakeSupplierRepository : ISupplierRepository
    {
        private readonly Dictionary<Guid, Supplier> _items;
        public FakeSupplierRepository(params Supplier[] suppliers) => _items = suppliers.ToDictionary(x => x.Id);
        public Task<string> ReserveNextSupplierNumberAsync(CancellationToken cancellationToken = default) => Task.FromResult("SUP-999999");
        public Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(_items.GetValueOrDefault(id));
        public Task<IReadOnlyList<Supplier>> SearchAsync(string? searchText, bool includeInactive, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<Supplier>>(_items.Values.ToArray());
    }

    private sealed class FakeCategoryRepository : ICategoryRepository
    {
        private readonly Dictionary<Guid, Category> _items;
        public FakeCategoryRepository(params Category[] categories) => _items = categories.ToDictionary(x => x.Id);
        public Task AddAsync(Category category, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task UpdateAsync(Category category, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(_items.GetValueOrDefault(id));
        public Task<IReadOnlyList<Category>> GetAllAsync(bool includeInactive, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<Category>>(_items.Values.ToArray());
    }

    private sealed class FakeInvoiceRepository : IInvoiceRepository
    {
        private readonly Dictionary<Guid, Invoice> _items;
        public FakeInvoiceRepository(params Invoice[] invoices) => _items = invoices.ToDictionary(x => x.Id);
        public Task<string> ReserveNextInvoiceNumberAsync(CancellationToken cancellationToken = default) => Task.FromResult("INV-1");
        public Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(_items.GetValueOrDefault(id));
        public Task<IReadOnlyList<Invoice>> SearchAsync(InvoiceSearchCriteria criteria, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<Invoice>>(_items.Values.ToArray());
        public Task<bool> ExternalInvoiceNumberExistsAsync(Guid supplierId, string externalInvoiceNumber, Guid? excludingInvoiceId, CancellationToken cancellationToken = default) => Task.FromResult(false);
    }

    private sealed class FakeDocumentRepository : IDocumentRepository
    {
        public Task AddAsync(ArchivedDocument document, IReadOnlyCollection<DocumentLink> initialLinks, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task UpdateMetadataAsync(ArchivedDocument document, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task<ArchivedDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult<ArchivedDocument?>(null);
        public Task<ArchivedDocument?> GetBySha256Async(string sha256Hash, CancellationToken cancellationToken = default) => Task.FromResult<ArchivedDocument?>(null);
        public Task<IReadOnlyList<ArchivedDocument>> SearchAsync(DocumentSearchCriteria criteria, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<ArchivedDocument>>([]);
        public Task<bool> AddLinkIfMissingAsync(DocumentLink link, CancellationToken cancellationToken = default) => Task.FromResult(true);
        public Task<IReadOnlyList<DocumentLink>> GetLinksAsync(Guid documentId, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<DocumentLink>>([]);
    }

    private sealed class FakeClock : IApplicationClock { public FakeClock(DateTimeOffset now) => UtcNow = now; public DateTimeOffset UtcNow { get; } }
}
