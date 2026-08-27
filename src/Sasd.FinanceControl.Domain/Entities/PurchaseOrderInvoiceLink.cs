namespace Sasd.FinanceControl.Domain.Entities;

/// <summary>Append/void relationship between a purchase order and a supplier invoice.</summary>
public sealed class PurchaseOrderInvoiceLink
{
    private PurchaseOrderInvoiceLink(Guid id, Guid purchaseOrderId, Guid invoiceId, string? note, DateTimeOffset createdAtUtc, bool isVoided, DateTimeOffset? voidedAtUtc, string? voidReason)
    {
        Id = id == Guid.Empty ? throw new ArgumentException("A link id is required.", nameof(id)) : id;
        PurchaseOrderId = purchaseOrderId == Guid.Empty ? throw new ArgumentException("An order id is required.", nameof(purchaseOrderId)) : purchaseOrderId;
        InvoiceId = invoiceId == Guid.Empty ? throw new ArgumentException("An invoice id is required.", nameof(invoiceId)) : invoiceId;
        Note = NormalizeOptional(note, 2000, nameof(note));
        CreatedAtUtc = createdAtUtc;
        IsVoided = isVoided;
        VoidedAtUtc = voidedAtUtc;
        VoidReason = NormalizeOptional(voidReason, 1000, nameof(voidReason));
        if ((!isVoided && (voidedAtUtc.HasValue || VoidReason is not null))
            || (isVoided && (!voidedAtUtc.HasValue || VoidReason is null)))
        {
            throw new ArgumentException("The void state, timestamp and reason must be consistent.");
        }
    }

    public Guid Id { get; }
    public Guid PurchaseOrderId { get; }
    public Guid InvoiceId { get; }
    public string? Note { get; }
    public DateTimeOffset CreatedAtUtc { get; }
    public bool IsVoided { get; private set; }
    public DateTimeOffset? VoidedAtUtc { get; private set; }
    public string? VoidReason { get; private set; }

    public static PurchaseOrderInvoiceLink Create(Guid purchaseOrderId, Guid invoiceId, string? note, DateTimeOffset createdAtUtc)
        => new(Guid.NewGuid(), purchaseOrderId, invoiceId, note, createdAtUtc, false, null, null);

    public static PurchaseOrderInvoiceLink Restore(Guid id, Guid purchaseOrderId, Guid invoiceId, string? note, DateTimeOffset createdAtUtc, bool isVoided, DateTimeOffset? voidedAtUtc, string? voidReason)
        => new(id, purchaseOrderId, invoiceId, note, createdAtUtc, isVoided, voidedAtUtc, voidReason);

    public void Void(DateTimeOffset voidedAtUtc, string reason)
    {
        if (IsVoided) throw new InvalidOperationException("The order/invoice link is already voided.");
        var normalizedReason = NormalizeOptional(reason, 1000, nameof(reason)) ?? throw new ArgumentException("A void reason is required.", nameof(reason));
        IsVoided = true;
        VoidedAtUtc = voidedAtUtc;
        VoidReason = normalizedReason;
    }

    private static string? NormalizeOptional(string? value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var normalized = value.Trim();
        if (normalized.Length > maxLength) throw new ArgumentException($"The value may not exceed {maxLength} characters.", parameterName);
        return normalized;
    }
}
