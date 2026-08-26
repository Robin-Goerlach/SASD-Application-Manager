# Security Policy

## Projektstatus

Der SASD Bewerbungsmanager befindet sich vor Version 1.0. Sicherheits- und Datenschutzanforderungen sind trotzdem verbindlicher Teil der Architektur, weil die Anwendung personenbezogene Bewerbungsdaten verarbeitet.

## Sicherheitsmeldungen

Bitte veröffentliche vermutete Schwachstellen **nicht zusammen mit realen Bewerbungsdaten, Lebensläufen, E-Mails oder Zugangsdaten** in einem öffentlichen Issue. Bis ein dedizierter privater Meldekanal im GitHub-Repository konfiguriert ist, sollte für ein öffentliches Repository GitHubs "Private vulnerability reporting" aktiviert werden.

## Besonders schützenswerte Bereiche

- SQLite-Datenbank und Migrationen
- Dokumentstore und Dokumentversionen
- Backup/Restore
- Export und endgültige Löschung
- zukünftige Mail-/Kalender-/Jobportal-Integrationen
- Logs und Diagnosepakete

Siehe `docs/20-architecture/SECURITY-PRIVACY-DATA-LIFECYCLE.md`.
