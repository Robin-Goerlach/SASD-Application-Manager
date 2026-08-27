# Milestone 9 – Implementierungsnotizen

## Ziel

Milestone 9 ergänzt SASD Finance Control um die Beschaffungsdomäne. Bestellungen werden als eigenständige Fachobjekte verwaltet und können mit Lieferanten, Kategorien, Archivdokumenten und später eingegangenen Rechnungen verbunden werden.

Der zentrale Finanzgrundsatz bleibt unverändert:

> Eine Bestellung dokumentiert eine Beschaffungsabsicht bzw. einen Beschaffungsvorgang. Sie ist weder Rechnung noch Zahlungsnachweis. Tatsächlicher Geldfluss wird weiterhin ausschließlich durch unveränderliche Banktransaktionen dargestellt.

## 1. PurchaseOrder Aggregate

Eine Bestellung besitzt eine stabile technische GUID und eine menschenlesbare SASD-Nummer im Format `PO-000001`.

Erfasst werden:

- Lieferant
- optionale Lieferanten-/Webshop-Bestellnummer
- Bestelldatum
- erwartetes Lieferdatum
- Status
- Währung
- Geschäftszweck
- interne Notizen
- mehrere stabile Bestellpositionen

Statuswerte:

- `Draft`
- `Ordered`
- `PartiallyReceived`
- `Received`
- `Cancelled`
- `Closed`

Nicht-Entwürfe benötigen mindestens eine Position. Aktive bzw. abgeschlossene Bestellungen benötigen einen positiven Gesamtbetrag.

## 2. PurchaseOrderLine

Bestellpositionen erhalten stabile GUIDs und enthalten:

- Positionsnummer
- Artikel-/Leistungsname
- optionale Beschreibung
- Menge und Einheit
- Einzelpreis netto
- Umsatzsteuersatz
- optionale Finanzkategorie
- Kennzeichnung als Asset-Kandidat
- Kennzeichnung als Inventar-Kandidat

Netto, Steuer und Brutto werden deterministisch mit `decimal` und kaufmännischer Cent-Rundung berechnet.

Die Asset-/Inventar-Kennzeichnungen sind ausdrücklich nur Vormerkungen. Ein Asset- oder Lageraggregate wird in Milestone 9 noch nicht vorgetäuscht.

## 3. Kategorien

Eine Bestellposition kann optional eine vorhandene Finanzkategorie referenzieren. Für neue Zuordnungen muss die Kategorie aktiv sein. Eine bereits historisch verwendete, später deaktivierte Kategorie darf beim Bearbeiten einer Bestellung erhalten bleiben.

Projekte und Kostenstellen bleiben davon getrennte Dimensionen. Sie werden nicht in die Kategorienhierarchie gezwängt.

## 4. Dokumentverknüpfung

Archivierte Dokumente werden über den bestehenden `DocumentLinkTargetType.PurchaseOrder` mit Bestellungen verbunden.

Beispiele:

- Angebot
- Bestellbestätigung
- Lieferschein
- E-Mail
- Garantieunterlage

Es entsteht keine zweite Dateikopie; das bestehende inhaltsadressierte Dokumentenarchiv bleibt maßgeblich.

## 5. Bestellung ↔ Rechnung

Milestone 9 führt eine typisierte Beziehung `PurchaseOrderInvoiceLink` ein:

```text
PurchaseOrder 1 --- n PurchaseOrderInvoiceLink n --- 1 Invoice
```

Neue Verknüpfungen werden nur zugelassen, wenn:

- Bestellung und Rechnung denselben Lieferanten besitzen,
- Bestellung und Rechnung dieselbe Währung verwenden,
- die Rechnung nicht storniert ist,
- die Bestellung nicht storniert ist,
- noch keine aktive identische Verknüpfung existiert.

Korrekturen folgen dem bereits in Milestone 8 eingeführten Append/Void-Prinzip. Rechnungslinks werden nicht gelöscht, sondern mit Pflichtgrund storniert.

## 6. Bestellung → Rechnung → Zahlung

Die Bestellung erhält bewusst keine zweite Geldallokation. Der Zahlungsweg ist:

```text
PurchaseOrder
    ↓
PurchaseOrderInvoiceLink
    ↓
Invoice
    ↓
InvoicePaymentAllocation
    ↓
BankTransaction (immutable)
```

Dadurch kann die Beschaffung bis zur tatsächlichen Zahlung nachvollzogen werden, ohne Geldbeträge doppelt zu verbrauchen oder den Kontoauszug zu verändern.

## 7. SQLite-Schutz

Migration 8 ergänzt:

- `purchase_orders`
- `purchase_order_items`
- `purchase_order_invoice_links`

Trigger schützen:

- physisches Löschen von Bestellungen,
- Änderung technischer Identitäten,
- physisches Löschen von Rechnungslinks,
- Änderung verknüpfter Bestellpositionen, solange aktive Rechnungslinks existieren,
- Lieferanten-/Währungswechsel verknüpfter Bestellungen.

Für eine fachliche Korrektur muss zuerst der betroffene Rechnungslink nachvollziehbar storniert werden.

## 8. Benutzeroberfläche

Der bisherige Platzhalter **Bestellungen** ist jetzt eine echte Arbeitsoberfläche mit:

- Suche und Statusfilter
- Bestellung anlegen/bearbeiten
- Bestellpositionen anlegen/bearbeiten/entfernen
- Kategorie und Asset-/Inventar-Kennzeichnung
- Dokumentverknüpfung
- Rechnungsverknüpfung
- Stornierung fehlerhafter Rechnungslinks
- Detailtabs für Positionen, Rechnungen und Dokumente

## 9. Bewusste Abgrenzung

Noch nicht Bestandteil von Milestone 9:

- Wareneingangs-/Lieferschein-Aggregate
- Lagerbestände
- Asset-Management
- automatische Bestellvorschläge
- Freigabe-Workflows mit mehreren Benutzern
- automatische Rechnung-zu-Bestellung-Erkennung
- Berichte und Monatskontrolle

Diese Themen bleiben spätere Ausbaustufen.
