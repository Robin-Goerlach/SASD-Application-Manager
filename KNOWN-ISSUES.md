# Known Issues und offene Einschränkungen

> **Status:** Arbeitsliste  
> **Dokumentversion:** 0.1  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Zweck

Dieses Dokument enthält **bekannte offene Punkte und bewusst akzeptierte Einschränkungen**, nicht nur Softwarebugs. Vor 1.0.0 muss jeder Eintrag entweder geschlossen, als dokumentierte Einschränkung akzeptiert oder in eine spätere Version verschoben sein.

## 2. Offene Architekturentscheidungen

| ID | Thema | Wirkung | Fällig |
|---|---|---|---|
| KI-001 | Installer und Code Signing | Distribution/Upgrade/Vertrauen | vor M5 |
| KI-002 | SQLite-Pragmas und GUID-Format | Datenintegrität/Kompatibilität | M0/M1 |
| KI-003 | Volltextsuche | Suche/Index-Rebuild | vor M4 |
| KI-004 | Backupverschlüsselung | Vertraulichkeit exportierter Sicherungen | vor M5 |
| KI-005 | Restore-Generation-Switch | Recovery-Sicherheit | vor M5 |
| KI-006 | UI-Automation | Systemtestabdeckung | M3/M4 |
| KI-007 | CSV-Parser | Importrobustheit | vor Import |
| KI-008 | File Logging Provider | Diagnose | M0 |
| KI-009 | Veröffentlichungslizenz | öffentliche Distribution | vor Public Release |

## 3. Bewusste V1-Grenzen

- Windows 11 x64 ist die primär unterstützte Plattform.
- kein Cloud-Sync, keine Mehrbenutzersemantik;
- kein Auto-Updater als V1-Pflicht;
- kein IMAP/OAuth-Mailboxzugriff;
- kein Browser-Autofill/Auto-Apply;
- keine KI-Cloud als notwendige Betriebsabhängigkeit;
- kein serverseitiger Dienst;
- Downgrade auf ältere Binärversion bei neuerem DB-Schema wird nicht automatisch unterstützt.

## 4. Pre-Implementation-Einschränkungen

- Build-/Testbefehle sind Zielbaseline, noch kein ausgeführter Nachweis.
- Performanceziele sind spezifiziert, aber noch nicht gemessen.
- Screenshot und UI-Konzept sind Designrichtung, kein pixelgenauer Abnahmevertrag.
- Packaging-, Signing- und UI-Automationstechnologie sind noch offen.
