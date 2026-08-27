# SASD Bewerbungsmanager / SASD Application Tracker

Local-first Windows-Desktopanwendung für die persönliche Arbeitssuche.

> **Der nächste Schritt ist wichtiger als der aktuelle Status.**
>
> **Morgen produktiv nutzbar statt in Monaten perfekt.**

## Aktueller Entwicklungsstand

Diese Lieferung implementiert **v0.4.0 – Jobsuche und Quellenadapter** auf Basis des v0.3.0-Standes.

Der Bewerbungsmanager kann jetzt zusätzlich:

- gefundene Stellen zunächst als eigene **JobLead-Inbox** verwalten, bevor sie Opportunities werden,
- versionierte JSON-Jobquellen-Handoffs importieren,
- semikolongetrennte UTF-8-CSV-Handoffs einschließlich gequoteter mehrzeiliger Beschreibungen importieren,
- einzelne Treffer aus der Windows-Zwischenablage erfassen,
- importierte Treffer über externe Stellen-ID, kanonisierte URL und SHA-256-Fingerprint deduplizieren,
- Suchprofile nach erfolgreichem Batchimport automatisch als geprüft markieren,
- Treffer als geprüft oder ignoriert kennzeichnen,
- Quell-URLs direkt im Browser öffnen,
- einen geprüften Treffer bewusst als Opportunity übernehmen und dabei den Beschreibungstext als Snapshot erhalten.

Der Bewerbungsmanager **scrapt keine Portale selbst**. JSON, CSV und Clipboard bilden eine kleine lokale
Adaptergrenze, an die spätere portal- oder browsernahe Werkzeuge anschließen können.

Details: [`docs/MILESTONE-5-JOB-SEARCH-ADAPTERS.md`](docs/MILESTONE-5-JOB-SEARCH-ADAPTERS.md)

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
Kommunikation
Jobsuche
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

### Kommunikationsintegration

Die Seite `Kommunikation` importiert lokale Mail-Workbench-Handoffs oder Clipboard-Text. Direkte
Recruiter-/Bewerbungsnachrichten können automatisch als Timeline-Aktivität erscheinen. Job-Alerts
werden ohne externe Dienste auf HTTP/HTTPS-Links und einen Titelvorschlag untersucht.

Das Handoff-Format ist versioniert (`schemaVersion = 1`); ein synthetisches Beispiel liegt unter
[`docs/examples/mail-workbench-handoff-v1.json`](docs/examples/mail-workbench-handoff-v1.json).

### Jobsuche und Quellenadapter

Die neue Seite `Jobsuche` verwaltet normalisierte Suchtreffer getrennt von dauerhaften Opportunities.
Unterstützt werden JSON-Handoff v1, CSV-Handoff v1 und manuelle Clipboard-Erfassung. Duplikate werden
deterministisch erkannt; erst die bewusste Aktion **Als Stelle übernehmen** erzeugt eine Opportunity.

Synthetische Beispiele liegen unter `docs/examples/job-source-handoff-v1.json` und `.csv`.

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
202608270003_CommunicationIntegration
202608270004_JobSearchAdapters
```

Beim Start wird die neue JobLead-Tabelle automatisch über EF Core migriert. Bestehende Daten bleiben erhalten.

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

Gemäß Versionspfad folgt nach dem verifizierten v0.4.0-Stand **v0.5.0 – optionale Assistenz/KI**.
Diese Ausbaustufe bleibt optional; die Kernanwendung und alle v0.4.0-Workflows funktionieren weiterhin
ohne Cloud- oder KI-Abhängigkeit.

Historie:

- [`docs/MILESTONE-1.md`](docs/MILESTONE-1.md)
- [`docs/MILESTONE-2-OPERATIONAL-MVP.md`](docs/MILESTONE-2-OPERATIONAL-MVP.md)
- [`docs/MILESTONE-3-EVIDENCE-EXPORT.md`](docs/MILESTONE-3-EVIDENCE-EXPORT.md)
- [`docs/MILESTONE-4-COMMUNICATION-INTEGRATION.md`](docs/MILESTONE-4-COMMUNICATION-INTEGRATION.md)
- [`docs/MILESTONE-5-JOB-SEARCH-ADAPTERS.md`](docs/MILESTONE-5-JOB-SEARCH-ADAPTERS.md)
- [`docs/TROUBLESHOOTING.md`](docs/TROUBLESHOOTING.md)
