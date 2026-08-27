using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Abstractions;
using Sasd.FinanceControl.Application.Orders;
using Sasd.FinanceControl.Application.Time;
using Sasd.FinanceControl.Domain.Entities;
using Sasd.FinanceControl.Infrastructure.Persistence;
using Sasd.FinanceControl.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Sasd.FinanceControl.Infrastructure.Tests;

public sealed class SqlitePurchaseOrderPersistenceTests : IDisposable
{
    private readonly string _directory;
    private readonly SqliteConnectionFactory _connectionFactory;
    private readonly FakeClock _clock = new(new DateTimeOffset(2026, 8, 27, 7, 0, 0, TimeSpan.Zero));

    public SqlitePurchaseOrderPersistenceTests()
    {
        _directory = Path.Combine(Path.GetTempPath(), "SasdFinanceOrderTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
        _connectionFactory = new SqliteConnectionFactory(Path.Combine(_directory, "finance-control-test.db"));
    }

    [Fact]
    public async Task AddAndGetAsync_RoundTripsOrderAndStableLines()
    {
        await CreateInitializer().InitializeAsync();
        var supplier = await CreateSupplierAsync();
        var category = await CreateCategoryAsync();
        var repository = new SqlitePurchaseOrderRepository(_connectionFactory, _clock);
        var line = PurchaseOrderLine.Create(1, "Router", "Lab router", 2m, "Stück", 100m, 19m, category.Id, true, true);
        var order = PurchaseOrder.Create(
            await repository.ReserveNextOrderNumberAsync(), supplier.Id, "WEB-42", new DateOnly(2026, 8, 27),
            new DateOnly(2026, 8, 30), PurchaseOrderStatus.Ordered, "EUR", "Network lab", "Roundtrip", [line]);

        await repository.AddAsync(order);
        var restored = await repository.GetByIdAsync(order.Id);

        Assert.NotNull(restored);
        Assert.Equal("PO-000001", restored!.OrderNumber);
        Assert.Equal(line.Id, restored.Lines[0].Id);
        Assert.Equal(category.Id, restored.Lines[0].CategoryId);
        Assert.Equal(238m, restored.GrossAmount);
    }

    [Fact]
    public async Task UpdateAsync_RemovingMiddleLine_ReindexesWithoutUniqueConstraintFailure()
    {
        await CreateInitializer().InitializeAsync();
        var supplier = await CreateSupplierAsync();
        var repository = new SqlitePurchaseOrderRepository(_connectionFactory, _clock);
        var first = PurchaseOrderLine.Create(1, "A", null, 1m, null, 10m, 19m, null, false, false);
        var removed = PurchaseOrderLine.Create(2, "B", null, 1m, null, 20m, 19m, null, false, false);
        var third = PurchaseOrderLine.Create(3, "C", null, 1m, null, 30m, 19m, null, false, false);
        var order = PurchaseOrder.Create(await repository.ReserveNextOrderNumberAsync(), supplier.Id, null, new DateOnly(2026, 8, 27), null, PurchaseOrderStatus.Ordered, "EUR", null, null, [first, removed, third]);
        await repository.AddAsync(order);

        order.Update(supplier.Id, null, order.OrderDate, null, order.Status, order.CurrencyCode, null, null,
            [
                PurchaseOrderLine.Revise(first.Id, 1, first.ItemName, first.Description, first.Quantity, first.Unit, first.UnitPriceNet, first.TaxRatePercent, first.CategoryId, first.AssetCandidate, first.InventoryCandidate),
                PurchaseOrderLine.Revise(third.Id, 2, third.ItemName, third.Description, third.Quantity, third.Unit, third.UnitPriceNet, third.TaxRatePercent, third.CategoryId, third.AssetCandidate, third.InventoryCandidate),
            ]);

        await repository.UpdateAsync(order);
        var restored = await repository.GetByIdAsync(order.Id);

        Assert.NotNull(restored);
        Assert.Equal(2, restored!.Lines.Count);
        Assert.Equal(third.Id, restored.Lines[1].Id);
        Assert.Equal(2, restored.Lines[1].Position);
    }

    [Fact]
    public async Task InvoiceLink_CanBeVoidedButNotDeleted()
    {
        await CreateInitializer().InitializeAsync();
        var supplier = await CreateSupplierAsync();
        var orderRepository = new SqlitePurchaseOrderRepository(_connectionFactory, _clock);
        var invoiceRepository = new SqliteInvoiceRepository(_connectionFactory, _clock);
        var order = PurchaseOrder.Create(await orderRepository.ReserveNextOrderNumberAsync(), supplier.Id, null, new DateOnly(2026, 8, 27), null, PurchaseOrderStatus.Ordered, "EUR", null, null,
            [PurchaseOrderLine.Create(1, "Service", null, 1m, null, 100m, 19m, null, false, false)]);
        var invoice = Invoice.Create(await invoiceRepository.ReserveNextInvoiceNumberAsync(), supplier.Id, "EXT-1", new DateOnly(2026, 8, 27), null, null, null, InvoiceStatus.Open, "EUR", null,
            [InvoiceLine.Create(1, "Service", 1m, null, 100m, 19m)]);
        await orderRepository.AddAsync(order);
        await invoiceRepository.AddAsync(invoice);

        var link = PurchaseOrderInvoiceLink.Create(order.Id, invoice.Id, "Matched", _clock.UtcNow);
        await orderRepository.AddInvoiceLinkAsync(link);
        Assert.True(await orderRepository.ActiveInvoiceLinkExistsAsync(order.Id, invoice.Id));

        link.Void(_clock.UtcNow.AddMinutes(1), "Correction");
        await orderRepository.UpdateInvoiceLinkAsync(link);
        Assert.False(await orderRepository.ActiveInvoiceLinkExistsAsync(order.Id, invoice.Id));

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync();
        var delete = await Assert.ThrowsAsync<SqliteException>(async () =>
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM purchase_order_invoice_links WHERE id = $id;";
            command.Parameters.AddWithValue("$id", link.Id.ToString("D"));
            await command.ExecuteNonQueryAsync();
        });
        Assert.Contains("cannot be deleted", delete.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Migration8_OrderCannotBeDeletedOrRenumbered()
    {
        await CreateInitializer().InitializeAsync();
        var supplier = await CreateSupplierAsync();
        var repository = new SqlitePurchaseOrderRepository(_connectionFactory, _clock);
        var order = PurchaseOrder.Create(await repository.ReserveNextOrderNumberAsync(), supplier.Id, null, new DateOnly(2026, 8, 27), null, PurchaseOrderStatus.Draft, "EUR", null, null, []);
        await repository.AddAsync(order);

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync();
        var update = await Assert.ThrowsAsync<SqliteException>(async () =>
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "UPDATE purchase_orders SET order_number = 'PO-999999' WHERE id = $id;";
            command.Parameters.AddWithValue("$id", order.Id.ToString("D"));
            await command.ExecuteNonQueryAsync();
        });
        Assert.Contains("identity", update.Message, StringComparison.OrdinalIgnoreCase);

        var delete = await Assert.ThrowsAsync<SqliteException>(async () =>
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM purchase_orders WHERE id = $id;";
            command.Parameters.AddWithValue("$id", order.Id.ToString("D"));
            await command.ExecuteNonQueryAsync();
        });
        Assert.Contains("cannot be deleted", delete.Message, StringComparison.OrdinalIgnoreCase);
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        if (Directory.Exists(_directory)) Directory.Delete(_directory, recursive: true);
    }

    private SqliteDatabaseInitializer CreateInitializer()
        => new(_connectionFactory, _clock, NullLogger<SqliteDatabaseInitializer>.Instance);

    private async Task<Supplier> CreateSupplierAsync()
    {
        var repository = new SqliteSupplierRepository(_connectionFactory, _clock);
        var supplier = Supplier.Create(await repository.ReserveNextSupplierNumberAsync(), "Example Supplier", "Supplier");
        await repository.AddAsync(supplier);
        return supplier;
    }

    private async Task<Category> CreateCategoryAsync()
    {
        var repository = new SqliteCategoryRepository(_connectionFactory, _clock);
        var category = Category.Create("Hardware");
        await repository.AddAsync(category);
        return category;
    }

    private sealed class FakeClock : IApplicationClock
    {
        public FakeClock(DateTimeOffset utcNow) => UtcNow = utcNow;
        public DateTimeOffset UtcNow { get; }
    }
}
