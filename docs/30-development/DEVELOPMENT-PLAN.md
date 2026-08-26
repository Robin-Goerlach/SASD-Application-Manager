# Entwicklungsplan – Umsetzung von M0 bis 1.0

> **Status:** Arbeitsbaseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Entwicklungsstrategie

Die Entwicklung erfolgt **vertikal in prüfbaren Meilensteinen**. Infrastruktur wird nur soweit vorgezogen, wie ein kommender Fachworkflow sie tatsächlich benötigt.

## 2. Repository-Zielstruktur

```text
/
├── README.md
├── ROADMAP.md
├── PROJECT-STATUS.md
├── AGENTS.md
├── src/
│   ├── SASD.Bewerbungsmanager.Domain/
│   ├── SASD.Bewerbungsmanager.Application/
│   ├── SASD.Bewerbungsmanager.Infrastructure/
│   └── SASD.Bewerbungsmanager.WinForms/
├── tests/
│   ├── SASD.Bewerbungsmanager.Domain.Tests/
│   ├── SASD.Bewerbungsmanager.Application.Tests/
│   ├── SASD.Bewerbungsmanager.Infrastructure.Tests/
│   ├── SASD.Bewerbungsmanager.Presentation.Tests/
│   └── SASD.Bewerbungsmanager.SystemTests/
├── docs/
├── scripts/
└── artifacts/          # nicht versionierte lokale Build-/Testausgaben
```

## 3. M0 Arbeitspakete

### M0.1 Repository und Toolchain

- `.gitignore`, `.editorconfig`, `Directory.Build.props`;
- `global.json`;
- Solution/Projekte;
- nullable, analyzers, warnings policy;
- README-Buildbefehle verifizieren.

### M0.2 Architekturgrenzen

- Projektverweise;
- Architekturtests gegen verbotene Referenzen;
- Composition Root nur im WinForms-Projekt;
- Domain ohne EF/WinForms-Abhängigkeit.

### M0.3 Host und UI-Shell

- Generic Host;
- DI/Logging/Configuration;
- MainForm, Navigation, Statusleiste;
- Presenter-Pattern mit erster Demo-View.

### M0.4 Persistenzspike

- DbContextFactory/Scope-Strategie;
- erste Migration;
- Foreign Keys;
- SQLite-ADR;
- Integrationstest mit echter temporärer DB.

### M0.5 Operationsbaseline

- AppData-Pfade;
- Single Instance;
- globale Fehlergrenze;
- File Logging ADR/Implementierung;
- Diagnosegrundlage.

## 4. M1 Feature-Slices

Empfohlene Reihenfolge:

1. Company list/detail/create/edit
2. Contact list/detail + Company relation
3. Opportunity + Source
4. JobPosting Snapshot
5. Application create/detail
6. Status transition + History
7. Archive
8. synthetic acceptance dataset

Jeder Slice umfasst Domain/Application/Persistence/Presenter/Tests; keine „erst alle Entities, später UI“-Phase.

## 5. M2 Feature-Slices

1. Activity/Timeline
2. Communication
3. Task + Checklist
4. NextAction
5. Commitment + Overdue logic
6. Dashboard Read Model
7. Dashboard Presenter

## 6. M3 Feature-Slices

1. Interview + participants
2. preparation/questions
3. Document logical model
4. Document Store import
5. DocumentVersion integrity
6. ApplicationDocument exact version
7. error/recovery scenarios

## 7. M4 Feature-Slices

1. structured global search
2. FTS ADR + implementation
3. saved filters/views
4. board pipeline
5. calendar view
6. sourced statements
7. analytics
8. performance tuning from measurements

## 8. M5 Feature-Slices

1. open export
2. CSV import preview/validation
3. backup creation
4. backup encryption ADR
5. restore staging/switch
6. merge/delete workflows
7. migration hardening
8. diagnostic bundle
9. installer/signing
10. RC automation and release evidence

## 9. Issue-Schnitt

Ein gutes Issue enthält:

- Ziel/Nutzen;
- betroffene REQ/PFL;
- In Scope / Out of Scope;
- Akzeptanzkriterien;
- Daten-/Migrationswirkung;
- Testanforderung;
- relevante ADRs.

Zu große Issues werden vertikal geschnitten, nicht nach technischen Schichten.

## 10. Codex-/Agenteneinsatz

Geeignet:

- Scaffolding nach klarer Baseline;
- Tests für definierte Invarianten;
- CRUD-/Presenter-Slices;
- Migrationen nach explizitem Schemaentwurf;
- Dokumentations-/Traceabilitypflege;
- statische Analyse und Refactoring.

Menschliche Entscheidung bleibt erforderlich bei:

- ADRs;
- neuen Dependencies;
- Datenlöschung/Migration;
- Security/Backupverschlüsselung;
- Änderungen an V1-Scope;
- UX-Grundsatzentscheidungen.

## 11. Technische Schulden

Technische Schuld wird nicht als TODO-Kommentar versteckt. Ein akzeptierter Debt-Eintrag enthält:

- Ursache;
- Risiko;
- betroffenen Meilenstein;
- geplante Rückzahlung;
- maximalen Zeitpunkt/Gate.

Debt, der Datenintegrität oder Security gefährdet, kann nicht einfach bis „nach 1.0“ verschoben werden.
