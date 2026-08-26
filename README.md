# SASD Bewerbungsmanager

[![CI](https://github.com/Robin-Goerlach/SASD-Bewerbungsmanager/actions/workflows/ci.yml/badge.svg)](https://github.com/Robin-Goerlach/SASD-Bewerbungsmanager/actions/workflows/ci.yml)

> **Status:** M0 Repository Scaffold  
> **Dokumentversion:** 0.1  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## Überblick

Der **SASD Bewerbungsmanager** ist eine lokale Windows-Desktopanwendung zur strukturierten Verwaltung des gesamten persönlichen Bewerbungsprozesses. Das Produkt soll nicht nur Bewerbungen in Pipeline-Spalten verschieben, sondern eine belastbare **Vorgangsakte** bereitstellen: Stelle, Unternehmen, Kontakte, Kommunikation, Aktivitäten, Aufgaben, nächste Aktion, Zusagen Dritter, Interviews, Dokumentversionen, quellenbezogene Aussagen, Ergebnis und Historie bleiben miteinander verbunden.

Die Version 1.0 wird als **Windows-Forms-Anwendung auf .NET 10 LTS** umgesetzt. Der Kern arbeitet vollständig lokal und benötigt keine Cloud- oder Netzwerkverbindung. Fachliche Daten werden in SQLite gespeichert; tatsächlich verwendete Dokumentversionen werden unveränderlich in einem verwalteten Dateispeicher abgelegt.

## GUI-Konzept

![GUI-Konzept des SASD Bewerbungsmanagers](docs/images/dashboard-concept.png)

> **Hinweis:** Der Screenshot ist ein GUI-Konzept für die Zielanwendung und kein Screenshot des aktuellen M0-Codebestands. Die M0-Shell bildet zunächst Navigation, Host und technische Grundstruktur ab; produktive Dashboard- und Bewerbungsfunktionen folgen vertikal ab M1/M2.

## Produktprinzipien

1. **Local first:** persönliche Bewerbungsdaten bleiben standardmäßig auf dem lokalen Rechner.
2. **Vorgangsakte statt Karte:** eine Bewerbung ist ein nachvollziehbarer Prozess mit Geschichte.
3. **Next Action first:** aktive Vorgänge sollen zeigen, was als Nächstes zu tun ist.
4. **Status ist nicht Aktivität:** Status, Aufgabe, Ereignis und Zusage werden getrennt modelliert.
5. **Quelle bleibt sichtbar:** widersprüchliche Informationen dürfen nebeneinander mit Herkunft bestehen.
6. **Mensch behält Kontrolle:** Automatisierung unterstützt, verändert aber V1 nicht heimlich fachliche Daten.
7. **Datenhoheit:** Export, Backup, Restore und kontrollierte Löschung sind Kernfunktionen.

## Aktueller Stand

Das Repository enthält jetzt den **initialen M0-Scaffold**: Solution, vier Hauptprojekte, fünf Testprojekte, Generic Host, WinForms-Shell, Central Package Management, lokale SQLite-/Migrationsbasis, erste Architektur- und Integrationstests sowie GitHub-CI. Fachliche Produktfeatures aus M1 ff. sind absichtlich noch nicht implementiert.

Der nächste technische Schritt ist die **Verifikation und Vervollständigung von M0** auf einer Windows/.NET-10-Entwicklungsmaschine: Build und Tests ausführen, Logging-/SQLite-ADRs schließen, Named-Pipe-Aktivierung ergänzen und das M0-Gate nachweisbar abschließen.

Siehe [PROJECT-STATUS.md](PROJECT-STATUS.md) und [ROADMAP.md](ROADMAP.md).

## Kernumfang Version 1.0

- Unternehmen und Kontakte
- Opportunities und Stellenanzeigen-Snapshots
- Bewerbungsakte und Statushistorie
- Aktivitäten und Kommunikation
- Aufgaben, Checklisten und Next Actions
- Commitments/Zusagen Dritter mit Fälligkeiten
- mehrere Interviewrunden und Teilnehmer
- unveränderliche Dokumentversionen
- quellenbezogene Aussagen
- Dashboard, Pipeline, Suche, Filter, Kalenderansicht und Basis-Analytics
- Archivierung, Merge und kontrollierte Löschung
- offener Datenexport und CSV-Import
- konsistentes Backup, Restore und Recovery
- lokale Diagnose und Logging

## Nicht Bestandteil von Version 1.0

- automatische Massenbewerbungen oder Auto-Apply
- Browser-Erweiterung und Formular-Autofill
- automatische Jobportal-Suche/Scraping
- Mailbox-Synchronisierung per IMAP/OAuth
- Cloud-Kalender-Synchronisierung
- Cloud-Sync oder Mehrbenutzerbetrieb
- generative KI als Pflichtbestandteil
- versteckte Telemetrie oder externe Datenübertragung

## Geplante technische Baseline

| Bereich | Zielbaseline |
|---|---|
| Plattform | Windows 11 x64 |
| UI | Windows Forms |
| Laufzeit | .NET 10 LTS, `net10.0-windows` |
| Deployment | self-contained `win-x64`, per-user |
| Architektur | modularer Monolith, MVP/Presenter |
| Anwendungsschicht | Commands/Queries, explizite Use Cases |
| Persistenz | SQLite + EF Core 10 |
| Dokumente | content-addressed, immutable, SHA-256 |
| Tests | Unit, Application, Presenter, SQLite-Integration, System/UX |
| Betrieb | lokal, Single Instance, keine Serverkomponente |

## Solution-Struktur

```text
SASD.Bewerbungsmanager.sln
├── src/
│   ├── SASD.Bewerbungsmanager.Domain/
│   ├── SASD.Bewerbungsmanager.Application/
│   ├── SASD.Bewerbungsmanager.Infrastructure/
│   └── SASD.Bewerbungsmanager.WinForms/
└── tests/
    ├── SASD.Bewerbungsmanager.Domain.Tests/
    ├── SASD.Bewerbungsmanager.Application.Tests/
    ├── SASD.Bewerbungsmanager.Infrastructure.Tests/
    ├── SASD.Bewerbungsmanager.Presentation.Tests/
    └── SASD.Bewerbungsmanager.SystemTests/
```

## Dokumentation

Der Einstiegspunkt ist [docs/README.md](docs/README.md). Besonders wichtig:

- [Lastenheft](docs/10-product/LASTENHEFT.md)
- [Pflichtenheft](docs/10-product/PFLICHTENHEFT.md)
- [Architekturdokument](docs/20-architecture/ARCHITECTURE.md)
- [Roadmap](ROADMAP.md)
- [Teststrategie](docs/30-development/TEST-STRATEGY.md)
- [Security/Privacy](docs/20-architecture/SECURITY-PRIVACY-DATA-LIFECYCLE.md)
- [Backup/Restore/Recovery](docs/20-architecture/BACKUP-RESTORE-RECOVERY.md)
- [Deployment](docs/40-release-operations/DEPLOYMENT-PLAN.md)

## Voraussetzungen für den M0-Scaffold

- Windows 11 x64 für den realen WinForms-Start;
- .NET SDK `10.0.400`;
- optional Visual Studio 2026 18.9 oder neuer;
- Git für Versionsverwaltung.

## Build, Test und Start

Die folgenden Befehle sind für den enthaltenen M0-Scaffold vorgesehen. Das ZIP wurde strukturell geprüft; in der Erstellungsumgebung war kein .NET-SDK installiert, daher ist ein erfolgreicher Build erst nach Ausführung auf einer .NET-10-Entwicklungsmaschine nachgewiesen.

```powershell
dotnet restore SASD.Bewerbungsmanager.sln
dotnet build SASD.Bewerbungsmanager.sln -c Debug --no-restore
dotnet test --solution SASD.Bewerbungsmanager.sln -c Debug --no-build
dotnet run --project src/SASD.Bewerbungsmanager.WinForms

# oder vollständige lokale Prüfung
.\scripts\verify.ps1
```

Weitere Details: [docs/30-development/DEVELOPMENT-PLAN.md](docs/30-development/DEVELOPMENT-PLAN.md).

## Mitarbeit und Änderungen

Das Projekt ist zunächst für einen kleinen Maintainer-Kreis ausgelegt. Nichttriviale Änderungen sollen an Anforderungen, Architektur und Tests zurückgeführt werden. Siehe [CONTRIBUTING.md](CONTRIBUTING.md) und [AGENTS.md](AGENTS.md).

## Lizenz

Die Veröffentlichungslizenz ist **noch nicht entschieden**. Bis zur dokumentierten Entscheidung gilt dieses Paket nicht als Freigabe zur öffentlichen Distribution. Siehe [docs/00-project/LICENSE-DECISION.md](docs/00-project/LICENSE-DECISION.md).

## SASD-Bezug

Das Dokumentationspaket orientiert sich an der am 24.08.2026 verfügbaren Approved-Baseline des SASD Development Standard (Version-1.0-Specification-Candidate). Die Projektdokumentation nutzt progressive disclosure: zentrale Entscheidungen sind auffindbar, ohne jeden Sachverhalt in mehreren Dokumenten zu duplizieren.
