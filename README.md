# SASD Bewerbungsmanager / SASD Application Tracker

Local-first Windows-Desktopanwendung für die persönliche Arbeitssuche.

> **Der nächste Schritt ist wichtiger als der aktuelle Status.**
>
> **Morgen produktiv nutzbar statt in Monaten perfekt.**

## Aktueller Entwicklungsstand

Diese Lieferung implementiert **v0.2.0 – Nachweise, Export und Austausch** auf Basis des verifizierten
Operational MVP.

Neben der täglichen Steuerung von ACTION, WAITING_FOR, Terminen, Suchquellen und Dokumentversionen
kann der Bewerbungsmanager jetzt:

- Versanddatum und Bewerbungskanal einer vorhandenen Bewerbung gezielt korrigieren,
- einen **Bewerbungsnachweis für einen frei wählbaren Zeitraum** anzeigen,
- tatsächlich versendete Bewerbungen als **CSV** exportieren,
- denselben Nachweis als **PDF** erzeugen,
- CSV und PDF gemeinsam in einen Zielordner schreiben,
- für eine konkrete Bewerbung ein **JSON-Austauschdossier** erzeugen,
- dasselbe Dossier als **Markdown** exportieren,
- lokale Dokumentpfade und Dokumentinhalte bewusst aus Austauschdateien heraushalten.

Details: [`docs/MILESTONE-3-EVIDENCE-EXPORT.md`](docs/MILESTONE-3-EVIDENCE-EXPORT.md)

## Technische Basis

- C# / .NET 10 LTS
- Windows Forms
- modularer Monolith
- SQLite
- Entity Framework Core 10
- Generic Host / Dependency Injection
- kurzlebige DbContexts über `IDbContextFactory<ApplicationTrackerDbContext>`
- lokale Datenhaltung ohne Cloudpflicht
- keine zusätzliche PDF-Bibliothek für den kompakten Nachweis

## Fachliche Bereiche

```text
Heute
Aufgaben
Termine
Verlauf
Suchquellen
Nachweise / Export
Bewerbungen
Stellen
Kontakte
Organisationen
Dokumente
```

### Operational MVP

`TrackerTask` trennt weiterhin bewusst:

- `ACTION`: Ich muss selbst etwas tun.
- `WAITING_FOR`: Ich warte auf eine andere Person oder Organisation.

Timeline, Termine, SearchProfiles, Dokumentversionen und „Kontext für ChatGPT kopieren“ bleiben
Bestandteil des täglichen Workflows.

### Bewerbungsnachweis

Der Nachweis verwendet ausschließlich Bewerbungen mit gesetztem `SubmittedAtUtc`. Damit werden
Entwürfe nicht versehentlich als tatsächlich versendete Bewerbungen ausgegeben.

CSV wird als UTF-8 mit BOM und Semikolon-Trennung erzeugt. Die PDF-Ausgabe ist ein kompakter,
mehrseitiger A4-Nachweis. Beide Formate basieren auf demselben Application-ReadModel.

### Austauschdossier

JSON und Markdown enthalten den gespeicherten Bewerbungszusammenhang einschließlich Stelle,
Organisationen, Quellen, Kontakten, Verlauf, Aufgaben und verwendeten Dokumentmetadaten.

Lokale absolute Dateipfade und Dokumentinhalte werden absichtlich nicht exportiert.

## Voraussetzungen

- Windows 11 x64
- .NET 10 SDK
- Visual Studio mit .NET-10-/WinForms-Unterstützung oder .NET CLI

## Bauen und testen

```powershell
dotnet clean .\SASD.Bewerbungsmanager.sln
dotnet restore .\SASD.Bewerbungsmanager.sln
dotnet build .\SASD.Bewerbungsmanager.sln -c Release --no-restore
dotnet test .\SASD.Bewerbungsmanager.sln -c Release --no-build
```

## Starten

```powershell
dotnet run --project .\src\SASD.Bewerbungsmanager.WinForms\SASD.Bewerbungsmanager.WinForms.csproj
```

## Lokale Daten

Die produktive SQLite-Datei liegt außerhalb des Repositorys:

```text
%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\application-tracker.db
```

Runtime-Diagnosen:

```text
%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\Logs\application.log
```

Private Dokument-Snapshots liegen ebenfalls unter `%LOCALAPPDATA%` und gehören nicht in Git.

## Migrationen

Aktuelle Migrationen:

```text
202608260001_InitialMilestone1
202608260002_OperationalMvp
```

**v0.2.0 benötigt keine neue Migration.** Die Exportfunktionen sind reine Read-/File-Use-Cases auf
Basis des bestehenden Datenmodells.

## Solution-Struktur

```text
src/
  SASD.Bewerbungsmanager.Domain/
  SASD.Bewerbungsmanager.Application/
  SASD.Bewerbungsmanager.Infrastructure/
  SASD.Bewerbungsmanager.WinForms/

tests/
  SASD.Bewerbungsmanager.Domain.Tests/
  SASD.Bewerbungsmanager.Application.Tests/
  SASD.Bewerbungsmanager.Infrastructure.Tests/
  SASD.Bewerbungsmanager.Presentation.Tests/
  SASD.Bewerbungsmanager.SystemTests/
```

## Nächster geplanter Entwicklungsschritt

Gemäß Versionspfad folgt **v0.3.0 – Kommunikationsintegration**. Der konkrete Umfang wird nicht in
v0.2.0 vorgezogen; insbesondere gibt es hier noch keinen automatischen E-Mail-Import.

Historie:

- [`docs/MILESTONE-1.md`](docs/MILESTONE-1.md)
- [`docs/MILESTONE-2-OPERATIONAL-MVP.md`](docs/MILESTONE-2-OPERATIONAL-MVP.md)
- [`docs/MILESTONE-3-EVIDENCE-EXPORT.md`](docs/MILESTONE-3-EVIDENCE-EXPORT.md)
