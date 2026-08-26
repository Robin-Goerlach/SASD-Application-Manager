# Quality Gates und Definition of Ready/Done

> **Status:** Verbindliche Entwicklungsbaseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Definition of Ready – Feature

Ein Feature darf regulär implementiert werden, wenn:

- Ziel/Nutzen verständlich;
- relevante REQ/PFL oder klarer technischer Auftrag vorhanden;
- Scope/Out-of-Scope benannt;
- Akzeptanzkriterien prüfbar;
- Daten-/Migrationswirkung geklärt;
- notwendige ADRs Accepted oder nicht betroffen;
- Security-/Privacywirkung bewertet;
- Testweg bekannt.

## 2. Definition of Done – Feature

- Code kompiliert ohne neue unbegründete Warnungen;
- Tests für neues Verhalten vorhanden und grün;
- Fehlerfälle berücksichtigt;
- UI hat Loading/Empty/Error soweit relevant;
- keine Log-/PII-Regel verletzt;
- Migration/DB-Änderung getestet;
- Dokumentation/Traceability aktualisiert;
- keine unerledigten TODOs, die Akzeptanz vortäuschen;
- lokale Quality Gates erfolgreich.

## 3. M0 Gate

- Restore/build/test von frischem Checkout reproduzierbar;
- Architekturtests grün;
- WinForms-Shell startet;
- erste SQLite-Migration grün;
- Logging/Fehlergrenze;
- Single Instance;
- keine Secrets/echten Nutzerdaten im Repo.

## 4. M1 Gate

- Kernobjekte CRUD/Validierung;
- Statushistory nicht umgehbar;
- Referenzen/Constraints geprüft;
- synthetische Kernabnahmen erfolgreich.

## 5. M2 Gate

- Timeline/NextAction/Commitment logisch konsistent;
- Dashboard zeigt relevante fällige/fehlende Aktionen;
- keine N+1-/offensichtlichen UI-Blocker in Kernqueries.

## 6. M3 Gate

- Document Store Import/Hash/Recovery getestet;
- ApplicationDocument verweist auf exakte Version;
- Interviewrunden vollständig abbildbar;
- fehlende/manipulierte Datei erzeugt kontrollierten Zustand.

## 7. M4 Gate

- Suche/FTS rebuildbar;
- Boardstatus konsistent mit Application-Status;
- gespeicherte Filter migrationsfähig;
- Referenzlast erfüllt akzeptable Bedienbarkeit.

## 8. M5 Gate

- Export, Import, Backup und Restore getestet;
- Migration N-1→N erfolgreich;
- Diagnosepaket auf PII geprüft;
- Installer/Upgrade/Uninstall getestet;
- Security-/Dependency-Gates grün.

## 9. Stop-Ship-Kriterien für 1.0

Ein Release wird blockiert bei:

- möglichem Datenverlust ohne sicheren Workaround;
- Restore nicht erfolgreich reproduzierbar;
- Migration kann produktive Daten beschädigen;
- Dokumentversionen können falsch zugeordnet werden;
- kritischer Securitybefund;
- unerwarteter externer Datenübertragung;
- Installer/Upgrade löscht oder überschreibt Nutzerdaten;
- AT-001…AT-015 mit blockerrelevantem Fehlschlag;
- unbekannter/ungeprüfter Drittanbieter-Lizenzkonflikt.

## 10. Release-Check kompakt

```text
[ ] Release build reproduzierbar
[ ] Tests grün
[ ] Analyzer/Architekturtests grün
[ ] Vulnerability-/Dependencycheck grün
[ ] Migrationstest grün
[ ] Backup/Restore grün
[ ] Clean Install/Upgrade/Uninstall grün
[ ] DPI/Keyboard UX geprüft
[ ] AT-001..AT-015 geprüft
[ ] Known Issues freigegeben
[ ] Release Notes vorhanden
[ ] Hash/Signing erzeugt
```
