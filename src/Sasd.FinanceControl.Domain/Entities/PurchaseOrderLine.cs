namespace Sasd.FinanceControl.Domain.Entities;

/// <summary>Represents one stable line item of a purchase order.</summary>
/// <remarks>
/// Order lines deliberately receive stable GUIDs. This keeps later links to assets,
/// deliveries or inventory possible without redesigning historical orders.
/// Monetary totals are rounded deterministically to cent precision.
/// </remarks>
public sealed class PurchaseOrderLine
{
    private PurchaseOrderLine(
        Guid id,
        int position,
        string itemName,
        string? description,
        decimal quantity,
        string? unit,
        decimal unitPriceNet,
        decimal taxRatePercent,
        Guid? categoryId,
        bool assetCandidate,
        bool inventoryCandidate)
    {
        Id = id == Guid.Empty ? throw new ArgumentException("An order-line id is required.", nameof(id)) : id;
        if (position <= 0) throw new ArgumentOutOfRangeException(nameof(position), "The position must be greater than zero.");
        if (quantity <= 0m || quantity > 1_000_000m) throw new ArgumentOutOfRangeException(nameof(quantity), "The quantity is outside the supported range.");
        if (unitPriceNet < 0m || unitPriceNet > 100_000_000m) throw new ArgumentOutOfRangeException(nameof(unitPriceNet), "The unit price is outside the supported range.");
        if (taxRatePercent < 0m || taxRatePercent > 100m) throw new ArgumentOutOfRangeException(nameof(taxRatePercent), "The tax rate must be between 0 and 100 percent.");

        Position = position;
        ItemName = NormalizeRequired(itemName, 300, nameof(itemName));
        Description = NormalizeOptional(description, 2000, nameof(description));
        Quantity = quantity;
        Unit = NormalizeOptional(unit, 40, nameof(unit));
        UnitPriceNet = unitPriceNet;
        TaxRatePercent = taxRatePercent;
        CategoryId = categoryId;
        AssetCandidate = assetCandidate;
        InventoryCandidate = inventoryCandidate;
        NetAmount = RoundMoney(quantity * unitPriceNet);
        TaxAmount = RoundMoney(NetAmount * taxRatePercent / 100m);
        GrossAmount = NetAmount + TaxAmount;
    }

    public Guid Id { get; }
    public int Position { get; }
    public string ItemName { get; }
    public string? Description { get; }
    public decimal Quantity { get; }
    public string? Unit { get; }
    public decimal UnitPriceNet { get; }
    public decimal TaxRatePercent { get; }
    public Guid? CategoryId { get; }
    public bool AssetCandidate { get; }
    public bool InventoryCandidate { get; }
    public decimal NetAmount { get; }
    public decimal TaxAmount { get; }
    public decimal GrossAmount { get; }

    public static PurchaseOrderLine Create(int position, string itemName, string? description, decimal quantity, string? unit, decimal unitPriceNet, decimal taxRatePercent, Guid? categoryId, bool assetCandidate, bool inventoryCandidate)
        => new(Guid.NewGuid(), position, itemName, description, quantity, unit, unitPriceNet, taxRatePercent, categoryId, assetCandidate, inventoryCandidate);

    public static PurchaseOrderLine Revise(Guid id, int position, string itemName, string? description, decimal quantity, string? unit, decimal unitPriceNet, decimal taxRatePercent, Guid? categoryId, bool assetCandidate, bool inventoryCandidate)
        => new(id, position, itemName, description, quantity, unit, unitPriceNet, taxRatePercent, categoryId, assetCandidate, inventoryCandidate);

    public static PurchaseOrderLine Restore(Guid id, int position, string itemName, string? description, decimal quantity, string? unit, decimal unitPriceNet, decimal taxRatePercent, Guid? categoryId, bool assetCandidate, bool inventoryCandidate)
        => new(id, position, itemName, description, quantity, unit, unitPriceNet, taxRatePercent, categoryId, assetCandidate, inventoryCandidate);

    private static decimal RoundMoney(decimal value) => decimal.Round(value, 2, MidpointRounding.AwayFromZero);
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
