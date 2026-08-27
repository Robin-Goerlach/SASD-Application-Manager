# Milestone 9 – SQLite-Schema

## Migration 8

Milestone 9 ergänzt die Beschaffungsdomäne um drei Tabellen.

## `purchase_orders`

Kopfdatensatz einer Bestellung.

Wichtige Felder:

- `id` – stabile GUID
- `order_number` – SASD-Nummer `PO-xxxxxx`
- `supplier_id`
- `supplier_order_number`
- `order_date`
- `expected_delivery_date`
- `status`
- `currency_code`
- `business_purpose`
- `notes`
- `total_net_decimal`
- `total_tax_decimal`
- `total_gross_decimal`
- `created_at_utc`
- `updated_at_utc`

Die Geldwerte werden wie in den bestehenden Finanzmodulen verlustfrei als invariant formatierte Dezimalstrings gespeichert und im Domainmodell als `decimal` rekonstruiert.

## `purchase_order_items`

Stabile Positionen einer Bestellung.

Wichtige Felder:

- `id`
- `purchase_order_id`
- `position`
- `item_name`
- `description`
- `quantity_decimal`
- `unit`
- `unit_price_net_decimal`
- `tax_rate_percent_decimal`
- `category_id`
- `asset_candidate`
- `inventory_candidate`
- `net_amount_decimal`
- `tax_amount_decimal`
- `gross_amount_decimal`

`(purchase_order_id, position)` ist eindeutig.

## `purchase_order_invoice_links`

Append/Void-Verbindung zwischen Bestellung und Rechnung.

Wichtige Felder:

- `id`
- `purchase_order_id`
- `invoice_id`
- `note`
- `created_at_utc`
- `is_voided`
- `voided_at_utc`
- `void_reason`

Ein partieller Unique Index verhindert zwei gleichzeitig aktive identische Bestellung/Rechnung-Beziehungen.

## Schutzregeln

SQLite-Trigger verhindern unter anderem:

- `DELETE` auf `purchase_orders`
- Änderung von `id`, `order_number` oder `created_at_utc`
- nachträgliches Umhängen einer Bestellposition auf eine andere Bestellung
- `DELETE` auf `purchase_order_invoice_links`
- Änderung der Identität eines Rechnungslinks
- Änderung/Löschung von Bestellpositionen bei aktiver Rechnungsverknüpfung
- Lieferanten-/Währungswechsel bei aktiver Rechnungsverknüpfung

Damit bleibt der Beschaffungsverlauf auch vor einem späteren vollständigen Audit-Journal nachvollziehbar.
