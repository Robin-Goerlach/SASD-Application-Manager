# AGENTS.md – Leitplanken für KI-gestützte Entwicklung

> **Status:** Vorläufige Agentenbaseline  
> **Dokumentversion:** 0.1  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Auftrag

KI-Coding-Agenten dürfen die Entwicklung beschleunigen, sollen aber **keine neue Architektur neben den Spezifikationen erfinden**. Dieses Dokument ist die operative Kurzfassung für Agenten. Bei Konflikten haben Lastenheft, Pflichtenheft, Architekturdokument und akzeptierte ADRs Vorrang.

## 2. Vor jeder größeren Änderung lesen

1. `README.md`
2. `PROJECT-STATUS.md`
3. `ROADMAP.md`
4. relevante Abschnitte in `docs/10-product/PFLICHTENHEFT.md`
5. `docs/20-architecture/ARCHITECTURE.md`
6. `docs/30-development/TEST-STRATEGY.md`

## 3. Nicht verhandelbare Architekturregeln

- WinForms enthält keine Geschäftslogik.
- UI → Application → Domain; Infrastructure implementiert technische Ports.
- Kein Generic Repository pro Entität.
- Kein Event Sourcing und kein allgemeiner Message Bus für V1.
- Kein langlebiger oder threadübergreifend geteilter `DbContext`.
- Commands haben explizite Transaktionsgrenzen.
- SQLite ist fachliche Source of Truth; Suchindex/Cache ist rebuildable.
- Dokumentversionen sind immutable und gehasht.
- Keine versteckten Netzwerkaufrufe oder Telemetrie.
- Persistente/destruktive Änderungen benötigen Test und nachvollziehbare Migration.

## 4. Arbeitsweise

- Kleine, vollständige Schritte bevorzugen.
- Vorhandene Patterns erweitern statt parallele Frameworks einzuführen.
- Tests gleichzeitig mit Verhalten ändern.
- Fehler nicht durch Catch-all/Ignore „lösen“.
- Warnungen nicht global deaktivieren, um Gates grün zu bekommen.
- Keine Secrets, echten Bewerbungsdaten oder privaten Dokumente als Testfixtures verwenden.
- Synthetische Daten nutzen.

## 5. Routinebefehle

Agenten dürfen im Rahmen eines klaren Entwicklungsauftrags routinemäßig lesen, bauen und testen:

```text
git status
git diff
git log
git branch
dotnet --info
dotnet restore
dotnet build
dotnet test
dotnet format --verify-no-changes
dotnet list package --vulnerable --include-transitive
```

Änderungen an Branches, Releases, Tags, Pushes, Paketquellen, Secrets oder lokalen Nutzerdaten müssen zum konkreten Auftrag passen und dürfen nicht als Nebenwirkung erfolgen.

## 6. Abschlussbericht eines Agenten

Jeder größere Arbeitsschritt berichtet mindestens:

- geänderte Dateien;
- implementierte Requirement-/PFL-IDs;
- ausgeführte Tests und Ergebnis;
- offene Risiken/Blocker;
- Migration oder Datenformatänderung;
- Abweichung von Architektur oder ADR-Bedarf.

## 7. Stoppregeln

Agenten sollen nicht eigenmächtig weiterbauen, wenn:

- eine offene ADR-Entscheidung die Implementierung wesentlich bestimmt;
- Datenverlust oder Migration nicht sicher beurteilt werden kann;
- Security-/Lizenzfrage blockiert;
- geforderte Änderung klar außerhalb V1-Scope liegt;
- eine fachliche Invariante widersprüchlich spezifiziert ist.
