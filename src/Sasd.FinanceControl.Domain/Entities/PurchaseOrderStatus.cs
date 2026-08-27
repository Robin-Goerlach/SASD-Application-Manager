namespace Sasd.FinanceControl.Domain.Entities;

/// <summary>Represents the business lifecycle of a purchase order.</summary>
public enum PurchaseOrderStatus
{
    Draft = 0,
    Ordered = 1,
    PartiallyReceived = 2,
    Received = 3,
    Cancelled = 4,
    Closed = 5,
}
