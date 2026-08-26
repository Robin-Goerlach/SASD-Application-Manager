# Lastenheft – SASD Bewerbungsmanager Version 1.0

**Dokumenttyp:** Lastenheft / fachliche Anforderungsspezifikation  
**Projekt:** SASD Bewerbungsmanager  
**Zielversion:** 1.0 (mit gekennzeichneten Kandidaten für 1.x)  
**Dokumentstatus:** Draft / zur fachlichen Freigabe  
**Stand:** 24. August 2026  
**Sprache:** Deutsch  
**Normativer Bezug:** SASD Development Standard, Version-1.0-Specification-Candidate / Approved Baseline 0.9.0  
**Primäre Eingangsartefakte:** `Bewerbungsmanager_Referenzprogramme_Funktionskatalog.md`, Markt- und Referenzanalyse vom 24.08.2026

---

## 1. Zweck dieses Dokuments

Dieses Lastenheft beschreibt **was** der SASD Bewerbungsmanager in Version 1 leisten soll, welchen fachlichen Nutzen das Produkt erzeugen muss, welche Qualitäts-, Sicherheits-, Datenschutz- und Datenanforderungen gelten und anhand welcher Kriterien die Zielversion abgenommen werden kann.

Das Dokument beschreibt bewusst **nicht im Detail, wie** die Anforderungen technisch umgesetzt werden. Architektur, konkrete UI-Technologie, Datenbankschema, Bibliotheksauswahl, Klassenstruktur, Installationsmechanismus und Implementierungsdetails gehören in nachgelagerte Pflichtenheft-, Architektur- und technische Spezifikationen.

Das Lastenheft wurde aus der zuvor erstellten Referenzanalyse mit 23 Produkten abgeleitet. Funktionen wurden nicht automatisch übernommen. Sie wurden anhand von Nutzwert, Komplexität, Datenschutz, Wartbarkeit, Testbarkeit und Relevanz für eine eigenständige Version 1 ausgewählt.

### 1.1 SASD-Konformität der Anforderungsdarstellung

Der aktuelle SASD Development Standard verlangt unter anderem eine nachvollziehbare Beschreibung von Problem, Nutzen, Zielgruppe, Scope, Nicht-Zielen, funktionalen und nicht funktionalen Anforderungen, Sicherheits-/Datenschutzanforderungen, Prioritäten und prüfbaren Akzeptanzkriterien. Wesentliche Anforderungen erhalten deshalb in diesem Dokument stabile Kennungen und ein explizites Prioritätsniveau.

Verwendete Kennungsbereiche:

- `REQ-F-###` – funktionale Anforderungen
- `REQ-Q-###` – Qualitätsanforderungen
- `REQ-SEC-###` – Sicherheit und Datenschutz
- `REQ-DATA-###` – Daten, Integrität und Migration
- `REQ-OPS-###` – Betrieb, Diagnose und Wartung aus Anwendersicht
- `REQ-CON-###` – fachliche und organisatorische Constraints

### 1.2 Prioritätsschema

| Priorität | Bedeutung für Version 1 |
|---|---|
| **Must** | Muss für die Freigabe von Version 1.0 erfüllt sein. Fehlt die Anforderung, ist die Zielversion fachlich nicht vollständig. |
| **Should** | Soll in Version 1.0 enthalten sein. Eine Verschiebung ist nur mit dokumentierter Begründung und Auswirkungsbewertung zulässig. |
| **Could** | Sinnvolle Erweiterung innerhalb der Version-1-Produktlinie. Nicht blockierend für 1.0. |
| **Won't V1** | Bewusst nicht Bestandteil der Version-1-Produktlinie. Eine spätere Neubewertung ist möglich. |

Status dieses Lastenhefts: Die Anforderungen sind **Proposed**. Sie gelten erst nach fachlicher Freigabe als `Accepted`.

---

# 2. Ausgangssituation und Problem

Eine aktive Jobsuche erzeugt schnell eine große Menge voneinander abhängiger Informationen:

- Stellenanzeigen verschwinden oder werden verändert.
- Ein Unternehmen kann mehrere interessante Stellen besitzen.
- Zu einer Stelle können mehrere Ansprechpartner, Recruiter oder Vermittler gehören.
- Bewerbungen werden mit unterschiedlichen Lebenslauf- und Anschreibenversionen versendet.
- Telefonate, E-Mails, LinkedIn-Nachrichten, Interviews und Zusagen erzeugen eine fortlaufende Historie.
- Termine, Nachfassfristen und zugesagte Rückmeldungen müssen zuverlässig verfolgt werden.
- Aussagen aus Stellenanzeige, Recruiting und Fachabteilung können sich unterscheiden.
- Nach mehreren Wochen ist häufig nicht mehr sicher erinnerbar, welcher Lebenslauf versendet, welches Gehalt genannt oder welche Rückmeldung zugesagt wurde.
- Tabellen, E-Mail-Postfach, Kalender, Dateisystem und Notizen bilden jeweils nur einen Teil des Vorgangs ab.

Klassische Kanban-Tracker lösen dieses Problem nur teilweise. Sie zeigen zwar, **in welcher Phase** eine Bewerbung steht, beantworten aber oft nicht zuverlässig:

1. Was ist genau passiert?
2. Was ist als Nächstes zu tun?
3. Worauf wartet der Nutzer?
4. Welche Person hat welche Aussage oder Zusage gemacht?
5. Welche Unterlagen wurden tatsächlich verwendet?
6. Welche Informationen stammen aus welcher Quelle?
7. Wie erfolgreich sind bestimmte Quellen, Vorgehensweisen oder Bewerbungswege?

Der SASD Bewerbungsmanager soll diese Lücke schließen.

---

# 3. Produktvision

Der SASD Bewerbungsmanager ist ein **persönliches, lokal geführtes Bewerbungs-CRM und Vorgangssystem**. Er führt Stellen, Bewerbungen, Unternehmen, Ansprechpartner, Kommunikation, Aktivitäten, Aufgaben, Termine, Interviews, Dokumente, Zusagen und Ergebnisse in einer nachvollziehbaren Bewerbungsakte zusammen.

Die Anwendung soll nicht primär fragen:

> „In welcher Spalte liegt diese Bewerbung?“

sondern:

> **„Was ist bei dieser Bewerbung passiert, worauf warte ich und was muss ich als Nächstes tun?“**

Version 1 konzentriert sich deshalb auf zuverlässige Organisation, Historie, Nachverfolgbarkeit und Datenhoheit. Automatische Massenbewerbungen, KI-generierte Bewerbungen und tiefgreifende externe Plattformintegrationen sind ausdrücklich kein Kern von Version 1.

---

# 4. Ziele und erwarteter Nutzen

## 4.1 Fachliche Hauptziele

Version 1 soll folgende überprüfbare Verbesserungen schaffen:

1. **Keine aktive Bewerbung geht organisatorisch verloren.**
2. **Jeder Vorgang ist historisch nachvollziehbar.**
3. **Der Nutzer erkennt täglich, was zu tun ist und worauf er wartet.**
4. **Verwendete Dokumentversionen bleiben einer Bewerbung eindeutig zugeordnet.**
5. **Unternehmen, Kontakte, Stellen und Bewerbungen werden fachlich getrennt, aber verknüpft verwaltet.**
6. **Interviews und mehrere Gesprächsrunden sind als echte Ereignisse dokumentierbar.**
7. **Zugesagte Rückmeldungen und andere Commitments können überwacht werden.**
8. **Stellenanzeigen bleiben als historischer Snapshot erhalten, auch wenn die Originalseite nicht mehr verfügbar ist.**
9. **Die Kernfunktionen bleiben ohne Cloudkonto und ohne Internetverbindung nutzbar.**
10. **Der Nutzer kann seine Daten vollständig sichern, wiederherstellen und exportieren.**

## 4.2 Erfolgskriterien für Version 1.0

Die Version gilt fachlich als erfolgreich, wenn mindestens folgende Kriterien erfüllt sind:

- Eine neue Stelle mit Unternehmen, Quelle und Stellenbeschreibung kann in höchstens zwei Minuten als verwaltbarer Vorgang erfasst werden.
- Eine Bewerbung kann innerhalb desselben Vorgangs mit Bewerbungsdatum, Status, Dokumenten und nächster Aktion protokolliert werden.
- Das Dashboard zeigt alle heute fälligen, überfälligen und ohne nächsten Schritt verbleibenden aktiven Vorgänge korrekt an.
- Eine Bewerbung mit mindestens drei Interviewrunden kann vollständig dokumentiert werden.
- Der genaue Dokumentstand, der zu einer Bewerbung versendet wurde, kann später eindeutig rekonstruiert werden.
- Eine zugesagte Rückmeldung mit Fälligkeit kann erfasst werden und erscheint nach Fristablauf als überfällig.
- Der gesamte Datenbestand kann gesichert, in einer frischen Testumgebung wiederhergestellt und fachlich vollständig wiedergefunden werden.
- Die Kernfunktionen funktionieren offline.
- Ein Datenbestand mit mindestens 10.000 Bewerbungs-/Stellenvorgängen und 50.000 Aktivitäten bleibt in den definierten Performancegrenzen bedienbar.
- Es werden keine personenbezogenen Bewerbungsdaten ohne ausdrückliche Benutzeraktion an externe Dienste übertragen.

---

# 5. Zielgruppen und Stakeholder

## 5.1 Primäre Zielgruppe

Primäre Zielgruppe ist eine einzelne Person, die über einen längeren Zeitraum mehrere berufliche Möglichkeiten, Bewerbungen und Recruitingkontakte systematisch verwalten möchte.

Typische Nutzungssituationen:

- aktive Jobsuche mit mehreren parallelen Bewerbungen;
- Kontakt über Recruiter oder Personalvermittler;
- Direktbewerbungen bei Unternehmen;
- mehrere Stellen bei demselben Unternehmen;
- mehrere Gesprächsrunden;
- Bewerbung über verschiedene Portale und Kommunikationskanäle;
- längere Wartezeiten mit Follow-ups;
- spätere Wiederaufnahme eines früheren Unternehmenskontakts.

## 5.2 Sekundäre Zielgruppen

- Freelancer oder Consultants, die Anbahnungen und Festanstellungsangebote ähnlich verwalten wollen;
- technisch versierte Anwender mit besonderem Interesse an Datenhoheit;
- Nutzer, die bestehende Excel-/Notiz-/Kanban-Lösungen ablösen möchten.

## 5.3 Stakeholder

- Endnutzer;
- Produktverantwortlicher SASD;
- Entwicklung und Qualitätssicherung;
- gegebenenfalls spätere Open-Source-Mitwirkende;
- indirekt Recruiter, Ansprechpartner und Unternehmen, deren Kontaktdaten gespeichert werden.

---

# 6. Produktprinzipien für Version 1

Die folgenden Prinzipien dienen als fachliche Leitplanken und unterstützen spätere Entscheidungen bei konkurrierenden Anforderungen.

## 6.1 Local first

Die Kernanwendung muss ohne Cloudkonto und ohne permanente Internetverbindung nutzbar sein. Persönliche Bewerbungsdaten bleiben standardmäßig unter Kontrolle des Nutzers.

## 6.2 Bewerbung als Vorgangsakte

Eine Bewerbung ist kein einzelnes Kanban-Kärtchen, sondern ein Vorgang mit Historie, Dokumenten, Personen, Aufgaben, Terminen und Ergebnissen.

## 6.3 Status ist nicht Aktivität

Status beschreibt die aktuelle Phase. Aktivitäten beschreiben Ereignisse. Aufgaben beschreiben zu erledigende Arbeit. Diese Konzepte dürfen fachlich nicht miteinander vermischt werden.

## 6.4 Next Action first

Für aktive Bewerbungen soll stets erkennbar sein, was als Nächstes zu tun ist oder bis wann auf eine Reaktion gewartet wird.

## 6.5 Herkunft von Informationen bleibt sichtbar

Wichtige Aussagen sollen auf Wunsch mit Quelle, Zeitpunkt und Person verknüpft werden können. Widersprüchliche Informationen sollen nicht still überschrieben werden müssen.

## 6.6 Mensch behält Kontrolle

Version 1 darf keine Bewerbung automatisch absenden und keine externe Kommunikation ohne explizite Benutzeraktion initiieren.

## 6.7 Datenhoheit und Wiederherstellbarkeit

Export, Backup und Restore sind Kernfunktionen und keine nachträglichen Komfortfeatures.

---

# 7. Scope von Version 1.0

## 7.1 Enthaltene Funktionsbereiche

Version 1.0 umfasst:

- Dashboard und Tagesübersicht;
- Unternehmen;
- Kontakte und Recruiter;
- Opportunities / berufliche Gelegenheiten;
- konkrete Stellenanzeigen und deren Snapshots;
- Bewerbungen;
- Status und Pipeline;
- Aktivitäten und vollständige Timeline;
- Next Action und Wiedervorlagen;
- Zusagen / Commitments;
- Aufgaben und Checklisten;
- Interviews und Interviewrunden;
- Dokumentverwaltung und Zuordnung verwendeter Dokumentversionen;
- manuell protokollierte Kommunikation;
- Notizen, Tags, Quellen und strukturierte Zusatzinformationen;
- Listen-, Tabellen-, Board- und Detailansichten;
- Suche und Filter;
- grundlegende Auswertungen;
- Archivierung und Outcomes;
- Backup, Restore und Datenexport;
- Datenschutz- und Sicherheitsgrundfunktionen;
- lokale/offline nutzbare Kernfunktionalität.

## 7.2 Bewusst nicht in Version 1 enthalten

Folgende Funktionen sind `Won't V1`, auch wenn sie in Referenzprodukten vorkommen:

- automatisches Absenden von Bewerbungen;
- automatische Massenbewerbungen;
- generative Erstellung kompletter Bewerbungen durch KI;
- automatisches Resume-Tailoring durch Cloud-KI;
- integrierte Stellensuche über externe Jobbörsen;
- automatisches Scraping fremder Jobportale;
- Browser-Erweiterung oder Autofill für fremde Bewerbungsformulare;
- IMAP-, Gmail- oder Microsoft-Mailbox-Synchronisierung;
- automatische E-Mail-Klassifikation;
- automatischer Statuswechsel aus eingehenden E-Mails;
- Google-/Microsoft-Kalendersynchronisierung;
- Cloud-Synchronisierung zwischen Geräten;
- Mehrbenutzer- oder Teamfunktionen;
- Rollen- und Rechteverwaltung für mehrere Benutzer;
- Mobile Apps;
- eigenes Jobportal;
- Arbeitgeber-/Recruiting-ATS-Funktionen;
- Social-Network-Funktionen;
- vollwertiger E-Mail-Client;
- komplexes Projektmanagement;
- vollautomatische Company-Enrichment-Dienste;
- agentische Automationen, die Daten ohne vorherige Freigabe verändern.

Diese Abgrenzung ist bewusst. Version 1 soll zuerst ein **zuverlässiges persönliches Bewerbungsbetriebssystem** werden, bevor externe Automatisierungskomplexität hinzukommt.

---

# 8. Fachliches Domänenmodell

Dieses Kapitel beschreibt fachliche Begriffe, nicht deren technische Implementierung.

## 8.1 Company / Unternehmen

Ein Arbeitgeber, Personalvermittler, Recruitingunternehmen oder anderes relevantes Unternehmen. Ein Unternehmen kann mehrere Kontakte, Opportunities und Bewerbungen besitzen.

## 8.2 Contact / Kontakt

Eine konkrete Person, beispielsweise Recruiter, HR-Ansprechpartner, Hiring Manager, Fachansprechpartner oder Vermittler. Ein Kontakt kann mit mehreren Unternehmen beziehungsweise Rollen und mehreren Bewerbungen in Beziehung stehen.

## 8.3 Opportunity / berufliche Gelegenheit

Die fachliche Möglichkeit, eine bestimmte Rolle bei einem Unternehmen zu erhalten. Eine Opportunity kann länger bestehen als eine einzelne veröffentlichte Stellenanzeige.

## 8.4 Job Posting / Stellenanzeige

Eine konkrete veröffentlichte oder zugesandte Beschreibung einer Opportunity. Eine Opportunity kann mehrere Job-Posting-Versionen haben, zum Beispiel wenn eine Stelle erneut veröffentlicht oder verändert wird.

## 8.5 Application / Bewerbung

Die konkrete Bewerbung des Nutzers auf eine Opportunity. Die Bewerbung besitzt einen eigenen Lebenszyklus, Status, verwendete Unterlagen, Aktivitäten, Interviews und Outcome.

## 8.6 Activity / Aktivität

Ein bereits eingetretenes Ereignis oder eine bewusst protokollierte Handlung, beispielsweise Bewerbung versendet, Telefonat, E-Mail erhalten, Status geändert oder Interview durchgeführt.

## 8.7 Task / Aufgabe

Eine noch zu erledigende Handlung mit optionaler Priorität und Fälligkeit.

## 8.8 Next Action / nächste Aktion

Der wichtigste nächste Schritt einer aktiven Bewerbung beziehungsweise Opportunity. Eine Next Action kann eine Aufgabe, ein Termin oder ein bewusstes Warten bis zu einem Datum sein.

## 8.9 Commitment / Zusage

Eine erwartbare, von einer Person oder Partei zugesagte Handlung, beispielsweise „Wir melden uns bis Freitag“. Commitments unterscheiden sich von eigenen Aufgaben, weil die Erfüllung von einer anderen Partei abhängt.

## 8.10 Interview

Ein Gespräch oder eine Gesprächsrunde im Auswahlprozess. Mehrere Interviews können derselben Bewerbung zugeordnet sein.

## 8.11 Document / Dokument

Ein Lebenslauf, Anschreiben, Zeugnis, Stellenanzeigen-Snapshot oder sonstiges Dokument. Entscheidend ist die Möglichkeit, die **konkrete verwendete Dokumentversion** einer Bewerbung zuzuordnen.

## 8.12 Communication / Kommunikation

Eine protokollierte Nachricht oder Interaktion über E-Mail, Telefon, LinkedIn, Teams, Portal oder einen anderen Kanal.

## 8.13 Information / Aussage

Eine fachlich relevante Information, die aus einer bestimmten Quelle stammt, beispielsweise Gehalt, Remote-Regelung, Technologieeinsatz oder Starttermin. Mehrere voneinander abweichende Aussagen zu demselben Thema können nebeneinander dokumentiert werden.

## 8.14 Outcome / Ergebnis

Der fachliche Abschluss eines Vorgangs, beispielsweise Angebot, angenommen, abgelehnt, Absage durch Arbeitgeber, Bewerbung zurückgezogen oder ohne Rückmeldung beendet.

---

# 9. Zentrale Nutzungsszenarien

## 9.1 Stelle gefunden und vorgemerkt

Der Nutzer findet eine interessante Ausschreibung. Er legt Unternehmen und Opportunity an, speichert URL, Quelle und Stellenbeschreibung als Snapshot, ergänzt Standort, Remote-Modell, Gehaltsinformationen und persönliche Priorität. Der Vorgang ist zunächst vorgemerkt, aber noch keine Bewerbung wurde versendet.

## 9.2 Bewerbung versendet

Der Nutzer entscheidet sich zur Bewerbung, protokolliert Bewerbungsdatum und -weg, ordnet die tatsächlich versendeten Dokumentversionen zu und setzt eine Next Action, beispielsweise „bis 31.08. auf Eingangsbestätigung warten“.

## 9.3 Recruiter ruft an

Der Nutzer protokolliert das Telefonat als Aktivität, verknüpft den Recruiter als Kontakt, hält Gesprächsnotizen fest und erfasst eine Zusage „Recruiter sendet Stellenbeschreibung morgen“. Das System überwacht diese Zusage unabhängig von den eigenen Aufgaben des Nutzers.

## 9.4 Einladung zum Interview

Die Bewerbung wechselt in eine Interviewphase. Der Nutzer legt das Interview mit Zeitpunkt, Format, Meeting-Link und Teilnehmern an, notiert offene Fragen und erstellt Vorbereitungstasks.

## 9.5 Mehrere Interviewrunden

Nach dem ersten Gespräch folgen Fachinterview und Teamgespräch. Jede Runde erhält eigene Teilnehmer, Notizen, Vorbereitung, Learnings und Follow-up. Die Timeline zeigt die Reihenfolge vollständig.

## 9.6 Unterschiedliche Aussagen

Die Stellenanzeige nennt „Hybrid“, der Recruiter nennt „drei Tage remote“ und die Fachabteilung sagt „nach Einarbeitung flexibel“. Der Nutzer kann alle drei Aussagen mit Herkunft dokumentieren, ohne ältere Informationen zu verlieren.

## 9.7 Angebot oder Absage

Der Vorgang wird mit einem Outcome abgeschlossen. Angebot, Gehaltsrahmen und Entscheidungsfrist können dokumentiert werden. Bei Absage bleibt die Historie archiviert und später auffindbar.

## 9.8 Wiederaufnahme nach Monaten

Monate später kontaktiert derselbe Recruiter den Nutzer wegen einer neuen Stelle. Der bestehende Kontakt und die frühere Historie sind auffindbar, während die neue Opportunity und Bewerbung getrennt geführt werden.

---

# 10. Funktionale Anforderungen

## 10.1 Grundbetrieb und Benutzerkontext

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-001 | Must | Die Anwendung MUSS ohne Benutzerkonto bei einem externen Dienst nutzbar sein. | Eine frische Installation kann vollständig eingerichtet und genutzt werden, ohne Registrierung oder Login bei einem Fremddienst. |
| REQ-F-002 | Must | Die Anwendung MUSS einen lokalen Einzelbenutzerbetrieb unterstützen. | Ein Nutzer kann sämtliche Kernfunktionen lokal verwenden; Mehrbenutzerrollen sind nicht erforderlich. |
| REQ-F-003 | Must | Die Anwendung MUSS einen klar erkennbaren Start-/Dashboardbereich bereitstellen. | Nach dem Start ist ohne Navigation in Untermenüs sichtbar, welche Vorgänge heute Aufmerksamkeit benötigen. |
| REQ-F-004 | Should | Die Anwendung SOLL einen kurzen Erststart-Assistenten oder eine gleichwertige Einführung bieten. | Ein neuer Nutzer kann Grundeinstellungen und erste Daten ohne Handbuchsuche anlegen. |
| REQ-F-005 | Must | Der Nutzer MUSS Grundeinstellungen wie bevorzugte Datumsdarstellung, Standardstatus und Standardquellen verwalten können. | Änderungen bleiben nach Neustart erhalten und wirken auf neue Datensätze. |
| REQ-F-006 | Could | Die Anwendung KANN einen Demo-/Beispieldatenbestand für Lern- und Testzwecke anbieten. | Beispieldaten sind klar als Demo gekennzeichnet und lassen sich vollständig entfernen. |

## 10.2 Unternehmen

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-010 | Must | Unternehmen MÜSSEN als eigenständige Datensätze verwaltet werden. | Ein Unternehmen kann unabhängig von einer konkreten Bewerbung angelegt, geändert, geöffnet und gesucht werden. |
| REQ-F-011 | Must | Zu einem Unternehmen MÜSSEN Name, Website, Hauptstandort und freie Notizen hinterlegt werden können. | Alle genannten Informationen können gespeichert, geändert und wieder angezeigt werden. |
| REQ-F-012 | Should | Branche, Unternehmensart und weitere strukturierte Merkmale SOLLEN pflegbar sein. | Mindestens Branche und Typ können gefiltert beziehungsweise angezeigt werden. |
| REQ-F-013 | Must | Ein Unternehmen MUSS mehrere Opportunities, Bewerbungen und Kontakte besitzen können. | Drei Bewerbungen und fünf Kontakte lassen sich demselben Unternehmen zuordnen, ohne Datenduplikate erzwingen zu müssen. |
| REQ-F-014 | Must | Die Unternehmensakte MUSS frühere und aktuelle Bewerbungen zusammenhängend anzeigen. | Beim Öffnen eines Unternehmens sind alle verknüpften Vorgänge auffindbar. |
| REQ-F-015 | Should | Der Nutzer SOLL Unternehmensnotizen für Recherche, Kultur, Produkte, Technik und Gesprächsvorbereitung strukturieren können. | Notizen können thematisch getrennt oder mit Tags versehen werden. |
| REQ-F-016 | Should | Offensichtliche Unternehmensdubletten SOLLEN vor oder nach dem Anlegen erkennbar sein. | Bei identischem oder sehr ähnlichem Namen wird der Nutzer auf mögliche Dubletten hingewiesen; automatische Zusammenführung erfolgt nicht ohne Bestätigung. |
| REQ-F-017 | Should | Unternehmen SOLLEN zusammengeführt werden können, ohne verknüpfte Bewerbungen oder Aktivitäten zu verlieren. | Nach einem Merge zeigen alle bisherigen Verknüpfungen auf den verbleibenden Datensatz. |

## 10.3 Kontakte und Recruiter-CRM

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-020 | Must | Kontakte MÜSSEN als eigenständige Personenobjekte verwaltet werden. | Ein Kontakt bleibt auch dann erhalten und auffindbar, wenn eine einzelne Bewerbung archiviert wird. |
| REQ-F-021 | Must | Ein Kontakt MUSS mindestens Name, Rolle, Unternehmen, E-Mail, Telefon und Profil-/Weblink aufnehmen können. | Die Felder können einzeln gepflegt werden; nicht vorhandene Werte sind optional. |
| REQ-F-022 | Must | Ein Kontakt MUSS mit mehreren Bewerbungen und Opportunities verknüpft werden können. | Derselbe Recruiter kann an mehreren Vorgängen erscheinen, ohne mehrfach angelegt werden zu müssen. |
| REQ-F-023 | Must | Die Kontaktakte MUSS die zugehörigen Aktivitäten und letzten Interaktionen anzeigen. | Telefonate, Nachrichten und Interviews mit dem Kontakt sind aus der Kontaktansicht erreichbar. |
| REQ-F-024 | Should | Für Kontakte SOLLEN Tags beziehungsweise Gruppen verwaltet werden können. | Ein Kontakt kann beispielsweise als `Recruiter`, `HR`, `Fachabteilung` und `Vermittler` gekennzeichnet werden. |
| REQ-F-025 | Should | Der Nutzer SOLL einen nächsten Kontakt- oder Follow-up-Zeitpunkt hinterlegen können. | Fällige Kontaktwiedervorlagen erscheinen in der Tagesübersicht. |
| REQ-F-026 | Should | Mögliche Kontaktdubletten SOLLEN erkennbar und kontrolliert zusammenführbar sein. | Gleiche E-Mail-Adresse oder stark übereinstimmende Kontaktdaten erzeugen einen Hinweis; Merge benötigt Bestätigung. |
| REQ-F-027 | Could | Kontakte KÖNNEN mit einer persönlichen Beziehungs-/Relevanzeinschätzung versehen werden. | Die Bewertung ist optional und beeinflusst keine automatischen Entscheidungen. |

## 10.4 Opportunities und Stellenanzeigen

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-030 | Must | Eine berufliche Opportunity MUSS unabhängig von einer konkreten Stellenanzeigen-Version angelegt werden können. | Eine Opportunity kann existieren, auch wenn nur ein Recruiterhinweis und keine öffentliche Anzeige vorhanden ist. |
| REQ-F-031 | Must | Eine Opportunity MUSS einem Unternehmen zugeordnet werden können. | Der Vorgang erscheint sowohl in der Opportunity- als auch in der Unternehmensakte. |
| REQ-F-032 | Must | Eine Opportunity MUSS Stellenbezeichnung, Standort, Beschäftigungsart und Arbeitsmodell aufnehmen können. | Die Angaben sind speicher-, änder- und filterbar. |
| REQ-F-033 | Must | Gehalts-/Vergütungsinformationen MÜSSEN strukturiert und zusätzlich als Freitext erfassbar sein. | Beispielsweise `60.000–70.000 EUR/Jahr` und erläuternde Notiz können gemeinsam gespeichert werden. |
| REQ-F-034 | Must | Eine persönliche Priorität oder Interessensbewertung MUSS erfassbar sein. | Der Nutzer kann Chancen priorisieren und danach filtern/sortieren. |
| REQ-F-035 | Must | Quellen einer Opportunity beziehungsweise Stellenanzeige MÜSSEN dokumentiert werden können. | Quelle kann z. B. Recruiter, Unternehmenswebsite, LinkedIn, Bundesagentur oder sonstige Quelle sein. |
| REQ-F-036 | Must | Zur Stellenanzeige MUSS die Original-URL gespeichert werden können. | URL ist später aufrufbar, selbst wenn der Snapshot lokal vorliegt. |
| REQ-F-037 | Must | Der Text einer Stellenanzeige MUSS als lokaler Snapshot archiviert werden können. | Wird die Originalseite entfernt, bleibt der gespeicherte Inhalt lesbar. |
| REQ-F-038 | Must | Der Zeitpunkt der Erfassung beziehungsweise des Snapshots MUSS nachvollziehbar sein. | Jeder Snapshot besitzt einen sichtbaren Erfassungszeitpunkt. |
| REQ-F-039 | Should | Eine Opportunity SOLL mehrere Versionen beziehungsweise Snapshots einer Ausschreibung verwalten können. | Zwei zeitlich unterschiedliche Ausschreibungstexte können demselben Vorgang zugeordnet werden. |
| REQ-F-040 | Could | Unterschiede zwischen zwei gespeicherten Stellenanzeigen-Versionen KÖNNEN hervorgehoben werden. | Ein Nutzer kann nachvollziehen, welche Textteile hinzugefügt, entfernt oder verändert wurden. |
| REQ-F-041 | Must | Stellenbeschreibungen MÜSSEN zusätzlich frei annotierbar sein. | Eigene Notizen verändern den Originalsnapshot nicht. |
| REQ-F-042 | Should | Relevante Skills, Technologien und Anforderungen SOLLEN strukturiert oder per Tags erfassbar sein. | Skills lassen sich aus mehreren Opportunities suchen und filtern. |
| REQ-F-043 | Should | Wichtige fachliche Aussagen SOLLEN mit Herkunft, Zeitpunkt und optionaler Person dokumentiert werden können. | Zu „Remote-Regelung“ können mehrere Aussagen mit unterschiedlichen Quellen gespeichert werden. |
| REQ-F-044 | Should | Widersprüchliche Aussagen SOLLEN nebeneinander bestehen können, ohne ältere Informationen zwangsweise zu überschreiben. | Drei unterschiedliche Remote-Aussagen bleiben separat sichtbar und quellenbezogen. |
| REQ-F-045 | Should | Wiederveröffentlichungen derselben oder sehr ähnlicher Rolle SOLLEN als mögliche Dubletten erkennbar sein. | Das System weist auf eine bestehende Opportunity hin; der Nutzer entscheidet über Zuordnung oder Neuanlage. |
| REQ-F-046 | Must | Das bloße Merken einer Opportunity DARF NICHT automatisch als versendete Bewerbung gewertet werden. | Vorgemerkte Opportunity und tatsächliche Bewerbung bleiben eindeutig unterscheidbar. |

## 10.5 Bewerbungsakte und Bewerbungsprozess

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-050 | Must | Eine konkrete Bewerbung MUSS als eigener Datensatz innerhalb einer Opportunity geführt werden. | Bewerbung besitzt eigene Daten und Historie unabhängig vom Job-Posting-Snapshot. |
| REQ-F-051 | Must | Eine Opportunity MUSS mehr als eine Bewerbung zulassen, falls der Nutzer sich zu einem späteren Zeitpunkt erneut bewirbt. | Zwei Bewerbungen zu derselben Opportunity können getrennt datiert und ausgewertet werden. |
| REQ-F-052 | Must | Bewerbungsdatum und Bewerbungsweg MÜSSEN erfassbar sein. | Datum und Kanal/Portal sind später eindeutig rekonstruierbar. |
| REQ-F-053 | Must | Jede Bewerbung MUSS einen fachlichen Status besitzen. | Der aktuelle Status ist in Detail-, Listen- und Boardansicht konsistent sichtbar. |
| REQ-F-054 | Must | Version 1 MUSS mindestens Standardphasen für vorgemerkt, Vorbereitung, beworben, Interview, Angebot und abgeschlossen bereitstellen. | Ein vollständiger Beispielvorgang kann durch diese Phasen geführt werden. |
| REQ-F-055 | Must | Abgeschlossene Ergebnisse MÜSSEN differenziert werden können. | Mindestens `angenommen`, `Angebot abgelehnt`, `Absage Arbeitgeber`, `zurückgezogen`, `keine Rückmeldung/geschlossen` sind unterscheidbar. |
| REQ-F-056 | Should | Statusstufen SOLLEN durch den Nutzer erweitert oder angepasst werden können. | Eine zusätzliche Phase kann angelegt werden, ohne bestehende Vorgänge zu beschädigen. |
| REQ-F-057 | Must | Statusänderungen MÜSSEN in der Timeline nachvollziehbar werden. | Nach einem Statuswechsel ist Zeitpunkt, alter und neuer Status erkennbar. |
| REQ-F-058 | Must | Die Bewerbungsakte MUSS alle verknüpften Kontakte, Aktivitäten, Aufgaben, Interviews, Dokumente und Commitments zugänglich machen. | Der Nutzer kann den gesamten Vorgang aus einer zentralen Detailansicht rekonstruieren. |
| REQ-F-059 | Must | Die Bewerbungsakte MUSS freie Notizen unterstützen. | Notizen können gespeichert, geändert und gesucht werden. |
| REQ-F-060 | Must | Der Nutzer MUSS ein erwartetes oder bereits genanntes Gehalt und weitere Konditionen bewerbungsspezifisch dokumentieren können. | Angaben einer Bewerbung können von den ursprünglichen Stellenanzeigenwerten abweichen. |
| REQ-F-061 | Should | Eine Bewerbung SOLL eine frei definierbare persönliche Bewertung beziehungsweise Attraktivität besitzen können. | Attraktivität kann geändert und für Sortierung/Filter verwendet werden. |
| REQ-F-062 | Must | Archivierte oder abgeschlossene Bewerbungen MÜSSEN erhalten und weiterhin durchsuchbar bleiben. | Archivierung entfernt Datensätze nicht aus Historie oder Suche, sofern Archivfilter aktiviert ist. |
| REQ-F-063 | Must | Der Nutzer MUSS eine Bewerbung bewusst zurückziehen können. | Rückzug wird als Outcome und Aktivität dokumentiert. |
| REQ-F-064 | Must | Der Nutzer MUSS einen Vorgang duplizieren können, ohne historische Aktivitäten oder Bewerbungsereignisse versehentlich zu kopieren. | Kopie enthält nur ausdrücklich ausgewählte Stammdaten. |
| REQ-F-065 | Should | Die Anwendung SOLL vor offensichtlich doppelten aktiven Bewerbungen warnen. | Bei gleicher Opportunity und ähnlichem Bewerbungsdatum erscheint ein Hinweis. |
| REQ-F-066 | Must | Eine Bewerbung DARF NICHT automatisch versendet oder extern eingereicht werden. | Im gesamten V1-Funktionsumfang existiert kein automatischer Auto-Apply-Prozess. |

## 10.6 Pipeline, Board und Statussicht

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-070 | Must | Aktive Bewerbungen MÜSSEN in einer Pipeline-/Boardansicht darstellbar sein. | Karten werden nach Status gruppiert angezeigt. |
| REQ-F-071 | Must | Ein Statuswechsel MUSS direkt aus der Boardansicht möglich sein. | Eine Karte kann in eine andere Phase verschoben oder entsprechend umgestellt werden. |
| REQ-F-072 | Must | Die Boardansicht MUSS wesentliche Kurzinfos anzeigen, ohne die Detailakte öffnen zu müssen. | Mindestens Unternehmen, Rolle, Status, nächster Schritt/Fälligkeit und Priorität sind erkennbar. |
| REQ-F-073 | Must | Abgeschlossene Vorgänge MÜSSEN standardmäßig aus der aktiven Pipeline ausgeblendet, aber gezielt einblendbar sein. | Filter `aktiv/archiviert/alle` funktioniert reproduzierbar. |
| REQ-F-074 | Should | Boardansicht SOLL nach Quelle, Unternehmen, Priorität oder Tags filterbar sein. | Mehrere Filter können den sichtbaren Bestand einschränken. |
| REQ-F-075 | Should | Der Nutzer SOLL die Reihenfolge innerhalb einer Statusphase nach relevanten Kriterien sortieren können. | Sortierung nach Fälligkeit oder Priorität ist möglich. |

## 10.7 Aktivitäten, Kommunikation und Timeline

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-080 | Must | Aktivitäten MÜSSEN getrennt von Status und Aufgaben verwaltet werden. | Ein Telefonat ändert nicht automatisch den Status und bleibt als eigenständiges Ereignis erhalten. |
| REQ-F-081 | Must | Mindestens Bewerbung, E-Mail, Telefonat, LinkedIn/Netzwerk, Meeting, Interview, Dokumentversand, Angebot, Absage, Statusänderung und freie Notiz MÜSSEN als Aktivität abbildbar sein. | Für jeden Typ kann ein Beispielereignis angelegt und in der Timeline dargestellt werden. |
| REQ-F-082 | Must | Aktivitäten MÜSSEN Zeitpunkt, Typ, Beschreibung und relevante Verknüpfungen speichern können. | Ein Telefonat kann mit Bewerbung, Unternehmen und Kontakt verknüpft werden. |
| REQ-F-083 | Must | Die Bewerbungsakte MUSS eine chronologisch sortierte Timeline anzeigen. | Ereignisse mehrerer Typen erscheinen in korrekter zeitlicher Reihenfolge. |
| REQ-F-084 | Must | Die Timeline MUSS zwischen bereits geschehenen Ereignissen und zukünftigen Aufgaben/Terminen unterscheiden. | Vergangenes Telefonat und zukünftiges Interview werden visuell/fachlich unterscheidbar dargestellt. |
| REQ-F-085 | Must | Manuell angelegte Aktivitäten MÜSSEN korrigierbar sein. | Tippfehler und falsche Zeitpunkte können geändert werden; Korrektur zerstört nicht unbemerkt abhängige Daten. |
| REQ-F-086 | Should | Das Löschen einer Aktivität SOLL eine Bestätigung erfordern, wenn Verknüpfungen oder Folgeinformationen betroffen sind. | Löschen eines verknüpften Commitments führt zu Warnung oder definierter Folgebehandlung. |
| REQ-F-087 | Must | Kommunikation MUSS manuell protokolliert werden können, ohne dass ein externes Postfach verbunden ist. | Eine E-Mail oder LinkedIn-Nachricht kann mit Betreff/Kurzbeschreibung und Datum erfasst werden. |
| REQ-F-088 | Should | Eine Kommunikation SOLL optional einen Dateianhang oder externen Referenzlink besitzen können. | Beispielsweise kann eine `.eml`-Datei oder ein Screenshot als Dokument verknüpft werden. |
| REQ-F-089 | Could | Aktivitäten KÖNNEN über Vorlagen schneller erfasst werden. | Vorlage `Recruiter-Telefonat` legt sinnvolle Standardfelder vor. |

## 10.8 Next Action, Wiedervorlage und Commitments

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-090 | Must | Jede aktive Bewerbung MUSS eine nächste Aktion oder einen bewusst gesetzten Wartezustand besitzen können. | `Nächster Schritt: Interview vorbereiten` und `Warten bis 31.08.` sind beide abbildbar. |
| REQ-F-091 | Must | Aktive Bewerbungen ohne Next Action MÜSSEN im Dashboard erkennbar sein. | Ein Vorgang ohne nächste Aktion erscheint in einer eigenen Aufmerksamkeitssicht. |
| REQ-F-092 | Must | Next Actions MÜSSEN ein Fälligkeits- oder Wiedervorlagedatum besitzen können. | Nach Überschreiten des Datums wird der Eintrag als überfällig dargestellt. |
| REQ-F-093 | Must | Erledigte Next Actions MÜSSEN in die Historie übergehen können. | Nach Abschluss bleibt erkennbar, wann der Schritt erledigt wurde. |
| REQ-F-094 | Should | Eine Next Action SOLL direkt in eine Aufgabe oder einen Termin überführt werden können. | Aktion kann ohne erneute Dateneingabe als Task/Termin weitergeführt werden. |
| REQ-F-095 | Should | Eine Wiedervorlage SOLL verschoben beziehungsweise „gesnoozed“ werden können. | Neues Datum wird gespeichert; Historie der ursprünglichen Fälligkeit bleibt nachvollziehbar oder wird protokolliert. |
| REQ-F-096 | Must | Commitments anderer Personen MÜSSEN als eigener fachlicher Typ erfasst werden können. | `Recruiter meldet sich bis Freitag` ist nicht als eigene Aufgabe des Nutzers gespeichert. |
| REQ-F-097 | Must | Ein Commitment MUSS mindestens Beteiligten, Inhalt, Fälligkeit, Status und Bezug zum Vorgang speichern können. | Alle genannten Informationen sind in Detail- und Übersichtsansicht abrufbar. |
| REQ-F-098 | Must | Ein Commitment MUSS mit der auslösenden Aktivität oder Kommunikation verknüpft werden können. | Zusage aus einem Telefonat ist aus beiden Richtungen nachvollziehbar. |
| REQ-F-099 | Must | Überfällige, nicht erfüllte Commitments MÜSSEN auf dem Dashboard erscheinen. | Am Tag nach Fälligkeit erscheint die Zusage automatisch in der Überfällig-Sicht. |
| REQ-F-100 | Must | Ein Commitment MUSS als erfüllt, nicht erfüllt, entfallen oder verschoben markiert werden können. | Jeder Status ist speicherbar und historisch nachvollziehbar. |
| REQ-F-101 | Should | Aus einem überfälligen Commitment SOLL eine eigene Follow-up-Aufgabe erzeugt werden können. | `Nachfragen` kann direkt aus der Zusage erstellt werden. |

## 10.9 Aufgaben und Checklisten

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-105 | Must | Aufgaben MÜSSEN unabhängig und mit Bezug zu Bewerbung, Opportunity, Unternehmen oder Kontakt angelegt werden können. | Eine allgemeine Aufgabe und eine bewerbungsspezifische Aufgabe können parallel existieren. |
| REQ-F-106 | Must | Aufgaben MÜSSEN Titel, Status, Priorität, Fälligkeit und Notiz unterstützen. | Alle Felder können angelegt, geändert und gefiltert werden. |
| REQ-F-107 | Must | Aufgaben MÜSSEN als erledigt markiert werden können und danach historisch nachvollziehbar bleiben. | Erledigungszeitpunkt ist sichtbar. |
| REQ-F-108 | Should | Aufgaben SOLLEN einfache Checklisten beziehungsweise Unterpunkte unterstützen. | Interviewvorbereitung kann aus mehreren abhaktbaren Punkten bestehen. |
| REQ-F-109 | Must | Heute fällige und überfällige Aufgaben MÜSSEN im Dashboard erscheinen. | Tagesansicht entspricht den Testdaten für Fälligkeit. |
| REQ-F-110 | Should | Aufgaben SOLLEN nach Priorität, Fälligkeit, Status und Bezug filterbar sein. | Nutzer kann beispielsweise nur überfällige Aufgaben zu aktiven Bewerbungen anzeigen. |
| REQ-F-111 | Could | Wiederkehrende Aufgaben KÖNNEN innerhalb von V1.x unterstützt werden. | Wiederholungsregel erzeugt zukünftige Aufgabe ohne Duplikatfehler. |
| REQ-F-112 | Could | Aufgaben- oder Checklisten-Vorlagen KÖNNEN bereitgestellt werden. | Vorlage `Interview vorbereiten` kann wiederverwendet werden. |

## 10.10 Interviews und Gesprächsvorbereitung

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-120 | Must | Interviews MÜSSEN als eigenständige Ereignisse innerhalb einer Bewerbung verwaltet werden. | Eine Bewerbung kann mindestens drei getrennte Interviewdatensätze besitzen. |
| REQ-F-121 | Must | Ein Interview MUSS Datum, Uhrzeit, Format, Ort beziehungsweise Meeting-Link und Notizen aufnehmen können. | Online- und Präsenzinterview lassen sich vollständig erfassen. |
| REQ-F-122 | Must | Ein Interview MUSS mehrere Teilnehmer/Kontakte unterstützen. | Recruiter und zwei Fachansprechpartner können demselben Interview zugeordnet werden. |
| REQ-F-123 | Must | Interviewrunden MÜSSEN fachlich benennbar sein. | Beispielsweise `Recruiting`, `Fachinterview`, `Teamgespräch`, `Geschäftsführung`. |
| REQ-F-124 | Must | Der Nutzer MUSS Vorbereitungspunkte und eigene Fragen zum Interview dokumentieren können. | Fragen bleiben dem konkreten Interview zugeordnet. |
| REQ-F-125 | Must | Nach dem Interview MÜSSEN Gesprächsnotizen, Learnings und Follow-up festgehalten werden können. | Post-Interview-Notiz erscheint in der Bewerbungshistorie. |
| REQ-F-126 | Should | Der Nutzer SOLL das Interview nach eigenen Kriterien bewerten können. | Bewertung ist als persönliche Einschätzung gekennzeichnet und bleibt optional. |
| REQ-F-127 | Should | Wichtige Aussagen aus einem Interview SOLLEN als quellenbezogene Information in die Bewerbungsakte übernommen werden können. | Interviewaussage zu Remote-Regelung ist später zusammen mit anderen Quellen sichtbar. |
| REQ-F-128 | Should | Ein Interview SOLL automatisch beziehungsweise direkt eine vorbereitende Aufgabe und ein Follow-up anlegen können, wenn der Nutzer dies auslöst. | Nutzer kann aus dem Interview heraus entsprechende Tasks erzeugen. |
| REQ-F-129 | Could | Eine persönliche, wiederverwendbare Fragenbibliothek KANN in V1.x verfügbar sein. | Fragen können unabhängig vom einzelnen Interview gespeichert und ausgewählt werden. |

## 10.11 Dokumente und verwendete Dokumentversionen

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-130 | Must | Die Anwendung MUSS Lebensläufe, Anschreiben, Zeugnisse und weitere Bewerbungsdokumente verwalten können. | Mehrere Dokumente unterschiedlicher Kategorien können hinterlegt werden. |
| REQ-F-131 | Must | Mehrere Versionen desselben Dokumenttyps MÜSSEN unterscheidbar bleiben. | Zwei Lebensläufe mit unterschiedlichen Versionen können parallel bestehen. |
| REQ-F-132 | Must | Einer Bewerbung MÜSSEN genau die Dokumentversionen zugeordnet werden können, die tatsächlich verwendet beziehungsweise versendet wurden. | Monate später kann eindeutig gezeigt werden, welcher CV und welches Anschreiben verwendet wurden. |
| REQ-F-133 | Must | Dokumente MÜSSEN mit Titel, Kategorie, Version/Stand, Datum und optionalen Notizen versehen werden können. | Metadaten sind sichtbar und suchbar. |
| REQ-F-134 | Must | Stellenanzeigen-Snapshots MÜSSEN ebenfalls als historische Bewerbungsunterlagen erhalten bleiben können. | Jobanzeige bleibt nach Entfernen der externen Website verfügbar. |
| REQ-F-135 | Should | Die Anwendung SOLL erkennen können, wenn exakt dieselbe Datei mehrfach hinzugefügt wird. | Nutzer erhält bei identischer Datei einen Dublettenhinweis. |
| REQ-F-136 | Must | Fehlende oder extern verschobene Dokumentdateien DÜRFEN nicht zum Verlust der restlichen Bewerbungsakte führen. | Bewerbung öffnet sich weiterhin und zeigt den fehlenden Dokumentverweis verständlich an. |
| REQ-F-137 | Must | Der Nutzer MUSS Dokumente aus ihrer Akte öffnen beziehungsweise in der vorgesehenen Standardanwendung aufrufen können. | Unterstützte Datei kann über dokumentierten Benutzerbefehl geöffnet werden. |
| REQ-F-138 | Must | Das Löschen eines verwendeten Dokuments MUSS vor dem Entfernen auf bestehende Bewerbungszuordnungen hinweisen. | Nutzer wird gewarnt und muss bewusst entscheiden. |
| REQ-F-139 | Should | Dokumente SOLLEN nach Kategorie, Verwendung, Unternehmen oder Bewerbung auffindbar sein. | Suche/Filter liefert relevante Dokumente. |
| REQ-F-140 | Could | Die Anwendung KANN eine reine Metadaten-/Linkverwaltung zusätzlich zu verwalteten lokalen Kopien erlauben. | Nutzer kann wählen, ob ein Dokument referenziert oder in den verwalteten Bestand übernommen wird. |

## 10.12 Dashboard und Tagessteuerung

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-150 | Must | Das Dashboard MUSS heute fällige Aufgaben und Next Actions anzeigen. | Für Testdaten mit heutigem Fälligkeitsdatum erscheinen alle erwarteten Einträge. |
| REQ-F-151 | Must | Das Dashboard MUSS überfällige Aufgaben, Next Actions und Commitments hervorheben. | Überfällige Testeinträge sind getrennt von zukünftigen sichtbar. |
| REQ-F-152 | Must | Das Dashboard MUSS bevorstehende Interviews anzeigen. | Interviews der nächsten konfigurierten Tage erscheinen chronologisch. |
| REQ-F-153 | Must | Das Dashboard MUSS aktive Bewerbungen ohne definierte Next Action anzeigen. | Jeder aktive Testvorgang ohne Next Action wird gelistet. |
| REQ-F-154 | Must | Das Dashboard MUSS Vorgänge anzeigen können, bei denen der Nutzer bewusst auf eine Reaktion wartet. | Wartezustände sind getrennt von eigenen Aufgaben erkennbar. |
| REQ-F-155 | Should | Das Dashboard SOLL kompakte Kennzahlen zu aktiven Bewerbungen, Interviews, Angeboten und offenen Aufgaben anzeigen. | Kennzahlen stimmen mit Testdaten überein. |
| REQ-F-156 | Should | Dashboardbereiche SOLLEN direkt in passende Detail- oder Filteransichten führen. | Klick auf `überfällig` öffnet die zugrunde liegenden Einträge. |
| REQ-F-157 | Could | Der Nutzer KANN in V1.x Dashboardbereiche ein-/ausblenden oder anordnen. | Konfiguration bleibt erhalten. |

## 10.13 Suche, Filter und Ansichten

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-160 | Must | Die Anwendung MUSS eine globale Suche über zentrale fachliche Objekte anbieten. | Suche nach Firmenname, Kontaktname, Stellenbezeichnung und Notiz findet die erwarteten Datensätze. |
| REQ-F-161 | Must | Bewerbungen und Opportunities MÜSSEN in einer tabellarischen beziehungsweise Listenansicht darstellbar sein. | Nutzer kann mehrere Datensätze effizient vergleichen. |
| REQ-F-162 | Must | Filter MÜSSEN mindestens Status, Unternehmen, Quelle, Priorität, Tags und Zeitraum unterstützen. | Testfilter liefern korrekte Teilmengen. |
| REQ-F-163 | Should | Mehrere Filter SOLLEN kombinierbar sein. | Beispiel: `Status=Interview` UND `Remote` UND `Quelle=Recruiter`. |
| REQ-F-164 | Should | Häufig verwendete Filter SOLLEN als gespeicherte Ansicht abgelegt werden können. | Gespeicherte Sicht ist nach Neustart wieder verfügbar. |
| REQ-F-165 | Should | Termine und fällige Aktionen SOLLEN in einer Kalenderansicht darstellbar sein. | Interviews, Aufgaben und Wiedervorlagen eines Monats erscheinen am richtigen Datum. |
| REQ-F-166 | Must | Die Detailakte MUSS aus Such-, Listen-, Board- und Dashboardansicht erreichbar sein. | Jeder gezeigte Vorgang kann ohne Umweg geöffnet werden. |
| REQ-F-167 | Should | Listen SOLLEN nach relevanten Spalten sortierbar sein. | Mindestens Datum, Unternehmen, Status, Priorität und Fälligkeit sind sortierbar. |
| REQ-F-168 | Must | Tags MÜSSEN frei pflegbar und mehreren zentralen Objekten zuweisbar sein. | Ein Tag kann mehreren Bewerbungen/Unternehmen zugeordnet und gefiltert werden. |

## 10.14 Grundlegende Analytics

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-170 | Must | Die Anwendung MUSS die Anzahl aktiver, abgeschlossener und insgesamt erfasster Bewerbungen auswerten können. | Kennzahlen entsprechen dem Testdatenbestand. |
| REQ-F-171 | Must | Statusverteilung MUSS auswertbar sein. | Zahl pro Pipelinephase ist korrekt. |
| REQ-F-172 | Should | Response-, Interview- und Offer-Rate SOLLEN berechnet werden können. | Berechnungsdefinition ist dokumentiert und Ergebnis reproduzierbar. |
| REQ-F-173 | Should | Zeit bis zur ersten Rückmeldung SOLL auswertbar sein, sofern ausreichende Daten vorliegen. | Zeitraum wird aus Bewerbungs- und erster Rückmeldungsaktivität korrekt berechnet. |
| REQ-F-174 | Should | Ergebnisse SOLLEN nach Quelle verglichen werden können. | Nutzer kann z. B. Recruiter und Jobportal hinsichtlich Interviews/Outcomes vergleichen. |
| REQ-F-175 | Could | Erfolgswerte verschiedener Dokumentversionen KÖNNEN in V1.x verglichen werden. | Auswertung zeigt nur Fälle, in denen die verwendete Version eindeutig zugeordnet ist. |
| REQ-F-176 | Must | Analytics DÜRFEN niedrige Fallzahlen nicht als scheinbar belastbare Erkenntnis darstellen. | Kleine Stichproben werden sichtbar gekennzeichnet oder mit Fallzahl gezeigt. |
| REQ-F-177 | Should | Auswertungen SOLLEN nach Zeitraum filterbar sein. | Nutzer kann z. B. nur Bewerbungen eines Quartals auswerten. |
| REQ-F-178 | Could | Kennzahlen KÖNNEN als CSV oder vergleichbares offenes Format exportiert werden. | Export entspricht der sichtbaren Auswertung. |

## 10.15 Import, Export, Backup und Restore

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-180 | Must | Der Nutzer MUSS den vollständigen fachlichen Datenbestand sichern können. | Backup enthält alle fachlichen Datensätze sowie verwaltete Dokumente oder dokumentierte Referenzen. |
| REQ-F-181 | Must | Ein Backup MUSS wiederherstellbar sein. | Wiederherstellung in einer frischen Testinstallation führt zum fachlich gleichen Datenbestand. |
| REQ-F-182 | Must | Vor einer Wiederherstellung MUSS die Anwendung einen erkennbaren Schutz gegen versehentliches Überschreiben des aktuellen Bestands bieten. | Restore verlangt bewusste Bestätigung beziehungsweise bietet vorherige Sicherung an. |
| REQ-F-183 | Must | Der Nutzer MUSS seine zentralen Daten in einem offenen, dokumentierten Format exportieren können. | Export umfasst mindestens Unternehmen, Kontakte, Opportunities, Bewerbungen, Aktivitäten, Aufgaben und Interviews. |
| REQ-F-184 | Should | Dokumentmetadaten und Zuordnungen SOLLEN exportierbar sein. | Export lässt erkennen, welches Dokument welcher Bewerbung zugeordnet war. |
| REQ-F-185 | Should | Ein CSV-Import für einen pragmatischen Einstieg aus Tabellen SOLL unterstützt werden. | Mindestens einfache Bewerbungs-/Opportunity-Datensätze können mit Feldzuordnung importiert werden. |
| REQ-F-186 | Must | Ein Import DARF vorhandene manuell gepflegte Daten nicht stillschweigend überschreiben. | Konflikte werden angezeigt oder nach klar dokumentierter Regel behandelt. |
| REQ-F-187 | Should | Vor größeren Importen SOLL eine Vorschau mit Anzahl neuer, geänderter und problematischer Datensätze möglich sein. | Nutzer kann Import vor Ausführung abbrechen. |
| REQ-F-188 | Must | Backup- und Restore-Vorgänge MÜSSEN Erfolg oder Fehler eindeutig melden. | Fehlgeschlagene Sicherung darf nicht als erfolgreich dargestellt werden. |
| REQ-F-189 | Should | Backups SOLLEN eine prüfbare Integritätsinformation besitzen. | Manipuliertes oder beschädigtes Backup wird beim Restore erkannt. |
| REQ-F-190 | Could | V1.x KANN einen portablen Komplett-Export zur Migration auf einen anderen Rechner anbieten. | Testmigration stellt Daten und Dokumente vollständig her. |

## 10.16 Archivierung, Löschung und Datenpflege

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-200 | Must | Abgeschlossene Vorgänge MÜSSEN archiviert werden können, ohne ihre Historie zu verlieren. | Archivierter Vorgang bleibt such- und wiederherstellbar. |
| REQ-F-201 | Must | Der Nutzer MUSS archivierte Vorgänge wieder aktivieren können. | Reaktivierung stellt ursprüngliche Verknüpfungen wieder in aktive Ansichten. |
| REQ-F-202 | Must | Endgültiges Löschen MUSS von Archivierung klar unterschieden sein. | UI und Verhalten verhindern Verwechslung der beiden Aktionen. |
| REQ-F-203 | Must | Endgültiges Löschen verknüpfter Kernobjekte MUSS vor Datenfolgen warnen. | Löschen eines Unternehmens mit Bewerbungen ist nicht still möglich. |
| REQ-F-204 | Should | Der Nutzer SOLL veraltete Daten gezielt nach Alter, Status und Archivzustand auffinden können. | Filter kann z. B. abgeschlossene Vorgänge älter als zwei Jahre anzeigen. |
| REQ-F-205 | Could | Eine optionale Aufbewahrungs-/Bereinigungsunterstützung KANN in V1.x angeboten werden. | System schlägt alte Daten vor, löscht aber nicht ohne Bestätigung. |

## 10.17 Einstellungen und Anpassbarkeit

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-F-210 | Must | Nutzerdefinierte Tags, Quellen und grundlegende Listenwerte MÜSSEN pflegbar sein. | Neue Quelle `Empfehlung` kann angelegt und verwendet werden. |
| REQ-F-211 | Should | Pipelinephasen SOLLEN mit verständlichen Regeln konfigurierbar sein. | Reihenfolge und Bezeichnung können geändert werden, bestehende Daten bleiben gültig. |
| REQ-F-212 | Should | Standardzeiträume für Dashboard und bevorstehende Interviews SOLLEN konfigurierbar sein. | Änderung wirkt auf die Anzeige und bleibt gespeichert. |
| REQ-F-213 | Should | Datums-/Zeitdarstellung und grundlegende UI-Präferenzen SOLLEN anpassbar sein. | Präferenzen bleiben nach Neustart erhalten. |
| REQ-F-214 | Could | V1.x KANN benutzerdefinierte Zusatzfelder für ausgewählte Objekte unterstützen. | Ein neues Feld lässt sich hinzufügen, anzeigen und exportieren. |

---

# 11. Qualitätsanforderungen

## 11.1 Bedienbarkeit

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-Q-001 | Must | Häufige Kernaktionen MÜSSEN mit wenigen, nachvollziehbaren Schritten erreichbar sein. | Neue Opportunity plus Bewerbung kann im Usability-Test ohne unnötige Dialogketten angelegt werden. |
| REQ-Q-002 | Must | Die Oberfläche MUSS konsistente Begriffe für Company, Kontakt, Opportunity, Stellenanzeige, Bewerbung, Aktivität, Aufgabe und Commitment verwenden. | Begriffe werden in UI und Dokumentation nicht widersprüchlich verwendet. |
| REQ-Q-003 | Must | Fehlermeldungen MÜSSEN die Auswirkung und eine sinnvolle nächste Handlung nennen. | Testfehler wie fehlende Datei oder gesperrtes Backup führen zu verständlicher Meldung. |
| REQ-Q-004 | Must | Destruktive Aktionen MÜSSEN klar von normalen Bearbeitungsaktionen unterscheidbar sein. | Lösch-/Restore-Test zeigt explizite Bestätigung und Konsequenz. |
| REQ-Q-005 | Should | Die Anwendung SOLL weitgehend per Tastatur bedienbar sein. | Hauptnavigation, Dateneingabe und Dialogbestätigung sind ohne Maus möglich. |
| REQ-Q-006 | Must | Information DARF NICHT ausschließlich über Farbe vermittelt werden. | Status und Warnungen besitzen zusätzlich Text, Icon oder andere Semantik. |
| REQ-Q-007 | Should | Die Oberfläche SOLL bei üblichen Windows-Skalierungen von 100 % bis 200 % ohne abgeschnittene Kerninhalte nutzbar bleiben. | Stichprobentest auf mehreren Skalierungsstufen. |
| REQ-Q-008 | Should | Relevante Bedienelemente SOLLEN zugängliche Namen/Labels für unterstützende Technologien besitzen. | Automatisierter oder manueller Accessibility-Check der Hauptdialoge. |

## 11.2 Performance und Skalierbarkeit

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-Q-010 | Must | Die Hauptansichten MÜSSEN bei einem Referenzbestand von 10.000 Vorgängen und 50.000 Aktivitäten im Regelfall innerhalb von 1 Sekunde auf Benutzeraktionen reagieren. | Messung auf definierter Referenzhardware; 95. Perzentil für typische Filter-/Listenaktionen ≤ 1 s, ausgenommen dokumentierte schwere Operationen. |
| REQ-Q-011 | Must | Eine globale Standardsuche MUSS bei Referenzbestand innerhalb von 2 Sekunden ein Ergebnis liefern. | 95. Perzentil ≤ 2 s auf Referenzhardware. |
| REQ-Q-012 | Should | Programmstart mit Referenzbestand SOLL innerhalb von 5 Sekunden zur bedienbaren Hauptansicht führen. | Messung auf definierter Referenzhardware. |
| REQ-Q-013 | Must | Längere Operationen wie Backup, Restore oder großer Import DÜRFEN die Anwendung nicht scheinbar eingefroren wirken lassen. | Fortschritt beziehungsweise klarer Beschäftigungszustand ist sichtbar; Abbruchfähigkeit wird dort angeboten, wo sicher möglich. |

## 11.3 Zuverlässigkeit und Datenintegrität

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-Q-020 | Must | Erfolgreich bestätigte Speichervorgänge DÜRFEN bei einem normalen Neustart nicht verloren gehen. | Wiederholter Save-/Restart-Test zeigt konsistente Daten. |
| REQ-Q-021 | Must | Ein unerwarteter Programmabbruch DARF den gesamten Datenbestand nicht unbrauchbar machen. | Fehler-/Crash-Test lässt Anwendung mit konsistentem Bestand erneut starten oder stellt einen dokumentierten Recovery-Pfad bereit. |
| REQ-Q-022 | Must | Backup/Restore MUSS fachliche Beziehungen erhalten. | Nach Restore sind Beziehungen Company–Contact–Opportunity–Application–Activity identisch. |
| REQ-Q-023 | Must | Datenmigrationen zwischen kompatiblen V1.x-Releases DÜRFEN vorhandene fachliche Daten nicht still verlieren. | Upgrade-Test mit Referenzdatensatz und Vorher-/Nachher-Vergleich. |
| REQ-Q-024 | Must | Fehlende externe Dokumentdateien DÜRFEN den restlichen Datenbestand nicht blockieren. | Datensatz bleibt zugänglich und Fehler ist lokal auf den Anhang begrenzt. |

## 11.4 Offline-Fähigkeit und Netzabhängigkeit

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-Q-030 | Must | Alle V1-Kernfunktionen MÜSSEN ohne Internetverbindung verfügbar sein. | Vollständiger Abnahmelauf mit deaktiviertem Netzwerk ist möglich; externe Links sind naturgemäß nicht erreichbar. |
| REQ-Q-031 | Must | Fehlende Internetverbindung DARF lokale Funktionen nicht unnötig blockieren. | Offline-Test erlaubt Anlegen, Bearbeiten, Suche, Timeline, Dokumente, Backup und Restore. |

## 11.5 Sprache und Verständlichkeit

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-Q-040 | Must | Version 1 MUSS vollständig in deutscher Sprache bedienbar sein. | Keine produktiven Kernansichten enthalten unbeabsichtigte englische Platzhalter. |
| REQ-Q-041 | Should | Texte und Fachbegriffe SOLLEN so strukturiert sein, dass eine spätere Lokalisierung möglich bleibt. | UI-Texte sind nicht fachlich an deutsche Werte gekoppelt, wo dies vermeidbar ist. |

---

# 12. Sicherheits- und Datenschutzanforderungen

Bewerbungsdaten können Lebensläufe, Privatadressen, Telefonnummern, Gehaltsinformationen, Gesprächsnotizen, E-Mail-Inhalte und personenbezogene Daten von Ansprechpartnern enthalten. Der Schutzbedarf wird deshalb mindestens als **erhöht** betrachtet.

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-SEC-001 | Must | Personenbezogene Bewerbungsdaten MÜSSEN standardmäßig lokal verarbeitet werden. | Normalbetrieb erzeugt keine externe Datenübertragung. |
| REQ-SEC-002 | Must | Die Anwendung DARF ohne ausdrückliches Opt-in keine Telemetrie mit personenbezogenen oder inhaltlichen Bewerbungsdaten übertragen. | Netzwerk-/Konfigurationsprüfung zeigt keine unerwarteten Übertragungen. |
| REQ-SEC-003 | Must | Externe Übertragungen oder Online-Aufrufe MÜSSEN für den Benutzer erkennbar und zweckgebunden sein. | Öffnen einer Job-URL erfolgt nur nach Benutzeraktion. |
| REQ-SEC-004 | Must | Importierte Texte, URLs und Dateien MÜSSEN als nicht vertrauenswürdige Eingaben behandelt werden. | Sicherheitsprüfung deckt relevante Missbrauchsfälle ab. |
| REQ-SEC-005 | Must | Das Anzeigen oder Importieren einer Stellenbeschreibung DARF eingebettete aktive Inhalte nicht unkontrolliert ausführen. | Test mit HTML/Script-Inhalt führt zu keiner Scriptausführung im Anwendungskontext. |
| REQ-SEC-006 | Must | Importierte Dokumente DÜRFEN durch die Anwendung nicht automatisch ausgeführt werden. | Dokument wird nur nach expliziter Benutzeraktion an geeignete Anwendung übergeben. |
| REQ-SEC-007 | Must | Diagnosedaten und Logs MÜSSEN standardmäßig keine vollständigen Lebensläufe, E-Mail-Texte oder vergleichbar sensible Inhalte protokollieren. | Logprüfung mit Testdaten zeigt keine vollständigen sensiblen Inhalte. |
| REQ-SEC-008 | Must | Endgültiges Löschen personenbezogener Daten MUSS durch den Nutzer möglich sein, soweit keine verbleibenden Abhängigkeiten bewusst erhalten werden. | Testdatensatz kann nach Abhängigkeitsauflösung vollständig entfernt werden. |
| REQ-SEC-009 | Must | Datenexport MUSS dem Nutzer ermöglichen, seinen eigenen Datenbestand unabhängig vom Produkt zu sichern. | Offener Export lässt zentrale fachliche Daten lesen. |
| REQ-SEC-010 | Should | Backups mit sensiblen Daten SOLLEN optional geschützt beziehungsweise verschlüsselt werden können. | Geschütztes Backup ist ohne vorgesehenen Schlüssel/Passwort nicht lesbar. |
| REQ-SEC-011 | Must | Die Anwendung DARF in Version 1 keine Zugangsdaten für E-Mail-/Cloud-Dienste verlangen, da diese Integrationen nicht zum Scope gehören. | Kein V1-Kernworkflow fordert solche Credentials. |
| REQ-SEC-012 | Must | Externe Links MÜSSEN als externe Navigation erkennbar sein. | Nutzer kann unterscheiden, ob er lokale Daten öffnet oder den Browser startet. |
| REQ-SEC-013 | Must | Datei- und Backupoperationen MÜSSEN gegen unbeabsichtigtes Überschreiben beziehungsweise Pfadmissbrauch geschützt sein. | Negative Tests mit ungültigen/gefährlichen Pfaden führen zu sicherer Ablehnung. |
| REQ-SEC-014 | Should | Datenschutzrelevante Standardwerte SOLLEN Datenminimierung fördern. | Nicht benötigte Felder sind optional; keine unnötige Pflichtsammlung personenbezogener Daten. |
| REQ-SEC-015 | Must | Zukünftige optionale Online- oder KI-Funktionen DÜRFEN nicht vorausgesetzt werden, um lokale V1-Daten lesen oder verwalten zu können. | Kernbestand bleibt ohne externe API nutzbar. |

---

# 13. Daten- und Integritätsanforderungen

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-DATA-001 | Must | Zentrale fachliche Datensätze MÜSSEN stabile interne Identitäten besitzen. | Umbenennen eines Unternehmens zerstört keine Verknüpfungen. |
| REQ-DATA-002 | Must | Erstellungs- und Änderungszeitpunkte wesentlicher Datensätze MÜSSEN nachvollziehbar sein. | Zeitstempel sind für definierte Kernobjekte vorhanden. |
| REQ-DATA-003 | Must | Beziehungen zwischen Unternehmen, Kontakten, Opportunities, Bewerbungen, Aktivitäten und Dokumenten MÜSSEN referenziell konsistent bleiben. | Konsistenztest findet keine verwaisten Pflichtbeziehungen nach normalen Operationen. |
| REQ-DATA-004 | Must | Eine Statusänderung DARF die historische Timeline nicht löschen oder ersetzen. | Mehrere Statuswechsel bleiben chronologisch nachvollziehbar. |
| REQ-DATA-005 | Must | Änderungen an Stammdaten DÜRFEN historische, bereits verwendete Dokumentversionen nicht unkenntlich machen. | Änderung eines aktuellen CV-Titels ändert nicht automatisch den dokumentierten Versandbezug einer alten Bewerbung. |
| REQ-DATA-006 | Must | Originalsnapshots und eigene Notizen MÜSSEN fachlich getrennt sein. | Bearbeitung einer Notiz verändert nicht den archivierten Originaltext der Stellenanzeige. |
| REQ-DATA-007 | Should | Herkunft/Provenienz wichtiger Aussagen SOLL gespeichert werden können. | Quelle und Datum sind für strukturierte Aussagen abrufbar. |
| REQ-DATA-008 | Must | Konflikthafte Importdaten DÜRFEN manuell gepflegte Daten nicht still überschreiben. | Importtest erzeugt Konfliktbehandlung statt lautloser Ersetzung. |
| REQ-DATA-009 | Must | Textdaten MÜSSEN Unicode-Zeichen zuverlässig speichern und exportieren können. | Test mit Umlauten, Akzenten und nicht-lateinischen Zeichen bleibt verlustfrei. |
| REQ-DATA-010 | Must | Datum und Uhrzeit MÜSSEN eindeutig genug gespeichert werden, dass zeitliche Reihenfolgen rekonstruierbar bleiben. | Timeline sortiert Ereignisse korrekt auch bei gleichem Datum mit unterschiedlichen Zeiten. |
| REQ-DATA-011 | Must | Exportformate MÜSSEN dokumentiert sein. | Felder, Datumsformate und Beziehungen sind in Nutzerdokumentation oder Exportbeschreibung erläutert. |
| REQ-DATA-012 | Must | Ein Komplettbackup MUSS alle zur Wiederherstellung notwendigen Informationen enthalten oder deren externe Abhängigkeiten eindeutig dokumentieren. | Restore-Test auf frischem System ist erfolgreich oder meldet bewusst externe fehlende Dateien. |
| REQ-DATA-013 | Should | Dokumentdateien SOLLEN eine Integritäts-/Identitätsinformation erhalten, um identische Dateien und Veränderungen zu erkennen. | Identische Testdatei wird als gleich erkannt. |
| REQ-DATA-014 | Must | Archivierung DARF keine versteckte Löschung fachlicher Historie bedeuten. | Archivierter Datensatz bleibt vollständig wiederherstellbar. |
| REQ-DATA-015 | Should | Eine spätere Erweiterung des Datenmodells innerhalb V1.x SOLL migrationsfähig sein. | Upgradepfad wird vor Release mit Referenzdaten geprüft. |

---

# 14. Betriebs-, Diagnose- und Wartungsanforderungen aus Nutzersicht

| ID | Prio | Anforderung | Akzeptanzkriterium / Nachweis |
|---|---|---|---|
| REQ-OPS-001 | Must | Die Anwendung MUSS auf der vorgesehenen Zielplattform reproduzierbar installierbar sein. | Installation auf einer sauberen unterstützten Umgebung führt zu lauffähiger Anwendung. |
| REQ-OPS-002 | Must | Ein Update innerhalb der V1-Linie DARF vorhandene Nutzerdaten nicht löschen. | Upgrade-Test mit bestehendem Datenbestand erfolgreich. |
| REQ-OPS-003 | Must | Deinstallation und Nutzerdatenlöschung MÜSSEN konzeptionell getrennt sein. | Entfernen der Anwendung löscht Nutzerdaten nicht ohne explizite Entscheidung oder dokumentierte Option. |
| REQ-OPS-004 | Must | Die Anwendung MUSS eine verständliche Versionsinformation anzeigen. | Version ist aus der Oberfläche abrufbar. |
| REQ-OPS-005 | Must | Bei Fehlern MUSS eine Diagnosemöglichkeit existieren, die ohne Offenlegung unnötiger personenbezogener Inhalte genutzt werden kann. | Diagnosebericht enthält technische Metadaten, aber keine vollständigen sensiblen Bewerbungsinhalte standardmäßig. |
| REQ-OPS-006 | Should | Vor riskanten Datenmigrationen SOLL eine Sicherung empfohlen oder automatisiert angeboten werden. | Upgrade-/Restore-Workflow enthält Sicherungshinweis oder gleichwertigen Schutz. |
| REQ-OPS-007 | Must | Die Anwendung MUSS eine Benutzerhilfe für Backup, Restore, Export und Datenlöschung bereitstellen. | Ein Nutzer kann diese kritischen Operationen ohne Quellcodekenntnis korrekt durchführen. |
| REQ-OPS-008 | Should | Release Notes SOLLEN für V1.x Änderungen an Daten, Funktionen und bekannten Einschränkungen verständlich dokumentieren. | Update enthält nachvollziehbare Änderungsinformationen. |

---

# 15. Constraints und verbindliche Abgrenzungen

| ID | Prio | Constraint |
|---|---|---|
| REQ-CON-001 | Must | Version 1 ist primär ein persönlicher Einzelbenutzer-Bewerbungsmanager, kein Unternehmens-ATS. |
| REQ-CON-002 | Must | Der Kernbetrieb ist local-first und offline-fähig. |
| REQ-CON-003 | Must | Keine Bewerbung wird automatisch abgesendet. |
| REQ-CON-004 | Must | Keine externe E-Mail-/Kalender-/Cloud-Integration ist Voraussetzung für Version 1. |
| REQ-CON-005 | Must | Personen-, Unternehmens-, Opportunity-, Stellenanzeigen- und Bewerbungsobjekte bleiben fachlich unterscheidbar. |
| REQ-CON-006 | Must | Status, Aktivität, Aufgabe, Next Action und Commitment bleiben fachlich unterscheidbar. |
| REQ-CON-007 | Must | Backup und Restore sind Release-Gate-Funktionen und dürfen nicht auf eine spätere Version verschoben werden. |
| REQ-CON-008 | Must | Datenschutz- und Datenintegritätsanforderungen dürfen nicht allein wegen fehlender Sichtbarkeit im UI niedrig priorisiert werden. |
| REQ-CON-009 | Should | Für die Umsetzung sollen die anwendbaren SASD-Profile `Core`, `C#/.NET` und `Desktop` geprüft werden. Die konkrete UI-Technologie bleibt Entscheidung des Pflichtenhefts/der Architektur. |
| REQ-CON-010 | Must | Die primäre Zielsprache von Version 1 ist Deutsch. |
| REQ-CON-011 | Must | Eine Windows-Desktop-Nutzung ist als primäre Betriebsannahme für Version 1 zu unterstützen. Die konkrete technische Umsetzung wird separat spezifiziert. |
| REQ-CON-012 | Must | Code oder Funktionen aus Referenzprodukten dürfen nicht ungeprüft übernommen werden; Lizenz- und Herkunftsfragen sind vor Wiederverwendung zu klären. |

---

# 16. Aus den Referenzprodukten bewusst übernommene Produktmuster

Die folgenden Muster wurden aus mehreren Referenzen abgeleitet. Sie sind Inspiration, keine 1:1-Kopie.

| Produktmuster | Hauptreferenzen | Umsetzungsidee für SASD V1 |
|---|---|---|
| vollständige Bewerbungsakte | Huntr, Teal, Workable | Zentraler Vorgang mit Kontakten, Dokumenten, Timeline, Interviews und Aufgaben |
| Next Action | Pipedrive, Todoist, Teal | Jeder aktive Vorgang soll einen nächsten Schritt oder Wartezustand besitzen |
| Kontaktgedächtnis | Dex, Pipedrive, Monica | Recruiter/Ansprechpartner als wiederverwendbare Personenobjekte |
| Local first | JSE, Jobtra, JobNest, JobSync | Kernbetrieb ohne Cloudkonto und Internet |
| mehrere Interviewrunden | JobTrail, Greenhouse, Workable | Interviews als eigene Datensätze statt nur Status `Interview` |
| Dokumentversion pro Bewerbung | Huntr, Jobscan, Teal, JobSync | Eindeutig nachvollziehbarer versendeter CV/Anschreiben-Stand |
| Timeline | Huntr, Workable, Greenhouse, Pipedrive | Chronologische Vorgangsakte über alle wichtigen Ereignisse |
| Basic Analytics | JSE, Greenhouse, Lever, Careerflow | Response-/Interview-/Offer-Rate ohne Scheingenauigkeit |
| kontrollierte Automation | JobTrackerPro, JobOps | In V1 noch manuell; spätere Automation soll Vorschläge statt unkontrollierter Änderungen bevorzugen |
| Source/Provenance | JSE, ATS-Muster | Aussagen können nach Quelle und Zeitpunkt unterschieden werden |
| Commitment Tracking | aus CRM-/Follow-up-Mustern weiterentwickelt | Zusagen anderer Personen als eigenständiger, überwachbarer Typ |

---

# 17. Bewusst verschobene Funktionen und Begründung

## 17.1 Automatische Jobquellen und Scraping – Won't V1

**Begründung:** Hohe Wartungskosten, rechtliche und technische Abhängigkeiten von fremden Plattformen, wechselnde HTML-Strukturen und Gefahr, den Kern des Produkts zu überladen.

**Spätere Option:** Provider-/Source-Adapter mit klaren Vertrauensgrenzen und Nutzerfreigabe.

## 17.2 E-Mail-Synchronisierung – Won't V1

**Begründung:** OAuth, IMAP, Credentials, Datenschutz, Entity Matching und Fehlklassifikation erzeugen eine erhebliche Sicherheits- und Testdimension.

**Spätere Option:** Zunächst Import/Forwarding oder nutzerbestätigte Vorschläge; automatische Statusänderungen nur kontrolliert.

## 17.3 Kalender-Synchronisierung – Won't V1

**Begründung:** Externe APIs und Authentifizierung sind nicht erforderlich, um Termine intern zuverlässig zu verwalten.

**Spätere Option:** Optionaler Export/Sync nach stabiler V1-Basis.

## 17.4 KI-Funktionen – Won't V1

**Begründung:** Version 1 soll beweisen, dass das fachliche Datenmodell und der Workflow eigenständig wertvoll sind. KI soll später gezielt und nachvollziehbar auf einer belastbaren Datenbasis aufsetzen, nicht fehlende Produktlogik kaschieren.

## 17.5 Browser-Erweiterung und Autofill – Won't V1

**Begründung:** Plattformabhängigkeit und Wartung vieler fremder Bewerbungsformulare stehen nicht im Verhältnis zum V1-Kernnutzen.

## 17.6 Cloud-Synchronisierung und Mehrbenutzerbetrieb – Won't V1

**Begründung:** Local-first und persönlicher Einzelbenutzerbetrieb reduzieren Komplexität und Angriffsfläche erheblich. Ein späterer Sync darf die lokale Datenhoheit nicht rückwirkend schwächen.

---

# 18. End-to-End-Abnahmefälle für Version 1.0

Die folgenden Szenarien dienen als fachliche Release-Gates. Detaillierte Testfälle werden später abgeleitet.

## AT-001 – Neue Stelle vollständig erfassen

**Gegeben:** Ein neues Unternehmen und eine externe Stellenanzeige.  
**Wenn:** Der Nutzer Unternehmen, Opportunity, URL, Quelle, Snapshot, Gehalt, Standort, Arbeitsmodell und Priorität erfasst.  
**Dann:** Der Vorgang ist lokal gespeichert, suchbar, in Listenansicht sichtbar und noch nicht fälschlich als Bewerbung markiert.

## AT-002 – Bewerbung mit Dokumentversionen versenden/protokollieren

**Gegeben:** Eine bestehende Opportunity mit zwei CV-Versionen.  
**Wenn:** Der Nutzer eine Bewerbung protokolliert und CV-Version B sowie Anschreiben C als verwendet markiert.  
**Dann:** Bewerbungsdatum, Bewerbungsweg und exakte Dokumentzuordnung sind später eindeutig rekonstruierbar.

## AT-003 – Recruiter-Telefonat und Zusage

**Gegeben:** Eine aktive Bewerbung.  
**Wenn:** Ein Recruiter-Telefonat protokolliert und die Zusage `Rückmeldung bis Freitag` erfasst wird.  
**Dann:** Telefonat erscheint in Timeline, Kontakt ist verknüpft und Commitment besitzt Fälligkeit.

## AT-004 – Überfällige Zusage

**Gegeben:** Commitment aus AT-003 ist nach Freitag nicht erfüllt.  
**Wenn:** Die Anwendung am Folgetag geöffnet wird.  
**Dann:** Commitment erscheint sichtbar als überfällig und kann direkt in eine Follow-up-Aufgabe überführt werden.

## AT-005 – Aktive Bewerbung ohne nächsten Schritt

**Gegeben:** Eine aktive Bewerbung besitzt weder Next Action noch Wartezustand.  
**Wenn:** Das Dashboard geöffnet wird.  
**Dann:** Der Vorgang erscheint in `ohne nächste Aktion`.

## AT-006 – Drei Interviewrunden

**Gegeben:** Eine Bewerbung im Interviewprozess.  
**Wenn:** Recruitinggespräch, Fachinterview und Teamgespräch mit unterschiedlichen Teilnehmern angelegt werden.  
**Dann:** Alle drei Interviews besitzen eigene Daten, Notizen und chronologische Position in der Timeline.

## AT-007 – Widersprüchliche Remote-Aussagen

**Gegeben:** Stellenanzeige sagt `Hybrid`, Recruiter sagt `3 Tage remote`, Fachabteilung sagt `nach Einarbeitung flexibel`.  
**Wenn:** Die Informationen quellenbezogen erfasst werden.  
**Dann:** Keine Aussage wird zwangsläufig überschrieben; alle bleiben mit Herkunft sichtbar.

## AT-008 – Mehrere Rollen beim selben Unternehmen

**Gegeben:** Ein Unternehmen mit zwei unterschiedlichen Opportunities.  
**Wenn:** Bewerbungen auf beide Rollen erfasst werden.  
**Dann:** Beide Vorgänge teilen Unternehmens-/Kontaktdaten, behalten aber eigene Bewerbungshistorien.

## AT-009 – Frühere Recruiter-Beziehung wiederfinden

**Gegeben:** Eine neun Monate alte archivierte Bewerbung und derselbe Recruiter meldet sich erneut.  
**Wenn:** Kontakt gesucht und neue Opportunity angelegt wird.  
**Dann:** Frühere Interaktionen sind auffindbar, neue Opportunity bleibt fachlich getrennt.

## AT-010 – Abschluss und Archiv

**Gegeben:** Eine aktive Bewerbung erhält eine Absage.  
**Wenn:** Outcome gesetzt und Vorgang archiviert wird.  
**Dann:** Vorgang verschwindet aus aktiver Pipeline, bleibt aber vollständig such- und auswertbar.

## AT-011 – Backup und Restore

**Gegeben:** Referenzdaten mit Unternehmen, Kontakten, Bewerbungen, Dokumenten, Interviews, Aufgaben und Aktivitäten.  
**Wenn:** Komplettbackup erstellt und in frischer Umgebung wiederhergestellt wird.  
**Dann:** Alle fachlichen Datensätze, Beziehungen und verwalteten Dokumente sind vollständig vorhanden.

## AT-012 – Offline-Betrieb

**Gegeben:** Netzwerkverbindung ist deaktiviert.  
**Wenn:** Nutzer Kernoperationen ausführt.  
**Dann:** Anlegen, Bearbeiten, Suche, Dashboard, Timeline, Aufgaben, Dokumentverwaltung, Backup und Restore funktionieren weiterhin.

## AT-013 – Datenexport

**Gegeben:** Ein realitätsnaher Datenbestand.  
**Wenn:** Gesamtexport ausgelöst wird.  
**Dann:** Zentrale Daten liegen in dokumentiertem, offenem Format vor und können unabhängig vom Programm gelesen beziehungsweise weiterverarbeitet werden.

## AT-014 – Datenintegrität nach Update

**Gegeben:** Datenbestand aus einem früheren V1.x-Release.  
**Wenn:** Anwendung auf eine neuere kompatible V1.x-Version aktualisiert wird.  
**Dann:** Alle Kernobjekte und Beziehungen bleiben erhalten; notwendige Migration wird erfolgreich protokolliert.

## AT-015 – Keine unerwartete externe Datenübertragung

**Gegeben:** Anwendung wird mit Testdaten und aktiviertem Netzwerk verwendet.  
**Wenn:** ausschließlich lokale V1-Kernfunktionen benutzt werden.  
**Dann:** Es werden keine Bewerbungsinhalte ohne explizite Benutzeraktion an externe Dienste übertragen.

---

# 19. Release-Gates für Version 1.0

Version 1.0 darf aus fachlicher Sicht erst freigegeben werden, wenn:

1. alle `Must`-Anforderungen entweder `Verified` oder mit formaler, akzeptierter Abweichung dokumentiert sind;
2. alle End-to-End-Abnahmefälle AT-001 bis AT-015 erfolgreich oder nachvollziehbar mit akzeptierter Abweichung abgeschlossen sind;
3. Backup und Restore mit realitätsnahem Referenzbestand erfolgreich verifiziert wurden;
4. keine bekannte kritische Datenverlustlücke besteht;
5. keine bekannte kritische Datenschutz- oder Sicherheitslücke besteht;
6. Datenexport dokumentiert und getestet wurde;
7. Upgrade-/Migrationstest mit mindestens einem Vorgängerstand erfolgt ist;
8. Benutzerhilfe für Datenmanagement vorhanden ist;
9. bekannte Einschränkungen in Release Notes dokumentiert sind;
10. der Scope von V1.0 gegen dieses Lastenheft überprüft wurde.

---

# 20. Vorgeschlagene fachliche Meilensteine

Die Meilensteine beschreiben fachliche Ergebnisse und sind keine technische Implementierungsreihenfolge.

## M1 – Kernakte

Ergebnis:

- Unternehmen;
- Kontakte;
- Opportunity;
- Stellenanzeige/Snapshot;
- Bewerbung;
- Status;
- Grundlisten und Detailakte.

**Abnahmewert:** Eine Bewerbung lässt sich strukturiert und dauerhaft erfassen.

## M2 – Verlauf und Tagessteuerung

Ergebnis:

- Aktivitäten;
- Timeline;
- Next Action;
- Aufgaben;
- Commitments;
- Dashboard.

**Abnahmewert:** Das System beantwortet „Was ist passiert und was ist als Nächstes zu tun?“

## M3 – Interviews und Dokumente

Ergebnis:

- mehrere Interviewrunden;
- Gesprächsvorbereitung;
- Dokumentbestand;
- verwendete Dokumentversionen;
- quellenbezogene Aussagen.

**Abnahmewert:** Ein realer mehrstufiger Bewerbungsprozess kann vollständig rekonstruiert werden.

## M4 – Finden und Verstehen

Ergebnis:

- globale Suche;
- Filter;
- Board/List/Kalendersicht;
- Grundanalytics;
- Archiv/Outcome.

**Abnahmewert:** Auch ein großer historischer Datenbestand bleibt nutzbar und auswertbar.

## M5 – Datenhoheit und Releasefähigkeit

Ergebnis:

- Export;
- Backup;
- Restore;
- Integritätsprüfung;
- Datenschutz-/Sicherheitsprüfung;
- Upgradepfad;
- Benutzerhilfe;
- Release- und Abnahmebericht.

**Abnahmewert:** Version 1.0 ist als dauerhaft nutzbares Produkt statt als Demo freigabefähig.

---

# 21. Risiken und Gegenmaßnahmen

| ID | Risiko | Wahrscheinlichkeit | Auswirkung | Gegenmaßnahme |
|---|---|---:|---:|---|
| R-001 | Scope Creep durch Übernahme zu vieler Funktionen der Referenzprodukte | Hoch | Hoch | Won't-V1-Liste verbindlich behandeln; Änderungen nach SASD-Change-Prozess bewerten. |
| R-002 | Zu komplexes Datenmodell erschwert erste Benutzung | Mittel | Hoch | Fachliche Tiefe hinter einfacher Erfassung verbergen; sinnvolle Defaults; progressive disclosure in der UI. |
| R-003 | Dokumentverwaltung führt zu Datei-/Pfadproblemen | Mittel | Hoch | Klare Ownership-Regel, Missing-File-Verhalten, Backup-Tests, Integritätsinformationen. |
| R-004 | Datenverlust durch Migration/Update | Niedrig-Mittel | Sehr hoch | Backup vor Migration, automatisierte Migrations- und Restore-Tests, Referenzdatensatz. |
| R-005 | Datenschutzverletzung durch spätere Onlinefunktionen | Mittel | Sehr hoch | Local-first als Baseline; externe Übertragung opt-in; Security Review vor jeder Integration. |
| R-006 | Pipeline wird wichtiger als Next Action und Timeline | Mittel | Mittel | Dashboard und Next Action als V1-Must-Anforderungen behandeln. |
| R-007 | Zu viele Freitextfelder verhindern spätere Suche/Analytics | Mittel | Mittel | Kerninformationen strukturiert plus ergänzender Freitext; Tags/Quellenmodell. |
| R-008 | Zu starre Struktur passt nicht zu unterschiedlichen Bewerbungsprozessen | Mittel | Mittel | konfigurierbare Statuswerte/Tags als Should; Grundmodell stabil halten. |
| R-009 | Analytics erzeugen Scheingenauigkeit bei kleinen Datenmengen | Hoch | Mittel | Fallzahlen sichtbar machen; keine unbegründeten Scores; Interpretation zurückhaltend. |
| R-010 | Referenzcode wird lizenzrechtlich unklar übernommen | Niedrig | Hoch | Referenzanalyse als Produktinspiration behandeln; Codeübernahme nur nach Lizenzprüfung. |

---

# 22. Annahmen und offene Punkte

| ID | Annahme / offene Frage | Status für Lastenheft |
|---|---|---|
| A-001 | Version 1 wird primär als Windows-Desktop-Anwendung eingesetzt. | Arbeitsannahme / im Pflichtenheft bestätigen |
| A-002 | Der erste Produktumfang ist Einzelbenutzerbetrieb. | Accepted für V1-Scope |
| A-003 | Local-first ist ein Produktmerkmal, nicht nur technische Präferenz. | Accepted für V1-Scope |
| A-004 | Deutsch ist die primäre UI-Sprache. | Accepted für V1-Scope |
| A-005 | Exakte technische Persistenzlösung ist nicht Bestandteil des Lastenhefts. | Offen für Architektur |
| A-006 | Umgang mit verwalteten Kopien vs. Dateireferenzen muss im Pflichtenheft endgültig entschieden werden. | Offen |
| A-007 | Optional verschlüsselte Backups sind Should; konkrete Kryptografie ist Architekturthema. | Offen für technische Spezifikation |
| A-008 | Individuelle Custom Fields sind nicht releasekritisch für 1.0. | Accepted / Could |
| A-009 | Direkte E-Mail- und Kalenderintegration wird frühestens nach stabiler V1-Datenbasis neu bewertet. | Accepted / Won't V1 |
| A-010 | Generative KI ist keine Voraussetzung für den Produkterfolg von V1. | Accepted / Won't V1 |

---

# 23. Traceability: Funktionsbereiche zu Referenzprogrammen

Diese Tabelle dokumentiert die Herkunft der Produktideen. Sie stellt **keine Lizenzfreigabe zur Codeübernahme** dar.

| Funktionsbereich | Berücksichtigte Hauptreferenzen |
|---|---|
| Bewerbungsakte | Huntr, Teal, Jobscan, JobTrail, Workable |
| Pipeline / Board | Huntr, Teal, JSE, Pipedrive, Greenhouse, Trello |
| Aktivitäten / Timeline | Huntr, Pipedrive, Dex, JobTrackerPro, Workable, Greenhouse |
| Next Action / Wiedervorlage | Pipedrive, Todoist, Teal, Careerflow, JSE |
| Aufgaben / Checklisten | Todoist, Teal, JobSync, Trello, Pipedrive |
| Interviews | JobTrail, Huntr, Greenhouse, Lever, Workable, JSE |
| Kontakte / Recruiter-CRM | Dex, Pipedrive, Monica, Huntr, Careerflow |
| Unternehmensakte | JSE, JobTrail, Pipedrive, Teal |
| Dokumentversionen | Huntr, JobSync, Jobtra, Jobscan, Simplify |
| E-Mail-Muster für spätere Versionen | JobTrackerPro, Jobtra, JobOps, Pipedrive, Workable |
| Kalender-/Terminmuster | Pipedrive, Workable, Todoist, Jobscan |
| Analytics | JSE, Greenhouse, Lever, Pipedrive, Careerflow |
| Local first / Privacy | JSE, Jobtra, JobSync, JobNest, JobOps |
| Erweiterbarkeit für spätere Versionen | Pipedrive, JSE, JobOps, JobSync, Todoist |
| Jobquellen für spätere Versionen | Simplify, JobOps, JobTrail, Bundesagentur, meinestadt.de |

---

# 24. SASD-Nachverfolgbarkeit

Das Lastenheft ist insbesondere auf folgende Regeln des aktuellen SASD Development Standard ausgerichtet:

- verständliche Problembeschreibung (`SASD-REQ-001`);
- Nutzen getrennt von technischer Lösung (`SASD-REQ-002`);
- Zielgruppen/Stakeholder (`SASD-REQ-003`);
- dokumentierter Scope und Nicht-Ziele (`SASD-REQ-010`, `SASD-REQ-011`);
- explizite Annahmen/offene Fragen (`SASD-REQ-012`);
- Unterscheidung funktionaler, qualitativer, Sicherheits-, Daten- und Betriebsanforderungen;
- stabile Anforderungskennungen für Recommended (`SASD-REQ-030`);
- prüfbare Akzeptanzkriterien (`SASD-REQ-040` ff.);
- Priorisierung (`SASD-REQ-050` ff.);
- Zuordnung von Anforderungen zu späteren Verifikationsnachweisen (`SASD-REQ-060`);
- dokumentierte Änderungsbewertung für freigegebenen Scope (`SASD-REQ-070` ff.);
- Datenschutz und Sicherheit als prüfbare Anforderungen (`SASD-SEC-010`);
- Datenminimierung und sichere Behandlung externer Eingaben (`SASD-SEC-040` ff.).

Für das Projekt wird vorläufig die Qualitätsstufe **Recommended** als angemessene Ausgangsbasis betrachtet. Aufgrund der sensiblen personenbezogenen Daten sind Security, Privacy, Datenintegrität, Backup und Restore risikobezogen mindestens mit erhöhter Tiefe zu behandeln.

---

# 25. Empfohlene Folgeartefakte

Nach fachlicher Freigabe dieses Lastenhefts sollten nicht unmittelbar alle Anforderungen in Code umgesetzt werden. Sinnvolle nächste Artefakte sind:

1. **Projektbrief** nach aktuellem SASD-Template, soweit noch nicht separat vorhanden.
2. **Glossar / Ubiquitous Language**, insbesondere für Opportunity, Job Posting, Application, Activity, Next Action und Commitment.
3. **fachliches Domänenmodell** mit Kardinalitäten und Lebenszyklen.
4. **Pflichtenheft** für Version 1.0: konkrete Umsetzung der Anforderungen.
5. **UX-/Navigationskonzept** für Dashboard, Akte, Board, Listen und Detailansichten.
6. **Datenmodell- und Persistenzspezifikation**.
7. **Security-/Privacy-Betrachtung** inklusive Schutzbedarf, Datenflüsse und Trust Boundaries.
8. **Backup-/Restore-Konzept** inklusive Verifikationsstrategie.
9. **Testkonzept und Traceability-Matrix** `REQ → Test/Abnahme`.
10. **Roadmap und Release-Schnitt** für 1.0 sowie mögliche 1.x-Erweiterungen.
11. **ADR-Sammlung** für wesentliche technische Entscheidungen.

---

# 26. Quellen

## 26.1 Interne Ausgangsbasis

- `Bewerbungsmanager_Referenzprogramme_Funktionskatalog.md`, Stand 24.08.2026.
- Darin untersuchte Referenzprogramme: Huntr, Teal, Careerflow, Jobscan, Simplify, JSE, JobSync, JobOps, JobTrackerPro, JobTrail, JobHunt, Jobtra, JobNest, Pipedrive, Dex, Monica, Todoist, Trello, Greenhouse, Lever, Workable, Bundesagentur für Arbeit – Jobsuche, meinestadt.de Jobs.

## 26.2 SASD Development Standard

- Repository: https://github.com/Robin-Goerlach/SASD-Development-Standard
- Quick Start: https://github.com/Robin-Goerlach/SASD-Development-Standard/blob/main/QUICKSTART.md
- Projektbrief-Template: https://github.com/Robin-Goerlach/SASD-Development-Standard/blob/main/templates/documents/PROJECT-BRIEF-TEMPLATE.md
- Anforderungsmanagement: https://github.com/Robin-Goerlach/SASD-Development-Standard/blob/main/docs/10-core-standard/REQUIREMENTS.md
- Sicherheitsstandard: https://github.com/Robin-Goerlach/SASD-Development-Standard/blob/main/docs/10-core-standard/SECURITY.md
- C#/.NET-Profil: https://github.com/Robin-Goerlach/SASD-Development-Standard/tree/main/docs/20-profiles/dotnet
- Desktop-Profil: https://github.com/Robin-Goerlach/SASD-Development-Standard/tree/main/docs/20-profiles/desktop

---

# 27. Freigabe

| Rolle | Name | Entscheidung | Datum | Bemerkung |
|---|---|---|---|---|
| Product Owner / Auftraggeber |  | offen |  |  |
| Entwicklung |  | offen |  |  |
| Review / Qualität |  | offen |  |  |

**Freigabestatus:** Draft – noch nicht formell freigegeben.

---

## Anhang A – Kurzfassung des V1-Kerns

Version 1.0 ist freigabefähig, wenn der Bewerbungsmanager zuverlässig folgende Fragen beantwortet:

1. **Welche beruflichen Möglichkeiten und Bewerbungen laufen aktuell?**
2. **Was ist bei jedem Vorgang bisher passiert?**
3. **Welche Person und welches Unternehmen gehören dazu?**
4. **Welche Unterlagen habe ich tatsächlich verwendet?**
5. **Welche Interviews fanden statt oder stehen an?**
6. **Was muss ich heute oder als Nächstes tun?**
7. **Auf welche zugesagte Rückmeldung warte ich?**
8. **Welche Informationen stammen aus welcher Quelle?**
9. **Wie ist der Vorgang ausgegangen?**
10. **Kann ich meinen gesamten Bestand sicher sichern, wiederherstellen und exportieren?**

Wenn diese zehn Fragen schnell, zuverlässig und nachvollziehbar beantwortet werden können, erfüllt Version 1 ihren Kernauftrag.
