# ADR-Register

> **Status:** Lebendes Entscheidungsregister  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Bereits verbindliche Architekturentscheidungen

Diese Entscheidungen sind im Architekturdokument bereits als Baseline festgelegt und benötigen nur dann ein separates ADR, wenn Kontext/Alternativen später ausführlicher dokumentiert werden sollen.

| ID | Entscheidung | Status |
|---|---|---|
| AD-001 | Windows Forms auf .NET 10 LTS | Accepted |
| AD-002 | modularer Monolith mit vier Hauptprojekten | Accepted |
| AD-003 | MVP/Presenter für größere UI-Bereiche | Accepted |
| AD-004 | leichtgewichtiges CQRS, keine getrennten Read-/Write-DBs | Accepted |
| AD-005 | kein Event Sourcing, kein allgemeiner Message Bus | Accepted |
| AD-006 | SQLite + EF Core 10 | Accepted |
| AD-007 | kurzer DbContext pro Use Case | Accepted |
| AD-008 | Write Commands im Prozess serialisieren | Accepted |
| AD-009 | GUIDs als stabile Identitäten | Accepted |
| AD-010 | Ereigniszeit UTC, fachliche Tage separat | Accepted |
| AD-011 | content-addressed immutable Document Store | Accepted |
| AD-012 | SQLite-konforme Backupstrategie | Accepted |
| AD-013 | Restore über Staging/Validierung | Accepted |
| AD-014 | Named Mutex + Named Pipe für Single Instance | Accepted |
| AD-015 | kein Netzwerkbedarf im V1-Kern | Accepted |

## 2. Noch zu schließende ADRs

| ADR | Entscheidung | Status | Spätestens |
|---|---|---|---|
| ADR-001 | Installer und Signing | Proposed | vor M5 |
| ADR-002 | SQLite-Pragmas und GUID-Repräsentation | Proposed | M0/M1 |
| ADR-003 | Volltextsuche/FTS5 | Proposed | vor M4 |
| ADR-004 | Backupverschlüsselung | Proposed | vor M5 |
| ADR-005 | Restore-Generation-Switch | Proposed | vor M5 |
| ADR-006 | UI-Automation | Proposed | M3/M4 |
| ADR-007 | CSV-Parser | Proposed | vor Import |
| ADR-008 | File Logging Provider | Proposed | M0 |

## 3. Entscheidungsregel

Ein ADR ist erforderlich, wenn eine Entscheidung:

- langfristig schwer rückgängig zu machen ist;
- Datenformat/Migration beeinflusst;
- Security/Privacy wesentlich verändert;
- neue externe Abhängigkeit einführt;
- Deployment-/Updatekompatibilität betrifft;
- mehrere Schichten/Features dauerhaft prägt.

Routineimplementierungen gehören nicht ins ADR-Register.

## 4. Lebenszyklus

`Proposed → Accepted → Superseded/Deprecated`.

Abgelehnte Optionen bleiben im ADR dokumentiert, damit dieselbe Debatte nicht ohne neue Erkenntnis wiederholt wird.
