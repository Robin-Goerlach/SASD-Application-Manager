# Referenzprogramme und Funktionskatalog für den SASD Bewerbungsmanager

**Dokumenttyp:** Markt- und Referenzanalyse / Funktionskatalog  
**Projekt:** SASD Bewerbungsmanager  
**Stand der Recherche:** 24. August 2026  
**Sprache:** Deutsch  
**Status:** Arbeitsdokument für Strategie, Projektbrief, Domänenmodell und spätere Anforderungsanalyse  

---

## 1. Zweck und Abgrenzung

Dieses Dokument hält fest, welche Programme und Dienste bei der bisherigen Vorlagenanalyse für den geplanten **SASD Bewerbungsmanager** berücksichtigt wurden und welchen öffentlich dokumentierten Funktionsumfang diese Produkte besitzen.

Ziel ist ausdrücklich **nicht**, eines der Produkte nachzubauen. Die Referenzen sollen helfen,

- etablierte Funktionsmuster zu erkennen,
- gute Bedien- und Datenmodellideen zu identifizieren,
- wiederkehrende Anforderungen von echten Bewerbungsprozessen zu verstehen,
- interessante Alleinstellungsmerkmale einzelner Produkte zu erfassen,
- typische Schwächen und Lücken bestehender Lösungen sichtbar zu machen und
- daraus später eigene, nachvollziehbar begründete Anforderungen für den SASD Bewerbungsmanager abzuleiten.

Die Recherche folgt dem Gedanken des SASD Development Standard, zunächst belastbare Ausgangsinformationen, Scope und Evidenz zu sammeln und erst danach tiefer in Anforderungen und Architektur einzusteigen. Der Standard selbst verwendet „progressive disclosure“ und empfiehlt für neue Projekte einen kurzen Projektbrief sowie eine proportional zum Risiko vertiefte Spezifikation.

### 1.1 Bedeutung von „alle Funktionen“ in diesem Dokument

Bei kleinen Open-Source-Projekten lässt sich der Funktionsumfang anhand des Repositorys und der Dokumentation weitgehend vollständig aufzählen. Bei großen SaaS-Produkten wie Pipedrive, Greenhouse, Lever, Todoist oder Workable gibt es dagegen hunderte Detailoptionen, Tarifunterschiede, Integrationen, Administrationsschalter und laufende Produktänderungen.

„Alle Funktionen“ bedeutet deshalb hier:

1. **Alle öffentlich dokumentierten fachlichen Funktionsbereiche**, die am Stichtag verifizierbar waren.
2. **Wesentliche Unterfunktionen**, soweit sie das Verhalten des Produkts beschreiben und für einen Produktvergleich relevant sind.
3. **Tarifabhängige Funktionen** werden als solche gekennzeichnet, sofern dies aus der Dokumentation hervorgeht.
4. **Beta-, Preview- oder Roadmap-Funktionen** werden nicht mit produktiv verfügbaren Funktionen vermischt.
5. Technische Implementierungsdetails werden nur dann aufgenommen, wenn sie selbst eine Produktfunktion ermöglichen, etwa Local-first, Self-hosting, E-Mail-Push, Offline-Betrieb oder lokale KI.
6. Reine Marketingaussagen ohne erkennbare Produktfunktion werden nicht als Funktion gezählt.

Damit ist das Dokument als **möglichst vollständiger Funktionskatalog auf Produktebene**, nicht als Ersatz für jedes einzelne Handbuch des jeweiligen Herstellers zu verstehen.

---

## 2. Berücksichtigte Programme

| Nr. | Programm | Kategorie | Hauptgrund für die Berücksichtigung |
|---:|---|---|---|
| 1 | Huntr | Bewerbungsmanager / Job Tracker | Bewerbungsakte, Pipeline, Aktivitäten, Kontakte, Dokumente |
| 2 | Teal Job Tracker | Bewerbungsmanager / Career Suite | Job-Tracking, Checklisten, Kontakte, Resume-Bezug, Interview-Unterstützung |
| 3 | Careerflow | Bewerbungsmanager / Career Suite | Dashboard, Tasks, Networking-Kontakte, Reminder, Matching |
| 4 | Jobscan Job Tracker | Bewerbungsmanager / Resume Suite | Job-Tracking plus Resume-Matching und Interview-/Kontaktinformationen |
| 5 | Simplify | Job Search / Autofill / Tracker | Browser-Integration, Autofill, automatisches Tracking, Job Discovery |
| 6 | JSE | Open Source, Local-first Desktop | Lokale Datenhoheit, Suche, Matching, Wissensbasis, Funnel-Lernen |
| 7 | JobSync | Open Source, Self-hosted | Bewerbungen, Aufgaben, Resume-Management, Job Discovery, MCP |
| 8 | JobOps | Open Source, Self-hosted | Multi-Source-Suche, Scoring, CV-Tailoring, E-Mail-Tracking |
| 9 | JobTrackerPro | Open Source | Automatische E-Mail-Erkennung und Statuspflege |
| 10 | JobTrail | Open Source, Self-hosted | Discovery, Mehrfachinterviews, Timeline, Skill Extraction, Company Enrichment |
| 11 | JobHunt | Open Source | Kompakter klassischer Kanban-Tracker als MVP-Referenz |
| 12 | Jobtra | Open Source, Self-hosted | Bewerbungen, Dokumente, IMAP-E-Mails, lokale/optionale KI |
| 13 | JobNest | Open Source, Local-first Desktop | Privacy-first Desktop UX und einfaches lokales Tracking |
| 14 | Pipedrive | CRM | Next Action, Aktivitäten, Kontakte, Pipeline, Kommunikation, Automationen |
| 15 | Dex | Personal CRM | Ansprechpartner, Beziehungshistorie, Reminder und Kontaktgedächtnis |
| 16 | Monica | Open Source Personal CRM | Kontaktmodell, Beziehungen, Aktivitäten, Erinnerungen, Datenhoheit |
| 17 | Todoist | Task Manager | Aufgabenmodell, Fälligkeiten, Prioritäten, Filter, schnelle Erfassung |
| 18 | Trello | Kanban / Work Management | Board-/Card-UX, Checklisten, Ansichten, Regeln und Automationen |
| 19 | Greenhouse | ATS / Recruiting | Bewerbungsprozess aus Unternehmenssicht, strukturierte Interviews, Analytics |
| 20 | Lever | ATS + Recruiting CRM | ATS/CRM-Verknüpfung, Candidate Journey, Automationen, Nurturing |
| 21 | Workable | ATS / Recruiting | Candidate Profile, Kommunikation, Interviews, Scorecards, Offers |
| 22 | Bundesagentur für Arbeit – Jobsuche | Jobportal | Deutsche Jobsuche, gespeicherte Suchen, Vormerkungen, Vermittlungsfunktionen |
| 23 | meinestadt.de Jobs | Jobportal | Deutsche Jobquelle, Suche, Merkliste, Suchabos und Bewerbungswege |

---

# 3. Direkte Bewerbungsmanager und Career-Suiten

## 3.1 Huntr

**Kategorie:** Job Application Tracker, Career Management und Resume Tools  
**Bereitstellung:** Webanwendung plus Browser-Erweiterung  
**Relevanz für SASD:** Sehr hoch; eine der vollständigsten direkten Referenzen für die Bewerbungsakte.

### 3.1.1 Job Board und Pipeline

Huntr stellt Bewerbungen auf einem visuellen Board dar. Die Karten lassen sich zwischen Status- beziehungsweise Pipeline-Spalten verschieben. Dadurch wird der Fortschritt einer Stelle visuell erkennbar, ohne dass die vollständige Akte geöffnet werden muss.

Zu den Funktionen gehören:

- Anlegen und Verwalten von Jobs/Bewerbungen als Karten.
- Verschieben von Karten zwischen Bewerbungsphasen per Drag-and-drop.
- Übersicht über aktive und weiter fortgeschrittene Bewerbungen.
- Manuelles Speichern einer Stelle.
- Speichern einer Stelle über die Huntr-Browser-Erweiterung.
- Übernahme typischer Daten aus der Stellenanzeige, insbesondere Titel, Unternehmen, Beschreibung und – sofern erkennbar – Vergütung.
- Öffnen einer detaillierten Job Card aus dem Board.
- Verwaltung einer größeren Zahl gespeicherter Stellen; das konkrete Limit ist tarifabhängig.

### 3.1.2 Job Card / Bewerbungsakte

Die Job Card bündelt alle Informationen zu einer einzelnen beruflichen Gelegenheit. Dokumentiert sind unter anderem:

- Stellenbezeichnung.
- Unternehmen.
- URL beziehungsweise Link zur ursprünglichen Ausschreibung.
- vollständige oder gespeicherte Stellenbeschreibung.
- Standort- und weitere Stelleninformationen, soweit übernommen oder manuell gepflegt.
- Vergütungsinformationen, soweit vorhanden.
- Bewerbungsinformationen und Bewerbungsdatum.
- Plattform beziehungsweise Weg, über den die Bewerbung eingereicht wurde.
- freie Notizen zur Bewerbung.
- Follow-up-Informationen.
- Verknüpfung mit Aktivitäten, Kontakten und Dokumenten.

### 3.1.3 Aktivitäten und Timeline

Huntr trennt den Status einer Bewerbung von den konkreten Ereignissen und Aufgaben. Das ist für unseren Bewerbungsmanager besonders wichtig.

Die Aktivitätsfunktionen umfassen:

- Erfassung bereits erfolgter Aktivitäten.
- Erfassung zukünftiger To-dos beziehungsweise geplanter Aktivitäten.
- Verknüpfung einer Aktivität mit einem konkreten Job.
- Möglichkeit, allgemeine Aktivitäten ohne Jobbezug zu führen.
- Kategorisierung nach Aktivitätstyp.
- Anzeige nach Datum und Kategorie.
- Filter nach offen, erledigt beziehungsweise Fälligkeit.
- Aktivitäten rund um Bewerbungen.
- Aktivitäten rund um Interviews.
- Aktivitäten rund um Angebote.
- Networking-Aktivitäten.
- Nutzung der Aktivitäten als chronologische Historie einer Bewerbung.

### 3.1.4 Bewerbung erfassen

Beim Protokollieren einer Bewerbung können Informationen hinterlegt werden, die über einen simplen Status hinausgehen:

- Zeitpunkt/Datum der Bewerbung.
- Einreichungsplattform oder Bewerbungsweg.
- Notizen zum Versand.
- geplantes Follow-up.
- Zuordnung zur gespeicherten Stelle.

Damit kann später rekonstruiert werden, **wann, wo und auf welchem Weg** eine Bewerbung tatsächlich versendet wurde.

### 3.1.5 Interviewverwaltung

Huntr erlaubt, Interviews an einer Bewerbung zu erfassen. Dokumentiert sind insbesondere:

- Datum und Uhrzeit.
- Format beziehungsweise Art des Interviews.
- Beteiligte/Teilnehmer.
- Vorbereitungshinweise.
- Bezug zur konkreten Bewerbung.
- Einordnung in die Activity Timeline.

Damit ist ein Interview ein eigenes Ereignis und nicht nur ein Pipeline-Status.

### 3.1.6 Kontaktverwaltung

Huntr enthält ein integriertes Kontaktmodul. Kontakte können unabhängig gespeichert und mit Jobs beziehungsweise Boards verknüpft werden.

Dokumentierte Kontaktinformationen sind:

- Name.
- Jobtitel/Rolle.
- Unternehmen.
- Standort.
- eine oder mehrere E-Mail-Adressen.
- Telefonnummern.
- Social-Media- beziehungsweise Profil-Handles.
- Verknüpfung mit Stellen/Bewerbungen.
- Verknüpfung mit Boards.

Damit können Recruiter, Hiring Manager, Fachansprechpartner oder Netzwerkpartner als eigene Objekte behandelt werden.

### 3.1.7 Dokumentverwaltung

Huntr besitzt einen zentralen Dokumentbereich. Dokumente können angelegt beziehungsweise hochgeladen und kategorisiert werden.

Beispiele dokumentierter Dokumenttypen:

- Lebensläufe.
- Anschreiben.
- Dankes-/Thank-you-Schreiben.
- Schreiben zur Ablehnung eines Angebots.
- weitere Bewerbungsdokumente.

Funktionen:

- Dokumente zentral verwalten.
- Dokumente hochladen.
- bestimmte Dokumenttypen innerhalb des Systems erzeugen.
- Dokumente kategorisieren.
- Dokumente einer Bewerbung zuordnen.
- tarifabhängig unterschiedliche Dokumentmengen verwalten.

### 3.1.8 Lebenslauf- und Matching-Funktionen

Huntr ist zugleich eine Resume-Suite. Je nach Tarif gehören dazu:

- Basis-Lebensläufe verwalten.
- auf konkrete Jobs zugeschnittene Lebensläufe erzeugen.
- Resume Review.
- Resume Tailoring mit KI-Unterstützung.
- PDF-Export.
- Resume Templates.
- Match- und Scoring-Funktionen.
- erweiterte Matching- beziehungsweise Insight-Funktionen in höheren Tarifen.
- Keyword Extraction aus Stellenanzeigen.
- Resume Checker.

### 3.1.9 Anschreiben und KI-Unterstützung

Zusätzliche Career-Funktionen umfassen:

- KI-gestützte Anschreiben.
- Nutzung gespeicherter Jobinformationen zur Anpassung der Bewerbungsunterlagen.
- unterschiedliche Umfangs- und Generierungslimits je Tarif.

### 3.1.10 Browser-Erweiterung und Autofill

Die Browser-Erweiterung unterstützt die Erfassung von Stellen und die Bewerbung selbst:

- Job Clipper zum Speichern einer Ausschreibung aus dem Browser.
- Übernahme von Stelleninformationen in den Tracker.
- Application Autofill für unterstützte Formulare.
- Nutzung gespeicherter Profildaten beim Ausfüllen.

### 3.1.11 Karten- und Kennzahlenansichten

Huntr bietet ergänzende Auswertungen:

- geografische Kartenansicht gespeicherter Stellen.
- Metriken zur Jobsuche beziehungsweise zum Bewerbungsfortschritt.
- Übersicht über das Bewerbungsportfolio.

### 3.1.12 Export und Kontodaten

Die Hilfedokumentation nennt außerdem Funktionen zum:

- Export eines Boards beziehungsweise gespeicherter Bewerbungsdaten.
- Download eigener Daten.
- Verwaltung des persönlichen Profils.
- Verwaltung von Tarif und Nutzung.
- Löschen des Kontos beziehungsweise Datenmanagement.

### 3.1.13 Besonders relevante Ideen für SASD

- Bewerbungsakte statt bloßer Kanban-Karte.
- Activity Timeline.
- eigenständige Kontakte.
- konkrete Dokumentversionen je Bewerbung.
- Interview als strukturiertes Ereignis.
- Board plus Detailansicht plus Metriken.

**Offizielle Quellen:**

- https://huntr.co/product/job-tracker
- https://help.huntr.co/

---

## 3.2 Teal Job Tracker

**Kategorie:** Job Tracker innerhalb einer Career-Management-Suite  
**Bereitstellung:** Webanwendung und Browser-Erweiterung  
**Relevanz für SASD:** Sehr hoch für Bewerbungsworkflow, Checklisten und Verbindung zwischen Job, Kontakt und Lebenslauf.

### 3.2.1 Stellen speichern

Teal erlaubt die Übernahme von Stellen direkt aus dem Web oder die manuelle Erfassung.

Funktionen:

- Speichern über Browser-Erweiterung.
- manuelles Anlegen eines Jobs.
- Erfassung von Jobtitel.
- Unternehmen.
- URL.
- Standort.
- vollständiger Stellenbeschreibung.
- Speicherdatum.
- Bewerbungsdatum.
- geplantes Follow-up-Datum.

### 3.2.2 Pipeline und Bewerbungsphasen

Teal strukturiert den Prozess in Pipeline-Stufen. Typische dokumentierte Stufen sind:

- Bookmarked/Saved.
- Applied.
- Interviewing.
- Negotiating.
- Accepted.

Zusätzlich können nicht weiter verfolgte Jobs geschlossen beziehungsweise archiviert werden, etwa wegen:

- Stelle zurückgezogen.
- nicht ausgewählt.
- keine Rückmeldung.
- archiviert.

Der Status kann im Tracker geändert und die Bewerbung dadurch entlang des Prozesses bewegt werden.

### 3.2.3 Guidance / kontextabhängige Handlungshilfe

Ein wesentliches Teal-Merkmal ist die kontextabhängige Guidance:

- Hinweise ändern sich abhängig von der aktuellen Bewerbungsphase.
- Das System zeigt passende nächste Schritte.
- Empfehlungen sind damit nicht global, sondern auf den konkreten Status bezogen.

Für SASD ist das als Vorbild für ein späteres **Next-Action-System** interessant.

### 3.2.4 Checklisten

Teal verwaltet für einzelne Jobs Checklisten mit vorgeschlagenen Best-Practice-Aufgaben.

Damit kann neben dem Status festgehalten werden, was noch konkret zu erledigen ist, beispielsweise:

- Unterlagen prüfen.
- Bewerbung abschicken.
- Follow-up durchführen.
- Interview vorbereiten.
- nach dem Interview danken.

Status und Aufgabe sind damit fachlich getrennt.

### 3.2.5 Notizen

Zu jeder Stelle können Notizen erfasst werden.

Dokumentierte Einsatzfälle:

- Interaktionen protokollieren.
- Interviewtermine aus E-Mails festhalten.
- Fragen für Gespräche sammeln.
- Eindrücke aus Interviews erfassen.
- sonstige freie Informationen zur Bewerbung speichern.

Die Notizen werden automatisch gespeichert und unterstützen Rich-Text-Formatierung.

### 3.2.6 Kontakte

Teal besitzt einen Contact Tracker beziehungsweise Kontaktfunktionen innerhalb des Job Trackers.

Verwendbar für:

- Recruiter.
- Hiring Manager.
- Mitarbeiterkontakte.
- Netzwerk-/Referral-Kontakte.

Kontakte können einer Bewerbung zugeordnet werden, sodass Ansprechpartner nicht nur als Freitext in Notizen vorkommen.

### 3.2.7 Unternehmen

Die Teal-Suite besitzt zusätzlich Unternehmens-/Companies-Funktionen. Dadurch können Informationen zum Arbeitgeber unabhängig vom einzelnen Job betrachtet werden. Für unseren Entwurf ist vor allem das Prinzip wichtig, **Unternehmen und konkrete Stelle als getrennte Objekte** zu behandeln.

### 3.2.8 Lebenslaufbezug pro Job

Ein besonders relevantes Merkmal:

- Lebensläufe können einem konkreten Job zugeordnet werden.
- Match-/Analysis-Ergebnisse können im Zusammenhang mit der Bewerbung angezeigt werden.
- dadurch ist nachvollziehbar, welche CV-Version für welche Stelle verwendet wurde.

### 3.2.9 Excitement / subjektive Priorisierung

Teal erlaubt eine persönliche Bewertung beziehungsweise ein „Excitement Rating“.

Damit wird ein wichtiger Unterschied modelliert:

- objektiver Bewerbungsstatus und
- subjektive Attraktivität einer Stelle

sind nicht dasselbe.

### 3.2.10 E-Mail-Vorlagen und Follow-up

Teal stellt Vorlagen für typische Bewerbungsnachrichten bereit, darunter beispielsweise:

- Dank nach einem Interview.
- Follow-up.
- Anfrage nach Referenzen beziehungsweise referenzbezogene Kommunikation.
- Rückzug einer Bewerbung.
- Ablehnung eines Angebots.

Vorlagen können abhängig von der Bewerbungsphase angeboten werden.

### 3.2.11 Interview-Unterstützung

Neben dem Tracker bietet die Plattform interviewbezogene Funktionen:

- Interviewphase als eigener Prozesszustand.
- Notizen zur Vorbereitung und Nachbereitung.
- Kontaktinformationen der Gesprächspartner.
- KI-gestützte Interview-Praxis beziehungsweise Interview Practice Agent in der Career-Suite.

### 3.2.12 Dashboard und Wochenreview

Teal unterstützt die regelmäßige Pflege der Jobsuche. Die Hilfedokumentation empfiehlt beziehungsweise ermöglicht die Überprüfung von:

- aktuellen Bewerbungen.
- Follow-ups.
- alten/stagnierenden Jobs.
- zu archivierenden Einträgen.
- Fortschritt über die verschiedenen Phasen.

### 3.2.13 Besonders relevante Ideen für SASD

- statusabhängige Handlungsempfehlungen.
- Checkliste je Bewerbung.
- subjektive Attraktivität getrennt vom Prozessstatus.
- konkrete CV-Version je Bewerbung.
- Unternehmen, Kontakt und Bewerbung als verknüpfte Objekte.

**Offizielle Quellen:**

- https://www.tealhq.com/tools/job-tracker
- https://support.tealhq.com/

---

## 3.3 Careerflow

**Kategorie:** Career Suite mit Job Tracker, Networking und Resume Tools  
**Bereitstellung:** Webanwendung und Browser-Erweiterung  
**Relevanz für SASD:** Hoch für Dashboard, Aufgaben, Kontakte, Reminder und Job-Matching.

### 3.3.1 Jobportal und Job Discovery

Careerflow bietet innerhalb seiner Plattform eine Jobsuche beziehungsweise ein Jobportal. Dokumentiert sind:

- Zugriff auf eine große Zahl von Stellenangeboten.
- Suche nach Jobs.
- Nutzung des Trackers zusammen mit gefundenen Stellen.
- Speichern von Stellen aus unterstützten externen Jobbörsen über die Browser-Erweiterung.

Unterstützte beziehungsweise dokumentierte Quellen umfassen unter anderem LinkedIn, Indeed, Glassdoor, Monster, ZipRecruiter und CareerBuilder; die Browser-Erweiterung unterstützt zusätzlich zahlreiche weitere Websites.

### 3.3.2 Bewerbungs-Tracker

Der Tracker verwaltet Bewerbungen in typischen Phasen:

- Saved.
- Applied.
- Interview.
- Offer.

Funktionen umfassen:

- Job anlegen.
- Status ändern.
- Job zwischen Statusbereichen bewegen.
- Job bearbeiten.
- Job entfernen.
- Unternehmen und Rolle anzeigen.
- Stellenbeschreibung hinterlegen.
- Notizen führen.
- Aufgaben an einem Job verwalten.
- Bewerbungsinformationen zentral anzeigen.

### 3.3.3 Eigene Kategorien und Organisation

Careerflow unterstützt zusätzliche Strukturierung:

- Kategorien nach Status.
- Kategorien nach Unternehmen.
- benutzerdefinierte Kategorien.
- Tags beziehungsweise frei definierbare Ordnungsmöglichkeiten.

### 3.3.4 Erinnerungen, Deadlines und Follow-ups

Der Tracker kann zeitbezogene Verwaltungsarbeit unterstützen:

- Follow-up-Erinnerungen.
- Bewerbungsdeadlines.
- Interviewtermine.
- Aufgabenfälligkeiten.
- E-Mail-Erinnerungen für Follow-ups im Kontaktmodul.

### 3.3.5 Dashboard und Analytics

Careerflow zeigt den Fortschritt der Jobsuche in einem zentralen Dashboard.

Dokumentierte Auswertungen umfassen:

- Bewerbungsaktivität.
- Statusverteilung.
- Response-/Rückmelderaten.
- Muster erfolgreichen beziehungsweise weniger erfolgreichen Vorgehens.
- Fortschrittsindikatoren.

### 3.3.6 Contacts / Networking CRM

Careerflow enthält eine Kontaktverwaltung für Networking.

Kontakte können:

- manuell angelegt werden.
- über LinkedIn übernommen werden.
- per Excel/Bulk Import importiert werden.

Mögliche Kontaktdaten beziehungsweise Felder:

- Name.
- Titel/Rolle.
- Unternehmen.
- LinkedIn-Profil.
- Beziehung/Relationship.
- Ziel/Goal.
- Status.
- Notizen.
- mit dem Kontakt verbundene Jobs.

### 3.3.7 Kontaktaktivitäten

Zu Kontakten lassen sich Aktivitäten dokumentieren:

- Aktivitätstyp beziehungsweise Titel.
- Ziel.
- Status.
- Datum.
- Beschreibung.
- Bearbeiten und Löschen bestehender Aktivitäten.
- Follow-up-Datum.
- Erinnerung an Follow-up.

Damit wird Networking als eigener Prozess behandelt und nicht nur als Adressbuch.

### 3.3.8 Resume Builder und Resume Optimizer

Zur Suite gehören:

- Resume Builder.
- KI-gestützte Lebenslaufoptimierung.
- „Write with AI“-Funktionen.
- Skill Matching zwischen Lebenslauf und Stelle.
- Verwendung des Matchings im Resume Builder, in Job Cards und in der Browser-Erweiterung.

### 3.3.9 KI- und Kommunikationsfunktionen

Zusätzliche Career-Funktionen sind:

- KI-Anschreiben.
- E-Mail-Writer.
- Elevator Pitch Generator.
- Personal-Branding-Funktionen.
- KI-Assistent.
- LinkedIn-Optimierung.

Diese Funktionen sind für den Kern unseres Bewerbungsmanagers nicht zwingend, zeigen aber, wie stark moderne Career Suites die Verwaltung mit Content-Generierung verbinden.

### 3.3.10 Import und Bulk-Funktionen

Careerflow bietet für Teile des Systems:

- manuellen Jobimport.
- Importvorlagen/Bulk-Import.
- Kontaktimport aus Excel.
- Übernahme von Jobs über Browser-Erweiterung.

### 3.3.11 Besonders relevante Ideen für SASD

- Aufgaben und Bewerbung auf demselben Dashboard.
- Networking als eigener Kontaktprozess.
- Follow-up-Datum direkt am Kontakt.
- Analytics über Rückmelderaten.
- flexible Kategorien statt ausschließlich fixer Pipeline.

**Offizielle Quellen:**

- https://www.careerflow.ai/job-tracker
- https://help.careerflow.ai/

---

## 3.4 Jobscan Job Tracker

**Kategorie:** Job Tracker innerhalb einer Resume-/ATS-Optimierungsplattform  
**Bereitstellung:** Webanwendung und Browser-Erweiterung  
**Relevanz für SASD:** Hoch für die Verbindung von Stelle, Bewerbungsakte, Resume-Version, Interview und Kontakt.

### 3.4.1 Jobsuche und Job Discovery

Jobscan kann Stellen auf Basis von Nutzerangaben beziehungsweise Lebenslaufdaten auffindbar machen.

Funktionen:

- Suche nach Titel/Keyword.
- Suche nach Standort.
- personalisierte Stellenempfehlungen.
- Jobdetails anzeigen.
- aus der Jobansicht heraus Lebenslauf anpassen beziehungsweise optimieren.
- anschließend zur Bewerbung wechseln.

### 3.4.2 Visueller Bewerbungs-Tracker

Der Job Tracker stellt Bewerbungen als visuelle Pipeline dar.

Funktionen:

- Jobs speichern.
- Bewerbung in Statusstufen verwalten.
- Bewerbungen beispielsweise von „Applied“ zu „Interviewed“ weiterbewegen.
- Fortschritt visuell vergleichen.
- Detailinformationen pro Job öffnen.

### 3.4.3 Job Card und Bewerbungsdetails

Eine Job Card kann verschiedene Informationen bündeln:

- Unternehmen.
- Stelle.
- Stellenbeschreibung.
- Bewerbungsstatus.
- Kontakte beziehungsweise Recruiterinformationen.
- Interviewdaten.
- Meeting-Links.
- Notizen.
- Aufgaben/Reminder.
- Lebenslaufversion.
- Match Rate.

### 3.4.4 Interviewverwaltung

Der Tracker unterstützt Interviewinformationen:

- Datum des Interviews.
- Zeit beziehungsweise Planung.
- Meeting-Link.
- Ansprechpartner/Teilnehmer.
- Notizen.
- Erinnerungen und Follow-ups.

### 3.4.5 Kontakte und Recruiter

Kontakte können im Kontext einer Bewerbung hinterlegt werden, sodass Recruiter- und Ansprechpartnerinformationen direkt mit dem Vorgang verbunden sind.

### 3.4.6 Notizen, Aufgaben und Follow-up

Der Tracker ermöglicht:

- freie Notizen.
- offene Aufgaben.
- Erinnerungen.
- Follow-up-Verwaltung.
- Sammeln von Informationen für die Interviewvorbereitung.

### 3.4.7 Resume-Version pro Job

Eine besonders relevante Funktion ist die Möglichkeit, einer Bewerbung die konkrete Resume-Version zuzuordnen. Dadurch wird dokumentiert, welcher Lebenslauf zu welcher Ausschreibung verwendet wurde.

### 3.4.8 Resume Scanner und Match Rate

Jobscan ist im Kern stark auf ATS-/Resume-Optimierung ausgerichtet:

- Stellenbeschreibung analysieren.
- Lebenslauf dagegen scannen.
- Match Rate anzeigen.
- fehlende beziehungsweise abweichende Schlüsselbegriffe identifizieren.
- Lebenslauf in Echtzeit optimieren.
- Jobbezogene Anpassungen durchführen.

### 3.4.9 Company Insights und Gesprächsvorbereitung

Für Jobs können zusätzliche Arbeitgeberinformationen beziehungsweise Gesprächshilfen eingeblendet werden, darunter dokumentierte Beispiele wie:

- Unternehmenswerte.
- Wettbewerber.
- Talking Points.

### 3.4.10 Anschreiben und Kommunikationshilfen

Im Umfeld einer Stelle können zusätzliche Inhalte erzeugt beziehungsweise vorbereitet werden:

- jobbezogenes Anschreiben.
- E-Mail zur Bestätigung beziehungsweise Klärung von Interviewdetails.
- Follow-up-Unterstützung.

### 3.4.11 Browser-Erweiterung

Die Erweiterung kann Jobs von unterstützten Jobportalen speichern, darunter dokumentiert:

- Indeed.
- LinkedIn.
- Glassdoor.

Dabei werden Jobinformationen automatisch in den Tracker übernommen.

### 3.4.12 Besonders relevante Ideen für SASD

- verwendetes Dokument als Bestandteil der Bewerbungsakte.
- Interviewdatum und Meeting-Link direkt am Vorgang.
- Aufgaben und Reminder im Job-Kontext.
- explizite Verbindung zwischen Stellenanforderung und verwendeter Bewerbungsversion.

**Offizielle Quellen:**

- https://www.jobscan.co/job-tracker
- https://www.jobscan.co/

---

## 3.5 Simplify

**Kategorie:** Job Discovery, Browser-Assistent, Autofill und Application Tracker  
**Bereitstellung:** Webplattform und Browser-Erweiterung „Simplify Copilot“  
**Relevanz für SASD:** Sehr hoch für spätere Import-/Capture-Automatisierung und automatische Erkennung von Bewerbungen.

### 3.5.1 Job Discovery und Suche

Simplify kombiniert einen Jobkatalog mit persönlichen Matching- und Filterfunktionen:

- Suche in einer sehr großen Zahl von Stellenangeboten.
- detaillierte Suchfilter.
- personalisierte Job Matches.
- Speichern/Bookmarking interessanter Stellen.
- Nutzung der Browser-Erweiterung auf externen Jobbörsen.

### 3.5.2 Browser-Assistent / Autofill

Simplify Copilot ist eine der Kernfunktionen:

- automatisches Ausfüllen von Bewerbungsformularen.
- Nutzung des hinterlegten Profils als Datenquelle.
- Übernahme von Kontaktdaten.
- Übernahme von Ausbildung.
- Übernahme von Berufserfahrung.
- Übernahme von Arbeitserlaubnis-/Work-Authorization-Angaben.
- Übernahme demografischer Angaben, sofern der Nutzer sie im Profil führt und das Formular sie verlangt.
- Übernahme von Links und Portfolioinformationen.
- Beantwortung häufig wiederkehrender Bewerbungsfragen.
- Speichern von Antworten zu individuellen Fragen anhand des exakten Fragetextes und Wiederverwendung bei späteren Bewerbungen.

### 3.5.3 Unterstützung vieler ATS-/Karriereseiten

Der Copilot unterstützt laut Anbieter zahlreiche Recruiting-Systeme und Jobseiten, unter anderem:

- Workday.
- Greenhouse.
- iCIMS.
- Taleo.
- Avature.
- Lever.
- SmartRecruiters.
- zahlreiche weitere Portale.

Das ist für SASD ein wichtiges Beispiel für eine **Adapter-/Capture-Schicht** über sehr heterogenen externen Systemen.

### 3.5.4 Lebenslaufverwaltung beim Autofill

Simplify kann:

- einen Standard-Lebenslauf verwalten.
- mehrere Lebenslaufversionen speichern.
- beim Bewerbungsprozess die passende Datei verwenden.
- Lebensläufe direkt hochladen.
- Lebensläufe mit der Stellenbeschreibung vergleichen.

### 3.5.5 Resume Matching und Tailoring

Funktionen umfassen:

- Resume Score gegen Stellenbeschreibung.
- Keyword-Vergleich.
- Identifikation fehlender Skills/Keywords.
- Anpassung eines Lebenslaufs im Resume Builder.
- KI-gestütztes Tailoring.
- ATS-Readiness beziehungsweise Qualitätsbewertung.
- Versionierung beziehungsweise mehrere Resume-Varianten.
- Export.

### 3.5.6 Anschreiben

Beim Bewerbungsprozess kann:

- ein Standardanschreiben verwendet werden.
- ein jobbezogenes Anschreiben erzeugt werden, insbesondere in kostenpflichtigen Funktionen.

### 3.5.7 Automatisches Application Tracking

Eine der relevantesten Funktionen:

- nach dem Absenden einer Bewerbung kann diese automatisch im Tracker gespeichert werden.
- Nutzer müssen Bewerbungen dadurch nicht zwingend doppelt erfassen.
- Bewerbungshistorie entsteht direkt aus dem eigentlichen Bewerbungsprozess.

### 3.5.8 Bewerbungs-Tracker

Der Tracker unterstützt:

- gespeicherte Bewerbungen.
- Bewerbungsstatus.
- Fortschrittsübersicht.
- Notizen.
- Recruiter-/Referral-Kontakte.
- Interviewnotizen.
- Interviewdaten.
- Follow-up-Unterstützung.
- Speicherung passender Lebensläufe und Anschreiben.
- eigene/manuell hinzugefügte Bewerbungen.
- Import beziehungsweise Übernahme aus anderen Erfassungswegen.

### 3.5.9 Follow-up und Kommunikation

Simplify kann beim Verfassen von Follow-up-E-Mails helfen und den Nutzer beim nächsten Schritt nach einer Bewerbung unterstützen.

### 3.5.10 Analytics

Der Tracker enthält Auswertungen, mit denen unter anderem betrachtet werden kann:

- Bewerbungsfortschritt.
- welche Resume-Varianten besser funktionieren.
- Verteilung beziehungsweise Entwicklung der Bewerbungen.

### 3.5.11 AI Talent Agent und Networking-nahe Funktionen

Die Plattform entwickelt zusätzlich agentische Funktionen, die unter anderem:

- den Markt beobachten.
- passende Jobs identifizieren.
- Resume-Anpassung unterstützen.
- potenzielle Referrals beziehungsweise warme Intro-Pfade identifizieren.
- Follow-ups in einem Dashboard bündeln.

Für unseren Produktkern ist dabei weniger „Agent“ als das Prinzip interessant, mehrere Verwaltungs- und Informationsschritte proaktiv vorzubereiten.

### 3.5.12 Nutzerkontrolle

Trotz weitreichendem Autofill bleibt der eigentliche Versand einer Bewerbung nach der öffentlich beschriebenen Copilot-Logik beim Nutzer. Das ist ein wichtiges Muster: **Automatisieren, ohne die Entscheidung zur Bewerbung aus der Hand zu nehmen.**

### 3.5.13 Besonders relevante Ideen für SASD

- Browser-Capture statt manueller Doppelpflege.
- Adapter für viele externe ATS.
- automatische Anlage einer Bewerbung nach tatsächlichem Versand.
- gespeicherte Antworten auf wiederkehrende Formularfragen.
- Lebenslauf- und Anschreibenversion direkt am Vorgang.

**Offizielle Quellen:**

- https://simplify.jobs/copilot
- https://help.simplify.jobs/

---

# 4. Open-Source- und Local-first-Referenzen

## 4.1 JSE

**Kategorie:** Local-first Desktop Job Search Assistant  
**Lizenz/Status:** Open Source; paketierte Builds werden als Beta bezeichnet  
**Plattformen:** Windows x64, macOS Intel/Apple Silicon, Ubuntu x64  
**Relevanz für SASD:** Außerordentlich hoch; stärkste Referenz für Local-first, evidenzbasiertes Lernen und Marktintelligenz.

### 4.1.1 Grundprinzip Local-first

JSE ist konsequent darauf ausgelegt, private Bewerbungsdaten lokal zu halten:

- Einstellungen lokal.
- Dokumente lokal.
- Datenbank lokal.
- Browserprofile lokal.
- Backups lokal.
- Matching und Bewertung können vollständig über ein lokales LLM laufen.
- Cloud-KI ist optional.
- beim Umschalten des hochvolumigen Matchings auf einen externen Provider wird vor der Übertragung von Anzeigen- und Resume-Kontext gewarnt.

### 4.1.2 Mehrere Career Lanes

JSE kann mehrere Karrierepfade parallel verwalten. Jede „Lane“ kann besitzen:

- eigenen Namen/Profilkontext.
- eigenen Basis-Lebenslauf.
- bevorzugte Standorte.
- erlaubte Arbeitsmodelle.
- Such-/Seitenlimits.
- eigene Matching-Regeln.
- eigene aktivierte Suchquellen.

Damit kann ein Nutzer beispielsweise „Linux/Platform“, „C# Entwicklung“ und „Trainer“ getrennt suchen und bewerten lassen.

### 4.1.3 Plugin-basierte Stellensuche

Die Discovery-Schicht arbeitet mit Scraper-Plugins:

- mehrere Jobquellen.
- Quellen global aktivieren/deaktivieren.
- Quellen je Lane aktivieren/deaktivieren.
- Standort konfigurieren.
- Seitenlimits konfigurieren.
- eigene kompatible Scraper importieren.
- neuen Scraper über „Build A Scraper Plugin“ erzeugen.
- Scraper zunächst in einem Dry Run testen.

### 4.1.4 Selbstreparierende Sucher

Ein außergewöhnliches Merkmal:

- fehlerhaften Scraper diagnostizieren.
- konfiguriertes lokales LLM kann einen Ersatz beziehungsweise eine Reparatur erzeugen.
- Reparatur isoliert testen.
- Ergebnis verifizieren.
- bei Problemen Rollback durchführen.

### 4.1.5 Parallele Suche und Analyse

JSE kann mehrere Jobs gleichzeitig analysieren:

- Standardparallelität 2.
- konfigurierbar bis 8 parallele Analysen.
- Pause.
- Cancel/Abbruch.
- Providerwahl passend zur benötigten Last.

### 4.1.6 Gestuftes Matching

Stellen werden nicht sofort mit maximalem Aufwand analysiert. Stattdessen gibt es eine abgestufte Bewertung:

1. schneller initialer Match/Triage.
2. tiefere Fragment-/Vollbewertung für interessante Treffer.
3. Hinweise zur Bewerbungsstrategie.

Die tiefere Bewertung kann beschreiben:

- Stärken des Kandidaten gegenüber der Stelle.
- Schwächen beziehungsweise Lücken.
- mögliche Positionierung.
- Eignung beziehungsweise Score.

### 4.1.7 Providerwahl je Workflow

JSE trennt KI-Provider funktional:

- Job Matching kann lokal oder über separaten Provider laufen.
- Application Documents können einen anderen, stärkeren Provider verwenden.
- Employer Research kann wiederum lokal oder extern laufen.

Unterstützte beziehungsweise dokumentierte Varianten:

- lokale OpenAI-kompatible Endpoints.
- LM Studio.
- Ollama.
- llama.cpp-artige Server.
- vLLM.
- OpenAI/ChatGPT API.
- Claude.
- Gemini.
- OpenAI-kompatible Dienste wie Groq, Cerebras, OpenRouter oder OpenCode Zen.

### 4.1.8 Candidate Knowledge Base / RAG

JSE baut aus der bisherigen Bewerbungshistorie eine private Wissensbasis:

- alte Bewerbungen aufnehmen.
- alte Lebensläufe aufnehmen.
- Anschreiben aufnehmen.
- KSC-/Selection-Criteria-Antworten aufnehmen.
- Positionsbeschreibungen aufnehmen.
- Dokumente in Fragmente zerlegen und indexieren.
- diese Evidenz beim Matching und bei der Dokumentgenerierung wiederverwenden.

Ziel ist nicht nur Keyword-Overlap, sondern ein wachsendes Modell realer beruflicher Erfahrungen und Belege.

### 4.1.9 Evidence Library

Die Evidence Library erlaubt aktive Verwaltung der Wissensbasis:

- anzeigen, welche Dokumente bereits aufgenommen wurden.
- nach Dokumenttyp differenzieren.
- Dokumente neu indexieren.
- Fragmente erneut aus Dokumenten gewinnen.
- Dokumenttyp neu klassifizieren.
- einzelne Dokumente aus der Evidenzbasis entfernen.

### 4.1.10 Interview-validierte Evidenz

Wenn eine Bewerbung zu einem Interview führt:

- JSE kann Stellenbeschreibung und eingereichte Dokumente erneut in Fragmente zerlegen.
- diese Evidenz wird höher gewichtet als Material, das lediglich zu einer abgeschickten Bewerbung geführt hat.
- Interviewerfolg wird damit als empirisches Signal verwendet, welche Erfahrungen und Formulierungen tatsächlich Resonanz erzeugen.

### 4.1.11 Employer Research

Vor einer Bewerbung kann JSE Arbeitgeberkontext recherchieren beziehungsweise zusammenstellen:

- Informationen zum Unternehmen.
- Kontext zur Rolle.
- vorgeschlagene Fragen für ein Interview.

### 4.1.12 Bewerbungsmaterial in eigener Stimme

JSE kann Bewerbungsdokumente erzeugen und dabei:

- alte eigene Bewerbungen.
- Evidenzfragmente.
- bisherigen Schreibstil.
- konkrete Stellenanforderungen

kombinieren. Der Fokus liegt ausdrücklich darauf, Material in der eigenen Stimme des Nutzers zu erzeugen.

### 4.1.13 Kanban-Pipeline

Die Pipeline umfasst:

- gefundenen Job.
- weitere Bewerbungsstufen.
- angewandte/beworbene Jobs.
- Interviewphase.
- Ergebnis/Outcome.
- Verschieben über ein Kanban-Board.

### 4.1.14 Interview- und Feedback-Tracking

Zu Bewerbungen können:

- Interviews erfasst werden.
- Feedback zum Interview gespeichert werden.
- daraus später Interview Learnings gewonnen werden.

### 4.1.15 Follow-up-Scheduling

Für Bewerbungen kann ein Follow-up-Zeitplan geführt werden, damit offene Vorgänge nicht unbeabsichtigt liegen bleiben.

### 4.1.16 Auto-Archivierung

Bewerbungen ohne direkte Reaktion des Arbeitgebers können nach einer konfigurierten Zeit automatisch archiviert werden. Das hält die aktive Pipeline übersichtlich, ohne die Historie zu vernichten.

### 4.1.17 Externe Bewerbungen protokollieren

Bewerbungen müssen nicht über JSE gestartet worden sein. Man kann auch Vorgänge nacherfassen, etwa:

- Bewerbung über die Karriereseite eines Unternehmens.
- Referral/Empfehlung.
- recruiter-geführte Bewerbung.

Dadurch bleiben Off-Platform-Interviews und Ergebnisse in den Statistiken sichtbar.

### 4.1.18 Lokale Datenbank und Backup

JSE verwendet einen lokalen SQLite-Datenbestand und bietet Funktionen für:

- Datenbankmanagement.
- Backup.
- lokale Arbeitsdatenordner.

### 4.1.19 Funnel Insights

JSE speichert beim Erreichen des Zustands „Applied“ einen Snapshot und analysiert später, welche Segmente tatsächlich zu Interviews führen.

Vergleichsdimensionen umfassen:

- Quelle.
- Advertiser/Inserent.
- Arbeitgebertyp.
- Match-Score-Band.
- Gehaltsband.
- Senioritätsstufe.
- Lane/Karrierepfad.

Segmente mit sehr wenigen Beobachtungen werden unterdrückt, damit kleine Stichproben nicht als belastbares Signal erscheinen.

### 4.1.20 Conversion beeinflusst Scoring

Beobachtete Erfolgsraten können das spätere Scoring begrenzt beeinflussen:

- erst ab einer Mindestmenge von Outcomes.
- nur als begrenzter Nudge.
- darf allein keinen Job über eine Auto-Reject-Schwelle drücken.

Das ist eine interessante Form von **erfahrungsbasiertem, aber kontrolliertem Lernen**.

### 4.1.21 Ehrliche Zählung / Deduplizierung

JSE versucht statistische Verzerrung zu vermeiden:

- dieselbe Rolle, die unter leicht unterschiedlichen Titeln erneut inseriert wird, kann als eine Rolle behandelt werden.
- Outcome-Snapshots bleiben erhalten, selbst wenn ursprüngliche Jobs später gelöscht beziehungsweise bereinigt werden.

### 4.1.22 Interview Learnings

Ein eigener Bereich zeigt interviewte Rollen und daraus gewonnene Evidenz:

- alle Rollen mit Interview.
- One-click Mining.
- resultierende Fragmente.
- Claim/Aussage.
- Keywords.
- Hinweise zur Wiederverwendung.
- Herkunftsrollen der Evidenz.

### 4.1.23 Hidden-Market-Analyse

JSE analysiert nicht nur konkrete Stellen, sondern den Markt:

- welche Recruiter passende Jobs in einer Region veröffentlichen.
- wo der Lebenslauf gegenüber der Nachfrage Lücken aufweist.
- welche Unternehmen besonders stark einstellen.

### 4.1.24 Current-Market-Analyse

Rolling-/Zeitfenster-Auswertungen umfassen:

- Zahl verfügbarer passender Jobs in letzter Woche/Monat.
- Bewerbungsaktivität.
- Fortschritt eigener Bewerbungen.
- Cut-through-/Conversion-Raten.

### 4.1.25 Explainable Opportunity Ranking

Recruiter, Arbeitgeber und mögliche Leadership-/Kontaktchancen können gerankt werden anhand von:

- Lane Fit.
- Wiederholung/Häufigkeit.
- Aktualität.
- Momentum.
- Identitätssicherheit.
- Kontaktierbarkeit.
- beobachteten Outreach-Ergebnissen.

Quelljobs bleiben für die Nachvollziehbarkeit erhalten.

### 4.1.26 Durable Outreach Intelligence

Zu einem Ziel können lokal strukturierte Outreach-Informationen gespeichert werden:

- Strategie.
- Kanal.
- Opening Message.
- Evidenz.
- Fragen.
- Follow-ups.
- Warnhinweise/Cautions.

### 4.1.27 Besonders relevante Ideen für SASD

- Local-first als Architekturprinzip.
- Interviewerfolg als Evidenzsignal.
- Bewerbungshistorie als private Wissensbasis.
- Deduplizierung wiederveröffentlichter Rollen.
- Funnel-Lernen mit Mindeststichproben.
- Arbeitgeber-, Recruiter- und Marktintelligenz getrennt von der einzelnen Bewerbung.

**Offizielle Quellen:**

- https://github.com/Keljian/JSE

---
## 4.2 JobSync

**Kategorie:** Self-hosted Job Search Assistant / Application Tracker  
**Lizenz:** MIT  
**Bereitstellung:** Self-hosted Webanwendung, typischerweise über Docker  
**Relevanz für SASD:** Sehr hoch für Kombination aus Bewerbungsverwaltung, Aufgaben, Lebensläufen, Discovery und kontrollierter Agentenintegration.

### 4.2.1 Application Tracker

JobSync verwaltet Bewerbungen strukturiert. Dokumentierte Informationen sind:

- Unternehmen.
- Stellenbezeichnung.
- Bewerbungsdatum.
- aktueller Bewerbungsstatus.
- weitere Jobinformationen und Beschreibungen.
- Zuordnung zum Bewerbungsprozess.

Der Tracker bildet damit die zentrale operative Liste der eigenen Bewerbungen.

### 4.2.2 Monitoring Dashboard

Das Dashboard visualisiert den Verlauf der Jobsuche:

- Zahl beziehungsweise Aktivität der Bewerbungen.
- aktuelle Bewerbungsaktivitäten.
- Erfolgsraten.
- anstehende Aufgaben.
- jüngste Bewerbungen.
- Wochenaktivität.
- Aktivitätskalender.

Damit verbindet JobSync Vorgangsverwaltung und Fortschrittsmessung.

### 4.2.3 Resume Management

JobSync besitzt eine eigenständige Lebenslaufverwaltung:

- mehrere Lebensläufe speichern.
- Lebensläufe bearbeiten/verwalten.
- Lebensläufe als PDF exportieren.
- Wahl zwischen mindestens zwei dokumentierten PDF-Layouts: Simple und Professional.
- bestehende PDF-Anhänge ersetzen oder den Export nur herunterladen.
- einen Standardlebenslauf im Profil festlegen.

### 4.2.4 Resume Import aus PDF und DOCX

Bestehende Lebensläufe können importiert werden:

- PDF-Dateien einlesen.
- Word-/DOCX-Dateien einlesen.
- KI extrahiert strukturierte Daten.
- Extraktion von Kontaktinformationen.
- Zusammenfassung/Profile Summary.
- Skills.
- Berufserfahrung.
- Ausbildung.
- Zertifizierungen.
- jede erkannte Sektion wird als eigener Review-Schritt präsentiert.
- Nutzer kann einzelne Abschnitte übernehmen oder verwerfen.

Diese Funktion ist ein gutes Muster für **KI als Vorschlags- und Extraktionssystem mit expliziter Benutzerbestätigung**.

### 4.2.5 AI Resume Review

Lebensläufe können mit einem KI-Modell geprüft werden. Die Plattform nutzt die vorhandenen Resume-Daten für:

- qualitative Resume Review.
- Verbesserungshinweise.
- Verwendung als Kandidatenprofil für Job Matching.

### 4.2.6 Job Matching

Jobbeschreibungen können gegen einen Lebenslauf analysiert werden:

- Match Score.
- Empfehlung beziehungsweise Einschätzung.
- schriftliche Begründung/Write-up.
- Speicherung des Match-Ergebnisses am Job.

### 4.2.7 Automated Job Discovery

JobSync kann neue Jobs zeitgesteuert von unterstützten Arbeitgeber-ATS abrufen:

- Automationen anlegen.
- Unternehmen überwachen.
- Suche nach einem Zeitplan ausführen.
- neue Ausschreibungen abrufen.
- zunächst lokale Relevanzbewertung verwenden.
- nur die interessanteren Treffer einer teureren KI-Bewertung zuführen.
- Treffer zur manuellen Review vorlegen.
- interessanten Treffer annehmen und in den Job Tracker übernehmen.
- uninteressanten Treffer verwerfen/dismiss.

### 4.2.8 Greenhouse Discovery

Für Greenhouse kann JobSync:

- konkrete Unternehmen aus einer integrierten Liste auswählen.
- alternativ eine Greenhouse-Board-URL angeben.
- sämtliche aktuell veröffentlichten Rollen des Unternehmens laden.
- gegen gewünschte Titel, Skills und Resume vorsortieren.
- Top-Kandidaten per KI matchen.
- benötigt dafür nach Repository-Dokumentation keinen Greenhouse-API-Key.

### 4.2.9 Lever Discovery

Für Lever existiert ein ähnlicher Workflow:

- integriertes Verzeichnis mit über tausend dokumentierten Unternehmen.
- alternativ Board-URL eingeben.
- passende regionale API automatisch wählen.
- Remote-/Hybrid-/Onsite-Signale aus dem Listing übernehmen.
- Jobs vorsortieren und per KI bewerten.
- Treffer annehmen oder verwerfen.

### 4.2.10 Task Management

JobSync enthält Aufgabenverwaltung:

- Aufgaben erstellen.
- Aufgaben verwalten.
- Aktivitäten mit Aufgaben verknüpfen.
- anstehende Aufgaben im Dashboard anzeigen.

### 4.2.11 Activity Management und Zeiterfassung

Zusätzlich zur Task-Liste können Aktivitäten protokolliert werden. Dokumentiert ist auch Time Tracking im Zusammenhang mit Aufgaben/Aktivitäten. Dadurch kann sichtbar werden, wie viel Aufwand in konkrete Job-Search-Aktivitäten fließt.

### 4.2.12 Question Bank

JobSync besitzt eine Question Bank, die insbesondere für Interviewfragen genutzt werden kann. Über die Plattform beziehungsweise den MCP-Zugang lassen sich:

- Interviewfragen speichern.
- eigene Antworten speichern.
- bestehende Wissenseinträge ergänzen.

Das ist eine interessante Referenz für eine spätere **Interview-Wissensbasis**.

### 4.2.13 MCP Server / AI Agent Integration

Ein integrierter MCP-Server erlaubt externen KI-Agenten, Daten in JobSync vorzubereiten beziehungsweise einzutragen.

Funktionen:

- Zugriffstoken in den Einstellungen erzeugen.
- Token benennen.
- Ablaufzeit definieren.
- Agenten wie Claude Desktop und andere MCP-kompatible Clients anbinden.
- neue Bewerbungen aus einem Chat heraus anlegen.
- Question-Bank-Einträge anlegen.
- Unternehmen, Titel, Standort und Tags anhand vorhandener Daten auflösen.
- falls nicht vorhanden, passende Datensätze erzeugen.
- Agent meldet zurück, was gefunden beziehungsweise neu erstellt wurde.
- bei ausreichend umfangreicher Jobbeschreibung und vorhandenem Standardresume einen Match durchführen.
- Match Score und Begründung am Job speichern.
- Aktionen erfolgen laut Projektbeschreibung mit Zustimmung des Nutzers.

### 4.2.14 Datenhoheit und Self-hosting

JobSync ist für Self-hosting ausgelegt:

- Docker-basierte Installation.
- lokale beziehungsweise selbst kontrollierte Datenhaltung.
- eigener Account innerhalb der selbst betriebenen Instanz.
- Konfiguration der Zeitzone.
- Authentifizierungs-Secret.
- Updates über bereitgestellte Deploy-Skripte.

### 4.2.15 Unterstützte KI-Provider

Dokumentiert sind:

- Ollama als lokale Standardoption.
- OpenAI.
- DeepSeek.
- Google Gemini.
- OpenRouter.

Modelle müssen für die KI-Funktionen strukturierte Ausgaben unterstützen.

### 4.2.16 Besonders relevante Ideen für SASD

- Agenten dürfen Daten **mit Approval** eintragen.
- Job Discovery liefert Treffer zur Review statt automatischer Bewerbung.
- Resume-Import mit sektionenweiser Übernahme.
- Question Bank für Interviewwissen.
- lokale Vorsortierung vor kostspieliger KI-Analyse.

**Offizielle Quelle:**

- https://github.com/Gsync/jobsync

---

## 4.3 JobOps

**Kategorie:** Self-hosted Job Search Pipeline  
**Lizenz:** AGPLv3 plus Commons Clause gemäß Repository  
**Bereitstellung:** Self-hosted per Docker; zusätzlich kommerziell gehostete Instanz  
**Relevanz für SASD:** Sehr hoch für Provider-/Source-Architektur, kontrollierte Automatisierung und E-Mail-basiertes Tracking.

### 4.3.1 Zentrales Produktprinzip

JobOps bündelt fünf große Schritte:

1. **Search** – Stellen aus mehreren Quellen suchen.
2. **Score** – Eignung gegen das eigene Profil bewerten.
3. **Tailor** – Lebenslauf für eine konkrete Rolle anpassen.
4. **Export** – fertigen Resume als PDF ausgeben.
5. **Track** – Bewerbungen nach dem Absenden nachverfolgen.

Das Projekt betont ausdrücklich, dass es **keine Bewerbungen automatisch absendet**. Der Nutzer bewirbt sich weiterhin selbst.

### 4.3.2 Multi-Source-Stellensuche

JobOps durchsucht mehrere Jobbörsen aus einer Oberfläche. Dokumentiert sind aktuell unter anderem:

- LinkedIn.
- Indeed.
- Glassdoor.
- Adzuna.
- Hiring Cafe.
- startup.jobs.
- Working Nomads.
- Gradcracker.
- UK Visa Jobs.
- Golang Jobs.
- Seek für Australien/Neuseeland über Apify.
- WUZZUF.
- Khamsat.

Die Liste kann sich mit Releases erweitern.

### 4.3.3 Erweiterbare Extractor-Architektur

Weitere Quellen können über eigene TypeScript-Extractor ergänzt werden. Das ist für SASD als Referenz für eine **Provider-/Adapter-Schnittstelle** besonders relevant.

### 4.3.4 Suchkriterien und laufende Suchverbesserungen

Die Projektentwicklung dokumentiert neben der Grundsuche zusätzliche Such- und Watchlist-Funktionen, darunter je nach Release:

- Posting Age beziehungsweise Alter einer Ausschreibung.
- Radius-/Umkreissuche.
- Watchlists.
- Greenhouse-orientierte Watchlists/Unternehmensüberwachung.
- zusätzliche Karriereboard-Quellen.

Bei solchen releaseabhängigen Funktionen muss im Detail immer die eingesetzte JobOps-Version berücksichtigt werden.

### 4.3.5 AI Fit Score

Jeder Job kann gegen das Kandidatenprofil bewertet werden:

- Score von 0 bis 100.
- Ranking der Suchergebnisse nach Eignung.
- Nutzung des hinterlegten Profils beziehungsweise Resume-Kontexts.
- Unterstützung verschiedener KI-Provider.

### 4.3.6 Visa Sponsorship Check

JobOps kann den Sponsoring-/Visa-Kontext einer Ausschreibung beziehungsweise eines Arbeitgebers prüfen. Das ist ein gutes Beispiel für eine **zielgruppenspezifische Zusatzdimension**, die nicht bei jedem Nutzer gleich wichtig sein muss.

### 4.3.7 Resume Tailoring

Für eine konkrete Stelle kann JobOps den Lebenslauf anpassen:

- Jobbeschreibung als Kontext.
- Überarbeitung des CV für die Rolle.
- jobbezogene Priorisierung/Umformulierung.
- Resume-Studio-Funktionen wurden über Releases weiter ausgebaut.

### 4.3.8 PDF-Export

Ein angepasster Resume kann:

- lokal als PDF erzeugt werden.
- alternativ über die Integration mit Reactive Resume formatiert/ausgegeben werden.

### 4.3.9 Application Tracking

Bewerbungen werden nach dem tatsächlichen Bewerben in einer zentralen Pipeline verfolgt. Der Nutzer erhält eine gemeinsame Sicht statt verschiedener Tabellen und Jobportale.

### 4.3.10 Gmail-Integration und automatische Statusänderungen

JobOps kann Gmail anbinden und Recruiter-/Arbeitgeberantworten beobachten.

Dokumentierter Ablauf:

- Gmail verbinden.
- relevante Recruiter-Nachrichten erkennen.
- Intervieweinladung erkennen.
- Status automatisch auf „Interviewing“ setzen.
- Absage erkennen.
- Status auf „Rejected“ setzen.
- weitere Antworttypen wie Offers werden im Projekt als Teil des Tracking-Konzepts berücksichtigt.

Neuere Releases dokumentieren zusätzlich eine E-Mail-Timeline im Bewerbungsumfeld.

### 4.3.11 Nutzerkontrolle statt Auto-Apply

Ein zentraler Produktentscheid ist, Verwaltung und Vorbereitung zu automatisieren, **nicht aber die finale Bewerbungshandlung**. Das Projekt begründet dies damit, dass automatisierte Massenbewerbungen qualitativ nachteilig sein können und Recruiter sie erkennen können.

### 4.3.12 KI-Provider

Dokumentiert unterstützt JobOps:

- Codex über lokalen App-Server/Docker und `codex login`.
- OpenAI.
- Anthropic Claude.
- GLM/Zhipu AI.
- Google Gemini.
- OpenRouter.
- beliebige OpenAI-kompatible Endpoints.
- lokale Modelle über Ollama oder LM Studio.

### 4.3.13 Self-hosting

Die lokale Variante wird typischerweise per Docker Compose gestartet:

- eigene Instanz.
- Kontrolle über Daten und Konfiguration.
- Self-hosted bleibt laut Projektbeschreibung kostenlos nutzbar.

### 4.3.14 Hosted Cloud

Alternativ gibt es JobOps Cloud:

- eigene gehostete Instanz.
- Managed Updates.
- BYOK-Modell oder Tarif mit enthaltenem KI-Zugang.
- mögliche Quoten für rechenintensive Funktionen wie Suche, Tailoring, Ghostwriter oder PDF-Export.

### 4.3.15 Telemetrie

Das Repository weist auf anonyme Nutzungsanalyse über Umami hin und beschreibt eine Möglichkeit, diese durch Blockieren der betreffenden Domain zu unterbinden. Das ist für unseren Datenschutzvergleich relevant, weil „self-hosted“ nicht automatisch „ohne ausgehende Telemetrie“ bedeutet.

### 4.3.16 Besonders relevante Ideen für SASD

- klar getrennte Pipeline Search → Score → Tailor → Export → Track.
- Source-/Extractor-Plug-ins.
- keine automatische Massenbewerbung.
- Gmail-Antworten als Statussignal.
- lokale und Cloud-KI austauschbar.
- Privacy-Prüfung auch bei Self-hosted-Produkten.

**Offizielle Quellen:**

- https://github.com/DaKheera47/job-ops
- https://jobops.dakheera47.com/

---

## 4.4 JobTrackerPro

**Kategorie:** Open-Source Application Tracker mit automatischer E-Mail-Ingestion  
**Lizenz:** MIT  
**Bereitstellung:** Full-Stack-Webanwendung  
**Relevanz für SASD:** Außerordentlich hoch als Referenz für ereignisgetriebene E-Mail-Erkennung.

### 4.4.1 Kernziel

JobTrackerPro versucht, die manuelle Pflege eines Bewerbungstrackers zu reduzieren. Statt jede Statusänderung per Hand einzutragen, werden Bewerbungs-E-Mails analysiert und der passende Jobdatensatz automatisch erstellt beziehungsweise aktualisiert.

### 4.4.2 Gmail OAuth2

Nutzer können ihr Gmail-Konto anbinden:

- OAuth2-Authentifizierung.
- autorisierter Zugriff auf relevante Mailereignisse.
- kein statisches Passwort als Integrationsprinzip.

### 4.4.3 Push-/Webhook-basierte E-Mail-Erkennung

Google Pub/Sub beziehungsweise Gmail-Push-Ereignisse werden genutzt:

- neue Mail erzeugt Event.
- Webhook-Service empfängt Benachrichtigung.
- relevante Nachricht wird in die Ingestion-Pipeline gegeben.
- dadurch ist keine dauernde Vollabfrage des Postfachs nötig.

### 4.4.4 Smart Routing

Nachrichten werden zunächst klassifiziert:

- bekannte Standardvorlagen erkennen.
- bei bekannten LinkedIn-/Indeed-Mustern lokalen Template Parser verwenden.
- unbekannte Nachrichten an KI-Extraktion weitergeben.
- im lokalen Entwicklungsmodus Mock-AI verwenden.

Damit werden kostengünstige deterministische Parser vor generativer KI priorisiert.

### 4.4.5 LinkedIn-/Indeed-Template-Parsing

Der Template Parser kann aus bekannten Nachrichtentypen extrahieren:

- Stellenbezeichnung.
- Unternehmen.
- Standort.
- Stellenlink.
- Statusinformation.

### 4.4.6 Weitergeleitete Nachrichten erkennen

JobTrackerPro rekonstruiert Forward-Header beziehungsweise erkennt weitergeleitete Mailstrukturen. Damit kann der Nutzer Bestätigungs-E-Mails an ein Sync-Postfach weiterleiten, ohne dass die Extraktion zwangsläufig an der Weiterleitung scheitert.

### 4.4.7 Body- und Subject-Fallback

Wenn eine Information nicht an der erwarteten Stelle steht:

- Body-Layout analysieren.
- alternativ Subject verwenden.
- Jobtitel und Unternehmen möglichst robust trennen.

### 4.4.8 URL-Bereinigung

Bei langen Tracking-/Redirect-URLs kann der Parser:

- Query-Parameter erkennen.
- Zielparameter wie `next=` auslesen.
- URL dekodieren.
- direkte Stellen-URL speichern.

### 4.4.9 Standortbereinigung

Extrahierte Standorttexte werden bereinigt, beispielsweise um Bewertungs- oder Metadaten zu entfernen, die nicht zum Standort gehören.

### 4.4.10 Schutz vor falscher Statusinterpretation

Sicherheitshinweise und Disclaimer in E-Mails können Schlüsselwörter wie „interview“ enthalten. JobTrackerPro filtert solche Texte, damit ein Warnsatz nicht versehentlich den Status „Interview Scheduled“ auslöst.

### 4.4.11 KI-Fallback

Für unbekannte Mailformate:

- Gemini-basierte Extraktion.
- bei ausgeschalteter KI lokaler Mock-Service für Entwicklung/Tests.
- strukturierte Übergabe des Extraktionsergebnisses an denselben Job-Service wie beim Template Parser.

### 4.4.12 Deduplizierung und Matching zum vorhandenen Job

Nach der Extraktion wird versucht, den passenden bestehenden Job zu finden:

- Vergleich von Unternehmen und Rolle.
- strengere Token-Regel für sehr kurze Firmennamen.
- Vermeidung von falschen Substring-Treffern.
- bestehende Bewerbung aktualisieren, wenn ein Match gefunden wird.
- neuen Datensatz anlegen, wenn kein aktiver Match existiert.

### 4.4.13 Automatische Statuspflege

Wird eine relevante Nachricht einer vorhandenen Bewerbung zugeordnet:

- Status kann anhand der E-Mail angepasst werden.
- E-Mail-Details werden an den Vorgang beziehungsweise dessen Notizen/Transaktionshistorie angehängt.

### 4.4.14 Dashboard

Die Anwendung bietet ein Dashboard mit:

- Echtzeit-/nahezu Echtzeit-Analytics.
- D3.js-Diagrammen.
- Statusverteilungen.
- Visualisierung auch kleiner Statusgruppen.

Eine spezielle Visualisierungslogik vergrößert sehr kleine Donut-Segmente optisch, ohne den im Tooltip gezeigten echten Zahlenwert zu verfälschen.

### 4.4.15 Profil- und Automationseinstellungen

Die Oberfläche dokumentiert einen Bereich für:

- Nutzerprofil.
- Automation.
- E-Mail-Forwarding-/Sync-Einrichtung.
- sichere Einstellungen für die Integrationen.

### 4.4.16 Security und Sessions

Die Anwendung demonstriert produktionsnahe Sicherheitsmechanismen:

- OAuth2.
- JWT-basierte Absicherung im Backend-Kontext.
- getrennte Backend-/Frontend-Architektur.
- sichere externe Speicherung von Objektdaten über Cloudflare R2 in der Cloud-Variante.

### 4.4.17 Caching und reaktive Aktualisierung

Für die sichtbare Produktwirkung relevant:

- Joblisten, Nutzerprofile und Dashboard-Analytics werden gecacht.
- Cache wird bei Jobänderungen invalidiert.
- Angular Signals aktualisieren die UI reaktiv.

### 4.4.18 Lokale Entwicklungs-/Testumgebung

Für lokale Tests stehen bereit:

- PostgreSQL per Docker.
- MailHog als E-Mail-Falle.
- Mock AI.
- Skript zur Simulation von E-Mail-Ingestion.

Das ist zwar eher Entwicklungsfunktion, aber für SASD als Referenz für **testbare Automatisierung ohne Live-Postfach** sehr relevant.

### 4.4.19 Besonders relevante Ideen für SASD

- Event-E-Mail als erstes-class Domain-Ereignis.
- deterministische Parser vor LLM-Fallback.
- vorhandene Bewerbung finden statt Duplikat erzeugen.
- externe Mailinformationen zunächst robust bereinigen.
- automatischer Status darf nur aus belastbarer Klassifikation entstehen.
- testbare Mailpipeline ohne produktives Postfach.

**Offizielle Quelle:**

- https://github.com/thughari/JobTrackerPro

---

## 4.5 JobTrail

**Kategorie:** Self-hosted Job Application Tracker mit Job Discovery und Company Enrichment  
**Relevanz für SASD:** Hoch für strukturierte Bewerbungsübersicht, mehrere Interviewrunden, lokale Skill-Extraktion und Unternehmensanreicherung.

### 4.5.1 Dashboard / Bewerbungsübersicht

JobTrail stellt alle Bewerbungen in einer sortierbaren Tabelle dar. Dokumentierte Spalten sind unter anderem:

- Unternehmen.
- Position.
- Status.
- Bewerbungsdatum.
- Jobtyp.
- Tags.

### 4.5.2 Sortierung

Die Bewerbungsübersicht kann nach ihren relevanten Spalten sortiert werden. Dadurch lässt sie sich sowohl chronologisch als auch nach Unternehmen, Status oder anderen Feldern verwenden.

### 4.5.3 Deadline-Anzeige

Deadlines werden sichtbar hervorgehoben:

- bevorstehende Deadline innerhalb einer Woche.
- bereits überschrittene Deadline.

### 4.5.4 Suche und Filter

JobTrail besitzt umfangreiche Filter:

- Freitextsuche über Unternehmen.
- Position.
- Stellenbeschreibung.
- Statusfilter.
- Tag-Filter mit exakter Übereinstimmung.
- Branchenfilter; mehrere Branchen können als OR kombiniert werden.
- Jobtypfilter.

### 4.5.5 Job manuell anlegen und CRUD

Bewerbungen können:

- angelegt.
- angezeigt.
- bearbeitet.
- gelöscht

werden.

### 4.5.6 Multi-round Interview Progress

JobTrail modelliert mehrere Interviewrunden statt eines einzigen „Interview“-Flags:

- Runden als Fortschrittskarte.
- visuelle Timeline.
- unterschiedliche Statussymbole je Runde.
- mehrere Schritte einer Interviewkette nachvollziehen.

### 4.5.7 Notizen je Interviewrunde

Für einzelne Interviewrunden können gespeichert werden:

- Notizen.
- Feedback.
- weitere rundenbezogene Informationen.

Zusätzlich gibt es freie Notizen auf Bewerbungsebene.

### 4.5.8 Stellenbeschreibung und Anforderungen

JobTrail speichert:

- vollständige Stellenbeschreibung.
- Anforderungen.
- dadurch bleibt der Inhalt auch nach dem Verschwinden der Ursprungsanzeige verfügbar.

### 4.5.9 Lokale Skill-Extraktion

Aus Stellenbeschreibungen werden lokal Skills erkannt und kategorisiert dargestellt. Das ermöglicht:

- technische Anforderungen schneller zu sehen.
- Jobprofile zu vergleichen.
- Skills als strukturierte Chips/Kategorien zu verwenden.

### 4.5.10 Activity Log

JobTrail führt eine einheitliche Aktivitätshistorie:

- Statusänderungen.
- freie Notizen.
- weitere Aktivitäten am Vorgang.

### 4.5.11 Job Discovery über JobSpy

Die Discovery-Seite durchsucht mehrere Portale, dokumentiert sind:

- LinkedIn.
- Indeed.
- Glassdoor.
- Google Jobs.
- ZipRecruiter.

### 4.5.12 Discovery-Filter

Bei der Stellensuche können eingestellt werden:

- Quellen.
- Suchbegriff.
- Standort.
- Zahl gewünschter Ergebnisse, dokumentiert bis in einen größeren Bereich.
- maximales Alter in Stunden.
- nur Remote.
- Jobtyp.
- Include Keywords.
- Exclude Keywords.
- Anzeige aktiver Filter.

### 4.5.13 Discovery Cache

Suchergebnisse werden kurzfristig gecacht, um unnötige wiederholte Abrufe zu vermeiden.

### 4.5.14 Import aus Discovery

Treffer können aus der Suchansicht in den Tracker übernommen werden. Dabei:

- Source und Quell-ID zur Wiedererkennung nutzen.
- Unternehmen anlegen beziehungsweise anreichern.
- bestehende Treffer möglichst nicht duplizieren.

### 4.5.15 Deduplizierung von Unternehmen

Unternehmensnamen werden normalisiert und mit Ähnlichkeitsverfahren verglichen, um Varianten desselben Unternehmens nicht unnötig zu vervielfachen.

### 4.5.16 Company Enrichment

Unternehmensinformationen können aus öffentlichen Quellen angereichert werden, dokumentiert sind:

- Wikipedia.
- Wikidata.
- SEC EDGAR.

Damit können Company Profiles über die Stellenanzeige hinaus aufgebaut werden.

### 4.5.17 Company Refresh und Caching

Zur Anreicherung existieren Mechanismen für:

- regelmäßiges Aktualisieren älterer Unternehmensdatensätze.
- Cache für externe Informationen.
- SEC-/Ticker-bezogene Zwischenspeicherung.

### 4.5.18 Falsche Company Matches korrigieren

Wenn eine automatische Unternehmenszuordnung falsch ist:

- falsches Unternehmen ablehnen.
- passende Wikidata-Alternative suchen.
- abgelehnte Zuordnungen persistent merken.

### 4.5.19 Import-Refresh ohne Verlust eigener Daten

Wird ein bereits bekannter Job erneut aus der Quelle importiert, können externe Felder aktualisiert werden, beispielsweise:

- URL.
- Gehalt.
- Standort.
- Beschreibung.

Eigene Daten wie:

- Status.
- Notizen

bleiben erhalten. Dieses Trennprinzip ist für unseren späteren Source-Import sehr wertvoll.

### 4.5.20 Datenreset / Danger Zone

Es gibt eine bewusste Funktion zum Zurücksetzen beziehungsweise Löschen des lokalen Datenbestands. Solche destruktiven Aktionen werden separat als „Danger Zone“ behandelt.

### 4.5.21 Local/Self-hosted

Die Anwendung ist lokal beziehungsweise self-hosted einsetzbar und benötigt nach Projektbeschreibung keine externe LLM-Funktion für ihren Kernbetrieb.

### 4.5.22 Besonders relevante Ideen für SASD

- externe Importdaten und eigene Bewerbungsdaten getrennt aktualisieren.
- mehrere Interviewrunden als eigene Entitäten.
- Company Enrichment mit korrigierbarer Zuordnung.
- lokale Skill-Extraktion.
- kombinierte Tabellen-/Filteransicht als Ergänzung zu Kanban.

**Offizielle Quelle:**

- https://github.com/kaylaehman/jobtrail

---

## 4.6 JobHunt

**Kategorie:** Open-Source Job Application Tracker  
**Relevanz für SASD:** Mittel; gute Referenz für einen kompakten, sauberen MVP.

### 4.6.1 Authentifizierung

JobHunt nutzt eine Benutzeranmeldung mit:

- E-Mail-Adresse.
- Passwort.
- Session-/Backend-Authentifizierung über Supabase.

### 4.6.2 Application CRUD

Kernfunktionen:

- Bewerbung anlegen.
- Bewerbung anzeigen.
- Bewerbung bearbeiten.
- Bewerbung löschen.

### 4.6.3 Kanban Board

Bewerbungen werden auf einem visuellen Board geführt:

- Statusspalten.
- Karten.
- Drag-and-drop zwischen Stufen.
- sofort sichtbarer Prozessfortschritt.

### 4.6.4 Job- und Unternehmensdaten

Zu einer Bewerbung können Informationen über:

- Unternehmen.
- Stellenbezeichnung.
- Jobdetails.
- Bewerbungsmetadaten

gespeichert werden.

### 4.6.5 Suche und Filter

Die Anwendung unterstützt:

- Suche nach Unternehmen.
- Suche nach Stellentitel.
- Filterung des Bewerbungsbestands.

### 4.6.6 Notizen und Metadaten

Bewerbungen können mit:

- umfangreicheren Notizen.
- zusätzlichen Metadaten

versehen werden.

### 4.6.7 Responsive UI

Die Oberfläche ist auf verschiedene Bildschirmgrößen ausgelegt:

- Desktop.
- Tablet.
- Mobile.

### 4.6.8 UI-Eigenschaften

Dokumentiert sind außerdem:

- Dark Mode.
- Animationen.
- moderner visueller Stil.
- Performanceorientierung.

### 4.6.9 Roadmap – ausdrücklich noch nicht als Ist-Funktion zu behandeln

Das Repository nennt weitere geplante Funktionen. Diese sind **nicht mit dem aktuellen Funktionsumfang gleichzusetzen**:

- Analytics Dashboard.
- erweiterte Suche.
- CSV-/PDF-Export.
- E-Mail-Benachrichtigungen.
- PWA-Funktionalität.

### 4.6.10 Besonders relevante Ideen für SASD

- gutes Beispiel für einen kleinen, verständlichen Tracker.
- klare Trennung zwischen existierender Funktion und Roadmap.
- responsive Kanban-UX als Vergleich, ohne daraus ein Muss für unseren Windows-Desktop-MVP abzuleiten.

**Offizielle Quelle:**

- https://github.com/kaitranntt/jobhunt

---

## 4.7 Jobtra

**Kategorie:** Self-hosted, privacy-focused Job Hunt Tracker  
**Technik:** SQLite, FastAPI, einfache HTML/JavaScript-Oberfläche  
**Relevanz für SASD:** Sehr hoch für lokale Bewerbungsakte, IMAP-Mailintegration und optionale KI.

### 4.7.1 Privacy-/Local-first-Grundprinzip

Jobtra ist auf lokale Datenhaltung ausgelegt:

- keine verpflichtenden Benutzerkonten.
- keine eigene Cloud als Voraussetzung.
- keine Telemetrie laut Projektbeschreibung.
- SQLite-Datenbank lokal.
- Self-hosting.

### 4.7.2 Bewerbungsverwaltung

Bewerbungen können vollständig angelegt und bearbeitet werden. Das Statusmodell umfasst dokumentiert mehrere Stufen, unter anderem:

- Open.
- Applied.
- Interview Invite.
- Interview Done.
- Rejected.
- Rejected after Interview.
- Accepted.

### 4.7.3 Umfangreiche Bewerbungsfelder

Zu einem Job können deutlich mehr Informationen gespeichert werden als nur Titel und Unternehmen, beispielsweise:

- Unternehmen.
- Position.
- Stadt.
- Adresse.
- HR-E-Mail.
- HR-Telefonnummer.
- WhatsApp.
- Telegram.
- Wochenstunden.
- Sprachen.
- Skills.
- Jobtyp.
- weitere Informationen aus der Ausschreibung.

### 4.7.4 KI-gestütztes Job Parsing

Eine Stellenanzeige kann:

- als Text eingefügt werden.
- über eine URL übernommen werden.

Ein konfiguriertes LLM versucht anschließend die strukturierten Felder automatisch zu füllen.

### 4.7.5 Reparse

Eine bereits gespeicherte Stelle kann anhand der ursprünglichen Quelle erneut geparst werden. Damit kann eine verbesserte Parsing-Logik oder eine aktualisierte Quelle später erneut ausgewertet werden.

### 4.7.6 Bookmarklet

Jobtra stellt ein Bookmarklet bereit:

- aktuelle Browserseite aufrufen.
- sichtbaren Inhalt erfassen.
- an Jobtra zum Anlegen/Parsen übergeben.

Das ist ein sehr einfacher alternativer Capture-Mechanismus zu einer komplexen Browser-Erweiterung.

### 4.7.7 Dashboard

Das Dashboard enthält Kennzahlen wie:

- Gesamtzahl der Bewerbungen.
- aktive Pipeline.
- Zahl der Interviews.
- Rejection Rate.
- Auswertung nach Monat.
- Status.
- Jobtyp.
- Stadt.
- Position.

### 4.7.8 Kartenansicht

Standorte können auf einer interaktiven Karte dargestellt werden:

- Geocoding von Orten/Adressen.
- OpenStreetMap-basierte Darstellung.
- geografischer Vergleich von Bewerbungen.

### 4.7.9 Treemap

Zusätzlich gibt es eine Treemap-Visualisierung für Bewerbungsdaten beziehungsweise Kategorien.

### 4.7.10 Dokumentmanager

Jobtra besitzt eine integrierte Dokumentverwaltung:

- Lebensläufe hochladen.
- Anschreiben hochladen.
- Dokumente einzelnen Bewerbungen zuordnen.
- anzeigen, wo ein Dokument verwendet wurde.
- Content-Hash zur Erkennung doppelter Dokumente.

### 4.7.11 IMAP-E-Mail-Synchronisierung

Jobtra kann ein oder mehrere E-Mail-Konten per IMAP anbinden:

- mehrere Accounts.
- E-Mails abrufen.
- Bewerbungsbezug analysieren.
- Synchronisierung manuell auslösen.
- konfigurierbare periodische Synchronisierung.

### 4.7.12 E-Mail-Klassifikation

Ein LLM bewertet eingehende Nachrichten:

- ist die Nachricht jobbezogen?
- welche Bedeutung hat sie?
- Absage.
- Interview.
- Angebot.
- andere relevante Bewerbungsnachricht.

### 4.7.13 Automatische Zuordnung zu Bewerbungen

Nach der Klassifikation versucht Jobtra:

- die passende bestehende Bewerbung zu finden.
- E-Mail dort zu verknüpfen.
- bei hinreichend klarer Information den Bewerbungsstatus weiterzuentwickeln.

### 4.7.14 Schutz von Mail-Credentials

IMAP-Passwörter werden laut Dokumentation verschlüsselt beziehungsweise mit Fernet at rest geschützt.

### 4.7.15 Getrennte KI-Konfiguration nach Aufgabe

Es können unterschiedliche Provider/Modelle für verschiedene Workflows verwendet werden, insbesondere:

- Parsing von Stellenanzeigen.
- Klassifikation von E-Mails.

### 4.7.16 KI-Provider

Dokumentiert sind:

- Ollama lokal.
- LM Studio lokal.
- Anthropic.
- OpenAI.

KI ist optional für Teile des Systems; das lokale Tracking selbst bleibt davon trennbar.

### 4.7.17 Import und Export

Jobtra unterstützt Bulk-Datenaustausch:

- CSV-Import.
- JSON-Import.
- tolerante Spaltenalias-Erkennung.
- verschiedene Statusvokabulare beim Import zuordnen.
- Duplikaterkennung.
- CSV-Export.

### 4.7.18 Pagination

Die Zahl der angezeigten Jobs beziehungsweise E-Mails pro Seite ist konfigurierbar.

### 4.7.19 UI

Dokumentiert sind:

- Dark Mode.
- Light Mode.
- responsive Oberfläche.
- keine zwingende Frontend-Build-Pipeline für den normalen Betrieb.

### 4.7.20 Betrieb und Backup

Jobtra kann:

- per Docker betrieben werden.
- manuell gestartet werden.
- über bereitgestellte Launcher verwendet werden.
- lokale Backups ermöglichen beziehungsweise den Datenbestand lokal sichern.

### 4.7.21 Besonders relevante Ideen für SASD

- eine sehr einfache Capture-Alternative über Bookmarklet.
- E-Mail-Klassifikation getrennt vom Stellen-Parsing konfigurieren.
- konkrete Dokumentnutzung an Bewerbungen nachverfolgen.
- externe Kommunikationsdaten automatisch zuordnen, ohne den gesamten Mailclient nachzubauen.

**Offizielle Quelle:**

- https://github.com/CU1KNIGHT/Jobtra

---

## 4.8 JobNest

**Kategorie:** Privacy-first Local-first Desktop Job Application Tracker  
**Technik:** Tauri/Next.js  
**Status:** frühe Entwicklung  
**Relevanz für SASD:** Mittel bis hoch als UX- und Architekturvergleich für einen ruhigen lokalen Desktop-Tracker.

### 4.8.1 Lokaler Desktopbetrieb

JobNest verfolgt ein lokales Desktopmodell:

- native Desktop-Anwendung über Tauri.
- kein zwingender Webservice als Kern.
- lokale Datenhaltung.
- kein Accountzwang.
- keine Cloudvoraussetzung.
- kein Tracking/keine Telemetrie nach Projektbeschreibung.

### 4.8.2 Bewerbungs-Pipeline

Kernfunktion ist eine lokale Bewerbungs-Pipeline:

- Bewerbungen erfassen.
- Bewerbungsstatus beziehungsweise Stufen verwalten.
- Verlauf einer Bewerbung nachvollziehen.

### 4.8.3 Unternehmen und Rollen

JobNest verwaltet Informationen zu:

- Unternehmen.
- Rollen/Stellen.
- den zugehörigen Bewerbungen.

### 4.8.4 Notizen und Links

Zu Jobs/Bewerbungen lassen sich:

- Notizen.
- relevante Links.
- ergänzende Kontextinformationen

speichern.

### 4.8.5 Deadlines und Follow-ups

Die Anwendung soll beziehungsweise dokumentiert im Kern:

- Deadlines verwalten.
- Follow-up-Termine festhalten.
- offene Bewerbungen übersichtlich halten.

### 4.8.6 Interview- und Follow-up-Notizen

Zu späteren Bewerbungsphasen können Notizen für:

- Interviews.
- Follow-up.
- Gesprächsergebnisse

erfasst werden.

### 4.8.7 Lokale Historie

Der Verlauf bleibt lokal gespeichert und dient als Bewerbungsarchiv beziehungsweise History.

### 4.8.8 Suche und Filter

Aus der Projektbeschreibung beziehungsweise Entwicklerdokumentation geht die Zielrichtung hervor, Bewerbungen zu durchsuchen und zu filtern. Da das Projekt noch jung ist, muss bei jedem konkreten Build geprüft werden, welche Detailfilter bereits implementiert sind.

### 4.8.9 Export

Der Entwickler beschreibt Exportmöglichkeiten in Richtung CSV/Excel beziehungsweise Datenportabilität. Wegen des frühen Projektstatus sollte auch hier versionsbezogen geprüft werden, ob eine konkrete Build-Version diese Funktion bereits vollständig enthält.

### 4.8.10 UX-Ziele

JobNest ist vor allem als Designreferenz interessant:

- einfache ruhige Oberfläche.
- Fokus auf den eigentlichen Bewerbungsvorgang.
- geringer Funktionsballast.
- schnelle lokale Desktop-Interaktion.
- Privacy-by-design.

### 4.8.11 Besonders relevante Ideen für SASD

- lokale Desktop-App muss nicht altmodisch wirken.
- Privacy-first als sichtbares Produktmerkmal.
- ruhige UX statt „AI everywhere“.
- Datenexport als Bestandteil von Datenhoheit.

**Offizielle Quelle:**

- https://github.com/maerzhase/jobnest

---
# 5. CRM- und Personal-CRM-Referenzen

## 5.1 Pipedrive

**Kategorie:** Sales CRM  
**Bereitstellung:** Web, Android, iOS; zahlreiche Integrationen  
**Relevanz für SASD:** Außerordentlich hoch für Pipeline, Next Action, Aktivitäten, Kontaktmodell, Timeline, Kommunikation und Automationen.

Pipedrive ist kein Bewerbungsmanager. Gerade deshalb ist es wertvoll: Ein Bewerbungsprozess ähnelt in vielen Punkten einem persönlichen CRM-Prozess. Der zentrale Gedanke lautet nicht nur „In welcher Phase befindet sich der Vorgang?“, sondern ebenso **„Welche Aktivität bringt ihn als Nächstes voran?“**

### 5.1.1 Pipelines und Stufen

Pipedrive verwaltet Vorgänge in Pipelines:

- mehrere Pipelines.
- definierte Stufen je Pipeline.
- visuelle Darstellung von Deals/Vorgängen.
- Verschieben von Vorgängen zwischen Stufen.
- Anpassung der Kartenansicht je nach Tarif.
- Pipeline-spezifische Felder in höheren Tarifen.
- Prognose-/Forecast-Ansichten in entsprechenden Tarifen.

Für den Bewerbungsmanager ist das ein Vorbild für eine Pipeline, die **nicht das gesamte Datenmodell ersetzt**, sondern nur eine Sicht auf einen reichhaltigeren Vorgang ist.

### 5.1.2 Leads und Lead Inbox

Pipedrive trennt frühe Leads von bereits qualifizierten Deals:

- Lead Inbox.
- Leads sammeln.
- später in einen Deal/Vorgang überführen.
- Lead-Daten pflegen.

Übertragen auf Bewerbungen wäre dies vergleichbar mit „interessante Stelle gefunden“ versus „aktive Bewerbung gestartet“.

### 5.1.3 Personen und Organisationen

CRM-Kontakte werden als eigene Objekte geführt:

- Personen.
- Organisationen.
- Beziehungen zwischen Kontakten und Vorgängen.
- Kontaktdaten.
- benutzerdefinierte Felder.
- Datenanreicherung je nach Tarif.
- Dubletten zusammenführen.
- Kontakt-Timeline in entsprechenden Tarifen.
- Kontakte auf einer Karte darstellen.

### 5.1.4 Custom Fields

Pipedrive ermöglicht an vielen Objekten benutzerdefinierte Felder:

- eigene Datentypen/Felder.
- wichtige Felder markieren.
- in höheren Tarifen Required Fields.
- Formula Fields.
- pipeline-spezifische Felder.
- Auswertungen über Custom Fields in entsprechenden Tarifen.

Dies ist ein starkes Vorbild dafür, nicht jede branchenspezifische Information fest in das Kernschema einbauen zu müssen.

### 5.1.5 Aktivitäten

Aktivitäten sind eine der wichtigsten Pipedrive-Funktionen. Dokumentierte Standardtypen umfassen:

- Anrufe.
- Meetings.
- Aufgaben.
- E-Mails.

Aktivitäten können mit mehreren CRM-Objekten verbunden werden:

- Person.
- Organisation.
- Lead.
- Deal.
- Projekt.

### 5.1.6 Aktivitäten anlegen und planen

Aktivitäten können aus verschiedenen Kontexten erzeugt werden:

- Pipeline-Ansicht.
- Detailansicht.
- Lead Inbox.
- Kalender.
- Activity List.
- Kontaktansicht.
- Mobile App.
- E-Mail-Kontext.
- Bulk-Aktionen.

Sie besitzen Termin-/Planungsinformationen und können als erledigt markiert beziehungsweise gelöscht werden.

### 5.1.7 Next-Action-Prinzip

Pipedrive benutzt Aktivitäten praktisch als nächste Handlungsstufe des Vorgangs. Ein Deal ohne geplante Aktivität ist organisatorisch schlechter geführt als ein Vorgang mit klarer nächster Aktion.

Für SASD ist daraus direkt ableitbar:

- Bewerbung besitzt Status.
- Bewerbung besitzt zusätzlich eine geplante nächste Aktivität.
- „Warten“ sollte idealerweise ebenfalls ein bewusst gesetztes Wiedervorlagedatum besitzen.

### 5.1.8 Kalender und Calendar Sync

Pipedrive bietet:

- Kalenderansicht.
- Aktivitätenkalender.
- Synchronisation mit Google Calendar.
- Synchronisation mit Microsoft-Kalendern.
- Termine aus CRM und externem Kalender zusammenführen.

### 5.1.9 Meeting Scheduler

Der Meeting Scheduler ermöglicht:

- eigene Verfügbarkeit definieren.
- Terminlinks teilen.
- Kontakte einen passenden Termin buchen lassen.
- gebuchte Meetings als Aktivitäten übernehmen.

### 5.1.10 Sales Inbox und E-Mail-Synchronisation

In unterstützten Tarifen können E-Mail-Konten synchronisiert werden:

- E-Mails innerhalb von Pipedrive anzeigen.
- E-Mails senden.
- antworten.
- Konversationen mit Leads/Deals/Projekten verbinden.
- Zuordnung automatisch oder manuell durchführen.
- persönliches oder Team-Postfach anbinden.
- E-Mail-Konversationen intern teilen oder privat halten.

### 5.1.11 Smart BCC

Auch ohne vollständige Mail-Synchronisierung kann Smart BCC verwendet werden, um relevante E-Mail-Kommunikation in Pipedrive zu protokollieren.

Das ist als einfacheres Integrationsmuster für SASD interessant: Nicht jede Mailintegration muss sofort ein vollständiger IMAP-/Graph-/Gmail-Sync sein.

### 5.1.12 E-Mail-Vorlagen und Signaturen

Je nach Tarif:

- E-Mail-Vorlagen.
- Signaturen.
- wiederverwendbare Nachrichtentexte.

### 5.1.13 E-Mail-Tracking

Dokumentiert sind:

- Öffnungstracking.
- Klicktracking.
- Benachrichtigungen über Engagement.

Für einen Bewerbungsmanager wäre dies datenschutz- und ethisch besonders sorgfältig zu bewerten und keinesfalls automatisch zu übernehmen.

### 5.1.14 E-Mail-Planung und Group Emailing

Tarifabhängig:

- E-Mails zeitversetzt senden.
- Gruppen-/Massen-E-Mails.

Auch dies ist im Bewerbungsbereich nur eingeschränkt relevant, weil der SASD Manager nicht zu einem Massenbewerbungswerkzeug werden sollte.

### 5.1.15 E-Mail als Aktivität

Gesendete E-Mails können automatisch als Aktivität protokolliert werden. Dadurch fließen Kommunikationsereignisse in Aktivitätsauswertungen ein und müssen nicht zusätzlich manuell als „E-Mail gesendet“ erfasst werden.

### 5.1.16 AI E-Mail-Funktionen

In höheren Tarifen dokumentiert:

- KI-gestützte E-Mail-Erstellung.
- KI-Zusammenfassung von E-Mail-Konversationen.

### 5.1.17 Mentions und Kommentare

Innerhalb von Vorgängen können Teammitglieder:

- Kommentare hinterlassen.
- andere Nutzer erwähnen.

Für einen persönlichen Bewerbungsmanager ist Teamkollaboration zunächst nicht nötig, das Aktivitäts-/Kommentarprinzip ist jedoch als Audit-Historie interessant.

### 5.1.18 Dateianhänge

Dateien können an CRM-Objekten gespeichert beziehungsweise angehängt werden. Für SASD ist dies analog zu:

- Stellenanzeige.
- Lebenslauf.
- Anschreiben.
- Einladung.
- Vertragsentwurf.

### 5.1.19 Benachrichtigungen

Pipedrive erzeugt Benachrichtigungen für relevante Vorgänge und Aktivitäten. In einem Bewerbungsmanager wären insbesondere fällige und überfällige Aktivitäten wichtig.

### 5.1.20 Automationen

Pipedrive bietet Workflow-Automationen, tarifabhängig mit:

- Triggern.
- Aktionen.
- Delay-/Warte-Schritten.
- If/Else-Verzweigungen.
- datumsgesteuerten Triggern.
- Reaktion auf Änderungen von Deals, Aktivitäten, Organisationen oder Kontakten.
- automatischer Zuweisung in entsprechenden Tarifen.

### 5.1.21 Automation Monitoring

Ein eigenes Automation Overview zeigt:

- Aktivität von Automationen.
- Erfolg beziehungsweise Fehler.
- Betrachtung unterschiedlicher Zeitfenster.
- zentrale Fehleranalyse.

Dies ist für SASD wichtig, falls später automatische Mail-/Statusregeln eingeführt werden: **Automationen müssen beobachtbar und nachvollziehbar sein.**

### 5.1.22 Sequences

Tarifabhängig können wiederholbare Kontakt-/Aktivitätssequenzen definiert werden. Übertragen könnte dies beispielsweise ein Follow-up-Muster sein, sollte aber bei Bewerbungen bewusst dezent bleiben.

### 5.1.23 Pulse Toolkit / Pulse Feed

Pipedrive bündelt Funktionen, die Aufmerksamkeit auf wichtige beziehungsweise priorisierte Verkaufschancen lenken. Das zugrunde liegende Muster ist für ein Bewerbungsdashboard interessant: Nicht alle offenen Vorgänge sind gleich wichtig.

### 5.1.24 Insights Reports

Pipedrive besitzt ein umfangreiches Reporting-System. Berichte können unter anderem:

- Aktivitäten analysieren.
- E-Mail-Performance analysieren.
- Deal-/Pipeline-Performance darstellen.
- Trends über Zeit zeigen.
- benutzerdefinierte Felder auswerten, tarifabhängig.

### 5.1.25 Dashboards

Insights-Dashboards bündeln Berichte:

- mehrere Visualisierungen.
- verschiedene Kennzahlen.
- mehrere Dashboards in höheren Tarifen.
- Freigabe/Zusammenarbeit abhängig von Berechtigungen.
- mobile Einsicht.

### 5.1.26 Goals

Pipedrive kann Ziele definieren und Fortschritt dagegen messen. Für SASD könnte dies später etwa für Aktivitätsziele dienen, sollte jedoch nicht in eine fragwürdige „Bewerbungsquote um jeden Preis“ führen.

### 5.1.27 KI-gestützte Berichterstellung

Aktuelle Tarife dokumentieren KI-gestützte Report-Erstellung. Das Prinzip wäre für natürliche Fragen an den eigenen Bewerbungsdatenbestand interessant, beispielsweise „Welche Quellen führen bei mir am häufigsten zu Interviews?“

### 5.1.28 Produktkatalog und wiederkehrende Produkte

Pipedrive besitzt vertriebsspezifisch:

- Produktkatalog.
- wiederkehrende Produkte.
- Ratenzahlungen.

Diese Funktionen sind für den Bewerbungsmanager fachlich kaum relevant, zeigen aber, dass ein CRM Deal und „verkaufte Leistung“ getrennt modelliert.

### 5.1.29 Smart Docs

Tarif-/Add-on-abhängig:

- Dokumenterstellung.
- Vorlagen.
- Dokumente im Deal-/Kontaktkontext.

Das Muster ist auf Bewerbungsunterlagen übertragbar, nicht zwingend die konkrete Implementierung.

### 5.1.30 Projects

Pipedrive kann über Add-on beziehungsweise höhere Tarife Projektmanagementfunktionen anbinden. Für unseren MVP ist ein separates Projektmodul nicht erforderlich.

### 5.1.31 LeadBooster

LeadBooster erweitert Pipedrive um vertriebsorientierte Leadgewinnung. Für SASD nicht direkt relevant; es zeigt lediglich die Trennung von Akquisequelle und CRM-Kern.

### 5.1.32 Datenanreicherung und Scoring

In höheren Tarifen:

- Contact Data Enrichment.
- Organization Data Enrichment.
- Custom Scoring.

Für Bewerbungen könnte später eine transparente eigene Prioritätsbewertung sinnvoll sein, aber ohne undurchsichtige Blackbox-Scores.

### 5.1.33 API und Webhooks

Pipedrive stellt bereit:

- API-Zugriff.
- Webhooks.
- Marketplace mit hunderten Integrationen.

Dies ist eine Referenz für spätere Erweiterbarkeit des SASD Bewerbungsmanagers.

### 5.1.34 Google-/Microsoft-Kontaktsynchronisation

Kontakte können mit Google und Microsoft synchronisiert werden.

### 5.1.35 Mobile Apps

Die aktuellen mobilen Apps dokumentieren unter anderem:

- Focus View.
- Nearby View.
- Pipeline View.
- Aktivitätsliste.
- Kalender.
- Kontaktliste.
- Filter.
- E-Mail-Sync-Widget.
- Offline-Modus.
- Detailansichten von Deals und Kontakten.
- Anrufe initiieren und protokollieren.
- Caller ID.
- SMS/Texting.
- Statistik-Dashboard.
- Telefonkontakte importieren.
- Audio-Notizen.
- Foto-Upload.
- Datei-Upload.
- Push-Benachrichtigungen.
- Android: Business-Card-Scanner.

### 5.1.36 Security und Governance

Dokumentiert sind je nach Tarif beziehungsweise allgemein:

- 2FA.
- SSO.
- Sichtbarkeitsoptionen.
- Sicherheits-Dashboard.
- Gerätehistorie.
- Audit Log.
- Custom Permission Sets.
- Visibility Groups.
- Security Alerts/Rules in hohen Tarifen.

### 5.1.37 Pipedrive Nova – eingeschränkt verfügbare Funktion

Pipedrive dokumentiert 2026 „Nova“ für ausgewählte Nutzer:

- Meeting-Briefing aus CRM, E-Mail, Kalender und Aktivitäten.
- Meeting-Transkription.
- Nachbereitung beziehungsweise Follow-up-Aktionen.
- Consent-Management der Teilnehmer.

Da Nova nur für ausgewählte Nutzer beziehungsweise eingeschränkt verfügbar ist, ist sie als **Preview/selektive Funktion** und nicht als universelle Kernfunktion zu behandeln.

### 5.1.38 Besonders relevante Ideen für SASD

- Next Action als elementare Eigenschaft eines aktiven Vorgangs.
- Aktivität ist etwas anderes als Status.
- Person, Organisation und Vorgang getrennt modellieren.
- E-Mail kann automatisch Aktivität werden.
- Automationen brauchen Monitoring und Fehlertransparenz.
- frei definierbare Felder und Views können spätere Erweiterung erleichtern.

**Offizielle Quellen:**

- https://www.pipedrive.com/
- https://support.pipedrive.com/de/article/activities
- https://support.pipedrive.com/en/article/email-sync
- https://support.pipedrive.com/en/article/what-features-do-the-pipedrive-plans-have
- https://support.pipedrive.com/en/article/insights-feature

---

## 5.2 Dex

**Kategorie:** Personal CRM  
**Bereitstellung:** Web/Apps und Integrationen  
**Relevanz für SASD:** Sehr hoch für Recruiter-/Ansprechpartnergedächtnis und Beziehungshistorie.

### 5.2.1 Zentrale Kontaktbasis

Dex sammelt persönliche und berufliche Kontakte in einer zentralen Datenbank. Ziel ist nicht nur Adressverwaltung, sondern die Pflege langfristiger Beziehungen.

### 5.2.2 Kontakte importieren

Dokumentiert sind Importe beziehungsweise Integrationen aus verschiedenen Quellen, darunter je nach Plattform und Tarif:

- LinkedIn.
- Gmail/Google Contacts.
- Outlook/Microsoft.
- iCloud.
- weitere Social-/Kontaktdienste.

### 5.2.3 LinkedIn-Synchronisierung

LinkedIn-Daten können genutzt werden, um Kontakte und Veränderungen aktuell zu halten. Dokumentiert ist unter anderem die automatische Aktualisierung von Jobtiteln beziehungsweise beruflichen Veränderungen.

### 5.2.4 E-Mail-Synchronisierung

Mit Gmail und – tarifabhängig – Outlook können Interaktionen automatisiert erkannt werden:

- E-Mail-Kontakt als Interaktion protokollieren.
- letzte Kontaktzeit aktualisieren.
- Beziehungshistorie ergänzen.

### 5.2.5 Kalender-/Meeting-Synchronisierung

Meetings können anhand von Teilnehmer-E-Mail-Adressen Kontakten zugeordnet werden. Dadurch fließen Kalendereinträge in die Beziehungshistorie ein.

### 5.2.6 Contact Timeline

Für einen Kontakt kann eine gemeinsame Timeline entstehen, beispielsweise aus:

- E-Mails.
- Meetings.
- manuell protokollierten Telefonaten.
- Notizen.
- weiteren Interaktionen.

### 5.2.7 Keep-in-touch Reminders

Dex erinnert daran, bestimmte Personen wieder zu kontaktieren:

- individuelle Reminder.
- Kontaktfrequenz.
- Erinnerung auf Basis des letzten Kontakts.
- Wiedervorlage für Beziehungen.

Das ist unmittelbar auf Recruiter und Netzwerkpartner übertragbar.

### 5.2.8 Notizen

Zu Kontakten können freie Notizen gespeichert werden, damit wichtige Gesprächsinformationen erhalten bleiben.

### 5.2.9 Meetings, Calls und Interaktionen manuell protokollieren

Über die Plattform beziehungsweise die dokumentierte MCP-Integration können Interaktionen wie:

- Meetings.
- Anrufe.
- Notizen

protokolliert werden.

### 5.2.10 Tags und Gruppen

Kontakte können gruppiert beziehungsweise mit Tags strukturiert werden. Für SASD wären beispielsweise denkbar:

- Recruiter.
- Personalvermittlung.
- ehemaliger Kollege.
- Fachkontakt.
- Firma X.

### 5.2.11 Custom Fields

Dex unterstützt benutzerdefinierte Felder, um Informationen über das Standard-Kontaktschema hinaus zu speichern.

### 5.2.12 Duplikate zusammenführen

Wenn dieselbe Person über mehrere Quellen importiert wird, können doppelte Kontakte zusammengeführt werden.

### 5.2.13 Such- und Schnellaktionen

Dex besitzt schnelle Navigations-/Befehlsfunktionen, unter anderem über eine Command Palette. Typische Aktionen:

- Kontakt anlegen.
- Meeting protokollieren.
- Reminder erstellen.
- Gruppen erzeugen.
- Notizen verwalten.
- Views aufrufen.

### 5.2.14 MCP-/AI-Zugriff

Die dokumentierte MCP-Funktion ermöglicht kompatiblen KI-Systemen unter anderem:

- Kontakte suchen.
- Kontakte anlegen.
- Kontakte aktualisieren.
- Kontakte löschen.
- Meetings/Calls/Notizen loggen.
- Reminder/Tasks verwalten.
- Tags und Gruppen nutzen.
- Custom Fields bearbeiten.
- Duplikate zusammenführen.

Für SASD ist der wesentliche Gedanke, dass ein Agent nicht nur freien Text erzeugt, sondern **klar definierte CRM-Fähigkeiten** erhält.

### 5.2.15 Besonders relevante Ideen für SASD

- derselbe Recruiter kann über Jahre und mehrere Bewerbungen hinweg relevant bleiben.
- „letzter Kontakt“ und „nächste Kontaktpflege“ gehören zum Kontakt, nicht zwingend zu einer einzelnen Bewerbung.
- E-Mail- und Kalenderereignisse können Beziehungshistorie automatisch ergänzen.
- Dublettenmanagement ist bei importierten Kontakten unverzichtbar.

**Offizielle Quellen:**

- https://getdex.com/
- https://getdex.com/features

---

## 5.3 Monica

**Kategorie:** Open-Source Personal Relationship Manager / Personal CRM  
**Lizenz:** AGPL  
**Bereitstellung:** Self-hosted beziehungsweise gehostete Variante  
**Relevanz für SASD:** Hoch als Datenmodellreferenz für Personen, Beziehungen, Erinnerungen und private Notizen.

### 5.3.1 Kontakte

Monica verwaltet Personen als zentrale Objekte:

- Kontaktdatensätze.
- Namen und persönliche Angaben.
- verschiedene Kontaktmethoden.
- Adressen.
- zusätzliche Felder.

### 5.3.2 Beziehungen zwischen Personen

Monica kann Beziehungen modellieren, sodass Kontakte nicht nur isoliert nebeneinander stehen. Für den Bewerbungsmanager könnte dieses Prinzip später für Beziehungen zwischen Recruiter, Unternehmen und weiteren Ansprechpartnern nützlich sein.

### 5.3.3 Erinnerungen

Funktionen:

- individuelle Erinnerungen.
- automatische Geburtstagserinnerungen.
- chronologische Verwaltung wiederkehrender persönlicher Ereignisse.

### 5.3.4 Notizen

Zu Kontakten können Notizen hinterlegt werden. In der stabilen 4.x-Reihe wurden Markdown-Notizen dokumentiert.

### 5.3.5 „How we met“ / Kennenlernkontext

Monica kann speichern, wie beziehungsweise wann man eine Person kennengelernt hat. Dieses Detail ist für Personal CRM bemerkenswert und für Bewerbungen übertragbar als:

- erster Kontakt über LinkedIn.
- auf Messe kennengelernt.
- Vermittlerkontakt.
- frühere Bewerbung.

### 5.3.6 Aktivitäten

Gemeinsame beziehungsweise kontaktbezogene Aktivitäten können protokolliert werden. Aktivitätstypen sind anpassbar.

### 5.3.7 Aufgaben

Zu Beziehungen/Kontakten können Aufgaben geführt werden, sodass aus einer Notiz eine konkrete nächste Handlung entstehen kann.

### 5.3.8 Kontaktmethoden und Feldtypen

Monica unterstützt unterschiedliche Contact Field Types, statt nur eine feste E-Mail-/Telefonstruktur vorzusehen.

### 5.3.9 Haustiere

Monica modelliert sogar Haustiere als Teil persönlicher Beziehungserinnerung. Für SASD nicht fachlich relevant, zeigt aber die Flexibilität eines reichhaltigen Kontaktmodells.

### 5.3.10 Journal / Tagebuch

Es gibt Tagebuch-/Journal-Funktionen:

- persönliche Einträge.
- Tagesverlauf.
- Stimmung/Mood beziehungsweise Tagesbewertung.
- Filter in neueren stabilen Versionen.

Für SASD ist nur das Muster einer getrennten persönlichen Reflexion interessant, etwa Interview-Eindruck unabhängig von objektiven Fakten.

### 5.3.11 Dateien und Fotos

Kontakten können:

- Dokumente.
- Fotos

zugeordnet werden.

### 5.3.12 Benutzerdefinierte Geschlechter und Aktivitätstypen

Monica erlaubt bestimmte Taxonomien zu konfigurieren:

- benutzerdefinierte Gender-Bezeichnungen.
- eigene Activity Types.

Das zeigt ein generelles Schema: Domänenvokabular sollte bei geeigneten Stellen konfigurierbar sein.

### 5.3.13 Favoriten

Wichtige Kontakte können als Favoriten markiert werden.

### 5.3.14 Vaults und mehrere Nutzer

Neuere Monica-Entwicklung arbeitet mit Vaults beziehungsweise getrennten Datenräumen und unterstützt mehrere Nutzerkontexte. Für einen persönlichen SASD-MVP ist dies nicht notwendig, aber als Privacy-/Mandantentrennungsreferenz interessant.

### 5.3.15 Labels

Kontakte können über Labels organisiert werden.

### 5.3.16 Anpassbare Kontaktansicht

Bestimmte Bereiche/Sections eines Contact Sheets können angepasst werden.

### 5.3.17 Währungen und Internationalisierung

Monica unterstützt verschiedene Währungen und zahlreiche Sprachen; das Projekt nennt rund zwei Dutzend Übersetzungen. Dies ist eher eine Plattform- als eine Kernfunktion.

### 5.3.18 vCard-/DAV-nahe Funktionen

Stabile Releases dokumentieren unter anderem:

- vCard-Export.
- einzelne Kontakte als vCard.
- DAV-bezogene Abonnements/Adressbuchgruppen beziehungsweise Integrationen je Version.
- LDAP-Import in bestimmten Versionen.

### 5.3.19 Datenschutz und Open Source

Monica positioniert sich bewusst als privater Relationship Manager:

- Self-hosting möglich.
- Open Source.
- keine Ausrichtung auf öffentliches Social Networking.
- persönliche Daten bleiben im kontrollierten System.

### 5.3.20 Besonders relevante Ideen für SASD

- Kontakt ist langfristiges Wissensobjekt.
- Beziehungen und Aktivitäten gehören strukturiert zur Person.
- „wie kennengelernt“ ist für Recruiter/Netzwerk tatsächlich nützlich.
- persönlicher Eindruck und objektive Kontaktdaten sollten trennbar bleiben.

**Offizielle Quelle:**

- https://github.com/monicahq/monica

---
# 6. Aufgaben- und Work-Management-Referenzen

## 6.1 Todoist

**Kategorie:** Task Manager / Personal & Team Productivity  
**Bereitstellung:** Web, Desktop, Android, iOS, Wearables, Browser-Erweiterungen, E-Mail-Add-ons  
**Relevanz für SASD:** Sehr hoch für Aufgabenmodell, Fälligkeiten, Reminder, Prioritäten, Filter und schnelle Erfassung.

### 6.1.1 Schnelle Aufgabenerfassung

Todoist ist besonders auf geringe Erfassungshürde optimiert:

- Quick Add/Schnelleingabe.
- natürliche Sprache für Datum und Zeit.
- Priorität direkt beim Erfassen.
- Labels direkt beim Erfassen.
- Projektzuordnung direkt beim Erfassen.
- wiederkehrende Termine in natürlicher Sprache.

Für den Bewerbungsmanager ist dies ein wichtiges UX-Vorbild: Ein Gedanke wie „Freitag Samuel zurückrufen“ sollte mit möglichst wenig Bedienaufwand als Aufgabe erfasst werden können.

### 6.1.2 Aufgaben

Eine Todoist-Aufgabe kann unter anderem enthalten:

- Titel.
- Beschreibung.
- Datum.
- Uhrzeit.
- Deadline.
- Dauer.
- Priorität.
- Labels.
- Projekt.
- Abschnitt.
- Kommentare.
- Unteraufgaben.
- Dateianhänge.
- Erinnerungen.
- Beauftragte Person in geteilten Projekten.

### 6.1.3 Beschreibungen

Aufgabenbeschreibungen unterstützen:

- ergänzenden Text.
- Formatierung.
- Links.
- Anhänge.

Damit bleibt der Titel kurz, während Kontext separat gespeichert wird.

### 6.1.4 Datum versus Deadline

Todoist unterscheidet inzwischen explizit:

- **Datum:** wann man an einer Aufgabe arbeiten beziehungsweise sie einplanen möchte.
- **Deadline:** fester spätester Abschlusszeitpunkt einer einmaligen Aufgabe.

Diese Trennung ist für Bewerbungen sehr interessant. Beispiel:

- 26.08.: mit Interviewvorbereitung beginnen.
- Deadline 01.09.: Vorbereitung muss vor dem Interview abgeschlossen sein.

### 6.1.5 Wiederkehrende Termine

Aufgaben können regelmäßig wiederkehren:

- täglich.
- wöchentlich.
- monatlich.
- benutzerdefinierte Wiederholungsmuster.

### 6.1.6 Dauer und Time Blocking

In entsprechenden Tarifen:

- geschätzte Aufgabendauer hinterlegen.
- Aufgaben mit Datum/Zeit in einen Zeitblock einplanen.
- Kalenderansicht für Planung verwenden.

### 6.1.7 Erinnerungen

Todoist unterstützt verschiedene Reminder-Arten:

- automatische Erinnerung bei Aufgabe mit Uhrzeit, abhängig von den Einstellungen.
- individuell gesetzte Erinnerungen.
- Erinnerung an sich selbst.
- in geeigneten Teamkontexten Erinnerung für andere.

### 6.1.8 Prioritäten

Vier Prioritätsstufen:

- P1 höchste Priorität.
- P2.
- P3.
- P4 beziehungsweise keine besondere Priorität.

Prioritäten beeinflussen die visuelle und sortierte Darstellung.

### 6.1.9 Labels

Labels gruppieren ähnliche Aufgaben projektübergreifend:

- beliebig viele Labels anlegen.
- Farbe festlegen.
- häufig genutzte Labels als Favorit.
- alle Aufgaben eines Labels anzeigen.
- Labels in Filtern verwenden.

Für SASD wären Beispiele:

- `@warten`.
- `@telefon`.
- `@interview`.
- `@unterlagen`.
- `@recherche`.

### 6.1.10 Projekte und Unterprojekte

Aufgaben werden in Projekten organisiert:

- Projekte anlegen.
- Unterprojekte.
- persönliche Projekte.
- Teamprojekte.
- Farbe/visuelle Kennzeichnung.

### 6.1.11 Abschnitte

Innerhalb eines Projekts können Aufgaben in Sections/Abschnitte gegliedert werden.

### 6.1.12 Unteraufgaben

Aufgaben können hierarchisch in Subtasks zerlegt werden. Für eine Bewerbungsaufgabe könnte das beispielsweise sein:

- Interview vorbereiten.
  - Unternehmen recherchieren.
  - Gesprächspartner recherchieren.
  - fünf Fragen notieren.
  - Gehaltsrahmen prüfen.

### 6.1.13 Inbox

Nicht sofort zugeordnete Aufgaben landen im Eingang und können später einem Projekt beziehungsweise Kontext zugeordnet werden.

### 6.1.14 Today und Upcoming

Vordefinierte zeitbezogene Ansichten:

- Heute.
- Demnächst/Upcoming.

Upcoming erlaubt die Planung beziehungsweise Umplanung per Drag-and-drop über kommende Tage/Wochen.

### 6.1.15 Listenansicht

Projekte und Filter können als klassische Liste dargestellt werden.

### 6.1.16 Board-Ansicht

Alternativ kann eine Board-/Kanban-Ansicht verwendet werden.

### 6.1.17 Kalenderansicht

In Pro/Business:

- Kalenderdarstellung.
- Wochen-/Monatsplanung je nach Kontext.
- Time Blocking.
- Aufgaben auf Zeitfenster verteilen.

### 6.1.18 Sortieren, Gruppieren und Filtern innerhalb von Views

Ansichten lassen sich anpassen:

- Gruppierung nach geeigneten Kriterien.
- Sortierung alphabetisch.
- nach Beauftragtem.
- nach Fälligkeitsdatum.
- nach Erstellungsdatum.
- nach Priorität.
- nach Projekt.
- zusätzliche Filter in einer Ansicht.

### 6.1.19 Benutzerdefinierte Filter

Todoist besitzt eine mächtige Filterabfragesprache. Filtern kann man unter anderem nach:

- Aufgabennamen.
- Datum.
- Deadline.
- Projekt.
- Unterprojekt.
- Abschnitt.
- Label.
- Priorität.
- Erstellungsdatum.
- Fälligkeit.
- wiederkehrenden Aufgaben.
- Vorhandensein/Fehlen einer Zeit.
- Verantwortlichem.
- Workspace.
- Kombinationen mit AND/OR/NOT-artiger Syntax.

Dadurch lassen sich dynamische Arbeitslisten erzeugen, etwa „alle heute fälligen P1-Aufgaben mit @telefon“.

### 6.1.20 Favoriten

Projekte, Labels und Filter können als Favoriten schnell erreichbar gemacht werden.

### 6.1.21 Geteilte Projekte

Todoist erlaubt Zusammenarbeit:

- Projekte teilen.
- Aufgaben gemeinsam bearbeiten.
- Verantwortliche zuweisen.

### 6.1.22 Kommentare und Anhänge

In Aufgaben können Nutzer:

- Kommentare schreiben.
- Dateien anhängen.
- Sprachnotizen senden.

### 6.1.23 Team Workspaces

Business-/Teamfunktionalität umfasst:

- gemeinsamen Workspace.
- persönliche und Teamarbeit getrennt halten.
- Teamprojekte.
- Ordner.
- eingeschränkte Projekte.
- öffentlich zugängliche oder private Teamprojekte.
- Projekt per Link anzeigen beziehungsweise beitreten.

### 6.1.24 Rollen und Berechtigungen

Teamadministration umfasst Rollen wie:

- Admin.
- Member.
- Guest.

Admins steuern Zugriffs- und Berechtigungsebenen.

### 6.1.25 Team Filters

Gemeinsame Filter können beispielsweise zeigen:

- Aufgaben eines Teammitglieds.
- delegierte Aufgaben.
- überfällige Teamaufgaben.
- Aufgaben der nächsten Woche.
- Workload einzelner Personen.

### 6.1.26 Vorlagen

Todoist bietet Templates für typische Workflows und Projekte. Für SASD wäre das Prinzip interessant, später beispielsweise Vorlagen für:

- Erstkontakt durch Recruiter.
- normale Direktbewerbung.
- Bewerbung mit mehreren Interviewstufen.

zu definieren.

### 6.1.27 Produktivitätsauswertung

Todoist besitzt persönliche Fortschrittsfunktionen:

- Tagesziele.
- Wochenziele.
- Produktivitätsvisualisierungen.
- Aktivitätsverlauf.
- Archiv erledigter Aufgaben.
- Todoist Karma als gamifizierte Kennzahl.

Für SASD sollten wir eher die nützliche Aktivitätshistorie übernehmen, nicht zwingend Gamification.

### 6.1.28 Geräte- und Plattform-Synchronisation

Todoist synchronisiert Daten über zahlreiche Clients:

- Desktop.
- Web.
- Android.
- iOS.
- Wearables.
- Browser-Erweiterungen.
- E-Mail-Add-ons.

### 6.1.29 Integrationen

Todoist besitzt eine große Integrationslandschaft mit Kategorien wie:

- KI-Agenten.
- Automatisierung.
- Browser.
- E-Mail.
- Kalender.
- Messaging.
- Notizen.
- Projektmanagement.
- Zeiterfassung.
- weitere Produktivitätswerkzeuge.

### 6.1.30 Besonders relevante Ideen für SASD

- Status, Datum, Deadline und Erinnerung sind verschiedene Dinge.
- Quick Add muss extrem schnell sein.
- Labels und Filter erzeugen flexible Sichten ohne neues Datenmodell.
- Aufgabe kann Unteraufgaben, Dokumente und Gesprächskontext enthalten.
- „Heute“ und „Demnächst“ sind für ein Bewerbungsdashboard wertvoller als bloße Gesamtzahlen.

**Offizielle Quellen:**

- https://www.todoist.com/de/features
- https://www.todoist.com/de/help/articles/todoist-glossary-cA60laWMH
- https://www.todoist.com/de/help/articles/introduction-to-filters-V98wIH
- https://www.todoist.com/de/help/articles/introduction-to-deadlines-in-todoist-uMqbSLM6U

---

## 6.2 Trello

**Kategorie:** Kanban-/Work-Management  
**Bereitstellung:** Web und mobile Apps; Atlassian-Ökosystem  
**Relevanz für SASD:** Mittel; gutes UX-Vorbild für Board/Card/Checklist und einfache regelbasierte Automatisierung.

### 6.2.1 Boards

Trello organisiert Arbeit in Boards:

- mehrere Boards.
- Board-Menü.
- Berechtigungen.
- Einstellungen.
- Aktivitätsverlauf eines Boards.
- Power-Ups und Automationen je Board.

### 6.2.2 Listen

Boards enthalten Listen, die beispielsweise Prozessstufen repräsentieren. Karten werden zwischen Listen verschoben.

### 6.2.3 Karten

Karten sind die zentrale Arbeitseinheit. Sie können enthalten:

- Titel.
- Beschreibung.
- Mitglieder.
- Labels.
- Termine/Fälligkeiten.
- Checklisten.
- Anhänge.
- Kommentare/Aktivität.
- benutzerdefinierte Felder je nach Konfiguration/Power-Up/Plan.

### 6.2.4 Drag-and-drop

Karten können:

- innerhalb einer Liste sortiert.
- zwischen Listen verschoben.
- zwischen geeigneten Ansichten genutzt

werden.

### 6.2.5 Mitglieder und Zuständigkeit

Karten können Personen zugeordnet werden. Für einen persönlichen Bewerbungsmanager ist dies im MVP weniger relevant, aber das sichtbare Responsibility-Prinzip kann bei späterem Mehrbenutzerbetrieb nützlich sein.

### 6.2.6 Labels

Karten können mehrere farbige Labels tragen, um sie quer zu Prozessstufen zu kategorisieren.

### 6.2.7 Fälligkeitsdaten

Karten unterstützen zeitbezogene Informationen und können nach Due Date gefiltert beziehungsweise in Kalender-/Planner-Kontexten betrachtet werden.

### 6.2.8 Checklisten

Eine Karte kann eine oder mehrere Checklisten enthalten:

- einzelne Checklist Items.
- Fortschritt sichtbar.
- wiederkehrende Arbeitsmuster als Checkliste.

Für Bewerbungen ist dies eine gute Inspiration für „Unterlagen vollständig?“, ohne dafür zehn eigenständige Pipeline-Stufen zu benötigen.

### 6.2.9 Anhänge

Dateien und Links können Karten zugeordnet werden.

### 6.2.10 Kommentare und Activity Feed

Änderungen und Kommunikation werden in der Karten-/Board-Aktivität sichtbar. Dadurch entsteht eine einfache Historie.

### 6.2.11 Inbox / Capture

Neuere Trello-Produktführung umfasst eine Inbox, über die neue Arbeit schnell gesammelt werden kann. Capture kann aus externen Kontexten wie E-Mail oder Slack unterstützt werden.

### 6.2.12 Planner

Der Planner kombiniert Aufgaben/Karten mit kalenderartiger Zeitplanung beziehungsweise Time Blocking.

### 6.2.13 Suche

Trello besitzt Suchfunktionen über Karten beziehungsweise Boards.

### 6.2.14 Filter

Board-Filter umfassen unter anderem:

- Keyword.
- Mitglieder.
- Kartenstatus.
- Due Date.
- Labels.
- Aktivität.
- Kombination verschiedener Filterkriterien.

### 6.2.15 Calendar View

Karten mit Terminen können in einer Kalenderansicht dargestellt werden.

### 6.2.16 Table View

Tabellenansicht für strukturierteren Vergleich von Karten.

### 6.2.17 Timeline View

Zeitliche Darstellung von Karten beziehungsweise Vorgängen entlang einer Timeline.

### 6.2.18 Dashboard View

Dashboard-Funktionen visualisieren Boarddaten mit Statistiken beziehungsweise Diagrammen.

### 6.2.19 Map View

Karten mit Ortsbezug können geografisch dargestellt werden.

### 6.2.20 Automation / Butler

Trello kann regelbasierte Automationen ausführen. Typische Trigger/Aktionen beziehen sich auf:

- Karte wird verschoben.
- Karte wird erstellt/geändert.
- Liste.
- Labels.
- Checklisten.
- Custom Fields.
- Termine.
- Kommentare.
- Anhänge.
- externe HTTP-/Integrationsaktionen in geeigneten Konfigurationen.

### 6.2.21 Automationsregeln, Buttons und Zeitpläne

Trello-Automation kennt unterschiedliche Bedienmuster:

- Rules.
- Card Buttons.
- Board Buttons.
- Scheduled Automations.
- Due-Date-bezogene Automationen.

### 6.2.22 Power-Ups

Boards können über Power-Ups erweitert werden, beispielsweise um externe Dienste oder zusätzliche Views/Funktionen.

### 6.2.23 Templates

Boards und Workflows können aus Vorlagen erstellt beziehungsweise als wiederverwendbare Struktur verwendet werden.

### 6.2.24 Integrationen

Trello integriert sich mit zahlreichen Atlassian- und Drittanbieterdiensten. Für SASD ist vor allem das Erweiterungsprinzip relevant, nicht die konkrete Produktliste.

### 6.2.25 Besonders relevante Ideen für SASD

- Kanban ist eine hervorragende **Ansicht**, aber keine ausreichende Domäne.
- Checklisten lösen kleine wiederkehrende Prozesse elegant.
- Boardfilter sollten Statusspalten nicht ersetzen.
- Automationen können einfach genug sein, dass Nutzer sie verstehen.

**Offizielle Quellen:**

- https://trello.com/guide
- https://support.atlassian.com/trello/

---

# 7. ATS- und Recruiting-Systeme als Gegenperspektive

Diese Systeme werden von Arbeitgebern verwendet. Sie sind deshalb besonders wertvoll, weil der SASD Bewerbungsmanager denselben Prozess von der **Bewerberseite** betrachtet. Begriffe und Entitäten lassen sich spiegeln: Job ↔ Stelle, Candidate ↔ eigene Bewerbung, Interview Stage ↔ Gesprächsrunde, Recruiter ↔ Ansprechpartner, Scorecard ↔ eigene Gesprächseinschätzung.

## 7.1 Greenhouse

**Kategorie:** Applicant Tracking System / Hiring Platform  
**Relevanz für SASD:** Sehr hoch für strukturierten Prozess, Interviewrunden, Scorecards, Timeline und Funnel Analytics.

### 7.1.1 Jobs und Hiring Process

Greenhouse strukturiert Recruiting um konkrete Jobs:

- Job anlegen/verwalten.
- Hiring Process definieren.
- Interviewstufen festlegen.
- konsistente Stages verwenden.
- Job Intake/Kickoff-Informationen erfassen.
- Job Posts und Bewerbungswege bereitstellen.

### 7.1.2 Candidate Profiles

Kandidaten besitzen zentrale Profile mit:

- Bewerbung/Candidacy.
- Resume und Dokumenten.
- Kontaktdaten.
- Stage-/Pipeline-Zustand.
- Interviewplanung.
- Scorecards.
- Offer-Informationen.
- Activity Feed.
- Notizen.
- weitere Bewerberdaten.

### 7.1.3 Pipeline

Kandidaten werden durch definierte Interview-/Hiring-Stufen geführt:

- visuelle Pipeline.
- bis zu mehrere definierte Stages.
- aktueller Prozessschritt.
- nächste Aktion sichtbar.
- Filter nach Kandidatenzustand und Aktivität.

### 7.1.4 Pipeline-Indikatoren

Greenhouse kennzeichnet in der Pipeline offene Aktionen, beispielsweise:

- Nutzeraktion erforderlich.
- Scorecard ausstehend.

Das ist ein hervorragendes Muster für unseren Manager: Nicht nur Status anzeigen, sondern **warum gerade Handlungsbedarf besteht**.

### 7.1.5 Interview Scheduling

Interviewfunktionen umfassen:

- Interviewtermine.
- Teilnehmer/Interviewer.
- Bestätigungen.
- Verfügbarkeiten.
- Planung innerhalb des Hiring-Prozesses.

### 7.1.6 Structured Interview Kits

Greenhouse arbeitet mit strukturierten Interview-Kits:

- vordefinierte Fragen.
- Bewertungskriterien.
- für dieselbe Rolle konsistent verwendbare Interviewstruktur.

### 7.1.7 Scorecards

Scorecards ermöglichen standardisierte Bewertung:

- Skills.
- Traits/Eigenschaften.
- Qualifikationen.
- Attributes.
- fokussierte Attributes je Interview.
- Gesamt-Empfehlung.
- ausstehende versus abgegebene Scorecards.

Für SASD wäre daraus nicht ein „Kandidatenscore“, sondern eine **eigene strukturierte Bewertung des Jobs und des Gesprächs** ableitbar.

### 7.1.8 Resume Parsing und Search

Kandidatenprofile können aus Bewerbungsunterlagen strukturiert werden. Recruiting-Teams können Kandidaten suchen und filtern anhand von:

- Profilinformationen.
- Tags.
- gespeicherten Daten.
- Bewerbungs-/Jobbezug.

### 7.1.9 Notes und Activity Feed

Der Kandidatenverlauf enthält:

- Stage-Wechsel.
- Notizen.
- Aktivitäten.
- Kommunikations-/Prozessereignisse.
- Filter.
- Suche in der Activity History.

### 7.1.10 Offer Management

Greenhouse verwaltet Angebotsprozesse:

- Offer-Daten.
- Offer Approvals.
- Offer Documents.
- Offer Packet/Unterlagen.

### 7.1.11 Approval Workflows

Bestimmte Schritte, insbesondere Angebote und Jobs, können Genehmigungsabläufe enthalten. Im persönlichen Bewerbungsmanager zunächst nicht relevant, aber als Modell für „Entscheidung noch nicht final“ interessant.

### 7.1.12 Sourcing und CRM

Greenhouse-Tarife enthalten Funktionen für:

- Sourcing.
- Recruiting CRM.
- Kandidatenpools.
- Kontakt-/Outreach-Prozesse.
- Sourcing Automation in höheren Paketen.

### 7.1.13 Texting und Kommunikation

Je nach Paket können Recruiter Kandidaten zusätzlich per Textnachricht/SMS ansprechen beziehungsweise Kommunikation zentralisieren.

### 7.1.14 AI Notetaker

Aktuelle Angebote dokumentieren AI-gestützte Gesprächsnotizen beziehungsweise Notetaking als Teil der Plattform.

### 7.1.15 AI Interview-/Meeting-Funktionen

Dokumentierte KI-Funktionen umfassen je nach Verfügbarkeit:

- Transkription.
- Zusammenfassungen.
- Interviewinformationen strukturieren.
- Notizen in Kandidatenevaluationen überführen.
- Scorecard-Informationen zusammenfassen.

### 7.1.16 AI Recruiting Plans

Greenhouse beschreibt KI-Unterstützung beim Erzeugen strukturierter Recruiting-Pläne, beispielsweise:

- Stages.
- Interviewfragen.
- Scorecards beziehungsweise Bewertungskriterien.

### 7.1.17 AI Resume/Identity Handling

Aktuelle Produktkommunikation beschreibt KI-gestützte Resume-Verarbeitung und Funktionen zur Reduzierung identifizierender Informationen beziehungsweise strukturierter Prüfung. Solche Arbeitgeberfunktionen sind für SASD vor allem als Hinweis auf Datenschutz und Erklärbarkeit relevant.

### 7.1.18 Reporting und Analytics

Greenhouse besitzt sehr umfangreiche Reports. Dokumentierte Report-/Kennzahlfamilien umfassen unter anderem:

- Current Pipeline.
- Pipeline Snapshot.
- Pipeline Pass-through/Conversion.
- Time in Stage.
- Time to Fill.
- Time to Hire.
- Hiring Activity.
- Interviewing Activity.
- Scheduling Activity.
- Scorecard Feedback.
- Interviewer Engagement.
- Interviewer Calibration.
- Job Status.
- Job Post Conversion.
- Candidate Source.
- Source Effectiveness.
- Hires.
- Offers.
- Offer Acceptance/Ablehnung.
- Rejections.
- Referrals.
- Candidate Quality nach Quelle/Kampagne/Recruiter/Referrer.
- Recruiter-/Sourcer-Leaderboards.
- Kontakte hinzugefügt beziehungsweise kontaktiert.
- Replies.
- Meetings booked.
- Nachrichten.
- Approval-Zeiten.
- Surveys.
- Candidate Experience.
- demografische/EEOC-/DE&I-bezogene Reports, soweit rechtlich/organisatorisch eingesetzt.

### 7.1.19 Report Builder

Zusätzlich zu Standardreports:

- eigene Reports.
- Filter nach Jobs.
- Offices.
- Departments.
- Zeiträumen.
- weiteren Kennzahlen.
- AI-/Textunterstützung für Reportfilter in neueren Angeboten.

### 7.1.20 Dashboards

Greenhouse bündelt Kennzahlen in Dashboards und erlaubt je nach Produktumfang:

- standardisierte Visualisierung.
- individualisierte Dashboards.
- Teilen mit Stakeholdern.
- BI-Anbindung in höheren Paketen.

### 7.1.21 DE&I und strukturierte Fairnessmechanismen

Greenhouse unterstützt Funktionen wie:

- strukturierte Interviewfragen.
- konsistente Scorecards.
- Berichte zur Zusammensetzung beziehungsweise Prozessverteilung.
- Candidate Surveys.
- teilweise anonymisierte Resume-/Identitätsdarstellung.

Für SASD ist davon vor allem das Prinzip wertvoll, subjektive Eindrücke von strukturierten Kriterien zu trennen.

### 7.1.22 Integrationen und API

Greenhouse besitzt ein großes Integrationsökosystem:

- offene APIs.
- HRIS-Integrationen.
- Jobbörsen.
- Assessments.
- Background Checks.
- Interview-/Scheduling-Tools.
- zahlreiche weitere Recruiting-Werkzeuge.

### 7.1.23 SSO und Enterprise-Funktionen

Aktuelle Produktpakete dokumentieren unter anderem SSO und enterpriseorientierte Integrations-/Governancefunktionen.

### 7.1.24 Besonders relevante Ideen für SASD

- mehrere Interviewstufen sind echte Domänenobjekte.
- ausstehende Aktion muss sichtbar sein.
- Activity Feed ist für Audit und Erinnerungsvermögen zentral.
- Funnel Conversion sollte nach genügend Daten auswertbar sein.
- strukturierte Kriterien helfen, Stellen nach Gesprächen rationaler zu vergleichen.

**Offizielle Quellen:**

- https://www.greenhouse.com/
- https://support.greenhouse.io/
- https://www.greenhouse.com/pricing

---

## 7.2 Lever

**Kategorie:** ATS + Recruiting CRM (LeverTRM)  
**Relevanz für SASD:** Sehr hoch für die Verbindung aus aktivem Bewerbungsprozess, langfristigen Kontakten, Automatisierung und kompletter Candidate Journey.

### 7.2.1 ATS und CRM in einer Plattform

Lever kombiniert:

- Applicant Tracking.
- Candidate Relationship Management.
- Analytics.

Dadurch können sowohl aktive Bewerber als auch Personen gepflegt werden, die erst später für eine Position relevant werden.

### 7.2.2 Sourcing

Recruiting-Teams können Kandidaten suchen beziehungsweise in Talentpools aufnehmen und für zukünftige Rollen verwalten.

### 7.2.3 Talent Pipelines

Lever unterstützt:

- aktive Bewerbungspipelines.
- zukünftige Talentpools.
- Reaktivierung früherer Kandidaten.
- sogenannte „Silver Medalists“.
- passive Kandidaten.
- Referral-Talent.
- interne Talentpools.

### 7.2.4 Candidate/Opportunity Management

Kandidaten können über eine beziehungsweise mehrere Opportunities/Rollen hinweg verfolgt werden. Profile werden mit Interaktionen und Statusänderungen aktualisiert.

### 7.2.5 Echtzeitnahe Kandidatenhistorie

Profile können sich aktualisieren, wenn:

- Outreach-E-Mail geöffnet/beantwortet wird.
- Interview geplant wird.
- Assessment abgeschlossen wird.
- Recruiter Kandidaten in eine andere Stage bewegen.
- Feedback/Score eingesammelt wird.

Das ist eine starke Referenz für eine ereignisorientierte Bewerbungsakte.

### 7.2.6 Job Posting

Lever dokumentiert:

- One-click-Posting auf über 200 Jobboards.
- Karriere-/Jobseiten.
- branded Career Sites.
- Bewerbungsflows.

### 7.2.7 Interview Scheduling

Funktionen:

- automatische Interviewplanung.
- Candidate Self-Scheduling.
- Follow-ups.
- Koordination mit dem Hiring-Prozess.

### 7.2.8 Structured Scorecards

Interviews können mit strukturierten Scorecards bewertet werden, um Vergleichbarkeit und konsistente Entscheidungen zu fördern.

### 7.2.9 Interview AI

Lever beschreibt aktuelle KI-Funktionen wie:

- Interviewtranskripte.
- Smart Summaries.
- strukturierte Zusammenfassung wichtiger Erkenntnisse.

### 7.2.10 AI Screening

Aktuelle Plattformfunktionen umfassen KI-basiertes Screening:

- Kandidaten priorisieren.
- Matching-/Fit-Signale.
- Ranking von Kandidaten.
- Zusammenfassungen.
- erklärbare/strukturierte Screening-Ergebnisse je nach eingesetztem Modul.

### 7.2.11 Candidate Dossier

Im Umfeld aktueller Screening-Funktionen dokumentiert Lever strukturierte Candidate Dossiers, beispielsweise mit:

- Kandidatenzusammenfassung.
- CV.
- Screening Insights gegenüber Rollenkriterien.
- Interviewtranskript.
- erklärbarem Fit Score.

### 7.2.12 Smart Workflows

Lever automatisiert Prozessschritte:

- Kandidaten automatisch weiterbewegen.
- Aktionen an Stages koppeln.
- Follow-ups auslösen.
- wiederkehrende Recruiting-Arbeit reduzieren.

### 7.2.13 Nurture Campaigns

Das integrierte CRM unterstützt personalisierte Kampagnen:

- Kandidaten über längere Zeit ansprechen.
- Talentpools pflegen.
- zukünftige Kandidaten nicht verlieren.

### 7.2.14 Automated Feedback Loops und Status Updates

Kandidatenkommunikation kann automatisiert werden:

- Statusupdates.
- Feedback-/Kommunikationsschritte.
- Follow-up.

Für SASD ist das Gegenstück: eingehende Arbeitgeber-Statusinformationen automatisch erkennen, aber Änderungen transparent halten.

### 7.2.15 Offers und Approval Chains

Lever unterstützt:

- anpassbare Genehmigungsketten.
- Offer Letters.
- Angebotsprozesse.

### 7.2.16 Analytics und Visual Insights

Dokumentierte Analysefunktionen:

- Echtzeit-/aktuelle Dashboards.
- Custom Reports.
- Pipeline-Visualisierung.
- Funnel Conversion.
- Pipeline Bottlenecks.
- Time-to-Fill.
- Source of Hire.
- Benchmarking.
- DEI-/Compliance-Analysen.
- Stakeholder-spezifische Dashboards.

### 7.2.17 Talent Rediscovery

Bestehende Datenbankeinträge können für neue Rollen wiederentdeckt werden, statt Kandidaten immer nur neu zu sourcen.

Übertragen auf SASD: Eine Firma, ein Recruiter oder eine frühere Opportunity sollte bei einer neuen Ausschreibung wiedererkannt werden.

### 7.2.18 Integrationsökosystem

Lever bietet eine große Marketplace-Landschaft, unter anderem Kategorien für:

- Assessments.
- Background Checks.
- Kalender/Scheduling.
- Communication.
- E-Signature.
- HRIS/HCM/Payroll.
- Job Boards.
- Offers.
- Onboarding.
- Reference Checks.
- Resume Screening.
- Sourcing.
- Video Interviews.
- Analytics und Automation.

### 7.2.19 Besonders relevante Ideen für SASD

- eine Person ist nicht identisch mit einer Opportunity.
- dieselbe Person/Firma kann später wieder relevant werden.
- Candidate Journey beziehungsweise Bewerbungsjourney sollte aus Ereignissen rekonstruiert werden können.
- Automation und Nurturing sind mächtig, müssen auf Bewerberseite aber bewusst persönlicher und zurückhaltender eingesetzt werden.

**Offizielle Quellen:**

- https://www.lever.co/lever-trm
- https://www.lever.co/applicant-tracking-system
- https://www.lever.co/marketplace

---

## 7.3 Workable

**Kategorie:** Recruiting-/ATS-Plattform  
**Relevanz für SASD:** Sehr hoch für detaillierte Candidate Profile, Kommunikationschronik, Interviewkits und Dokument-/Offer-Workflow.

### 7.3.1 Candidate Profile als zentrale Akte

Das Candidate Profile ist die zentrale Recruiting-Akte. Es verbindet:

- Bewerberprofil.
- Bewerbung zu einer Stelle.
- Pipeline-Stufe.
- Kommunikationshistorie.
- Interviews.
- Bewertungen.
- Dokumente.
- Angebot.

### 7.3.2 Candidate Profile – Stammdaten

Dokumentierte Informationen umfassen:

- Name.
- Headline.
- letzte Berufserfahrung.
- Standort.
- Telefonnummer.
- Tags.
- Quelle.
- Follower/interne Beobachter.
- Foto, abhängig von Privacy-Einstellungen.
- weitere Kontaktinformationen.

### 7.3.3 Resume und Bewerbungsdaten

Im Profil können enthalten sein:

- Resume.
- Berufserfahrung.
- Ausbildung.
- Skills.
- Keyword Matches.
- Application Questions.
- Custom Fields.
- mehrere Kontaktdaten.
- Referral-Informationen.
- Social-/LinkedIn-Informationen.

### 7.3.4 Pipeline-Stufe

Das Candidate Profile zeigt, in welcher Stufe des konkreten Jobs sich die Person befindet. Kandidaten können in eine andere Stufe verschoben werden.

### 7.3.5 Action Toolbar

Direkt aus dem Kandidatenprofil können Aktionen ausgelöst werden:

- E-Mail senden.
- E-Mail zeitversetzt senden.
- Text/SMS in geeigneten Paketen.
- Event/Interview planen.
- Kommentar hinterlassen.
- Kandidaten bewerten.
- disqualifizieren.
- in andere Stage verschieben.

### 7.3.6 Weitere Aktionen

Das More-Menü dokumentiert zusätzliche Workflows:

- E-Signature anfordern.
- Background Check anfordern.
- Reference Check anfordern.
- Kandidaten speichern.
- Snooze/Wiedervorlage.
- Kandidaten teilen.
- Profil drucken.
- Kandidaten in andere Stelle kopieren/verschieben.
- Kandidaten löschen/bearbeiten.

### 7.3.7 Timeline

Die Timeline zeigt Aktivitäten des Kandidaten über den Recruitingprozess hinweg. Dadurch ist nachvollziehbar, wann welche Aktion erfolgt ist.

### 7.3.8 Communication Tab

Kommunikation wird separat gebündelt:

- E-Mails.
- SMS/Textnachrichten, sofern verfügbar.
- automatisierte Kommunikation.
- E-Signatur-Anfragen.
- Events/Interviews.

### 7.3.9 E-Mail direkt aus Workable

Workable kann Kandidatenmails senden:

- Standard-Mailversand.
- Gmail-/Google-Integration.
- Microsoft-365-Integration.
- Attachments.
- Vorlagen.
- Signaturen.
- Delayed Send.

### 7.3.10 Antwortsynchronisierung

Antworten von Kandidaten können zurück in die Timeline beziehungsweise Inbox des Recruiting-Systems synchronisiert werden.

### 7.3.11 bestehende E-Mail-Konversation importieren

Bei integrierten Mailkonten können relevante bestehende Konversationen in den Bewerberkontext übernommen werden.

### 7.3.12 E-Mail-Vorlagen und Platzhalter

Vorlagen können dynamische Platzhalter enthalten, unter anderem für:

- Kandidatenname.
- Unternehmen.
- Absender/Nutzer.
- Stelle.
- Job-Link.
- Self-Scheduling-Link.
- Due Date.

### 7.3.13 Kalenderintegration

Mit Google-/Microsoft-Kalendern können:

- Calls/Interviews planen.
- Teilnehmer einladen.
- Kalenderverfügbarkeit prüfen.
- Räume berücksichtigen.
- Google Meet beziehungsweise Teams-Links nutzen.
- Termine verschieben/absagen.
- RSVPs verfolgen.

### 7.3.14 Candidate Self-Scheduling

Kandidaten können über geeignete Links selbst einen freien Interviewslot auswählen.

### 7.3.15 Multi-part Interviews

Workable unterstützt komplexere Interviews mit mehreren Abschnitten beziehungsweise Teilnehmerkonstellationen.

### 7.3.16 Interview Kits

Interview Kits sorgen für einen konsistenten Ablauf:

- gleiche Fragen in gleicher Reihenfolge.
- definierte Skills/Traits/Requirements.
- einer Pipeline-Stage zuordnen.

### 7.3.17 Scorecards / Evaluation

Bewertung kann erfolgen mit:

- Daumen-/binären Bewertungen.
- Sternen.
- numerischen Skalen.
- Bewertung einzelner Kriterien.
- Notizen zu Fragen.
- Gesamtrating.
- Gesamtkommentar.

Bewertungen anderer Interviewer können bis zur eigenen Abgabe verborgen bleiben, um Beeinflussung zu reduzieren.

### 7.3.18 Assessments

Assessments können:

- manuell oder automatisch ausgelöst werden.
- beim Wechsel in eine Stage automatisch versendet werden.
- aus Vorlagen stammen.
- Deadline besitzen.
- Reminder vor Fälligkeit auslösen.
- detaillierte Ergebnisse zurückgeben, beispielsweise kognitive beziehungsweise Persönlichkeitsdimensionen je Assessment-Anbieter.

### 7.3.19 Video Interviews und externe Bewertungstools

Das Review-Umfeld kann Ergebnisse aus:

- Assessments.
- Video Interviews.
- Reference Checks.
- Background Checks

zusammenführen.

### 7.3.20 Comments

Interne Kommentare erlauben Recruiting-Team und Hiring Manager, Informationen zu diskutieren, ohne diese als Kandidatenkommunikation zu versenden.

### 7.3.21 Files

Dateien können aus verschiedenen Workflows im Profil gesammelt werden:

- E-Mail-Anhänge.
- Kommentare.
- Bewerbungsunterlagen.
- Offer-Dokumente.
- weitere Recruiting-Dateien.

### 7.3.22 Offer Tab und E-Signature

Angebotsworkflow umfasst:

- Offer-Daten.
- Dokumente.
- elektronische Signatur.
- Audit Trail des Signaturprozesses.

### 7.3.23 Custom Fields

Workable unterstützt benutzerdefinierte Felder für:

- Candidate Profile.
- Application.
- weitere Datentypen.

Feldtypen umfassen beispielsweise:

- Text/strukturierte Werte.
- Salary.
- Datum.
- Datei.
- Dropdown.
- Multiple Choice.

Bestimmte Felder können vertraulich beziehungsweise nur für ausgewählte Nutzer sichtbar sein.

### 7.3.24 Candidate Overview

Die Übersicht kann bündeln:

- letzte Aktivitäten.
- weitere Bewerbungen/Candidacies derselben Person.
- nächsten geplanten Termin.

### 7.3.25 LinkedIn Recruiter System Connect

Integration kann:

- LinkedIn-Informationen im Profil zugänglich machen.
- InMail/Notizen teilweise in die Recruiting-Historie bringen.
- Kandidatenprofile anreichern.

### 7.3.26 Besonders relevante Ideen für SASD

- Bewerbungsakte in Tabs strukturieren: Profil, Timeline, Kommunikation, Interview Review, Dateien, Offer.
- dieselbe Person kann mehrere Candidacies haben – analog kann ein Recruiter/Unternehmen mehrere Opportunities besitzen.
- „Snooze“ ist ein gutes Muster für bewusstes Warten.
- Interviewevaluation und freie Notiz sollten getrennt werden.

**Offizielle Quellen:**

- https://www.workable.com/
- https://help.workable.com/

---

# 8. Deutsche Jobportale als Quellen- und Capture-Referenzen

## 8.1 Bundesagentur für Arbeit – Jobsuche

**Kategorie:** Öffentliches deutsches Jobportal  
**Relevanz für SASD:** Hoch als deutsche Stellenquelle und für gespeicherte Suchen, Vormerkungen und Vermittlungsprozesse.

### 8.1.1 Jobsuche

Die BA Jobsuche unterstützt die Suche nach verschiedenen Angebotsarten, darunter:

- Arbeit.
- Ausbildung/Duales Studium.
- Praktikum/Trainee/Werkstudent.
- Selbständigkeit.

### 8.1.2 Suchbegriffe und Beruf

Gesucht werden kann nach:

- Beruf.
- Tätigkeit.
- Keyword/Suchbegriff.
- weiteren berufsbezogenen Kriterien.

### 8.1.3 Ort und Umkreis

Standortsuche umfasst:

- Ort.
- definierte Radius-/Umkreisstufen.
- deutschlandweite beziehungsweise weiter gefasste Suchen.

### 8.1.4 Filter

Je nach Angebotsart stehen Filter zur Verfügung, beispielsweise:

- Arbeitszeit.
- Beginn/Startdatum.
- Berufsfelder.
- Angebotsart.
- weitere stellenbezogene Merkmale.

### 8.1.5 Sortierung

Suchergebnisse können unter anderem nach Kriterien wie:

- Relevanz.
- Aktualität.
- letzter Änderung.
- Startzeitpunkt

sortiert werden, abhängig von der konkreten Ergebnisansicht.

### 8.1.6 Ergebnisliste und Stellendetail

Die Jobsuche zeigt:

- Ergebnisliste.
- Arbeitgeber.
- Titel.
- Standort.
- relevante Eigenschaften/Badges.
- Detailseite der Ausschreibung.
- Bewerbungs-/Kontaktweg je Ausschreibung.

### 8.1.7 Merkliste/Vormerkungen

Interessante Stellen können vorgemerkt beziehungsweise gespeichert werden, insbesondere mit angemeldetem Nutzerprofil.

### 8.1.8 Gespeicherte Suchen

Suchkonfigurationen können gespeichert werden, um dieselbe Kombination nicht wieder neu eingeben zu müssen.

### 8.1.9 Job-Benachrichtigungen per E-Mail

Für gespeicherte beziehungsweise abonnierte Suchkonstellationen können Nutzer passende neue Stellen per E-Mail erhalten.

### 8.1.10 Benutzerprofil

Nach Registrierung/Anmeldung stehen persönliche Funktionen zur Verfügung, abhängig vom BA-Konto und dessen Freischaltung.

### 8.1.11 Vermittlungspostfach

Über das Vermittlungspostfach können Nachrichten im Kontext der Arbeitsvermittlung beziehungsweise von Arbeitgebern bearbeitet werden:

- Nachrichten lesen.
- Antworten senden.
- Kommunikation mit Arbeitsagentur beziehungsweise Vermittlungskontext.

### 8.1.12 Vermittlungsvorschläge

Nutzer können konkrete Vermittlungsvorschläge der Arbeitsagentur einsehen. Diese unterscheiden sich fachlich von selbst gefundenen Stellen und wären im SASD Datenmodell als **Source/Origin** interessant.

### 8.1.13 Jobempfehlungen

Die Plattform kann Stellenempfehlungen bereitstellen, die zum hinterlegten Profil beziehungsweise Suchkontext passen.

### 8.1.14 Arbeitgeber kontaktieren

Bei geeigneten Ausschreibungen können Kontakt-/Bewerbungswege direkt aus dem Portal heraus angestoßen beziehungsweise angezeigt werden.

### 8.1.15 Stellengesuch

Arbeitssuchende können ein eigenes Stellengesuch beziehungsweise Bewerberprofil veröffentlichen:

- bis zu mehrere Zielberufe angeben.
- Ort und Radius.
- gewünschte Arbeitszeit.
- Gehalts-/Rahmenangaben, soweit vorgesehen.
- Vorschau.
- Veröffentlichung für Arbeitgeber.

### 8.1.16 Bewerberprofil

Das Profil kann Informationen über:

- Qualifikationen.
- Berufserfahrung.
- Fähigkeiten.
- Kontaktdaten.
- gewünschte Tätigkeit

enthalten.

### 8.1.17 Bewerbungsunterlagen im Konto

Im persönlichen Bereich können je nach BA-Prozess Profil- beziehungsweise Bewerbungsunterlagen für Online-Bewerbungen hinterlegt beziehungsweise genutzt werden.

### 8.1.18 Besonders relevante Ideen für SASD

- Jobquelle muss strukturiert gespeichert werden.
- „Vermittlungsvorschlag“ ist etwas anderes als „selbst gefunden“ oder „Recruiter hat mich angeschrieben“.
- gespeicherte Suche kann später als Search Profile im SASD Manager modelliert werden.
- Job-Abo/E-Mail-Benachrichtigung ist ein Importkanal, nicht zwingend ein Scraper.

**Offizielle Quellen:**

- https://www.arbeitsagentur.de/jobsuche
- https://www.arbeitsagentur.de/

---

## 8.2 meinestadt.de Jobs

**Kategorie:** Deutsches Jobportal  
**Relevanz für SASD:** Mittel bis hoch als zusätzliche deutsche Jobquelle und Referenz für Merkliste/Search Subscription.

### 8.2.1 Stellensuche

meinestadt.de Jobs ermöglicht:

- Suche nach Stellenbegriff/Beruf.
- Suche nach Ort beziehungsweise Region.
- Durchsuchen lokaler und überregionaler Angebote.

### 8.2.2 Ergebnislisten

Suchergebnisse zeigen typische Jobinformationen:

- Stellentitel.
- Arbeitgeber.
- Ort.
- weitere Eckdaten der Ausschreibung.

### 8.2.3 Filter und Jobkategorien

Das Portal strukturiert den Markt unter anderem nach Bereichen beziehungsweise Einstiegsarten wie:

- reguläre Stellenangebote.
- Ausbildung.
- Duales Studium.
- Minijobs.
- Praktikum.
- neueste Jobs.
- Quereinsteiger-Jobs.
- Teilzeit.

Zusätzliche Filter hängen von der konkreten Suche ab.

### 8.2.4 Stellendetail

Die Detailseite kann enthalten:

- vollständige Stellenbeschreibung.
- Arbeitgeberinformationen.
- Adresse beziehungsweise Standort.
- Arbeitgeberwebsite, sofern hinterlegt.
- Bewerbungsweg.

### 8.2.5 Merkliste

Interessante Jobs können in einer Merkliste gespeichert werden.

### 8.2.6 Mein Bereich

Das Portal besitzt einen persönlichen Bereich für nutzerbezogene Jobfunktionen und gespeicherte Inhalte.

### 8.2.7 Suche abonnieren / Job-Benachrichtigung

Eine Suche kann abonniert werden, um neue passende Stellen nicht regelmäßig manuell suchen zu müssen.

### 8.2.8 Bewerbung

Bei einzelnen Anzeigen kann:

- eine Bewerbung direkt über einen angebotenen Bewerbungsweg gestartet werden.
- alternativ auf externe Arbeitgeber-/Recruitingseiten verwiesen werden.

Der genaue Bewerbungsweg hängt von der jeweiligen Anzeige ab.

### 8.2.9 Arbeitgeber- und Skill-Kontext

meinestadt.de verknüpft Jobinhalte mit Arbeitgeber-, Berufs- und teilweise Skill-/Ratgeberseiten. Für SASD ist das weniger als Kernfunktion, aber als zusätzliche Kontextquelle interessant.

### 8.2.10 Besonders relevante Ideen für SASD

- externe Jobportale sollten nicht als Teil des internen Bewerbungsstatus betrachtet werden, sondern als Source Provider.
- Merkliste und SASD „Opportunity“ sind unterschiedliche Reifegrade.
- Suchabos können ein datenschutzfreundlicher Importweg sein: Nutzer erhält Mail, Manager erkennt daraus neue Opportunity.

**Offizielle Quellen:**

- https://jobs.meinestadt.de/
- https://www.meinestadt.de/

---
# 9. Querschnitt: Welche Funktionsbereiche deckt die Referenzlandschaft ab?

Die 23 betrachteten Programme zeigen zusammengenommen deutlich mehr als einen einfachen Bewerbungs-Kanban. Für die spätere Anforderungsanalyse lassen sich die beobachteten Funktionen in folgende Domänen gruppieren.

## 9.1 Opportunity- und Stellenerfassung

Beobachtete Funktionen:

- Stelle manuell anlegen.
- URL speichern.
- Stellenbeschreibung archivieren.
- Stelle per Browser-Erweiterung übernehmen.
- Stelle per Bookmarklet übernehmen.
- Stelle aus Jobportal übernehmen.
- Stelle aus E-Mail erkennen.
- Job aus Greenhouse-/Lever-API finden.
- Jobs aus mehreren Jobbörsen aggregieren.
- Search Profiles und gespeicherte Suchen.
- Job-Abonnements.
- Duplikate erkennen.
- wiederveröffentlichte Rollen zusammenführen.
- Quelle/Source speichern.
- extern aktualisierte Felder refreshen, ohne eigene Daten zu überschreiben.

**Hauptreferenzen:** Simplify, Huntr, JobTrail, JobOps, JSE, JobSync, BA Jobsuche, meinestadt.de.

## 9.2 Bewerbungsakte

Beobachtete Funktionen:

- Unternehmen.
- Stelle.
- Bewerbungsdatum.
- Bewerbungsweg.
- Status.
- Stellenbeschreibung.
- Gehalt.
- Standort.
- Arbeitsmodell.
- Notizen.
- Dokumente.
- Kontakte.
- Interviews.
- Aktivitäten.
- Aufgaben.
- Follow-ups.
- Ergebnis/Outcome.
- vollständige Timeline.

**Hauptreferenzen:** Huntr, Teal, Jobscan, JobTrail, Workable.

## 9.3 Pipeline und Status

Beobachtete Muster:

- feste Statusstufen.
- konfigurierbare Stufen.
- Drag-and-drop.
- Board/Kanban.
- Tabellenansicht.
- Archivstatus.
- abgelehnt/withdrawn/no response getrennt behandeln.
- Auto-Archivierung bei Inaktivität.
- Mehrfachrollen beziehungsweise Opportunities getrennt behandeln.

**Hauptreferenzen:** Huntr, Teal, JSE, Pipedrive, Greenhouse, Lever, Trello.

## 9.4 Aktivitäten und Timeline

Beobachtete Ereignisse:

- Bewerbung gesendet.
- E-Mail gesendet/empfangen.
- Telefonat.
- LinkedIn-/Networking-Interaktion.
- Meeting.
- Interview.
- Statusänderung.
- Angebot.
- Absage.
- freie Notiz.
- Dokument versendet.
- Follow-up.

**Hauptreferenzen:** Huntr, Pipedrive, Dex, JobTrackerPro, Workable, Greenhouse.

## 9.5 Next Action und Wiedervorlage

Beobachtete Funktionen:

- nächste Aktivität planen.
- Follow-up-Datum.
- Reminder.
- Deadline.
- Snooze.
- Wait-/Delay-Schritte.
- überfällige Aktionen anzeigen.
- keine aktive Bewerbung ohne geplanten nächsten Schritt sichtbar machen.

**Hauptreferenzen:** Pipedrive, Todoist, Teal, Careerflow, JSE, Workable.

## 9.6 Aufgaben und Checklisten

Beobachtete Funktionen:

- Task anlegen.
- Fälligkeit.
- Deadline.
- Priorität.
- Unteraufgaben.
- Checklisten.
- Labels.
- Reminder.
- Task mit Bewerbung verbinden.
- Zeitaufwand erfassen.
- wiederkehrende Aufgaben.

**Hauptreferenzen:** Todoist, Teal, JobSync, Trello, Pipedrive.

## 9.7 Interviews

Beobachtete Funktionen:

- Interviewdatum und Zeit.
- Format.
- Meeting-Link.
- Teilnehmer.
- mehrere Interviewrunden.
- Vorbereitung.
- Fragen.
- Notizen.
- Feedback.
- strukturierte Scorecard.
- Follow-up.
- Interview Learnings.
- Transkription/Zusammenfassung in großen Recruiting-Systemen.

**Hauptreferenzen:** JobTrail, Huntr, Greenhouse, Lever, Workable, JSE.

## 9.8 Kontakt-/Recruiter-CRM

Beobachtete Funktionen:

- Person als eigenes Objekt.
- Unternehmen als eigenes Objekt.
- Jobtitel/Rolle.
- E-Mail.
- Telefon.
- Social-/LinkedIn-Link.
- Beziehung zum Job.
- Beziehung zu mehreren Jobs.
- Kontakt-Timeline.
- letzter Kontakt.
- Follow-up.
- Reminder.
- Tags/Gruppe.
- Custom Fields.
- Dubletten zusammenführen.
- E-Mail-/Kalenderinteraktionen automatisch protokollieren.

**Hauptreferenzen:** Dex, Pipedrive, Monica, Huntr, Careerflow.

## 9.9 Unternehmensakte

Beobachtete Funktionen:

- Arbeitgeberstammdaten.
- mehrere Jobs pro Unternehmen.
- mehrere Kontakte pro Unternehmen.
- öffentliche Unternehmensinformationen.
- Company Enrichment.
- Arbeitgeberwebsite.
- Standort.
- Brancheninformation.
- Notizen.
- Company Research.
- Interviewfragen.
- historische Bewerbungen beim selben Unternehmen.

**Hauptreferenzen:** JSE, JobTrail, Pipedrive, Teal, Greenhouse/Lever als Gegenperspektive.

## 9.10 Dokumentmanagement

Beobachtete Funktionen:

- mehrere Lebensläufe.
- Anschreiben.
- Stellenanzeige als Dokument/Textsnapshot.
- Dokumente hochladen.
- Dokumente kategorisieren.
- konkrete Dokumentversion einer Bewerbung zuordnen.
- PDF-/DOCX-Import.
- PDF-Export.
- Dokumentnutzung anzeigen.
- Hash-basierte Deduplizierung.
- KI-gestützte Resume-Analyse.

**Hauptreferenzen:** Huntr, JobSync, Jobtra, Jobscan, Simplify.

## 9.11 E-Mail-Integration

Beobachtete Integrationsstufen:

1. **manuelle Notiz** – Nutzer protokolliert Mail selbst.
2. **Smart BCC/Forwarding** – relevante Mail wird an Tracker weitergegeben.
3. **IMAP Sync** – Tracker liest ausgewählte Mailkonten.
4. **Provider OAuth Sync** – Gmail/Microsoft-Integration.
5. **Push/Webhook** – neue Mail löst automatisch Verarbeitung aus.
6. **Klassifikation** – Nachricht als Eingang, Interview, Absage, Offer usw. erkennen.
7. **Entity Matching** – passende Bewerbung finden.
8. **Statusvorschlag/-änderung** – Workflow aus Inhalt ableiten.
9. **Timeline** – Originalereignis dauerhaft am Vorgang dokumentieren.

**Hauptreferenzen:** JobTrackerPro, Jobtra, JobOps, Pipedrive, Workable, Dex.

## 9.12 Kalender und Termine

Beobachtete Funktionen:

- Interview/Meeting als Termin.
- Kalenderansicht.
- Google-/Microsoft-Synchronisation.
- Meeting-Link.
- Teilnehmer.
- Self-Scheduling.
- Verfügbarkeiten.
- Reminder.
- Time Blocking.

**Hauptreferenzen:** Pipedrive, Workable, Todoist, Jobscan.

## 9.13 Resume-/Job-Matching

Beobachtete Funktionen:

- Keyword Match.
- Match Rate.
- Skill Gap.
- AI Fit Score.
- gestuftes Matching.
- lokale Vorselektion.
- Match anhand gesamter Evidenz statt nur eines CV.
- Erfolgs-/Conversion-Erfahrung als begrenztes Zusatzsignal.

**Hauptreferenzen:** Jobscan, Simplify, Careerflow, JobSync, JobOps, JSE.

## 9.14 KI-gestützte Dokumente

Beobachtete Funktionen:

- Resume Review.
- Resume Tailoring.
- Cover Letter Generation.
- Interviewfragen erzeugen.
- E-Mail-/Follow-up-Text unterstützen.
- Dokumente in eigener Stimme aus früherer Evidenz erzeugen.

**Hauptreferenzen:** Huntr, Teal, Simplify, JobOps, JSE, Careerflow.

## 9.15 Analytics

Beobachtete Kennzahlen:

- Zahl gespeicherter Jobs.
- Zahl Bewerbungen.
- Statusverteilung.
- Interviews.
- Angebote.
- Absagen.
- Response Rate.
- Interview Conversion.
- Source Conversion.
- Time in Stage.
- Zeit bis Rückmeldung.
- Erfolgsrate je Resume-Version.
- Erfolgsrate je Jobquelle.
- Erfolgsrate je Arbeitgebertyp.
- Erfolgsrate je Seniorität/Gehaltsband.
- Bewerbungsaktivität über Zeit.
- Aufgabenaktivität.
- Marktangebot über Zeit.

**Hauptreferenzen:** JSE, Greenhouse, Lever, Pipedrive, Careerflow, Jobtra.

## 9.16 Suche, Filter und Views

Beobachtete Sichten:

- Kanban.
- Tabelle.
- Liste.
- Kalender.
- Timeline.
- Dashboard.
- Karte.
- Treemap.
- Detailakte.
- Kontakte.
- Unternehmen.
- Aufgaben.
- Filteransichten.
- gespeicherte Suchabfragen.

**Hauptreferenzen:** Todoist, Trello, Pipedrive, JobTrail, Huntr, Jobtra.

## 9.17 Automationen

Beobachtete Automationsmuster:

- Jobquellen regelmäßig durchsuchen.
- E-Mails erkennen.
- Status aktualisieren.
- Follow-up planen.
- inaktive Bewerbung archivieren.
- Stage-Wechsel löst Aktion aus.
- datumsgesteuerter Trigger.
- If/Else.
- Wait/Delay.
- Agent trägt strukturierte Daten nach Approval ein.
- Monitoring für fehlerhafte Automation.
- Dry Run und Rollback bei automatisch reparierten Scrapers.

**Hauptreferenzen:** Pipedrive, JobTrackerPro, JobOps, JobSync, JSE, Trello.

## 9.18 Local-first, Privacy und Datenhoheit

Beobachtete Funktionen/Muster:

- lokale SQLite-Datenbank.
- Self-hosting.
- kein Accountzwang.
- Offline-/lokale Kernfunktionen.
- lokale LLMs.
- Cloud-KI optional.
- Warnung vor Datenübertragung an externen KI-Provider.
- Backup.
- Export.
- Telemetrie aus beziehungsweise abschaltbar.
- verschlüsselte Mail-Credentials.

**Hauptreferenzen:** JSE, Jobtra, JobSync, JobNest, JobOps.

## 9.19 Erweiterbarkeit

Beobachtete Mechanismen:

- API.
- Webhooks.
- Marketplace.
- Source-/Scraper-Plug-ins.
- eigene Extractors.
- MCP.
- Custom Fields.
- flexible Tags und Labels.

**Hauptreferenzen:** Pipedrive, JSE, JobOps, JobSync, Todoist, Lever/Greenhouse.

---

# 10. Auffällige Produktmuster und Konsequenzen für die spätere SASD-Konzeption

Dieses Kapitel ist **noch keine Anforderungsspezifikation**. Es dokumentiert lediglich die wiederkehrenden Muster, die aus dem Funktionsvergleich hervorgehen.

## 10.1 Kanban ist Commodity, nicht das Produkt

Fast jeder moderne Tracker kann Karten in Spalten verschieben. Ein Kanban-Board ist deshalb kein hinreichendes Differenzierungsmerkmal. Die besseren Produkte besitzen darunter ein reiches Datenmodell aus:

- Job.
- Unternehmen.
- Bewerbung.
- Kontakt.
- Aktivität.
- Interview.
- Aufgabe.
- Dokument.
- Outcome.

## 10.2 Status und Aktivität müssen getrennt sein

Ein Status beantwortet:

> Wo steht die Bewerbung?

Eine Aktivität beantwortet:

> Was ist passiert oder was soll passieren?

Beispiel:

- Status: `Interview`.
- letzte Aktivität: `Einladung am 24.08. erhalten`.
- nächste Aktivität: `Interview am 02.09. um 11:00`.
- Aufgabe: `Interview vorbereiten bis 01.09.`.

Diese Trennung findet sich in besonders starken Referenzen wie Huntr, Pipedrive, Greenhouse und Workable.

## 10.3 „Next Action“ ist wahrscheinlich eine Kernfunktion

Pipedrive, Todoist, Teal und Careerflow zeigen, dass eine reine Statusanzeige organisatorisch nicht genügt. Der spätere SASD Bewerbungsmanager sollte sehr wahrscheinlich schnell beantworten können:

- Was ist heute zu tun?
- Worauf warte ich?
- Wann darf/soll ich nachfragen?
- Welche zugesagte Rückmeldung ist überfällig?
- Welche Bewerbung besitzt keine nächste Aktion?

## 10.4 Die vollständige History ist ein zentraler Wert

E-Mail- und ATS-Produkte zeigen, wie wichtig eine chronologische Vorgangsakte ist. Für eine Bewerbung sollte langfristig rekonstruierbar sein:

- wann Stelle gefunden wurde.
- wann sie verändert oder erneut veröffentlicht wurde.
- wann beworben wurde.
- welche Dokumente verwendet wurden.
- wer wann Kontakt aufnahm.
- welche Interviews stattfanden.
- welche Zusagen gemacht wurden.
- wann Follow-ups erfolgten.
- wie der Vorgang endete.

## 10.5 Person, Unternehmen, Stelle und Bewerbung sind unterschiedliche Objekte

Die Referenzen stützen klar eine Trennung:

- **Person/Contact:** beispielsweise Recruiter.
- **Company:** Arbeitgeber oder Vermittler.
- **Opportunity/Role:** fachliche berufliche Gelegenheit.
- **Job Posting:** konkrete Ausschreibung/Version einer Gelegenheit.
- **Application:** konkrete Bewerbung des Nutzers.

Diese Trennung verhindert später viele Datenmodellprobleme.

## 10.6 Dokumentversionen sind Teil der Historie

Jobscan, Teal, Huntr, Jobtra und JobSync zeigen, dass es nicht reicht, „den Lebenslauf“ zu speichern. Entscheidend ist:

> Welcher Lebenslauf und welches Anschreiben wurden für **diese konkrete Bewerbung** verwendet?

## 10.7 E-Mail-Automation kann eine Killerfunktion sein

JobTrackerPro, JobOps und Jobtra zeigen eine realistische Automatisierungsrichtung:

- Verwaltungsarbeit automatisieren.
- Kommunikation automatisch erkennen.
- passende Bewerbung finden.
- Ereignis/Status vorschlagen.
- Nutzer behält Kontrolle.

Dies ist wahrscheinlich wertvoller als vollautomatische Massenbewerbungen.

## 10.8 Local-first ist im Bewerbungsbereich glaubwürdig

Bewerbungsdaten enthalten:

- Lebensläufe.
- private Notizen.
- Gehaltsinformationen.
- Kontaktdaten.
- Gesprächsinhalte.
- möglicherweise E-Mails.
- Arbeitszeugnisse.

JSE, Jobtra, JobNest und JobSync zeigen, dass Local-first/Self-hosted in dieser Domäne ein echtes Produktargument sein kann und nicht nur eine technische Vorliebe.

## 10.9 KI sollte möglichst kontrolliert und erklärbar arbeiten

Die interessantesten Muster sind nicht „KI macht alles“, sondern:

- Parser vor LLM-Fallback.
- lokale KI bei hochvolumigen privaten Daten.
- teure KI erst nach lokaler Vorsortierung.
- Approval bevor Agent Daten schreibt.
- Score begründen.
- Statistiken erst ab ausreichender Datenmenge nutzen.
- automatisierte Entscheidungen begrenzen.
- Originalquelle als Auditnachweis erhalten.

## 10.10 Interviewwissen kann über einzelne Bewerbungen hinaus wachsen

JSE und JobSync zeigen zwei Richtungen:

- Evidence Library/Interview Learnings.
- Question Bank.

Damit könnte der SASD Bewerbungsmanager langfristig nicht nur verwalten, sondern zu einem persönlichen **Bewerbungswissenssystem** werden.

---

# 11. Noch nicht als Produktanforderung interpretieren

Die Tatsache, dass eine Funktion in diesem Dokument steht, bedeutet ausdrücklich **nicht**, dass sie gebaut werden soll.

Vor einer Aufnahme in Scope oder Backlog sollte jede Funktion mindestens geprüft werden auf:

- konkreten Nutzwert.
- Zielgruppe.
- Häufigkeit des Anwendungsfalls.
- Komplexität.
- Datenschutz.
- Sicherheit.
- Abhängigkeit von externen Plattformen.
- Wartungsaufwand.
- Lizenzrisiken bei möglicher Code-Wiederverwendung.
- Testbarkeit.
- Offline-/Local-first-Verträglichkeit.
- MVP-Relevanz.

Das Dokument ist somit **Input für Anforderungen**, nicht selbst die Anforderungsliste.

---

# 12. Quellen- und Lizenzhinweise

## 12.1 Rechercheprinzip

Bevorzugt wurden:

1. offizielle Produktseiten,
2. offizielle Help Center/Dokumentation,
3. offizielle GitHub-Repositories,
4. aktuelle README-Dateien,
5. aktuelle Release-/Pricing-Dokumentation, wenn diese den Funktionsumfang genauer beschreibt.

Community-Beiträge wurden nur ergänzend verwendet und nicht als alleinige Grundlage für wichtige Funktionsbehauptungen behandelt.

## 12.2 Open-Source-Code nicht automatisch übernehmen

Die Aufnahme eines Open-Source-Produkts als Referenz bedeutet **keine Freigabe zur Codeübernahme**.

Vor Wiederverwendung muss die konkrete Lizenz einschließlich der verwendeten Version geprüft werden. Beispiele aus der Recherche:

- JobSync: MIT.
- JobTrackerPro: MIT.
- Monica: AGPL.
- JobOps: AGPLv3 plus Commons Clause laut Repository.

Gerade Copyleft- und Zusatzklauseln können andere Konsequenzen haben als eine reine Produktinspiration.

## 12.3 Primärquellen je Produkt

1. Huntr – https://huntr.co/product/job-tracker – https://help.huntr.co/
2. Teal – https://www.tealhq.com/tools/job-tracker – https://support.tealhq.com/
3. Careerflow – https://www.careerflow.ai/job-tracker – https://help.careerflow.ai/
4. Jobscan – https://www.jobscan.co/job-tracker
5. Simplify – https://simplify.jobs/copilot – https://help.simplify.jobs/
6. JSE – https://github.com/Keljian/JSE
7. JobSync – https://github.com/Gsync/jobsync
8. JobOps – https://github.com/DaKheera47/job-ops – https://jobops.dakheera47.com/
9. JobTrackerPro – https://github.com/thughari/JobTrackerPro
10. JobTrail – https://github.com/kaylaehman/jobtrail
11. JobHunt – https://github.com/kaitranntt/jobhunt
12. Jobtra – https://github.com/CU1KNIGHT/Jobtra
13. JobNest – https://github.com/maerzhase/jobnest
14. Pipedrive – https://www.pipedrive.com/ – https://support.pipedrive.com/
15. Dex – https://getdex.com/
16. Monica – https://github.com/monicahq/monica
17. Todoist – https://www.todoist.com/de/features – https://www.todoist.com/de/help/
18. Trello – https://trello.com/guide – https://support.atlassian.com/trello/
19. Greenhouse – https://www.greenhouse.com/ – https://support.greenhouse.io/
20. Lever – https://www.lever.co/lever-trm – https://www.lever.co/marketplace
21. Workable – https://www.workable.com/ – https://help.workable.com/
22. Bundesagentur für Arbeit – https://www.arbeitsagentur.de/jobsuche
23. meinestadt.de Jobs – https://jobs.meinestadt.de/

## 12.4 SASD Development Standard

- Repository: https://github.com/Robin-Goerlach/SASD-Development-Standard
- Quick Start: https://github.com/Robin-Goerlach/SASD-Development-Standard/blob/main/QUICKSTART.md

Zum Recherchezeitpunkt bezeichnet sich das Repository als **Version 1.0 Specification Candidate**; die normative Baseline ist laut README approved, Version 1.0.0 aber noch nicht veröffentlicht. Für dieses Dokument ist vor allem das SASD-Prinzip relevant, Evidenz und Produktentscheidungen nachvollziehbar zu halten und den Detailgrad proportional zum Projektbedarf zu vertiefen.

---

# 13. Empfohlener nächster Einsatz dieses Dokuments

Dieses Dokument eignet sich als Basis für einen separaten **Feature-Comparison-Katalog**, in dem nicht mehr pro Produkt, sondern pro fachlicher Funktion gearbeitet wird.

Sinnvolle nächste Artefakte wären:

1. **Referenzmatrix:** Funktionen × Produkte mit `vorhanden / teilweise / nicht vorhanden / nicht verifiziert`.
2. **Feature-Kandidatenliste:** alle aus der Recherche abgeleiteten möglichen Funktionen ohne Priorisierung.
3. **Domänenmodell:** Company, Contact, Opportunity, JobPosting, Application, Activity, Task, Appointment, Communication, Document, Outcome usw.
4. **MVP-Entscheidung:** Must / Should / Could / Not now.
5. **PROJECT-BRIEF:** Problem, Zielgruppe, Scope, Nicht-Ziele, Risiken und Erfolgskriterien nach SASD.
6. **Entscheidungslog:** Welche Ideen aus welchen Vorbildern übernommen, verändert oder bewusst verworfen wurden – jeweils mit Begründung.

Damit bleibt nachvollziehbar, dass der SASD Bewerbungsmanager **aus einer breiten Referenzanalyse eigenständig entwickelt** und kein einzelnes bestehendes Produkt kopiert wird.

---

**Ende des Dokuments**
