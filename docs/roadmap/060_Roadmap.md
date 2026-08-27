# Roadmap – SASD Finance Control

## Phase 0 – Dokumentation und Grundplanung

- Lastenheft
- Pflichtenheft
- Datenmodell
- Architekturübersicht
- Sicherheitskonzept

## Phase 1 – Anwendungsschale

- Solution-Struktur
- Logging
- Konfiguration
- Datenverzeichnis
- Startfenster

## Phase 2 – Lieferantenverwaltung

- Lieferanten anlegen
- bearbeiten
- deaktivieren
- suchen

## Phase 3 – Dokumentenarchiv

- Dokumente importieren
- Hash bilden
- Dokument öffnen
- Dokument verknüpfen

## Phase 4 – Bankkonten und Kontoauszüge

- Bankkonto anlegen
- Kontoauszug per CSV importieren
- Kontoauszug alternativ manuell erfassen
- manuelle Erfassung bis zum Abschluss editierbar halten
- nach Abschluss auch manuell erfasste Daten unveränderlich behandeln
- Transaktionen speichern
- Dubletten erkennen

## Phase 5 – Zahlungszuordnung

- Lieferanten zuordnen
- Kategorien zuordnen
- Klärungsstatus pflegen (ungeklärt / geklärt / nicht relevant)
- ungeklärte Zahlungen anzeigen
- stabile Zuordnungsschicht für spätere Rechnungs-, Vertrags- und Bestellbezüge schaffen

> Konkrete Rechnungs-, Vertrags- und Bestellreferenzen werden erst ergänzt, sobald die jeweiligen Fachobjekte existieren. So entstehen keine schwach typisierten Platzhalter-IDs.

## Phase 6 – Verträge und Abos

- Verträge und Abonnements anlegen und bearbeiten
- SASD-Vertragsnummern und Lieferantenbezug pflegen
- Laufzeit, Status, Kündigungsfrist und automatische Verlängerung verwalten
- erwartete Zahlungen als nicht-buchhalterische Projektion berechnen
- archivierte Vertragsdokumente append-only verknüpfen

> Erwartete Vertragszahlungen sind Prognosen und keine tatsächlichen Geldbewegungen. Der Kontoauszug bleibt der Single Point of Truth.

## Phase 7 – Eingangsrechnungen

- Eingangsrechnungen anlegen und bearbeiten
- SASD-interne sowie Lieferanten-Rechnungsnummern pflegen
- Rechnungsdatum, Fälligkeit und Leistungszeitraum erfassen
- stabile Rechnungspositionen erfassen
- Netto-, Steuer- und Bruttobeträge deterministisch aus Positionen berechnen
- archivierte Rechnungsdokumente verknüpfen
- Positionen bereits technisch für spätere Projekt-/Kostenstellen-Allokationen stabil identifizieren

> Der Zahlungsstatus wird in Phase 7 bewusst nicht manuell gepflegt. Er wird in Phase 8 aus den Beziehungen zu unveränderlichen Banktransaktionen abgeleitet.

## Phase 8 – Zahlungsabgleich und Kostenallokation

**Stand: in Milestone 8 implementiert.**

- Rechnungen mit konkreten Banktransaktionen verknüpfen
- Teilzahlungen und Mehrfachzuordnungen unterstützen
- Verträge und tatsächliche Zahlungen gegenüberstellen
- Rechnungspositionen Projekten und Kostenstellen zuordnen
- offene, teilbezahlte und bezahlte Beträge aus belastbaren Beziehungen ableiten
- Projekte und Kostenstellen als getrennte Stammdimensionen verwalten
- fehlerhafte Zuordnungen nachvollziehbar stornieren statt löschen

## Phase 9 – Bestellungen

**Stand: in Milestone 9 implementiert.**

- Bestellungen mit stabiler SASD-Bestellnummer erfassen
- Bestellpositionen mit stabilen IDs verwalten
- Lieferanten-, Kategorie- und Geschäftszweckbezug herstellen
- Asset-/Inventar-Kandidaten an Positionen markieren
- archivierte Bestelldokumente verknüpfen
- Bestellung und Lieferantenrechnung typisiert verknüpfen
- Bestellung → Rechnung → Zahlung nachvollziehen, ohne Geldbewegungen doppelt zu verbuchen
- Korrekturen von Rechnungslinks nachvollziehbar stornieren statt löschen

## Phase 10 – Berichte und Monatskontrolle

- Fixkosten
- offene Rechnungen
- ungeklärte Zahlungen
- Vertragsfristen
- Kosten nach Lieferant, Kategorie, Projekt und Kostenstelle

## Spätere Ausbaustufen

- Einstellungen mit eigener Benutzeroberfläche
- Asset-Management
- erweiterte Bank-/Rechnungsimporte
- Audit- und Compliance-Ausbau
- ERP-nahe Erweiterungen
