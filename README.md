# SASD Bewerbungsmanager / SASD Application Tracker

Local-first Windows-Desktopanwendung für die persönliche Arbeitssuche.

> **Der nächste Schritt ist wichtiger als der aktuelle Status.**
>
> **Morgen produktiv nutzbar statt in Monaten perfekt.**

## Stand dieser Lieferung

Diese ZIP enthält **Milestone 1 – Kernakte** als vollständige Repository-Baseline. Der Milestone konzentriert sich bewusst auf die fachliche Kernakte und baut noch nicht die Operational-MVP-Funktionen `Activity`, `Task`, `ACTION`, `WAITING_FOR`, Termine, SearchProfiles oder Dokumentversionen.

Enthalten sind:

- C# / .NET 10 / WinForms
- modularer Monolith mit Domain, Application, Infrastructure und WinForms
- SQLite + Entity Framework Core
- EF-Core-Migration `InitialMilestone1`
- kurzlebige DbContexts über `IDbContextFactory`
- Organization: Auflisten, Anlegen und Bearbeiten
- Contact: Auflisten, Anlegen und Bearbeiten
- Opportunity: Auflisten, Anlegen und Bearbeiten
- Rollenbeschreibung als Snapshot
- SourceLink-Erfassung
- Application-Anlage
- Application-Statuswechsel mit persistenter Statushistorie
- Dashboard-Grundgerüst
- Navigation `Heute / Organisationen / Kontakte / Stellen / Bewerbungen`
- Domain-, Application-, Infrastructure-, Presentation- und Systemtests
- synthetische Testdaten

## Voraussetzungen

- Windows 11 x64
- .NET 10 SDK
- Visual Studio 2026 bzw. eine Visual-Studio-Version mit .NET-10-/WinForms-Unterstützung

## Bauen und testen

```powershell
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

## EF-Core-Migrationen weiterentwickeln

Die Infrastructure-Schicht enthält eine Design-Time-Factory. Neue Migrationen können deshalb ohne Start der WinForms-Anwendung erzeugt werden:

```powershell
dotnet ef migrations add <Name> --project .\src\SASD.Bewerbungsmanager.Infrastructure --startup-project .\src\SASD.Bewerbungsmanager.WinForms
```

Die produktive SQLite-Datei wird standardmäßig **nicht im Repository** angelegt, sondern unter dem lokalen Benutzerprofil:

```text
%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\application-tracker.db
```

Damit können personenbezogene Bewerbungsdaten nicht versehentlich über eine normale Git-Operation committed werden.

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

## Nächster Milestone

Nach Stabilisierung dieses Kerns folgt das **Operational MVP / v0.1.0**. Dort soll `Heute` tatsächlich die operative Arbeit steuern: Timeline/Activities, `ACTION`, `WAITING_FOR`, Termine, SearchProfiles, Dokumentversionen und „Kontext für ChatGPT kopieren“.

Details: [`docs/MILESTONE-1.md`](docs/MILESTONE-1.md)

### Milestone-1-Hotfix (26.08.2026)

Der Hotfix aktualisiert die sicherheitsrelevanten NuGet-Abhängigkeiten und behebt den
C#-Namenskonflikt zwischen dem Projekt-Namespace `SASD.Bewerbungsmanager.Application`
und der Domain-Entität `Application`. NuGet-Sicherheitswarnungen werden bewusst nicht
unterdrückt.

## Runtime-Diagnose

Bei unerwarteten UI-/Laufzeitfehlern schreibt die Anwendung den vollständigen Stacktrace lokal nach:

```text
%LOCALAPPDATA%\SASD GmbH\SASD Bewerbungsmanager\Logs\application.log
```

Die Logdatei liegt bewusst außerhalb des Repositorys. Fehlermeldungen zeigen zusätzlich Exception-Typ und technische Kurzmeldung an, damit Laufzeitprobleme reproduzierbar diagnostiziert werden können.
