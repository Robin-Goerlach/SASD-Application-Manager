# Roadmap – SASD Bewerbungsmanager

> **Status:** Planungsbaseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Zweck der Roadmap

Diese Roadmap beschreibt **Ergebnisstände**, keine Kalenderzusagen. Die tatsächliche Dauer hängt von verfügbarer Entwicklungszeit, offenen ADRs, Testaufwand und Erkenntnissen während der Implementierung ab. Scope wird reduziert, bevor Datenintegrität, Wiederherstellbarkeit oder Security-Gates abgeschwächt werden.

## 2. Aktuelles Produktziel

**Version 1.0 soll eine vollständig lokal nutzbare Bewerbungsakte bereitstellen, mit der ein Anwender jederzeit beantworten kann:**

- Welche beruflichen Chancen und Bewerbungen sind aktiv?
- Was ist seit dem ersten Kontakt passiert?
- Welche Person hat wann was gesagt oder zugesagt?
- Welche nächste Aktion ist fällig?
- Welche Gespräche und Dokumentversionen gehören genau zu diesem Vorgang?
- Welche Daten müssen gesichert, exportiert, archiviert oder gelöscht werden?

## 3. Meilensteinübersicht

| Meilenstein | Ergebnis | Aufwandstendenz | Gate |
|---|---|---:|---|
| M0 | Architecture Skeleton | S–M | Build-/Testbaseline |
| M1 | Kernakte | L | Fachkern stabil |
| M2 | Verlauf & Tagessteuerung | L | Workflow nutzbar |
| M3 | Interviews & Dokumente | L | historische Nachvollziehbarkeit |
| M4 | Finden & Verstehen | L | produktive Informationsarbeit |
| M5 | Datenhoheit & Releasehärtung | XL | Release-Gates erfüllt |
| RC1 | Release Candidate 1 | M | vollständige Abnahme |
| 1.0.0 | General Availability | – | freigegebener Release |

Die Aufwandstendenzen sind bewusst relativ. Sie ersetzen keine Schätzung in Stunden oder Wochen.

## 4. M0 – Architecture Skeleton

### Ziel

Ein frischer Checkout lässt sich reproduzierbar bauen und testen. Die Anwendung startet als leere, aber technisch belastbare WinForms-Shell.

### Lieferumfang

- Solution und vier Hauptprojekte anlegen.
- Testprojekte und Architekturtests anlegen.
- .NET-10-SDK über `global.json` oder gleichwertige Baseline fixieren.
- Generic Host, DI, Configuration und Logging einrichten.
- MainForm als Shell mit Navigation und Platzhalteransichten.
- Single-Instance-Grundlage mit Named Mutex; Pipe-Verhalten als Spike.
- SQLite-EF-Core-Baseline mit erster Migration und realem Integrationstest.
- Datenverzeichnisse unter `%LOCALAPPDATA%` erzeugen.
- strukturierte Fehlergrenze und Operation-ID-Grundlage.
- CI-Baseline: restore, build, tests, Analyzer.
- erste ADRs für SQLite-Details und File Logging schließen.

### Nicht enthalten

Keine produktiven CRUD-Workflows außer technisch notwendigen Spikes.

### Done

- Debug- und Release-Build erfolgreich.
- Tests grün.
- Anwendung startet und beendet sauber.
- zwei gleichzeitige Instanzen werden kontrolliert behandelt.
- SQLite-Migration kann auf frischem Profil angewandt werden.
- keine Netzwerkabhängigkeit für Start und Grundbetrieb.

## 5. M1 – Kernakte

### Ziel

Eine reale Bewerbung kann vom Unternehmen bis zum Status nachvollziehbar erfasst werden.

### Lieferumfang

- Company
- Contact
- Opportunity
- JobPosting-Snapshot
- Application
- ApplicationStatus + StatusHistory
- Sources und Kern-Lookups
- Listen-, Detail- und Editieransichten
- Validierung und Dublettenhinweise
- Archivierungsgrundlage
- erste realistische synthetische Testdaten

### Kernabnahmen

- AT-001 Neue Stelle vollständig erfassen.
- AT-008 Mehrere Rollen beim selben Unternehmen.
- Statuswechsel erzeugt Historieneintrag.

## 6. M2 – Verlauf und Tagessteuerung

### Ziel

Die Anwendung wird zum täglichen Arbeitsinstrument statt zur statischen Ablage.

### Lieferumfang

- Activity und Communication
- Timeline
- Task und ChecklistItem
- NextAction einschließlich Wartezustand
- Commitment und CommitmentHistory
- Dashboard „Heute“
- Überfälligkeitslogik
- Prioritäten und Wiedervorlage
- kontextbezogene Schnellaktionen

### Kernabnahmen

- AT-003 Recruiter-Telefonat und Zusage.
- AT-004 Überfällige Zusage.
- AT-005 Aktive Bewerbung ohne nächsten Schritt sichtbar.

## 7. M3 – Interviews und Dokumente

### Ziel

Gesprächsphasen und tatsächlich verwendete Unterlagen bleiben langfristig beweisbar.

### Lieferumfang

- mehrere Interviewrunden
- Teilnehmer und Gesprächsfragen
- Gesprächsvorbereitung und Nachbereitung
- Document und immutable DocumentVersion
- content-addressed File Store
- SHA-256-Integritätsprüfung
- ApplicationDocument-Verknüpfung
- Stellenanzeigen-Snapshots als historische Quelle
- Datei-Importfehler und Recovery-Verhalten

### Kernabnahmen

- AT-002 Bewerbung mit exakt verwendeten Dokumentversionen.
- AT-006 Drei Interviewrunden.
- manipulierte/fehlende Dokumentdatei wird erkannt.

## 8. M4 – Finden und Verstehen

### Ziel

Auch ein größerer Datenbestand bleibt übersichtlich und auswertbar.

### Lieferumfang

- globale strukturierte Suche
- Volltextsuche nach abgeschlossenem ADR
- Filter und Saved Views
- Pipeline-/Boardansicht
- lokale Kalenderansicht
- SourcedStatements mit Herkunft und Zeitpunkt
- Basis-Analytics
- Merge-Workflows für Unternehmen/Kontakte
- Referenz-Performance mit 10.000 Vorgängen / 50.000 Aktivitäten

### Kernabnahmen

- AT-007 widersprüchliche Remote-Aussagen bleiben getrennt.
- AT-009 frühere Recruiter-Beziehung wiederfinden.
- Suchindex ist vollständig rebuildable.

## 9. M5 – Datenhoheit und Releasehärtung

### Ziel

Die Anwendung ist nicht nur funktional, sondern sicher wartbar und wiederherstellbar.

### Lieferumfang

- offener Datenexport mit stabilen IDs
- CSV-Import mit Preview/Validierung
- konsistentes Komplettbackup
- Backup-Manifest und Hashprüfung
- Backupverschlüsselung nach ADR
- Restore über Staging und Validierung
- Recovery für beschädigte Settings, Index und DB-Szenarien
- kontrollierte endgültige Löschung
- Diagnosebericht
- Upgrade-/Migrationspfad
- Deploymentpaket
- Uninstall-Verhalten
- Security-/Dependency-Gates

### Kernabnahmen

- AT-011 Backup und Restore.
- AT-012 Offline-Betrieb.
- AT-013 Datenexport.
- AT-014 Datenintegrität nach Update.
- AT-015 keine unerwartete externe Datenübertragung.

## 10. RC1 – Release Candidate 1

### Ziel

Ein Releasekandidat, der wie ein echter Endanwenderrelease geprüft wird.

### Pflichtprüfungen

- alle 15 End-to-End-Abnahmefälle erfolgreich;
- Clean Install, Upgrade, Smoke Test und Uninstall;
- Restore-Test aus Releasebackup;
- 100/125/150/200-%-DPI-Sichtung;
- Tastatur- und Fokusprüfung zentraler Flows;
- Fehler-/Recovery-Szenarien;
- Dependency- und Lizenzreview;
- Releaseartefakte gehasht, nach Signierungsentscheidung signiert;
- Known Issues geprüft;
- Dokumentation und Release Notes vollständig.

## 11. 1.0.0 – General Availability

1.0.0 wird erst freigegeben, wenn kein Stop-Ship-Defekt offen ist und die Freigabe nachvollziehbar dokumentiert wurde. Ein Release ohne getesteten Restore gilt nicht als GA-fähig.

## 12. Nach 1.0 – mögliche Produktentwicklung

Diese Punkte sind **Ideen, keine Zusagen**.

### 1.1 – Kommunikationsimport

- manuelles `.eml`-/Nachrichten-Importformat
- bessere Zuordnung von E-Mails zu Bewerbungsakten
- bewusst zunächst ohne dauerhafte Mailbox-Berechtigung

### 1.2 – Kalenderintegration

- Export/Import von iCalendar
- später optional Outlook/Google Calendar über explizite Autorisierung

### 1.3 – Jobquellen und Capture

- URL-/Clipboard-Capture
- standardisierte Provider-Schnittstelle
- optionale Browser-Erweiterung
- keine Massenbewerbungen

### 1.4 – Assistive KI

- optional, transparent und abschaltbar
- bevorzugt lokale Verarbeitung, wo sinnvoll
- Zusammenfassung, Extraktionsvorschläge, Vergleich Stelle ↔ Profil
- keine verdeckten Änderungen oder automatisches Bewerben

### 2.x – Synchronisierung / mehrere Geräte

Nur nach eigenständiger Architekturphase. Konfliktsemantik, Identität, Verschlüsselung und Mehrschreiberproblem gelten als neues Risikoprofil und werden nicht vorweggenommen.

## 13. Roadmap-Regeln

- Ein Meilenstein wird erst geschlossen, wenn seine Quality Gates erfüllt sind.
- Erkenntnisse aus Tests dürfen Scope oder Reihenfolge ändern.
- Neue Integrationen werden nicht „nebenbei“ in V1 hineingezogen.
- Eine offene ADR-Entscheidung blockiert nur den Bereich, den sie tatsächlich betrifft.
- Releasefähigkeit wird nicht aus der Anzahl implementierter Features abgeleitet.
