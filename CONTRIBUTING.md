# Contributing – Änderungs- und Reviewprozess

> **Status:** Arbeitsbaseline  
> **Dokumentversion:** 0.1  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Grundsatz

Änderungen sollen klein, nachvollziehbar und testbar bleiben. Auch bei Einzelentwicklung ersetzt Geschwindigkeit nicht die Pflicht, Anforderungen, Architektur und Datenrisiken konsistent zu halten.

## 2. Branching

- `main` soll jederzeit buildbar und nachvollziehbar bleiben.
- Dokumentationskorrekturen und sehr kleine risikoarme Änderungen dürfen nach lokalen Gates direkt auf `main` erfolgen.
- Nichttriviale Features, Migrationen, Security-/Backup-Änderungen und umfangreiche Agentenarbeit sollen in einem Feature-/Arbeitsbranch erfolgen.
- Branch-Namen: `feature/...`, `fix/...`, `chore/...`, `docs/...`, `codex/...`.

## 3. Commitregeln

Bevorzugt kurze imperative/konventionelle Betreffe:

```text
feat(applications): add status transition history
fix(backup): reject mismatched manifest hash
test(documents): cover interrupted import recovery
docs(roadmap): refine M4 exit criteria
```

Ein Commit soll eine verständliche Änderungseinheit bilden. Generated Files und lokale Benutzerdaten gehören nicht in Git.

## 4. Mindestprüfung vor Merge/Push

Ab M0:

```powershell
dotnet restore SASD.Bewerbungsmanager.sln
dotnet build SASD.Bewerbungsmanager.sln -c Release --no-restore
dotnet test --solution SASD.Bewerbungsmanager.sln -c Release --no-build
```

Zusätzlich bei relevanten Änderungen:

- Migrationstest bei Schemaänderung;
- Restoretest bei Backup-/Datenpfadänderung;
- Architekturtests bei Projekt-/Abhängigkeitsänderung;
- manuelle UI-Prüfung bei WinForms-Layout-/Navigationseingriff;
- Dependency-/Securityprüfung bei Paketänderung.

## 5. Dokumentationspflicht

Ein Codechange aktualisiert das Dokument, das seine langfristige Wahrheit besitzt. Keine Änderung wird nur deshalb in fünf Dokumenten dupliziert.

- Produktumfang → Lastenheft/Roadmap
- technische Pflicht → Pflichtenheft
- Struktur/Prinzip → Architektur/ADR
- Testweg → Teststrategie/Testcode
- Betrieb/Release → Deployment-/Maintenance-Dokument

## 6. Definition of Done

Es gilt [docs/30-development/QUALITY-GATES.md](docs/30-development/QUALITY-GATES.md). Ein Feature ist nicht „fertig“, wenn nur der Happy Path kompiliert.
