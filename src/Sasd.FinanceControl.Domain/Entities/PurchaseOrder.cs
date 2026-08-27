namespace Sasd.FinanceControl.Domain.Entities;

/// <summary>Represents a supplier purchase order and its stable line items.</summary>
/// <remarks>
/// A purchase order documents an approved or planned procurement. It is not an
/// invoice and does not prove that money moved. The later invoice/payment chain
/// remains represented by explicit links to invoice and immutable bank data.
/// </remarks>
public sealed class PurchaseOrder
{
    private List<PurchaseOrderLine> _lines;

    private PurchaseOrder(Guid id, string orderNumber, Guid supplierId, string? supplierOrderNumber, DateOnly orderDate, DateOnly? expectedDeliveryDate, PurchaseOrderStatus status, string currencyCode, string? businessPurpose, string? notes, IReadOnlyCollection<PurchaseOrderLine> lines)
    {
        Id = id == Guid.Empty ? throw new ArgumentException("An order id is required.", nameof(id)) : id;
        OrderNumber = NormalizeRequired(orderNumber, 32, nameof(orderNumber));
        _lines = [];
        Apply(supplierId, supplierOrderNumber, orderDate, expectedDeliveryDate, status, currencyCode, businessPurpose, notes, lines);
    }

    public Guid Id { get; }
    public string OrderNumber { get; }
    public Guid SupplierId { get; private set; }
    public string? SupplierOrderNumber { get; private set; }
    public DateOnly OrderDate { get; private set; }
    public DateOnly? ExpectedDeliveryDate { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public string CurrencyCode { get; private set; } = "EUR";
    public string? BusinessPurpose { get; private set; }
    public string? Notes { get; private set; }
    public IReadOnlyList<PurchaseOrderLine> Lines => _lines;
    public decimal NetAmount => _lines.Sum(line => line.NetAmount);
    public decimal TaxAmount => _lines.Sum(line => line.TaxAmount);
    public decimal GrossAmount => _lines.Sum(line => line.GrossAmount);

    public static PurchaseOrder Create(string orderNumber, Guid supplierId, string? supplierOrderNumber, DateOnly orderDate, DateOnly? expectedDeliveryDate, PurchaseOrderStatus status, string currencyCode, string? businessPurpose, string? notes, IReadOnlyCollection<PurchaseOrderLine> lines)
        => new(Guid.NewGuid(), orderNumber, supplierId, supplierOrderNumber, orderDate, expectedDeliveryDate, status, currencyCode, businessPurpose, notes, lines);

    public static PurchaseOrder Restore(Guid id, string orderNumber, Guid supplierId, string? supplierOrderNumber, DateOnly orderDate, DateOnly? expectedDeliveryDate, PurchaseOrderStatus status, string currencyCode, string? businessPurpose, string? notes, IReadOnlyCollection<PurchaseOrderLine> lines)
        => new(id, orderNumber, supplierId, supplierOrderNumber, orderDate, expectedDeliveryDate, status, currencyCode, businessPurpose, notes, lines);

    public void Update(Guid supplierId, string? supplierOrderNumber, DateOnly orderDate, DateOnly? expectedDeliveryDate, PurchaseOrderStatus status, string currencyCode, string? businessPurpose, string? notes, IReadOnlyCollection<PurchaseOrderLine> lines)
        => Apply(supplierId, supplierOrderNumber, orderDate, expectedDeliveryDate, status, currencyCode, businessPurpose, notes, lines);

    private void Apply(Guid supplierId, string? supplierOrderNumber, DateOnly orderDate, DateOnly? expectedDeliveryDate, PurchaseOrderStatus status, string currencyCode, string? businessPurpose, string? notes, IReadOnlyCollection<PurchaseOrderLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        if (supplierId == Guid.Empty) throw new ArgumentException("A supplier id is required.", nameof(supplierId));
        if (!Enum.IsDefined(status)) throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown order status.");
        if (expectedDeliveryDate is DateOnly expected && expected < orderDate) throw new ArgumentException("The expected delivery date must not be before the order date.", nameof(expectedDeliveryDate));

        var materialized = lines.OrderBy(line => line.Position).ToList();
        if (materialized.Select(line => line.Id).Distinct().Count() != materialized.Count) throw new ArgumentException("Order-line identifiers must be unique.", nameof(lines));
        if (materialized.Select(line => line.Position).Distinct().Count() != materialized.Count) throw new ArgumentException("Order-line positions must be unique.", nameof(lines));
        if (status != PurchaseOrderStatus.Draft && materialized.Count == 0) throw new ArgumentException("A non-draft order requires at least one line item.", nameof(lines));
        if ((status is PurchaseOrderStatus.Ordered or PurchaseOrderStatus.PartiallyReceived or PurchaseOrderStatus.Received or PurchaseOrderStatus.Closed)
            && materialized.Sum(line => line.GrossAmount) <= 0m)
            throw new ArgumentException("An active/completed order must have a positive gross amount.", nameof(lines));

        SupplierId = supplierId;
        SupplierOrderNumber = NormalizeOptional(supplierOrderNumber, 120, nameof(supplierOrderNumber));
        OrderDate = orderDate;
        ExpectedDeliveryDate = expectedDeliveryDate;
        Status = status;
        CurrencyCode = NormalizeCurrencyCode(currencyCode);
        BusinessPurpose = NormalizeOptional(businessPurpose, 1000, nameof(businessPurpose));
        Notes = NormalizeOptional(notes, 6000, nameof(notes));
        _lines = materialized;
    }

    private static string NormalizeCurrencyCode(string value)
    {
        var normalized = NormalizeRequired(value, 3, nameof(value)).ToUpperInvariant();
        if (normalized.Length != 3 || normalized.Any(character => !char.IsAsciiLetterUpper(character)))
            throw new ArgumentException("The currency code must contain exactly three letters.", nameof(value));
        return normalized;
    }
    private static string NormalizeRequired(string value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("A value is required.", parameterName);
        var normalized = value.Trim();
        if (normalized.Length > maxLength) throw new ArgumentException($"The value may not exceed {maxLength} characters.", parameterName);
        return normalized;
    }
    private static string? NormalizeOptional(string? value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var normalized = value.Trim();
        if (normalized.Length > maxLength) throw new ArgumentException($"The value may not exceed {maxLength} characters.", parameterName);
        return normalized;
    }
}
