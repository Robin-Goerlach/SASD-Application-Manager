# Changelog

## v0.4.0 – Jobsuche und Quellenadapter

- neue `JobLead`-Inbox zwischen Suchquelle und dauerhafter Opportunity
- neue Migration `202608270004_JobSearchAdapters` mit lokaler SQLite-Persistenz
- JSON-Handoff v1 und CSV-Handoff v1 als lokale Quellenadapter
- manuelle Clipboard-Erfassung für Quellen ohne Adapter
- deterministische Deduplizierung über externe ID, kanonisierte URL und SHA-256-Fingerprint
- Suchprofile werden nach erfolgreichem Batchimport als geprüft markiert
- Status `Neu`, `Geprüft`, `Als Stelle übernommen`, `Ignoriert`
- bewusste Übernahme eines JobLeads in Opportunity + SourceLink
- neue WinForms-Navigation `Jobsuche`
- neue Domain-, Application-, Infrastructure-, Presentation- und Systemtests
- Beispiele und Upgrade-/Milestone-Dokumentation ergänzt

## 0.3.0 – Kommunikationsintegration (2026-08-27)

### Added

- Added a local, versioned SASD Mail Workbench JSON handoff contract without adding mailbox protocol dependencies.
- Added `CommunicationMessage` persistence with external-message/fingerprint deduplication and EF migration `202608270003_CommunicationIntegration`.
- Added conservative automatic sender/contact/context matching that only links unambiguous existing relations.
- Added automatic timeline activities for recruiter and application-process e-mails.
- Added deterministic local job-alert classification, URL extraction and title suggestions without external AI services.
- Added clipboard communication import, user-confirmed context linking, ACTION creation and opportunity creation from communication text.
- Added the `Kommunikation` WinForms workspace and supporting dialogs.
- Added domain, application, SQLite, JSON-handoff, system and composition-root regression coverage.
- Added a synthetic Mail Workbench handoff example and troubleshooting guidance for antivirus-blocked test assemblies.

### Changed

- Updated main navigation/title for v0.3.0.
- Extended the pragmatic persistence port for normalized communication records.
- Clarified that runtime EF mappings and frozen migration snapshots intentionally remain separate.

### Database

- Added table `communication_messages`; existing v0.1.x/v0.2.0 data is migrated in place.


## 0.2.0 – Nachweise, Export und Austausch (2026-08-27)

### Added

- Added period-based application evidence built only from actually submitted applications.
- Added explicit editing of factual submission date/channel so evidence can be corrected without inferring dates from workflow status changes.
- Added UTF-8/BOM semicolon CSV export suitable for German spreadsheet workflows.
- Added compact multi-page A4 PDF evidence export without introducing another PDF framework dependency.
- Added a versioned privacy-conscious application exchange dossier.
- Added JSON and Markdown dossier exports that intentionally omit local document paths and file contents.
- Added the `Nachweise / Export` WinForms workspace with evidence preview and export controls.
- Added application, infrastructure and presentation regression tests for evidence semantics, file formats and DI composition.

### Changed

- Updated the main window title and navigation for v0.2.0.
- Added a stable German display label for `ApplicationChannel`.

### Database

- No schema change and no new migration are required for v0.2.0.

## v0.1.0 hotfix - migration snapshot consistency

- Fixed EF Core `PendingModelChangesWarning` caused by reusing runtime model configuration inside the migration snapshot.
- Restored explicit SQLite column types in migration metadata so runtime and snapshot models compare identically.
- Froze the Milestone 1 and Operational MVP target models separately, preventing historical migration metadata from drifting when the current model changes.

## 0.1.0 – Operational MVP (2026-08-26)

### Added

- Added `Activity` timeline entries and planned appointments, including interviews and authority appointments.
- Added explicit `ACTION` / `WAITING_FOR` operational work items with optional due dates.
- Turned the Today page into an operational cockpit for overdue actions, current actions, waiting states, appointments and due search checks.
- Added manual `SearchProfile` routines with browser opening and "Heute geprüft" scheduling.
- Added a document-version catalog with SHA-256 fingerprints.
- Added immutable per-application document snapshots copied below LocalApplicationData after hash re-verification.
- Added deterministic "Kontext für ChatGPT kopieren" clipboard handoff without embedding generative AI.
- Added EF Core migration `202608260002_OperationalMvp`.
- Added/expanded domain, application, SQLite integration, presentation, system and composition-root regression tests for the new operational paths.

### Changed

- Expanded navigation to Today, Tasks, Appointments, Timeline, Search Sources, Applications, Opportunities, Contacts, Organizations and Documents.
- Registered all operational WinForms views explicitly in DI so `ValidateOnBuild` can detect missing constructor dependencies.
- Preserved SQLite `DateTimeOffset` compatibility by materializing before date ordering/filtering where provider translation is not reliable.

## Hotfix 9 - Runtime diagnostics (2026-08-26)

- UI exceptions now persist their full stack trace to `%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\Logs\application.log`.
- The error dialog now shows the base exception type and message so runtime-only failures can be diagnosed without guessing.
- Diagnostic file writing is best-effort and never masks the original application exception.

## Milestone 1 - Hotfix 7 (2026-08-26)

- Fixed EF Core tracking for newly appended `ApplicationStatusHistory` entries by configuring
  their application-generated Guid primary keys with `ValueGeneratedNever()`.
- Added an infrastructure regression test that persists an application, reloads it, changes its
  stage, and verifies that the second history entry is inserted instead of updated.

## Hotfix 5 - 2026-08-26

## Hotfix 6 - 2026-08-26

- Fixed the final system-test cleanup failure on Windows.
- Disabled SQLite connection pooling for the temporary file-based `CoreWorkflowTests` database so disposed DbContexts release the database file before cleanup.
- Kept cleanup strict: file deletion errors are not hidden by retries or swallowed exceptions.

- Replaced a stale `ApplicationPathsTests.cs` left from an earlier repository state with a system test for the current `AppDataPath.GetDefaultDatabasePath()` API.
- Removed xUnit2009 patterns by asserting the expected database path directly.


## 0.0.1-hotfix4 - 2026-08-26

- Fixed `CS8754` in `ApplicationEditForm.ToUtc` by using an explicit `DateTimeOffset` construction before calling `ToUniversalTime()`.
- Removed obsolete High-DPI declarations from `app.manifest` to resolve WinForms analyzer `WFO0003`.
- Added `ApplicationHighDpiMode=PerMonitorV2` to the WinForms project; `ApplicationConfiguration.Initialize()` applies it at startup.
- Performed a complete static scan of WinForms and the downstream presentation/system test sources for the currently visible compiler/analyzer patterns.

## Milestone 1 Hotfix - 2026-08-26

### Fixed

- Updated .NET/EF Core servicing packages to 10.0.11.
- Pinned patched SQLitePCLRaw 2.1.13 transitive dependencies so NuGet audit no longer selects the vulnerable 2.1.11 native SQLite package.
- Pinned `System.Security.Cryptography.Xml` 10.0.11 for the EF design-time/MSBuild dependency graph instead of suppressing `NU1903`.
- Resolved the `Application` namespace/type collision in the application layer with the explicit `JobApplication` alias.
- Replaced the collection-existence assertion in `ArchitectureTests` with `Assert.DoesNotContain`, satisfying xUnit2012.

## Milestone 1 – 2026-08-26

### Added

- Domain model for Organization, Contact, Opportunity, SourceLink and Application.
- Persisted application status history.
- SQLite/EF Core persistence and initial migration.
- Generic Host and dependency injection composition root.
- WinForms navigation, dashboard baseline and CRUD workflows.
- Domain, application, infrastructure, presentation and system tests.
- LocalApplicationData storage policy and repository ignore rules.
## Hotfix 2 – 2026-08-26

- Resolved remaining `Application` namespace/type collisions in the Infrastructure layer and SQLite integration test by using the explicit `JobApplication` alias.
- Renamed the Infrastructure DI registration class and extension method to `InfrastructureServiceCollectionExtensions.AddTrackerInfrastructure` so stale source files from an overlay cannot cause a duplicate `DependencyInjection` type.
- Updated the WinForms startup registration accordingly.

## Milestone 1 - Hotfix 8 (2026-08-26)

- Fixed the startup/dashboard failure caused by server-side SQLite ordering on `DateTimeOffset`.
- Moved ordering for opportunities, source links and applications to in-memory sorting after the
  rows have been materialized, matching the documented EF Core SQLite provider limitation.
- Added regression coverage for all affected list methods and for the dashboard startup path.
