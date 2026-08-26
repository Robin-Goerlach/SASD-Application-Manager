# Desktop Deployment Plan

> **Status:** Baseline; Installerentscheidung offen  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Release Target

- **Produkt:** SASD Bewerbungsmanager 1.0
- **OS:** Windows 11 x64 primär
- **Architektur:** x64
- **Runtime:** .NET 10 LTS
- **Publish:** self-contained `win-x64`
- **Releasekanal:** zunächst stabile manuelle Releases; kein Auto-Updater als V1-Pflicht

## 2. Publish Model

Geplant:

```powershell
dotnet publish src/SASD.Bewerbungsmanager.WinForms `
  -c Release `
  -r win-x64 `
  --self-contained true
```

Single-file, trimming und ReadyToRun werden nicht allein aus Paketgrößengründen aktiviert. WinForms/Reflection/EF-Verhalten muss vor jeder Optimierung kompatibel getestet sein.

## 3. Packaging

Ziel:

- per-user Installation;
- möglichst keine Adminrechte;
- Startmenüeintrag;
- optionale Desktopverknüpfung;
- Upgrade-Code/Product Identity stabil;
- Programmdaten strikt getrennt von Nutzerdaten.

Installertechnologie: ADR-001.

## 4. Pfade

Beispiel:

```text
Programm:
%LOCALAPPDATA%\Programs\SASD\Bewerbungsmanager
Daten:
%LOCALAPPDATA%\SASD\Bewerbungsmanager├── data├── documents├── backups├── logs├── diagnostics├── cache└── settings.json
```

Update/Uninstall darf den Datenordner nicht still löschen.

## 5. Signing und Vertrauen

Zielzustand:

- EXE/Installer signieren, sobald organisatorisch praktikabel;
- Timestamping;
- SHA-256 für veröffentlichte Artefakte;
- Version/Commit/Buildherkunft dokumentieren;
- Third-Party Notices beilegen.

Wenn Signierung für 1.0 organisatorisch nicht möglich ist, muss der Release dies transparent dokumentieren und Hashverifikation bereitstellen.

## 6. Update

V1-Update erfolgt über neues Installationspaket.

Beim ersten Start nach Upgrade:

1. App-/Schema-Version erkennen;
2. bei riskanter Migration Pre-Migration-Backup;
3. Migration;
4. Integritäts-/Smokecheck;
5. normaler Start.

Unbekanntes neueres Schema wird nicht mit älterer Anwendung geöffnet.

## 7. Deinstallation

Standardmäßig werden nur Programmdateien entfernt. Bewerbungsdaten bleiben erhalten. Eine vollständige Datenlöschung ist eine separate, explizite Aktion.

## 8. Deployment-Verifikation

Pflicht vor GA:

- Clean Install auf sauberem Windows-Testprofil;
- Start ohne SDK/Entwicklertools;
- Erstinitialisierung Datenpfade;
- Upgrade von letztem unterstützten Releasekandidaten;
- Migration mit Testbestand;
- Uninstall mit Datenretention;
- Reinstall findet Bestand korrekt;
- Pfade mit Unicode-Benutzername;
- Betrieb ohne Internetverbindung.

## 9. Releaseartefakte

Mindestens:

- Installer/Package;
- SHA-256-Datei;
- Release Notes;
- Lizenz/Third-Party Notices;
- optional Symbol-/Diagnosepaket getrennt;
- Test-/Release-Record als Nachweis.
