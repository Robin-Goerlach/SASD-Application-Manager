# Changelog

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
