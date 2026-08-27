# SASD Finance Control – Milestone 9 Manifest

## Milestone

**Milestone 9 – Bestellungen / Procurement-Grundlage**

## Ziel

Milestone 9 ergänzt eine klar abgegrenzte Beschaffungsdomäne, ohne Bestellungen mit Rechnungen oder tatsächlichen Bankzahlungen zu vermischen. Bestellungen erhalten stabile Identitäten und Positionen; passende Lieferantenrechnungen werden typisiert und historisch nachvollziehbar verknüpft.

## Neu implementierte Fachfunktionen

- stabile SASD-Bestellnummern `PO-000001`, `PO-000002`, ...
- Lieferant und optionale externe Bestellnummer
- Bestell- und erwartetes Lieferdatum
- Status: Entwurf, Bestellt, Teilweise geliefert, Geliefert, Storniert, Abgeschlossen
- Währung, Geschäftszweck und Notizen
- mehrere Bestellpositionen mit stabilen GUIDs
- Menge, Einheit, Netto-Einzelpreis und Steuersatz
- deterministische Netto-/Steuer-/Bruttoberechnung
- optionale Finanzkategorie je Position
- Marker `Asset-Kandidat` und `Inventar-Kandidat`
- Archivdokumente append-only mit Bestellungen verknüpfen
- typisierte Bestellung-Rechnung-Verknüpfung
- Korrektur eines Rechnungslinks per Void statt physischem Löschen
- Anzeige des tatsächlichen Rechnungs-Zahlungsstatus aus Milestone 8

## Domain

Neu:

- `PurchaseOrder`
- `PurchaseOrderLine`
- `PurchaseOrderStatus`
- `PurchaseOrderInvoiceLink`

Wichtige Invarianten:

- technische IDs und Bestellnummern bleiben stabil
- nicht-leere Bestellungen außerhalb des Entwurfsstatus
- erwartetes Lieferdatum liegt nicht vor dem Bestelldatum
- Währung ist ein normalisierter dreistelliger ISO-artiger Code
- Positions-IDs und Positionsnummern sind innerhalb einer Bestellung eindeutig
- Mengen, Preise und Steuersätze werden validiert
- Rechnungslinks besitzen konsistenten Append/Void-Zustand

## Application

Neu:

- `IPurchaseOrderRepository`
- `PurchaseOrderService`
- Request-/Details-/List-Modelle

Der Service:

- reserviert SASD-Bestellnummern
- schützt neue Zuordnungen auf inaktive Lieferanten/Kategorien
- erhält historische Verweise auf später deaktivierte Stammdaten
- löst Lieferanten-/Kategorienamen für die UI auf
- verwaltet Dokumentlinks
- bietet nur Rechnungen desselben Lieferanten und derselben Währung als Kandidaten an
- verhindert aktive doppelte Bestellung-/Rechnungslinks

## Infrastructure / SQLite

Migration 8 ergänzt:

- `purchase_orders`
- `purchase_order_items`
- `purchase_order_invoice_links`
- Indizes und Foreign Keys
- Sequenz `purchase_order` über die bestehende `number_sequences`-Infrastruktur
- DELETE-Schutz für Bestellungen und Rechnungslinks
- Identity-Schutz für Bestellungen, Positionen und Links
- Schutz von Lieferant/Währung sowie Bestellpositionen, solange aktive Rechnungslinks bestehen

Geldwerte werden weiterhin verlustfrei als invariant formatierte Dezimalstrings persistiert.

## WinForms

Neu:

- `PurchaseOrderManagementView`
- `PurchaseOrderEditDialog`
- `PurchaseOrderLineDialog`
- `PurchaseOrderDocumentSelectionDialog`
- `PurchaseOrderInvoiceSelectionDialog`

Die Bestellansicht enthält:

- Suche und Statusfilter
- Bestellübersicht
- Positionsdetails
- verknüpfte Rechnungen einschließlich Zahlungsstatus
- verknüpfte Archivdokumente
- Aktionen für Neu/Bearbeiten/Dokument verknüpfen/Rechnung verknüpfen/Void

## Tests

Neu hinzugefügte Tests decken u. a. ab:

- deterministische Geldrundung
- Pflichtpositionen für nicht-entworfene Bestellungen
- Lieferdatum-Invariante
- Void-Invariante von Rechnungslinks
- aktive/inaktive Kategorien
- Supplier-Mismatch bei Rechnung/Bestellung
- SQLite-Roundtrip mit stabilen Positions-GUIDs
- Entfernen und Neuordnen von Positionen ohne Unique-Constraint-Kollision
- Void statt DELETE für Rechnungslinks
- DELETE-/Identity-Schutz für Bestellungen
- Navigation: Bestellungen ist als implementierter Bereich sichtbar

## Architekturentscheidung

Eine Bestellung erzeugt keine Zahlung. M9 nutzt bewusst den bereits vorhandenen Nachweispfad:

```text
PurchaseOrder
    ↓ typed link
Invoice
    ↓ InvoicePaymentAllocation
BankTransaction (immutable)
```

Damit bleibt der Kontoauszug weiterhin der Single Point of Truth für tatsächliche Geldbewegungen.

## Noch nicht Teil von Milestone 9

- Wareneingangs-/Lieferobjekte mit Teilmengen
- echtes Asset Management
- Lager-/Bestandsführung
- Freigabe-Workflow / Vier-Augen-Prinzip
- Lieferantenangebote / RFQ
- Reporting und Monatskontrolle (Milestone 10)

## Paketumfang

- Overlay-Dateien: **29**
- neue Dateien gegenüber Milestone 8: **20**
- geänderte Dateien gegenüber Milestone 8: **9**
- gelöschte Dateien: **0**
- vollständiger Arbeitsstand: **185 C#-Dateien** in **12 Projekten**

## Verifikation vor Auslieferung

- alle 8 SQLite-Migrationen wurden sequenziell gegen SQLite ausgeführt
- `PRAGMA integrity_check` ergibt `ok`
- Migration-8-Tabellen und Trigger sind vorhanden
- C#-Quellen wurden strukturell auf Klammer-/String-/Kommentarzustände geprüft
- Projektverweise und Projektgraph werden vor Paketierung geprüft
- Overlay wird gegen den M8-Basisstand erzeugt
- ZIP wird per CRC geprüft
- das Overlay wird auf einen frischen M8-Baum angewendet und bytegenau mit dem M9-Arbeitsstand verglichen

Ein echter `dotnet build` kann in der Erzeugungsumgebung nicht ausgeführt werden, da dort kein .NET SDK installiert ist. Der verbindliche Compiler-/Testlauf erfolgt deshalb auf der Entwicklungsmaschine des Projekts.
