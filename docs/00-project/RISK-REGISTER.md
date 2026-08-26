# Risikoregister

> **Status:** Lebendes Dokument  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## Bewertung

- **Wahrscheinlichkeit:** L = niedrig, M = mittel, H = hoch
- **Auswirkung:** L = niedrig, M = mittel, H = hoch
- Risiken mit hoher Auswirkung dürfen nicht allein durch „später beobachten“ geschlossen werden.

## Aktive Risiken

| ID | Risiko | W | A | Gegenmaßnahme | Nachweis |
|---|---|---:|---:|---|---|
| R-001 | Datenverlust durch beschädigte DB/Datenträger | M | H | Backup/Restore, Integrity Checks, Recovery-Runbook | Restore-Test vor Release |
| R-002 | Backup ist syntaktisch vorhanden, aber nicht wiederherstellbar | M | H | Manifest, Hashes, Staging-Restore, regelmäßiger Test | automatisierter + manueller Restore |
| R-003 | Migration beschädigt langfristigen Datenbestand | M | H | versionierte EF-Migrationen, Pre-Migration-Backup, Upgrade-Tests | N-1→N Test |
| R-004 | Dokumentdatei wird geändert/verschoben und Historie verliert Beweiswert | M | H | immutable managed copy, SHA-256, content-addressed store | Integritätstest |
| R-005 | Personenbezogene Daten gelangen in Logs/Diagnose | M | H | Logging-Minimierung, Redaction, Diagnosereview | Security-Test |
| R-006 | Scope Creep verzögert nutzbaren Kern | H | M | V1-Nichtziele, Roadmap-Gates, Change Review | Milestone Review |
| R-007 | WinForms-UI wird überladen/unbedienbar | M | M | Shell + klare Detailansichten, progressive disclosure, UX-Checks | DPI/Keyboard/manual UX |
| R-008 | SQLite-Locks blockieren UI | M | M | kurze DbContexts, serialisierte Writes, Background-Runner | Last-/Failure-Test |
| R-009 | Suchindex driftet von Fachdaten ab | M | M | Index nur derived state, rebuildbar | Rebuild-Test |
| R-010 | KI-Agent führt Architekturdrift ein | M | M | AGENTS.md, Architekturtests, kleine Changes, Traceability | Review + CI |
| R-011 | Abhängigkeit enthält Schwachstelle/Lizenzproblem | M | M | kleine Dependency-Oberfläche, Vulnerability-/License-Review | Release-Gate |
| R-012 | Importdatei enthält schädliche Pfade/Archive | M | H | Input-Härtung, Pfadnormalisierung, keine Ausführung | Security-Negativtests |
| R-013 | Uninstall/Update löscht Nutzerdaten | L | H | strikte Program-/Data-Pfadtrennung, Upgrade-/Uninstall-Test | Release-Gate |
| R-014 | Backup liegt unverschlüsselt auf fremdem Datenträger | M | M | optionale/definierte Backupverschlüsselung vor M5 | ADR + Security-Test |
| R-015 | Fehlende nächste Aktion macht Dashboard unzuverlässig | M | M | explizite NextAction-Invariante/Health-Hinweis | Domain/Application-Test |
| R-016 | Veröffentlichung ohne klare Lizenz | M | M | LICENSE-DECISION vor Public Release | Release-Gate |

## Review-Regeln

- Review am Ende jedes Meilensteins.
- neues H/H- oder H/M-Risiko kann das nächste Gate blockieren;
- geschlossene Risiken bleiben historisch erhalten;
- Risiken aus neuen Integrationen werden vor deren Implementierung ergänzt;
- Security- und Datenverlust-Risiken werden vor RC1 erneut bewertet.

## Top-5 für den Projektstart

1. **R-006 Scope Creep** – schützt den Weg zu einer nutzbaren V1.
2. **R-001/R-002 Wiederherstellbarkeit** – Datenhoheit ohne Restore ist unvollständig.
3. **R-003 Migration** – langfristige Wartung beginnt beim ersten Schema.
4. **R-004 Dokumentintegrität** – Kernmerkmal der historischen Bewerbungsakte.
5. **R-010 Architekturdrift durch Agentenarbeit** – relevant sobald Codex parallel arbeitet.
