# Projektstatus – SASD Bewerbungsmanager

> **Status:** Aktueller Status  
> **Dokumentversion:** 0.1  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## Statusübersicht

| Bereich | Status | Bewertung |
|---|---|---|
| Referenz-/Marktanalyse | abgeschlossen | ausreichende Produktbasis |
| Lastenheft V1 | abgeschlossen | fachliche Baseline vorhanden |
| Pflichtenheft WinForms V1 | abgeschlossen | technische Pflichten spezifiziert |
| Zielarchitektur | abgeschlossen | implementierbare Baseline vorhanden |
| Roadmap | abgeschlossen | M0–1.0.0 geplant |
| Projektklassifikation | vorläufig freigegeben | Medium / Recommended |
| Security-/Privacy-Plan | Baseline vorhanden | vor M5 erneut reviewen |
| Teststrategie | Baseline vorhanden | ab M0 mit realen Testprojekten verknüpfen |
| Repository-Scaffold | vorbereitet | Solution, Projekte, CI, Shell und SQLite-Baseline im initialen ZIP |
| Build-/Testnachweis | offen | auf Windows mit .NET 10 auszuführen; Packaging-Umgebung hatte kein SDK |
| Produktcode | M0-Skeleton | nur technische Baseline; fachliche Features beginnen ab M1 |
| Release | nicht begonnen | M5/RC1 |

## Aktuelle Phase

**M0 – Architecture Skeleton / initialer Repository-Scaffold.**

Die reguläre Produktimplementierung soll erst beginnen, wenn die technische M0-Baseline erzeugt ist und die unmittelbar M0-relevanten offenen Entscheidungen dokumentiert sind.

## Nächster Meilenstein

**M0 – Architecture Skeleton**

### Erfolgsnachweis

- Solution ist reproduzierbar buildbar.
- Testprojekte laufen lokal und in CI.
- WinForms-Shell startet.
- SQLite-Migration und Integrationstest funktionieren.
- Logging und globale Fehlergrenze arbeiten.
- Single-Instance-Grundlage ist verifiziert.
- keine unerwartete Netzwerkaktivität.

## Offene Entscheidungen mit unmittelbarer Wirkung

1. SQLite-Detailkonfiguration und GUID-Storage.
2. File-Logging-Provider.
3. Repository-/Lizenzentscheidung vor öffentlicher Veröffentlichung.

Nicht M0-blockierend, aber rechtzeitig vor späteren Meilensteinen:

- Installer/Signing vor M5;
- FTS5/Fulltext vor M4;
- Backupverschlüsselung und Restore-Switch vor M5;
- UI-Automation vor M3/M4;
- CSV-Parser vor Importfunktion.

## Bekannte Scope-Risiken

Der größte Projektrisikohebel ist nicht die Technologie, sondern **Feature Creep**. Der Markt bietet zahlreiche attraktive KI-, Jobportal-, Mail- und Cloudfunktionen. Version 1 bleibt bewusst auf lokale Prozessverwaltung und Datenhoheit begrenzt.

## Readiness-Einschätzung

**Fachlich und architektonisch: GO.**  
**Repository-Baseline: vorbereitet; Build-/Testverifikation und M0-Restpunkte noch offen.**

Die M0-Verifikation kann unmittelbar beginnen. Fachliche Featureentwicklung über technische Spikes hinaus sollte erst nach erfolgreichem M0-Gate starten.
