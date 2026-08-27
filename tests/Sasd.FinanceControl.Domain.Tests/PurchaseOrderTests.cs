using Sasd.FinanceControl.Domain.Entities;
using Xunit;

namespace Sasd.FinanceControl.Domain.Tests;

public sealed class PurchaseOrderTests
{
    [Fact]
    public void Create_OrderedOrderWithLines_CalculatesTotals()
    {
        var supplierId = Guid.NewGuid();
        var order = PurchaseOrder.Create(
            "PO-000001", supplierId, "WEB-4711", new DateOnly(2026, 8, 27), new DateOnly(2026, 8, 30),
            PurchaseOrderStatus.Ordered, "eur", "Lab hardware", null,
            [PurchaseOrderLine.Create(1, "SSD", null, 2m, "Stück", 100m, 19m, null, true, true)]);

        Assert.Equal("EUR", order.CurrencyCode);
        Assert.Equal(200m, order.NetAmount);
        Assert.Equal(38m, order.TaxAmount);
        Assert.Equal(238m, order.GrossAmount);
        Assert.Equal(PurchaseOrderStatus.Ordered, order.Status);
    }

    [Fact]
    public void Create_NonDraftWithoutLines_IsRejected()
    {
        var action = () => PurchaseOrder.Create(
            "PO-000001", Guid.NewGuid(), null, new DateOnly(2026, 8, 27), null,
            PurchaseOrderStatus.Ordered, "EUR", null, null, []);

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Create_DeliveryBeforeOrderDate_IsRejected()
    {
        var action = () => PurchaseOrder.Create(
            "PO-000001", Guid.NewGuid(), null, new DateOnly(2026, 8, 27), new DateOnly(2026, 8, 26),
            PurchaseOrderStatus.Draft, "EUR", null, null, []);

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void PurchaseOrderLine_RoundsMoneyDeterministically()
    {
        var line = PurchaseOrderLine.Create(1, "Service", null, 3m, "Stunde", 10.005m, 19m, null, false, false);

        Assert.Equal(30.02m, line.NetAmount);
        Assert.Equal(5.70m, line.TaxAmount);
        Assert.Equal(35.72m, line.GrossAmount);
    }

    [Fact]
    public void InvoiceLink_Void_PreservesIdentityAndRequiresReason()
    {
        var link = PurchaseOrderInvoiceLink.Create(Guid.NewGuid(), Guid.NewGuid(), "Initial", DateTimeOffset.UtcNow);
        var id = link.Id;

        link.Void(DateTimeOffset.UtcNow.AddMinutes(1), "Wrong invoice");

        Assert.True(link.IsVoided);
        Assert.Equal(id, link.Id);
        Assert.Equal("Wrong invoice", link.VoidReason);
        Assert.Throws<InvalidOperationException>(() => link.Void(DateTimeOffset.UtcNow, "Again"));
    }
}
