# Projektklassifikation – SASD Bewerbungsmanager

> **Status:** Vorläufig freigegeben  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## Identität

- **Projekt:** SASD Bewerbungsmanager
- **Anlass:** Initialklassifikation vor Implementierung
- **Lebenszyklus:** langfristig gepflegtes Produkt
- **Verteilung:** zunächst lokaler Windows-Desktop, Einzelbenutzer

## Strukturelle Größe

**Einstufung: Medium.**

Begründung:

- mehrere fachliche Aggregate und Beziehungen;
- persistentes relationales Datenmodell;
- unveränderlicher Dateispeicher zusätzlich zur Datenbank;
- Migration, Import/Export und Backup/Restore;
- anspruchsvolle WinForms-Navigation, Suche, Board und Timeline;
- keine verteilten Serverkomponenten und keine Cloudabhängigkeit in V1.

## Risikomerkmale

| Merkmal | Einstufung | Begründung |
|---|---|---|
| Vertraulichkeit | mittel | Bewerbungs-, Kontakt-, Gehalts- und Kommunikationsdaten |
| Integrität | hoch | falsche Historie/Dokumentversionen können reale Entscheidungen beeinträchtigen |
| Verfügbarkeit | mittel | lokales Arbeitswerkzeug, kein 24/7-Dienst |
| Wiederherstellbarkeit | hoch | persönlicher Langzeitdatenbestand darf nicht verloren gehen |
| Externe Erreichbarkeit | niedrig | V1 ohne eingehende/externe Dienste |
| personenbezogene Daten | hoch | Personen-, Kontakt- und Kommunikationsdaten sind Kernbestand |
| privilegierte Zugriffe | niedrig | keine Adminrechte im Normalbetrieb vorgesehen |
| Recht/Lizenz | mittel | Veröffentlichungslizenz und Drittanbieterpakete müssen geprüft werden |
| externe Dienste | niedrig V1 | keine notwendige Cloudintegration |

## Qualitätsstufe

**Recommended**.

Begründung: Das Projekt ist langfristig, datenhaltend und architektonisch nicht trivial, aber kein hochverfügbares Enterprise-/Safety-Critical-System.

### Freiwillig strengere Maßnahmen

Production-nahe Tiefe wird gezielt angewendet bei:

- Backup/Restore und Recovery;
- Migrationen und Datenintegrität;
- Security/Privacy;
- Release-/Upgradeprüfung;
- nachvollziehbaren Quality Gates.

## Anwendbare Profile

- Core: **ja**
- C#/.NET: **ja**
- Desktop/WinForms: **ja**
- Web/Cloud: **nein in V1**
- verteilte Systeme: **nein in V1**

## Artefakttiefe

Separat geführt werden Lastenheft, Pflichtenheft, Architektur, Roadmap, Teststrategie, Security/Privacy, Recovery, Deployment und Risikoübersicht. Kleinere Entscheidungen werden zusammengeführt, solange Nachvollziehbarkeit erhalten bleibt.

## Review

Vorläufig freigegeben für M0. Neubewertung vor RC1 sowie bei Einführung von Cloud-Sync, externer Mailboxintegration oder Mehrbenutzerbetrieb.
