# Traceability-Übersicht

> **Status:** Navigations- und Nachweiskonzept  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Ziel

Dieses Dokument ersetzt nicht die vollständigen Requirement-Matrizen in Lasten-/Pflichtenheft. Es definiert, **wie eine Anforderung bis zu Code und Test nachverfolgbar bleiben soll**.

## 2. Kette

```text
Fachlicher Bedarf (REQ-*)
        ↓
Technische Pflicht (PFL-*)
        ↓
Architektur / ADR
        ↓
Feature / Command / Query / Presenter
        ↓
automatisierter oder manueller Test
        ↓
Release-/Abnahmenachweis
```

## 3. Beispielketten

### Commitment

- fachlich: Commitment ist von Task getrennt;
- technisch: eigene Persistenz und Überfälligkeitslogik;
- Architektur: Domain/Application getrennt von UI;
- Code: `Commitment`, `CreateCommitmentCommand`, `OverdueCommitmentsQuery`;
- Tests: Domain-Fälligkeit + SQLite-Integration + Dashboard-Presenter;
- Abnahme: AT-003/AT-004.

### Dokumentversion

- fachlich: exakt versandte Version muss später identifizierbar sein;
- technisch: immutable `DocumentVersion` mit Hash;
- Architektur: content-addressed Document Store;
- Code: Import-Service/Port + ApplicationDocument-Verknüpfung;
- Tests: Hash, Duplicate Content, interrupted copy, missing file;
- Abnahme: AT-002.

### Backup/Restore

- fachlich: vollständige Wiederherstellbarkeit;
- technisch: konsistente SQLite-Sicherung + Dokumente + Manifest;
- Architektur: Staging-Restore;
- Tests: Roundtrip, corruption, insufficient disk, interrupted switch;
- Abnahme: AT-011.

## 4. Naming im Code

Issue-/PR-/Committexte sollen relevante IDs nennen, wenn eine Änderung direkt eine spezifizierte Pflicht erfüllt. Nicht jeder kleine Refactor benötigt eine Requirement-ID.

Beispiel:

```text
feat(commitments): add overdue calculation (PFL-DATA-013, REQ-F-096)
```

## 5. Testbenennung

Tests sollen Verhalten statt Dokumentnummern im Methodennamen ausdrücken. Requirement-IDs können als Trait/Kommentar/Testcase-Metadatum ergänzt werden.

## 6. Änderungskontrolle

Wenn Implementierung zeigt, dass eine Pflicht nicht sinnvoll umsetzbar ist:

1. nicht still abweichen;
2. betroffene REQ/PFL identifizieren;
3. fachliche Wirkung bewerten;
4. ADR oder Spezifikationsänderung erstellen;
5. Tests und Roadmap anpassen;
6. erst danach Implementierung als neue Baseline behandeln.
