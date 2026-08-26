# Teststrategie – SASD Bewerbungsmanager

> **Status:** Verbindliche Testbaseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Ziel und Scope

Die Teststrategie schützt vor den teuersten Fehlerklassen: falsche Fachlogik, Historienverlust, Migration-/Restorefehler, UI-Regressionen, Datenlecks und nicht reproduzierbare Releases.

## 2. Risikoorientierung

| Risiko | Testschwerpunkt |
|---|---|
| Datenverlust | SQLite-Integration, Migration, Backup/Restore |
| falsche Historie | Domain/Application Status-/Activity-Tests |
| falsche Dokumentversion | Document Store Hash-/Roundtriptests |
| UI-Blockierung | Performance + Background-Operation-Tests |
| PII im Log | Security-/Diagnosetests |
| Upgradefehler | N-1→N Migration/Systemtest |
| Importfehler | Negativtests, Preview, Encoding, Pfade |

## 3. Testarten

| Testart | Ziel | Umgebung | Automatisierung |
|---|---|---|---|
| Domain Unit | Invarianten/Value Objects | ohne DB/UI | sehr hoch |
| Application | Commands/Queries/Validierung | Ports/Fakes oder SQLite je Fall | hoch |
| Presenter | UI-Verhalten ohne echten Desktopstart | Fake View/Services | hoch |
| Infrastructure Integration | EF/SQLite/File Store | echte temporäre Dateien/DB | hoch |
| Migration | Schema-/Datenupgrade | reale SQLite-Dateien | hoch |
| Backup/Restore | vollständiger Roundtrip/Failure | temporäre Datengeneration | hoch |
| System/Smoke | Appstart, Navigation, Kernflow | Windows Testhost | mittel |
| UI Automation | kritische Desktopflows | Windows | gezielt |
| Manuell/Explorativ | UX, DPI, Accessibility | reale App | manuell |
| Performance | Referenzlast | definierte Hardwareklasse | automatisiert/benchmarkartig |
| Security | Input/Logs/Dependencies/Offline | CI + manuell | gemischt |

## 4. Domain-Unit-Tests

Mindestens:

- erlaubte/verbotene Statusübergänge;
- Outcome-Regeln;
- NextAction/Wartezustand;
- Commitment-Fälligkeit/Überfälligkeit;
- DateOnly/UTC-Konvertierungsregeln;
- Geldwerte;
- Archivierungsregeln;
- Dokumentversion-Identität.

## 5. Application-Tests

- Validierungsfehler ohne Teilpersistenz;
- Transaktionsgrenzen;
- Timeline bei Statuswechsel;
- Dashboard-Ermittlung;
- Merge-/Delete-Referenzanalyse;
- Commands idempotent, wo zugesichert;
- Fehlerklassifikation/Result-Modell.

## 6. SQLite-Integrationstests

**Kein EF-Core-InMemory als primäre Persistenzsimulation.**

Tests verwenden echte temporäre SQLite-Dateien und prüfen:

- FK-Constraints;
- Unique/Check Constraints;
- Transaktionen/Rollback;
- Migrationen;
- Query-Übersetzung;
- Lock-/busy-Verhalten;
- DateOnly/UTC/GUID-Mapping;
- Index-/Suchverhalten.

## 7. Document-Store-Tests

- gleicher Inhalt → erwartete Dedup-/Hashsemantik;
- Datei wird vollständig kopiert, bevor DB-Verweis finalisiert wird;
- Abbruch während Import hinterlässt keinen gültigen Phantomdatensatz;
- Hashmismatch erkannt;
- fehlende Datei sichtbar;
- sehr langer/ungewöhnlicher Originaldateiname beeinflusst Storage Key nicht;
- untrusted Pfade verlassen Storage Root nicht.

## 8. Backup-/Restore-Tests

Pflicht: kompletter Roundtrip mit synthetischem Bestand. Zusätzlich Fehlerfälle aus dem Recovery-Dokument.

Ein Release ohne mindestens einen automatisierten und einen manuellen Restore-Nachweis ist nicht freigabefähig.

## 9. Presenter-/UI-Tests

Presenter werden ohne WinForms-Message-Loop testbar gehalten. Geprüft werden:

- Loading/Empty/Error/Data states;
- Aktivierung von Buttons;
- Validierungsfeedback;
- Dirty-State-Verhalten;
- Navigationsergebnis;
- Cancellation/Retry bei langen Operationen.

## 10. System-/E2E-Abnahmen

Die 15 `AT-*`-Fälle aus dem Lastenheft bilden die fachliche Releaseabnahme. Automatisierung wird dort eingesetzt, wo sie stabilen Wert liefert; nicht jeder visuelle Check muss automatisiert werden.

## 11. Performance

Referenzbestand mindestens:

- 10.000 Opportunities/Applications kombiniert;
- 50.000 Activities;
- mehrere tausend Contacts/Documents;
- realistische Textmengen in JobPostings/Notes.

Zu messen:

- Startzeit;
- Dashboard;
- typische Listenfilter;
- globale Suche;
- Board;
- Backup;
- Restore;
- FTS-Rebuild.

Performanceziele werden mit Releasehardware dokumentiert, nicht als anonyme Benchmarkzahl.

## 12. Securitytests

- Path Traversal;
- CSV-Formel-/Encoding-Sonderfälle bei Export/Import;
- Archive/Dateinamen;
- kein Start externer Inhalte ohne explizite Aktion;
- Logs auf sensible Inhalte prüfen;
- Dependency Vulnerabilities;
- Offline-Test ohne notwendige Netzverbindung;
- Restore falscher Hash/Passwort/Formatversion.

## 13. Testdaten

Nur synthetisch oder bewusst anonymisiert. Keine realen Bewerbungsunterlagen im Repository/CI.

Testdaten enthalten bewusst:

- Umlaute/Unicode;
- lange Namen;
- gleiche Firmennamen;
- fehlende optionale Felder;
- mehrere Rollen/Firmenkontakte;
- widersprüchliche Aussagen;
- überfällige/erledigte Zusagen;
- mehrere Dokumentversionen;
- Archiv-/Merge-Fälle.

## 14. Flaky-Test-Regel

Ein flaky Test wird nicht kommentarlos retried und vergessen. Er wird entweder stabilisiert, quarantiniert mit Issue/Frist oder als falscher Test entfernt. Kritische Recovery-/Migrationstests dürfen nicht dauerhaft quarantiniert sein.

## 15. Eintritts-/Austrittskriterien RC1

### Eintritt

- M0–M5 Gates grün;
- keine offene schema-destructive Änderung;
- Testdatenstand versioniert;
- Installerkandidat vorhanden.

### Austritt

- alle Stop-Ship-Gates erfüllt;
- AT-001…AT-015 erfolgreich;
- Restore-Nachweis;
- Clean Install/Upgrade/Uninstall;
- Security-/Dependencyreview;
- DPI/Keyboard-Sichtung;
- Known Issues freigegeben.
