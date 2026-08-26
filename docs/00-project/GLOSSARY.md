# Glossar und fachliche Begriffe

> **Status:** Fachliche Baseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## Zweck

Dieses Glossar verhindert, dass ähnliche Begriffe im Code, UI und in Dokumenten unterschiedliche Bedeutungen erhalten. Englische Typnamen dürfen im Code verwendet werden; die UI verwendet überwiegend deutsche Begriffe.

| Begriff | Codebegriff | Definition |
|---|---|---|
| Unternehmen | `Company` | Organisation, bei der eine Rolle besteht oder mit der ein Kontakt verbunden ist. |
| Kontakt | `Contact` | konkrete Person, z. B. Recruiter, HR, Fachansprechpartner oder Vermittler. |
| Opportunity | `Opportunity` | fachliche berufliche Chance; kann mehrere Anzeigen oder Bewerbungsstände überleben. |
| Stellenanzeige | `JobPosting` | konkrete veröffentlichte Beschreibung/Version einer Stelle einschließlich Quelle und Snapshot. |
| Bewerbung | `Application` | konkrete Bewerbung auf eine Opportunity mit eigenem Prozessstatus. |
| Status | `ApplicationStatus` | Pipelinezustand der Bewerbung, z. B. Beworben, Gespräch, Angebot, Abgeschlossen. |
| Statushistorie | `ApplicationStatusHistory` | unverlierbare Historie der Statuswechsel. |
| Aktivität | `Activity` | Ereignis in der Timeline, z. B. Bewerbung versandt, Anruf, Notiz, Einladung. |
| Kommunikation | `Communication` | strukturierte Details eines Kommunikationsvorgangs, ggf. als Aktivität repräsentiert. |
| Aufgabe | `Task` | eigene zu erledigende Arbeit mit Fälligkeit/Priorität. |
| Next Action | `NextAction` | der aktuell wichtigste nächste Schritt oder bewusste Wartezustand eines aktiven Vorgangs. |
| Commitment | `Commitment` | Zusage/Verpflichtung einer anderen Person oder Partei, z. B. „Rückmeldung bis Freitag“. |
| Interview | `Interview` | konkrete Gesprächsrunde mit Zeitpunkt, Teilnehmern, Vorbereitung und Nachbereitung. |
| Dokument | `Document` | logisches Dokument, z. B. Lebenslauf oder Anschreiben. |
| Dokumentversion | `DocumentVersion` | unveränderliche konkrete Binärversion eines Dokuments mit Hash. |
| verwendetes Dokument | `ApplicationDocument` | Zuordnung, dass exakt diese Dokumentversion für eine Bewerbung verwendet/versandt wurde. |
| quellenbezogene Aussage | `SourcedStatement` | fachliche Information mit Quelle, Zeitpunkt und Kontext; konkurrierende Aussagen dürfen parallel existieren. |
| Quelle | `Source` | Herkunft einer Stelle oder Information, z. B. Unternehmenswebsite, Recruiter, Portal. |
| Outcome | `Outcome` | endgültiges Ergebnis, z. B. Angebot, Absage, zurückgezogen, angenommen. |
| Archiviert | – | aus aktiver Arbeit entfernt, aber weiterhin vorhanden und suchbar. |
| Löschen | – | gezielte irreversible Entfernung nach Sicherheitsabfrage und Referenzprüfung. |
| Backup | – | vollständige konsistente Sicherung zur Wiederherstellung des Anwendungszustands. |
| Export | – | offenes, nachvollziehbares Datenformat zur Weiterverwendung; kein Ersatz für Backup. |
| Restore | – | kontrollierte Wiederherstellung eines vollständigen Backupbestands. |
| Recovery | – | Wiederanlauf nach Fehlern, ggf. ohne vollständigen Restore. |
| Derived State | – | aus fachlichen Daten ableitbarer Zustand wie FTS-Index oder Cache; darf verworfen/rebuilt werden. |
| Source of Truth | – | autoritativer Datenspeicher; für Fachzustand SQLite plus verwalteter Dokumentstore. |

## Abgrenzungen

### Opportunity ≠ JobPosting

Eine Chance kann erneut ausgeschrieben oder über verschiedene URLs gefunden werden. Anzeigen sind deshalb Snapshots/Quellen, nicht die fachliche Chance selbst.

### Application ≠ Opportunity

Eine Opportunity kann existieren, ohne dass eine Bewerbung versandt wurde. Eine Bewerbung ist der konkrete Prozess.

### Task ≠ Commitment

Task = **ich** muss etwas tun. Commitment = **jemand anderes** hat etwas zugesagt. Beide können fällig/überfällig sein, werden aber unterschiedlich ausgewertet.

### Activity ≠ Status

Eine Aktivität dokumentiert ein Ereignis. Ein Status beschreibt den aktuellen Prozesszustand. Ein Statuswechsel erzeugt Historie, darf aber nicht jede Aktivität ersetzen.
