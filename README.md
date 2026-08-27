# SASD Bewerbungsmanager / SASD Application Tracker

Local-first Windows-Desktopanwendung für die persönliche Arbeitssuche.

> **Der nächste Schritt ist wichtiger als der aktuelle Status.**
>
> **Morgen produktiv nutzbar statt in Monaten perfekt.**

## Aktueller Entwicklungsstand

Diese Lieferung implementiert das **Operational MVP / v0.1.0** auf Basis des zuvor verifizierten
Milestone-1-Kerns.

Der Bewerbungsmanager kann damit neben Organisationen, Kontakten, Stellen und Bewerbungen nun auch
die tägliche operative Arbeit steuern:

- **Heute-Cockpit** mit überfälligen und aktuellen ACTIONs
- **WAITING_FOR** als eigener, sichtbarer nächster Schritt
- **Activity / Timeline** für Kommunikation, Notizen und Verlauf
- **Termine** einschließlich Interviews, Meetings und Behördenterminen
- **SearchProfiles** für regelmäßig manuell geprüfte Jobsuchen
- **Dokumentkatalog** mit SHA-256-Fingerprints
- **unveränderliche Dokument-Snapshots pro Bewerbung**
- **„Kontext für ChatGPT kopieren“** ohne KI-Aufruf innerhalb der Anwendung

Details des Milestones: [`docs/MILESTONE-2-OPERATIONAL-MVP.md`](docs/MILESTONE-2-OPERATIONAL-MVP.md)

## Technische Basis

- C# / .NET 10 LTS
- Windows Forms
- modularer Monolith
- SQLite
- Entity Framework Core 10
- Generic Host
- Dependency Injection
- kurzlebige DbContexts über `IDbContextFactory<ApplicationTrackerDbContext>`
- lokale Datenhaltung ohne Cloudpflicht

## Fachliche Bereiche

```text
Heute
Aufgaben
Termine
Verlauf
Suchquellen
Bewerbungen
Stellen
Kontakte
Organisationen
Dokumente
```

### ACTION und WAITING_FOR

`TrackerTask` trennt bewusst zwei Verantwortlichkeiten:

- `ACTION`: Ich muss selbst etwas tun.
- `WAITING_FOR`: Ich warte auf eine andere Person oder Organisation.

Diese Trennung ist zentral für das Heute-Cockpit.

### Dokumentversionen

Beim Registrieren einer Datei werden Pfad, Größe und SHA-256 erfasst. Erst wenn die konkrete
Version tatsächlich einer Bewerbung zugeordnet wird, prüft die Anwendung den Hash erneut und legt
eine private lokale Kopie ab.

```text
%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\Documents\<ApplicationId>\
```

Damit bleibt nachvollziehbar, welche Datei wirklich verwendet wurde.

### Kontext für ChatGPT

Der Bewerbungsmanager erzeugt einen strukturierten lokalen Text aus Position, Organisation,
Kontakten, Verlauf, offenen Aufgaben, WAITING_FOR, Dokumentversionen und nächstem Termin und kopiert
ihm in die Windows-Zwischenablage. Es findet **kein automatischer KI-Aufruf** statt.

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

Alternativ unter Windows:

```text
build.cmd
test.cmd
```

## Starten

```powershell
dotnet run --project .\src\SASD.Bewerbungsmanager.WinForms\SASD.Bewerbungsmanager.WinForms.csproj
```

Beim Start führt `DatabaseInitializer` ausstehende EF-Core-Migrationen automatisch aus. Die
bestehende Milestone-1-Datenbank muss für v0.1.0 **nicht gelöscht** werden.

## Lokale Daten

Die produktive SQLite-Datei liegt standardmäßig außerhalb des Repositorys:

```text
%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\application-tracker.db
```

Runtime-Diagnosen werden lokal geschrieben nach:

```text
%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\Logs\application.log
```

Persönliche Dokument-Snapshots liegen ebenfalls unter `%LOCALAPPDATA%` und gehören nicht in Git.

## EF-Core-Migrationen weiterentwickeln

Die Infrastructure-Schicht enthält eine Design-Time-Factory:

```powershell
dotnet ef migrations add <Name> `
  --project .\src\SASD.Bewerbungsmanager.Infrastructure `
  --startup-project .\src\SASD.Bewerbungsmanager.WinForms
```

Aktuelle Migrationen:

```text
202608260001_InitialMilestone1
202608260002_OperationalMvp
```

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

Nach einer kurzen realen Nutzung von v0.1.0 folgt gemäß Versionspfad **v0.2.0 – Nachweise, Export
und Austausch**. Der konkrete Umfang bleibt eine Strategieentscheidung und wird nicht in diesen
Milestone vorgezogen.

Historischer Kern: [`docs/MILESTONE-1.md`](docs/MILESTONE-1.md)

## Upgrade aus dem verifizierten Milestone-1-Stand

Bei Repositorys, die während der Hotfix-Runde mehrfach überkopiert wurden, bitte einmal die
Hinweise in [`docs/UPGRADE-v0.1.0.md`](docs/UPGRADE-v0.1.0.md) beachten. Dort ist insbesondere das
optionale Entfernen des alten M0-`MainForm`-/`MainShellPresenter`-Gerüsts beschrieben.
