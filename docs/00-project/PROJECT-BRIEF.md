# Projektbrief – SASD Bewerbungsmanager

> **Status:** Baseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## Problem

Persönliche Bewerbungsprozesse verteilen sich typischerweise auf Stellenportale, E-Mails, Kalender, Dateien, Notizen und Erinnerungen. Dadurch gehen Zusammenhänge verloren: Welche CV-Version wurde versandt? Wer hat bis wann eine Rückmeldung zugesagt? Welche Aussage zur Remote-Regelung stammt aus Stellenanzeige, Recruiting oder Fachgespräch? Was ist heute der nächste sinnvolle Schritt?

Klassische Kanban-Tracker lösen nur einen Teil des Problems, weil sie den Vorgang auf Statuskarten reduzieren.

## Ziel

Eine lokale Windows-Anwendung soll jede Bewerbung als **nachvollziehbare Vorgangsakte** verwalten und den Benutzer durch Next Actions, Fälligkeiten, Timeline und strukturierte Beziehungen im Alltag unterstützen.

## Zielgruppe

Primär einzelne Bewerber, die mehrere parallele Bewerbungsprozesse professionell dokumentieren wollen und Wert auf lokale Datenhaltung, Historie und Wiederherstellbarkeit legen.

## Erwarteter Nutzen

- weniger vergessene Follow-ups;
- reproduzierbare Gesprächsvorbereitung;
- nachvollziehbare Kontakt- und Unternehmenshistorie;
- exakte Zuordnung versandter Dokumentversionen;
- schnelle Wiederaufnahme alter Kontakte;
- weniger Abhängigkeit von SaaS-Trackern und Tabellen;
- kontrollierter Export, Backup und Restore.

## Scope Version 1.0

Enthalten sind Kernakte, Kontakte, Stellen, Timeline, Kommunikation, Tasks, Next Actions, Commitments, Interviews, Dokumentversionen, Suche/Filter, Board, Kalenderansicht, Basis-Analytics, Import/Export, Backup/Restore, Archivierung und Diagnose.

## Nicht-Ziele Version 1.0

- Auto-Apply und Massenbewerbungen;
- dauerhafter Zugriff auf externe Mailboxen;
- Cloud-Synchronisierung;
- Browser-Autofill;
- vollautomatische Jobportalsuche;
- KI als notwendige Systemkomponente;
- Mehrbenutzer-/Serverbetrieb.

## Randbedingungen

- Windows Forms auf .NET 10 LTS;
- Windows 11 x64 als primäre Plattform;
- lokale SQLite-Persistenz;
- keine versteckten Netzwerkaufrufe;
- personenbezogene Daten werden lokal und minimiert verarbeitet;
- Daten müssen exportier- und wiederherstellbar sein.

## Wichtigste Risiken

1. Scope Creep durch attraktive Integrations-/KI-Ideen.
2. Datenverlust oder falsches Vertrauen in Backups.
3. Historienverlust durch mutierbare Dokumente oder stille Statusänderungen.
4. UI-Komplexität durch zu viele Fachobjekte.
5. Migration-/Upgradefehler in einem langfristig gepflegten Datenbestand.

## Erfolgskriterien V1.0

- alle 15 fachlichen End-to-End-Abnahmen des Lastenhefts bestehen;
- ein Benutzer kann einen vollständigen Bewerbungsprozess ohne externe Dienste führen;
- aktive Vorgänge ohne nächste Aktion sind erkennbar;
- verwendete Dokumentversionen bleiben unveränderlich nachweisbar;
- Backup und Restore sind praktisch getestet;
- keine unbeabsichtigte Datenübertragung;
- Clean Install, Upgrade und Uninstall sind reproduzierbar geprüft.

## Lebenszyklus

Langfristig gepflegtes Desktopprodukt. Erweiterungen nach V1 werden nur ergänzt, wenn der lokale Kern stabil bleibt.
