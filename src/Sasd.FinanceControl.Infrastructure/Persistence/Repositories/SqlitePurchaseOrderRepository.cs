using System.Globalization;
using Microsoft.Data.Sqlite;
using Sasd.FinanceControl.Application.Common;
using Sasd.FinanceControl.Application.Orders;
using Sasd.FinanceControl.Application.Time;
using Sasd.FinanceControl.Domain.Entities;

namespace Sasd.FinanceControl.Infrastructure.Persistence.Repositories;

/// <summary>SQLite persistence for purchase orders, line items and invoice links.</summary>
public sealed class SqlitePurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;
    private readonly IApplicationClock _clock;

    public SqlitePurchaseOrderRepository(SqliteConnectionFactory connectionFactory, IApplicationClock clock)
    {
        _connectionFactory = connectionFactory;
        _clock = clock;
    }

    public async Task<string> ReserveNextOrderNumberAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await using var ensure = connection.CreateCommand();
            ensure.Transaction = transaction;
            ensure.CommandText = "INSERT OR IGNORE INTO number_sequences(sequence_name, current_value) VALUES ('purchase_order', 0);";
            await ensure.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            await using var update = connection.CreateCommand();
            update.Transaction = transaction;
            update.CommandText = "UPDATE number_sequences SET current_value = current_value + 1 WHERE sequence_name = 'purchase_order' RETURNING current_value;";
            var value = Convert.ToInt64(await update.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false), CultureInfo.InvariantCulture);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return $"PO-{value:000000}";
        }
        catch
        {
            await SafeRollbackAsync(transaction, cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    public async Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await InsertHeaderAsync(connection, transaction, order, cancellationToken).ConfigureAwait(false);
            foreach (var line in order.Lines)
                await InsertLineAsync(connection, transaction, order.Id, line, cancellationToken).ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await SafeRollbackAsync(transaction, cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    public async Task UpdateAsync(PurchaseOrder order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText =
                    """
                    UPDATE purchase_orders
                    SET supplier_id = $supplierId,
                        supplier_order_number = $supplierOrderNumber,
                        order_date = $orderDate,
                        expected_delivery_date = $expectedDeliveryDate,
                        status = $status,
                        currency_code = $currency,
                        business_purpose = $purpose,
                        notes = $notes,
                        total_net_decimal = $net,
                        total_tax_decimal = $tax,
                        total_gross_decimal = $gross,
                        updated_at_utc = $updated
                    WHERE id = $id;
                    """;
                AddHeaderParameters(command, order, includeIdentity: false);
                if (await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
                    throw new MasterDataNotFoundException("Die Bestellung wurde beim Speichern nicht gefunden.");
            }

            var existingIds = await LoadLineIdsAsync(connection, transaction, order.Id, cancellationToken).ConfigureAwait(false);
            var requestedIds = order.Lines.Select(line => line.Id).ToHashSet();
            foreach (var obsoleteId in existingIds.Except(requestedIds))
            {
                await using var delete = connection.CreateCommand();
                delete.Transaction = transaction;
                delete.CommandText = "DELETE FROM purchase_order_items WHERE id = $id AND purchase_order_id = $orderId;";
                delete.Parameters.AddWithValue("$id", obsoleteId.ToString("D"));
                delete.Parameters.AddWithValue("$orderId", order.Id.ToString("D"));
                await delete.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }

            // Avoid transient unique-position collisions while positions are reordered.
            foreach (var existingId in existingIds.Intersect(requestedIds).ToArray())
            {
                await using var temp = connection.CreateCommand();
                temp.Transaction = transaction;
                temp.CommandText = "UPDATE purchase_order_items SET position = position + 1000000 WHERE id = $id AND purchase_order_id = $orderId;";
                temp.Parameters.AddWithValue("$id", existingId.ToString("D"));
                temp.Parameters.AddWithValue("$orderId", order.Id.ToString("D"));
                await temp.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }

            foreach (var line in order.Lines)
            {
                if (existingIds.Contains(line.Id))
                    await UpdateLineAsync(connection, transaction, line, cancellationToken).ConfigureAwait(false);
                else
                    await InsertLineAsync(connection, transaction, order.Id, line, cancellationToken).ConfigureAwait(false);
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await SafeRollbackAsync(transaction, cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        var header = await GetHeaderAsync(connection, id, cancellationToken).ConfigureAwait(false);
        if (header is null) return null;
        var lines = await LoadLinesAsync(connection, id, cancellationToken).ConfigureAwait(false);
        return Restore(header, lines);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> SearchAsync(PurchaseOrderSearchCriteria criteria, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT id, order_number, supplier_id, supplier_order_number, order_date,
                   expected_delivery_date, status, currency_code, business_purpose, notes
            FROM purchase_orders
            WHERE ($supplierId IS NULL OR supplier_id = $supplierId)
              AND ($status IS NULL OR status = $status)
              AND ($includeClosed = 1 OR status NOT IN ('cancelled', 'closed'))
              AND ($search IS NULL OR order_number LIKE $pattern COLLATE NOCASE
                   OR IFNULL(supplier_order_number, '') LIKE $pattern COLLATE NOCASE
                   OR IFNULL(business_purpose, '') LIKE $pattern COLLATE NOCASE
                   OR IFNULL(notes, '') LIKE $pattern COLLATE NOCASE)
            ORDER BY order_date DESC, order_number DESC;
            """;
        AddNullable(command, "$supplierId", criteria.SupplierId?.ToString("D"));
        AddNullable(command, "$status", criteria.Status.HasValue ? ToDatabaseStatus(criteria.Status.Value) : null);
        command.Parameters.AddWithValue("$includeClosed", criteria.IncludeClosed ? 1 : 0);
        var search = string.IsNullOrWhiteSpace(criteria.SearchText) ? null : criteria.SearchText.Trim();
        AddNullable(command, "$search", search);
        AddNullable(command, "$pattern", search is null ? null : $"%{search}%");

        var headers = new List<OrderHeader>();
        await using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
        {
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) headers.Add(ReadHeader(reader));
        }

        var result = new List<PurchaseOrder>(headers.Count);
        foreach (var header in headers)
            result.Add(Restore(header, await LoadLinesAsync(connection, header.Id, cancellationToken).ConfigureAwait(false)));
        return result;
    }

    public async Task AddInvoiceLinkAsync(PurchaseOrderInvoiceLink link, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(link);
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO purchase_order_invoice_links
                (id, purchase_order_id, invoice_id, note, created_at_utc, is_voided, voided_at_utc, void_reason)
            VALUES ($id, $orderId, $invoiceId, $note, $created, 0, NULL, NULL);
            """;
        command.Parameters.AddWithValue("$id", link.Id.ToString("D"));
        command.Parameters.AddWithValue("$orderId", link.PurchaseOrderId.ToString("D"));
        command.Parameters.AddWithValue("$invoiceId", link.InvoiceId.ToString("D"));
        AddNullable(command, "$note", link.Note);
        command.Parameters.AddWithValue("$created", link.CreatedAtUtc.ToString("O", CultureInfo.InvariantCulture));
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateInvoiceLinkAsync(PurchaseOrderInvoiceLink link, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(link);
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = connection.CreateCommand();
        command.CommandText = "UPDATE purchase_order_invoice_links SET is_voided = $voided, voided_at_utc = $voidedAt, void_reason = $reason WHERE id = $id;";
        command.Parameters.AddWithValue("$id", link.Id.ToString("D"));
        command.Parameters.AddWithValue("$voided", link.IsVoided ? 1 : 0);
        AddNullable(command, "$voidedAt", link.VoidedAtUtc?.ToString("O", CultureInfo.InvariantCulture));
        AddNullable(command, "$reason", link.VoidReason);
        if (await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
            throw new MasterDataNotFoundException("Die Bestell-/Rechnungsverknüpfung wurde beim Speichern nicht gefunden.");
    }

    public async Task<PurchaseOrderInvoiceLink?> GetInvoiceLinkAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = connection.CreateCommand();
        command.CommandText = LinkSelect + " WHERE id = $id;";
        command.Parameters.AddWithValue("$id", id.ToString("D"));
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        return await reader.ReadAsync(cancellationToken).ConfigureAwait(false) ? ReadLink(reader) : null;
    }

    public async Task<IReadOnlyList<PurchaseOrderInvoiceLink>> GetInvoiceLinksAsync(Guid orderId, bool includeVoided, CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = connection.CreateCommand();
        command.CommandText = LinkSelect + " WHERE purchase_order_id = $orderId AND ($includeVoided = 1 OR is_voided = 0) ORDER BY created_at_utc;";
        command.Parameters.AddWithValue("$orderId", orderId.ToString("D"));
        command.Parameters.AddWithValue("$includeVoided", includeVoided ? 1 : 0);
        var result = new List<PurchaseOrderInvoiceLink>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) result.Add(ReadLink(reader));
        return result;
    }

    public async Task<bool> ActiveInvoiceLinkExistsAsync(Guid orderId, Guid invoiceId, CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT EXISTS(SELECT 1 FROM purchase_order_invoice_links WHERE purchase_order_id = $orderId AND invoice_id = $invoiceId AND is_voided = 0);";
        command.Parameters.AddWithValue("$orderId", orderId.ToString("D"));
        command.Parameters.AddWithValue("$invoiceId", invoiceId.ToString("D"));
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false), CultureInfo.InvariantCulture) == 1;
    }

    private const string LinkSelect = "SELECT id, purchase_order_id, invoice_id, note, created_at_utc, is_voided, voided_at_utc, void_reason FROM purchase_order_invoice_links";

    private async Task InsertHeaderAsync(SqliteConnection connection, SqliteTransaction transaction, PurchaseOrder order, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO purchase_orders
                (id, order_number, supplier_id, supplier_order_number, order_date, expected_delivery_date,
                 status, currency_code, business_purpose, notes, total_net_decimal, total_tax_decimal,
                 total_gross_decimal, created_at_utc, updated_at_utc)
            VALUES ($id, $number, $supplierId, $supplierOrderNumber, $orderDate, $expectedDeliveryDate,
                    $status, $currency, $purpose, $notes, $net, $tax, $gross, $created, $updated);
            """;
        AddHeaderParameters(command, order, includeIdentity: true);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private void AddHeaderParameters(SqliteCommand command, PurchaseOrder order, bool includeIdentity)
    {
        command.Parameters.AddWithValue("$id", order.Id.ToString("D"));
        if (includeIdentity) command.Parameters.AddWithValue("$number", order.OrderNumber);
        command.Parameters.AddWithValue("$supplierId", order.SupplierId.ToString("D"));
        AddNullable(command, "$supplierOrderNumber", order.SupplierOrderNumber);
        command.Parameters.AddWithValue("$orderDate", FormatDate(order.OrderDate));
        AddNullable(command, "$expectedDeliveryDate", order.ExpectedDeliveryDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$status", ToDatabaseStatus(order.Status));
        command.Parameters.AddWithValue("$currency", order.CurrencyCode);
        AddNullable(command, "$purpose", order.BusinessPurpose);
        AddNullable(command, "$notes", order.Notes);
        command.Parameters.AddWithValue("$net", order.NetAmount.ToString("G29", CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$tax", order.TaxAmount.ToString("G29", CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$gross", order.GrossAmount.ToString("G29", CultureInfo.InvariantCulture));
        if (includeIdentity) command.Parameters.AddWithValue("$created", _clock.UtcNow.ToString("O", CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$updated", _clock.UtcNow.ToString("O", CultureInfo.InvariantCulture));
    }

    private static async Task InsertLineAsync(SqliteConnection connection, SqliteTransaction transaction, Guid orderId, PurchaseOrderLine line, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO purchase_order_items
                (id, purchase_order_id, position, item_name, description, quantity_decimal, unit,
                 unit_price_net_decimal, tax_rate_percent_decimal, category_id, asset_candidate,
                 inventory_candidate, net_amount_decimal, tax_amount_decimal, gross_amount_decimal)
            VALUES ($id, $orderId, $position, $itemName, $description, $quantity, $unit,
                    $unitPrice, $taxRate, $categoryId, $asset, $inventory, $net, $tax, $gross);
            """;
        AddLineParameters(command, orderId, line);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task UpdateLineAsync(SqliteConnection connection, SqliteTransaction transaction, PurchaseOrderLine line, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            UPDATE purchase_order_items
            SET position = $position, item_name = $itemName, description = $description,
                quantity_decimal = $quantity, unit = $unit, unit_price_net_decimal = $unitPrice,
                tax_rate_percent_decimal = $taxRate, category_id = $categoryId,
                asset_candidate = $asset, inventory_candidate = $inventory,
                net_amount_decimal = $net, tax_amount_decimal = $tax, gross_amount_decimal = $gross
            WHERE id = $id;
            """;
        AddLineParameters(command, null, line);
        if (await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
            throw new MasterDataNotFoundException("Eine Bestellposition wurde beim Speichern nicht gefunden.");
    }

    private static void AddLineParameters(SqliteCommand command, Guid? orderId, PurchaseOrderLine line)
    {
        command.Parameters.AddWithValue("$id", line.Id.ToString("D"));
        if (orderId is Guid id) command.Parameters.AddWithValue("$orderId", id.ToString("D"));
        command.Parameters.AddWithValue("$position", line.Position);
        command.Parameters.AddWithValue("$itemName", line.ItemName);
        AddNullable(command, "$description", line.Description);
        command.Parameters.AddWithValue("$quantity", line.Quantity.ToString("G29", CultureInfo.InvariantCulture));
        AddNullable(command, "$unit", line.Unit);
        command.Parameters.AddWithValue("$unitPrice", line.UnitPriceNet.ToString("G29", CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$taxRate", line.TaxRatePercent.ToString("G29", CultureInfo.InvariantCulture));
        AddNullable(command, "$categoryId", line.CategoryId?.ToString("D"));
        command.Parameters.AddWithValue("$asset", line.AssetCandidate ? 1 : 0);
        command.Parameters.AddWithValue("$inventory", line.InventoryCandidate ? 1 : 0);
        command.Parameters.AddWithValue("$net", line.NetAmount.ToString("G29", CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$tax", line.TaxAmount.ToString("G29", CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$gross", line.GrossAmount.ToString("G29", CultureInfo.InvariantCulture));
    }

    private static async Task<HashSet<Guid>> LoadLineIdsAsync(SqliteConnection connection, SqliteTransaction transaction, Guid orderId, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "SELECT id FROM purchase_order_items WHERE purchase_order_id = $orderId;";
        command.Parameters.AddWithValue("$orderId", orderId.ToString("D"));
        var result = new HashSet<Guid>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) result.Add(Guid.Parse(reader.GetString(0)));
        return result;
    }

    private static async Task<OrderHeader?> GetHeaderAsync(SqliteConnection connection, Guid id, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT id, order_number, supplier_id, supplier_order_number, order_date, expected_delivery_date, status, currency_code, business_purpose, notes FROM purchase_orders WHERE id = $id;";
        command.Parameters.AddWithValue("$id", id.ToString("D"));
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        return await reader.ReadAsync(cancellationToken).ConfigureAwait(false) ? ReadHeader(reader) : null;
    }

    private static async Task<IReadOnlyList<PurchaseOrderLine>> LoadLinesAsync(SqliteConnection connection, Guid orderId, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT id, position, item_name, description, quantity_decimal, unit, unit_price_net_decimal,
                   tax_rate_percent_decimal, category_id, asset_candidate, inventory_candidate
            FROM purchase_order_items WHERE purchase_order_id = $orderId ORDER BY position;
            """;
        command.Parameters.AddWithValue("$orderId", orderId.ToString("D"));
        var result = new List<PurchaseOrderLine>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(PurchaseOrderLine.Restore(
                Guid.Parse(reader.GetString(0)), reader.GetInt32(1), reader.GetString(2), ReadNullableString(reader, 3),
                decimal.Parse(reader.GetString(4), CultureInfo.InvariantCulture), ReadNullableString(reader, 5),
                decimal.Parse(reader.GetString(6), CultureInfo.InvariantCulture), decimal.Parse(reader.GetString(7), CultureInfo.InvariantCulture),
                reader.IsDBNull(8) ? null : Guid.Parse(reader.GetString(8)), reader.GetInt32(9) == 1, reader.GetInt32(10) == 1));
        }
        return result;
    }

    private static PurchaseOrder Restore(OrderHeader header, IReadOnlyCollection<PurchaseOrderLine> lines)
        => PurchaseOrder.Restore(header.Id, header.OrderNumber, header.SupplierId, header.SupplierOrderNumber, header.OrderDate, header.ExpectedDeliveryDate, header.Status, header.CurrencyCode, header.BusinessPurpose, header.Notes, lines);

    private static OrderHeader ReadHeader(SqliteDataReader reader)
        => new(Guid.Parse(reader.GetString(0)), reader.GetString(1), Guid.Parse(reader.GetString(2)), ReadNullableString(reader, 3),
            DateOnly.ParseExact(reader.GetString(4), "yyyy-MM-dd", CultureInfo.InvariantCulture),
            reader.IsDBNull(5) ? null : DateOnly.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture),
            ParseStatus(reader.GetString(6)), reader.GetString(7), ReadNullableString(reader, 8), ReadNullableString(reader, 9));

    private static PurchaseOrderInvoiceLink ReadLink(SqliteDataReader reader)
        => PurchaseOrderInvoiceLink.Restore(
            Guid.Parse(reader.GetString(0)), Guid.Parse(reader.GetString(1)), Guid.Parse(reader.GetString(2)), ReadNullableString(reader, 3),
            DateTimeOffset.Parse(reader.GetString(4), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind), reader.GetInt32(5) == 1,
            reader.IsDBNull(6) ? null : DateTimeOffset.Parse(reader.GetString(6), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind), ReadNullableString(reader, 7));

    private static string ToDatabaseStatus(PurchaseOrderStatus status) => status switch
    {
        PurchaseOrderStatus.Draft => "draft",
        PurchaseOrderStatus.Ordered => "ordered",
        PurchaseOrderStatus.PartiallyReceived => "partially_received",
        PurchaseOrderStatus.Received => "received",
        PurchaseOrderStatus.Cancelled => "cancelled",
        PurchaseOrderStatus.Closed => "closed",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown order status."),
    };

    private static PurchaseOrderStatus ParseStatus(string value) => value switch
    {
        "draft" => PurchaseOrderStatus.Draft,
        "ordered" => PurchaseOrderStatus.Ordered,
        "partially_received" => PurchaseOrderStatus.PartiallyReceived,
        "received" => PurchaseOrderStatus.Received,
        "cancelled" => PurchaseOrderStatus.Cancelled,
        "closed" => PurchaseOrderStatus.Closed,
        _ => throw new InvalidDataException($"Unknown purchase-order status '{value}'."),
    };

    private static string FormatDate(DateOnly value) => value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    private static string? ReadNullableString(SqliteDataReader reader, int ordinal) => reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    private static void AddNullable(SqliteCommand command, string name, string? value) => command.Parameters.AddWithValue(name, (object?)value ?? DBNull.Value);

    private static async Task SafeRollbackAsync(SqliteTransaction transaction, CancellationToken cancellationToken)
    {
        try { await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false); }
        catch (InvalidOperationException) { }
    }

    private sealed record OrderHeader(Guid Id, string OrderNumber, Guid SupplierId, string? SupplierOrderNumber, DateOnly OrderDate, DateOnly? ExpectedDeliveryDate, PurchaseOrderStatus Status, string CurrencyCode, string? BusinessPurpose, string? Notes);
}
