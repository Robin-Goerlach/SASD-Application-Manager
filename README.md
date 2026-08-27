# SASD Bewerbungsmanager / SASD Application Tracker

Local-first Windows-Desktopanwendung für die persönliche Arbeitssuche.

> **Der nächste Schritt ist wichtiger als der aktuelle Status.**
>
> **Morgen produktiv nutzbar statt in Monaten perfekt.**

## Aktueller Entwicklungsstand

Diese Lieferung implementiert **v0.5.0 – Optionale Assistenz** auf Basis des verifizierten v0.4.0-Standes.

Der Bewerbungsmanager kann jetzt zusätzlich:

- für eine Bewerbung oder Stelle eine **Assistenz-Sitzung** vorbereiten,
- sechs versionierbare Aufgabenarten verwenden: Passungsanalyse, nächste Schritte, Recruiter-Antwort, Interviewvorbereitung, Stellenanalyse und Bewerbungscheck,
- den verwendeten fachlichen Kontext per SHA-256 eindeutig referenzieren,
- fremde Stellen-/Kommunikationstexte im Prompt ausdrücklich als **untrusted source material** abgrenzen,
- den vollständigen Prompt vor jeder externen Verwendung prüfen und bewusst in die Windows-Zwischenablage kopieren,
- Antworten aus ChatGPT, einem anderen Cloud-Assistenten oder einem lokalen Modell bewusst zurückkopieren und historisch speichern,
- Assistenz-Sitzungen abschließen oder verwerfen, ohne Modelloutput automatisch auf fachliche Daten anzuwenden.

Es gibt **keine automatische KI-/Cloud-Verbindung**. Die Kernanwendung funktioniert unverändert offline;
v0.5.0 speichert weder API-Schlüssel noch Tokens und führt keine Provider-HTTP-Aufrufe aus.

Details: [`docs/MILESTONE-6-OPTIONAL-ASSISTANCE.md`](docs/MILESTONE-6-OPTIONAL-ASSISTANCE.md)

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
Assistenz
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

### Optionale Assistenz

Die Seite `Assistenz` erzeugt lokale, vollständig prüfbare Prompt-Handoffs. Der fachliche Kontext wird
zwischen klaren Grenzen eingebettet; Stellenanzeigen und Kommunikationsinhalte werden als untrusted
source material markiert. Erst der Benutzer entscheidet, ob der Prompt über die Zwischenablage an
einen externen oder lokalen Assistenten übergeben wird. Zurückkopierte Antworten bleiben reiner Text
und verändern keine Bewerbung, Stelle, Aufgabe oder andere Fachdaten automatisch.

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

Gemäß Versionspfad folgt nach dem verifizierten v0.5.0-Stand **v1.0.0 – gehärtete Produktbaseline**.
Die optionale Assistenz in v0.5.0 bleibt bewusst providerneutral und clipboard-basiert; alle Kernworkflows
funktionieren weiterhin ohne Cloud- oder KI-Abhängigkeit.

Historie:

- [`docs/MILESTONE-1.md`](docs/MILESTONE-1.md)
- [`docs/MILESTONE-2-OPERATIONAL-MVP.md`](docs/MILESTONE-2-OPERATIONAL-MVP.md)
- [`docs/MILESTONE-3-EVIDENCE-EXPORT.md`](docs/MILESTONE-3-EVIDENCE-EXPORT.md)
- [`docs/MILESTONE-4-COMMUNICATION-INTEGRATION.md`](docs/MILESTONE-4-COMMUNICATION-INTEGRATION.md)
- [`docs/MILESTONE-5-JOB-SEARCH-ADAPTERS.md`](docs/MILESTONE-5-JOB-SEARCH-ADAPTERS.md)
- [`docs/MILESTONE-6-OPTIONAL-ASSISTANCE.md`](docs/MILESTONE-6-OPTIONAL-ASSISTANCE.md)
- [`docs/TROUBLESHOOTING.md`](docs/TROUBLESHOOTING.md)
