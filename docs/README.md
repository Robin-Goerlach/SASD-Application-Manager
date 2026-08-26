# Dokumentationsindex

> **Status:** Index  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## Lesepfade

### Ich will verstehen, was gebaut wird

1. [PROJECT-BRIEF.md](00-project/PROJECT-BRIEF.md)
2. [LASTENHEFT.md](10-product/LASTENHEFT.md)
3. [UI-UX-CONCEPT.md](10-product/UI-UX-CONCEPT.md)
4. [ROADMAP.md](../ROADMAP.md)

### Ich will implementieren

1. [PFLICHTENHEFT.md](10-product/PFLICHTENHEFT.md)
2. [ARCHITECTURE.md](20-architecture/ARCHITECTURE.md)
3. [DATA-MODEL.md](20-architecture/DATA-MODEL.md)
4. [DEVELOPMENT-PLAN.md](30-development/DEVELOPMENT-PLAN.md)
5. [TEST-STRATEGY.md](30-development/TEST-STRATEGY.md)
6. [AGENTS.md](../AGENTS.md) bei KI-gestützter Entwicklung

### Ich will Release-/Betriebsfähigkeit prüfen

1. [SECURITY-PRIVACY-DATA-LIFECYCLE.md](20-architecture/SECURITY-PRIVACY-DATA-LIFECYCLE.md)
2. [BACKUP-RESTORE-RECOVERY.md](20-architecture/BACKUP-RESTORE-RECOVERY.md)
3. [QUALITY-GATES.md](30-development/QUALITY-GATES.md)
4. [DEPLOYMENT-PLAN.md](40-release-operations/DEPLOYMENT-PLAN.md)
5. [RELEASE-MAINTENANCE.md](40-release-operations/RELEASE-MAINTENANCE.md)

## Dokumentkatalog

| Dokument | Rolle | Status |
|---|---|---|
| `00-project/PROJECT-BRIEF.md` | kompakter Auftrag/Scope | Baseline |
| `00-project/PROJECT-CLASSIFICATION.md` | Größe, Qualitätsstufe, Profile | vorläufig freigegeben |
| `00-project/GLOSSARY.md` | verbindliche Fachbegriffe | Baseline |
| `00-project/RISK-REGISTER.md` | Projektrisiken | laufend |
| `00-project/LICENSE-DECISION.md` | Veröffentlichungs-/Lizenzentscheidung | offen |
| `10-product/REFERENCE-PRODUCTS.md` | recherchierte Inspirationsquellen | abgeschlossen |
| `10-product/LASTENHEFT.md` | fachliches Was/Warum | abgeschlossen |
| `10-product/PFLICHTENHEFT.md` | technische Umsetzungspflichten | abgeschlossen |
| `10-product/UI-UX-CONCEPT.md` | Screen-/Interaktionsmodell | Baseline |
| `10-product/TRACEABILITY-OVERVIEW.md` | Navigation zwischen Anforderungen/Code/Tests | Baseline |
| `20-architecture/ARCHITECTURE.md` | technische Zielarchitektur | abgeschlossen |
| `20-architecture/DATA-MODEL.md` | Persistenz-/Relationenmodell | Baseline |
| `20-architecture/SECURITY-PRIVACY-DATA-LIFECYCLE.md` | Schutz- und Datenlebenszyklus | Baseline |
| `20-architecture/BACKUP-RESTORE-RECOVERY.md` | Wiederherstellbarkeit | Baseline |
| `20-architecture/ADR-REGISTER.md` | Architekturentscheidungen | laufend |
| `30-development/DEVELOPMENT-PLAN.md` | Umsetzungsreihenfolge | Baseline |
| `30-development/TEST-STRATEGY.md` | Testsystem | Baseline |
| `30-development/QUALITY-GATES.md` | Ready/Done/Release-Gates | Baseline |
| `40-release-operations/DEPLOYMENT-PLAN.md` | Packaging/Install/Upgrade | Baseline, Installer offen |
| `40-release-operations/RELEASE-MAINTENANCE.md` | Release und Pflege | Baseline |

## Source-of-Truth-Regel

Ein Sachverhalt soll einen primären Besitzer haben:

- **Lastenheft:** fachliche Erwartung
- **Pflichtenheft:** technische Pflicht
- **Architektur/ADR:** Struktur und langfristige technische Entscheidung
- **Teststrategie/Testcode:** Prüfnachweis
- **Roadmap:** Reihenfolge und Releaseziel
- **Security/Recovery/Deployment:** Betriebs- und Schutzverfahren

Andere Dokumente verlinken statt widersprüchlich zu kopieren.
