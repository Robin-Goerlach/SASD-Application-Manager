# Pflichtenheft – SASD Bewerbungsmanager Version 1.0 (Windows Forms)

**Dokumenttyp:** Pflichtenheft / technische Umsetzungsspezifikation  
**Projekt:** SASD Bewerbungsmanager  
**Zielversion:** 1.0  
**Zielplattform:** Windows Desktop  
**UI-Technologie:** Windows Forms (WinForms)  
**Programmiersprache:** C#  
**Target Framework:** .NET 10 LTS, `net10.0-windows`  
**Primäre Architektur:** modularer Monolith, geschichtete Architektur, MVP/Presenter-orientierte WinForms-Präsentation  
**Persistenz:** SQLite mit Entity Framework Core 10  
**Betriebsmodell:** local-first, Einzelbenutzer, offline-fähig  
**Primäre Zielarchitektur:** Windows 11 x64  
**Dokumentstatus:** Draft / technische Freigabebasis  
**Stand:** 24. August 2026  
**Primäres Eingangsartefakt:** `SASD-Bewerbungsmanager-Lastenheft-v1.0.md`  
**Normativer Bezug:** SASD Development Standard – Approved Baseline 0.9.0 / Version-1.0-Specification-Candidate, Profile Core + C#/.NET + Desktop

---

# 1. Zweck und Rolle dieses Pflichtenhefts

Dieses Pflichtenheft beschreibt, **wie** die im Lastenheft für Version 1.0 festgelegten fachlichen, qualitativen, sicherheitsbezogenen und betrieblichen Anforderungen technisch umgesetzt werden sollen. Es übersetzt die fachliche Produktdefinition in eine konkrete, überprüfbare Implementierungsarchitektur für eine Windows-Forms-Anwendung.

Das Dokument ist zugleich:

- technische Arbeitsgrundlage für Implementierung und Code-Reviews;
- verbindliche Basis für das Datenmodell und die Persistenz;
- Vorgabe für die WinForms-UI-Architektur und die Navigationsstruktur;
- Grundlage für automatisierte Tests, Systemtests und Abnahme;
- Referenz für Build, Packaging, Upgrade und Datenmigration;
- Traceability-Brücke vom Lastenheft zu Quellcode, Tests und Release-Gates.

Das Pflichtenheft ersetzt das Lastenheft **nicht**. Bei einem Widerspruch gilt die fachliche Anforderung des freigegebenen Lastenhefts; technische Abweichungen müssen durch eine dokumentierte Änderung des Lastenhefts oder einen nachvollziehbaren ADR geklärt werden.

## 1.1 Abgrenzung zu nachgelagerten Artefakten

Dieses Dokument trifft die für Version 1 wesentlichen technischen Entscheidungen. Detailentscheidungen, die sich während der Implementierung ändern können, ohne den Vertrag dieses Pflichtenhefts zu verletzen, gehören in ADRs, Entwicklerdokumentation oder Code.

Beispiele:

- Die Wahl `SQLite + EF Core` ist Bestandteil dieses Pflichtenhefts.
- Die genaue SQL-Indexdefinition darf während Performance-Tests optimiert werden, solange das fachliche Verhalten unverändert bleibt.
- Das Präsentationsmuster `MVP/Presenter` ist verbindlich; die genaue Aufteilung eines einzelnen kleinen Dialogs in View und Presenter darf proportional erfolgen.
- Die Datenablage unter `%LOCALAPPDATA%` ist verbindlich; konkrete Unterordnernamen können bis zur ersten Release-Baseline vereinheitlicht werden.

## 1.2 Normative Begriffe

In diesem Dokument werden folgende Begriffe verwendet:

- **MUSS** – für Version 1.0 verbindlich.
- **SOLL** – vorgesehene Umsetzung; Abweichung benötigt dokumentierte Begründung.
- **KANN** – zulässige oder für V1.x vorgesehene Erweiterung.
- **DARF NICHT** – ausgeschlossene Umsetzung.

## 1.3 SASD-Einordnung

Das Projekt wird mindestens auf **Recommended**-Qualitätsniveau entwickelt. Aufgrund der Verarbeitung personenbezogener Bewerbungsdaten, der persistenten Datenhaltung, der Backup-/Restore-Anforderungen und des geplanten längerfristigen Produktlebens werden einzelne Bereiche bereits auf Production-nahem Niveau behandelt, insbesondere:

- Datenintegrität und Migration;
- Backup und Restore;
- Security/Privacy;
- reproduzierbarer Build;
- Release- und Upgrade-Tests;
- Fehlerbehandlung und Diagnose.

Der SASD Development Standard verlangt für gepflegte Desktopanwendungen eine erkennbare Trennung von Präsentation, Fachlogik und technischen Integrationen. Für WinForms sollen umfangreiche Abläufe aus Form-Klassen herausgehalten und in Presenter, Controller oder Anwendungsdienste verlagert werden. Dieses Pflichtenheft setzt das deshalb ausdrücklich mit einer MVP-orientierten Struktur um.

---

# 2. Technische Zielsetzung

Version 1.0 soll eine **robuste, lokale Windows-Desktopanwendung** ergeben, die die Bewerbungsorganisation vollständig ohne Cloudkonto und ohne Internetzugang beherrscht. Die Architektur soll bewusst so gewählt werden, dass spätere V1.x-/V2-Erweiterungen wie E-Mail-Import, Kalenderintegration oder optionale KI-Dienste über Adapter ergänzt werden können, ohne den lokalen Kern umzubauen.

Die technischen Hauptziele lauten:

1. **Datenhoheit:** Fach- und Dokumentdaten bleiben lokal und unter Kontrolle des Benutzers.
2. **Nachvollziehbarkeit:** Jeder relevante Bewerbungsprozess bleibt historisch rekonstruierbar.
3. **Testbarkeit:** Fachlogik funktioniert ohne gestartete WinForms-Oberfläche und ohne echte Benutzerinteraktion im Test.
4. **Designerfähigkeit:** WinForms-Forms und UserControls bleiben mit dem Visual-Studio-Designer pflegbar.
5. **Wartbarkeit:** UI, Anwendungslogik, Domäne und Infrastruktur besitzen klare Abhängigkeitsgrenzen.
6. **Wiederherstellbarkeit:** Ein vollständiges, integritätsgeprüftes Backup stellt Daten und verwaltete Dokumente wieder her.
7. **Performance:** Die Referenzmenge von 10.000 Vorgängen und 50.000 Aktivitäten bleibt flüssig bedienbar.
8. **Offline-Fähigkeit:** Kein V1-Kernworkflow hängt von Netzwerkdiensten ab.
9. **Erweiterbarkeit ohne Plugin-Zwang:** Die Architektur besitzt Adaptergrenzen, führt in V1 jedoch kein komplexes Plugin-System ein.
10. **Sichere Defaults:** keine Telemetrie, keine versteckten Netzaufrufe, keine automatische Ausführung importierter Inhalte.

---

# 3. Verbindliche technische Baseline

## 3.1 Laufzeit und Framework

Für Version 1.0 wird folgende Baseline festgelegt:

| Bereich | Festlegung |
|---|---|
| Sprache | C# |
| UI | Windows Forms |
| Target Framework | `net10.0-windows` |
| .NET-Linie | .NET 10 LTS |
| Nullable Reference Types | aktiviert |
| Implicit Usings | aktiviert, soweit projekttauglich |
| CPU-Architektur Release | x64 |
| Betriebssystem | Windows 11 x64, von Microsoft unterstützte Releases |
| Build | .NET SDK / MSBuild reproduzierbar aus CLI und CI |
| Persistenz | SQLite |
| ORM | Entity Framework Core 10, SQLite Provider |
| DI / Host | `Microsoft.Extensions.Hosting` Generic Host |
| Logging-Abstraktion | `Microsoft.Extensions.Logging` |
| Konfiguration | `Microsoft.Extensions.Configuration`, lokale JSON-Konfiguration für nicht sensible Einstellungen |
| Tests | xUnit als Standard; Integrations- und Architekturtests ergänzend |
| Release | self-contained `win-x64` |

.NET 10 ist zum Dokumentstand die aktive LTS-Linie. Die Verwendung einer LTS-Linie folgt der SASD-Empfehlung für Recommended-/Production-Projekte. Preview-.NET, Preview-C# oder Preview-NuGet-Pakete sind für produktive V1.0-Artefakte ausgeschlossen.

## 3.2 Unterstützte Windows-Matrix

**Verbindlich unterstützt:**

- Windows 11 x64 in zum Releasezeitpunkt von Microsoft unterstützten Versionen.

**Nicht Bestandteil der V1.0-Supportzusage:**

- Windows on ARM;
- Windows x86;
- Windows Server als Desktopziel;
- Windows 10 Consumer-Editionen außerhalb offizieller Supportzeiträume;
- Linux/macOS über Wine, Mono oder andere Kompatibilitätsschichten.

Ein Start auf nicht freigegebenen Plattformen kann technisch funktionieren, darf aber nicht als offiziell unterstützt dokumentiert werden.

## 3.3 Deployment-Modell

Version 1.0 wird als **self-contained x64 Desktopanwendung** veröffentlicht. Der Benutzer benötigt deshalb keine separat installierte .NET Desktop Runtime.

Das Release MUSS enthalten:

- Anwendungsdateien;
- eingebettete .NET Runtime der freigegebenen Patch-Version;
- Installer oder gleichwertiges per-user Installationspaket;
- Versions- und Releaseinformationen;
- Lizenz-/Third-Party-Notices;
- Benutzerhilfe bzw. Link/Datei zur lokalen Hilfe;
- Prüfsumme des Releaseartefakts.

Die Installation SOLL ohne administrative Rechte im Benutzerkontext möglich sein. Eine spätere maschinenweite Installation ist nicht Teil des V1-Kerns.

---

# 4. Architekturübersicht

## 4.1 Architekturform

Die Anwendung wird als **modularer Monolith** umgesetzt. Sie läuft als ein Desktopprozess und besitzt eine lokale Datenbank. Es gibt keine Microservices, keinen lokalen Webserver und keinen separaten Backend-Dienst.

Die logische Architektur besteht aus vier Hauptschichten:

```text
┌─────────────────────────────────────────────────────────────┐
│ Sasd.Bewerbungsmanager.WinForms                             │
│ Forms · UserControls · Presenter · Navigation · UI Services │
└──────────────────────────────┬──────────────────────────────┘
                               │
┌──────────────────────────────▼──────────────────────────────┐
│ Sasd.Bewerbungsmanager.Application                          │
│ Use Cases · Commands · Queries · DTOs · Validation · Ports  │
└──────────────────────────────┬──────────────────────────────┘
                               │
┌──────────────────────────────▼──────────────────────────────┐
│ Sasd.Bewerbungsmanager.Domain                               │
│ Entities · Value Objects · Rules · Enums · Domain Services  │
└──────────────────────────────▲──────────────────────────────┘
                               │
┌──────────────────────────────┴──────────────────────────────┐
│ Sasd.Bewerbungsmanager.Infrastructure                       │
│ EF Core · SQLite · Files · Backup · Export · Diagnostics    │
└─────────────────────────────────────────────────────────────┘
```

Die Richtung der Abhängigkeiten ist verbindlich:

- `Domain` kennt keine UI, EF-Core- oder Dateisystemklassen.
- `Application` darf `Domain` verwenden, aber keine konkreten WinForms-Controls kennen.
- `Infrastructure` implementiert Ports/Abstraktionen aus `Application` bzw. technische Interfaces und verwendet `Domain`.
- `WinForms` verwendet `Application` und UI-Verträge. Direkter SQL-/DbContext-Zugriff aus Forms/UserControls ist verboten.

## 4.2 Warum kein „alles in MainForm“

Die Fachdomäne umfasst mehrere langlebige, miteinander verknüpfte Aggregate. Geschäftsregeln wie Statuswechsel, Archivierung, Dokumentversionen, Commitments, Restore und Merge dürfen nicht in Click-Handlern implementiert werden. Eine solche Struktur wäre kurzfristig schnell, würde aber Testbarkeit, Datenintegrität und spätere Erweiterungen gefährden.

## 4.3 Warum MVP/Presenter

Für WinForms wird ein pragmatisches **Model-View-Presenter**-Modell verwendet:

- View = Form/UserControl und UI-spezifischer Zustand;
- Presenter = koordiniert Benutzeraktionen, ruft Application Use Cases auf, transformiert Ergebnisse in ViewModel/DisplayModel;
- Application = fachliche Anwendungsfälle;
- Domain = Invarianten und fachliche Regeln;
- Infrastructure = persistente und technische Umsetzung.

Die Presenter dürfen keine SQL-Befehle enthalten und keine statischen `MessageBox`-Aufrufe als versteckte Abhängigkeit verwenden. Wiederverwendete Dialoge werden über schmale UI-Service-Interfaces angesprochen.

## 4.4 Composition Root

`Program.cs` ist der technische Composition Root. Der Generic Host übernimmt:

- Konfiguration;
- Dependency Injection;
- Logging;
- Lebenszyklus lang laufender Dienste;
- Erstellung des `MainForm`;
- sauberes Herunterfahren.

Beispielhafte Struktur:

```text
Program.Main
  -> ApplicationConfiguration.Initialize()
  -> Host.CreateApplicationBuilder()
  -> AddApplication()
  -> AddInfrastructure()
  -> AddWinFormsPresentation()
  -> Run migrations / startup checks
  -> resolve MainForm
  -> Application.Run(MainForm)
```

Datenmigrationen werden vor Freigabe der Hauptoberfläche ausgeführt. Bei Fehlern startet die Anwendung nicht in einen scheinbar funktionsfähigen Zustand, sondern zeigt eine sichere Recovery-/Diagnoseoberfläche.

---

# 5. Solution- und Repository-Struktur

## 5.1 Vorgesehene Solution

```text
Sasd.Bewerbungsmanager.sln
│
├── src/
│   ├── Sasd.Bewerbungsmanager.WinForms/
│   │   ├── Forms/
│   │   ├── Controls/
│   │   ├── Dialogs/
│   │   ├── Presentation/
│   │   │   ├── Dashboard/
│   │   │   ├── Applications/
│   │   │   ├── Opportunities/
│   │   │   ├── Companies/
│   │   │   ├── Contacts/
│   │   │   ├── Tasks/
│   │   │   ├── Interviews/
│   │   │   ├── Documents/
│   │   │   ├── Search/
│   │   │   ├── Analytics/
│   │   │   └── Settings/
│   │   ├── Navigation/
│   │   ├── Services/
│   │   ├── Resources/
│   │   └── Program.cs
│   │
│   ├── Sasd.Bewerbungsmanager.Application/
│   │   ├── Abstractions/
│   │   ├── Common/
│   │   ├── Companies/
│   │   ├── Contacts/
│   │   ├── Opportunities/
│   │   ├── Applications/
│   │   ├── Activities/
│   │   ├── Tasks/
│   │   ├── Commitments/
│   │   ├── Interviews/
│   │   ├── Documents/
│   │   ├── Dashboard/
│   │   ├── Search/
│   │   ├── Analytics/
│   │   ├── ImportExport/
│   │   └── BackupRestore/
│   │
│   ├── Sasd.Bewerbungsmanager.Domain/
│   │   ├── Companies/
│   │   ├── Contacts/
│   │   ├── Opportunities/
│   │   ├── Applications/
│   │   ├── Activities/
│   │   ├── Tasks/
│   │   ├── Commitments/
│   │   ├── Interviews/
│   │   ├── Documents/
│   │   ├── Tags/
│   │   ├── Shared/
│   │   └── ValueObjects/
│   │
│   └── Sasd.Bewerbungsmanager.Infrastructure/
│       ├── Persistence/
│       │   ├── Configurations/
│       │   ├── Migrations/
│       │   ├── Queries/
│       │   └── Interceptors/
│       ├── Documents/
│       ├── Backup/
│       ├── Export/
│       ├── Import/
│       ├── Diagnostics/
│       ├── FileSystem/
│       └── Configuration/
│
├── tests/
│   ├── Sasd.Bewerbungsmanager.Domain.Tests/
│   ├── Sasd.Bewerbungsmanager.Application.Tests/
│   ├── Sasd.Bewerbungsmanager.Infrastructure.IntegrationTests/
│   ├── Sasd.Bewerbungsmanager.ArchitectureTests/
│   └── Sasd.Bewerbungsmanager.SystemTests/
│
├── docs/
│   ├── requirements/
│   ├── architecture/
│   ├── adr/
│   ├── testing/
│   ├── operations/
│   └── user/
│
├── scripts/
├── artifacts/
├── Directory.Build.props
├── Directory.Packages.props
├── global.json
└── README.md
```

## 5.2 Projektabhängigkeiten

Zulässige Referenzen:

```text
WinForms        -> Application, Domain
Application     -> Domain
Infrastructure  -> Application, Domain
Domain          -> keine Projektabhängigkeit
Tests           -> jeweils gezielt getestete Projekte
```

Nicht zulässig:

- `Domain -> Infrastructure`;
- `Domain -> WinForms`;
- `Application -> WinForms`;
- Forms -> `DbContext`;
- Presenter -> konkrete EF-Core-Typen;
- Infrastructure -> konkrete Formulare.

Architekturtests prüfen diese Regeln automatisiert.

---

# 6. WinForms-Präsentationsarchitektur

## 6.1 MainForm als Shell

Die Anwendung besitzt eine primäre `MainForm`. Sie ist Shell und nicht fachlicher Megacontroller.

Die Shell enthält:

- Hauptnavigation links;
- Titel-/Kontextleiste;
- zentralen Content-Bereich;
- globales Suchfeld bzw. Suchaktion;
- Statusleiste für Datenbank-/Speicherstatus und kurzlebige Rückmeldungen;
- optional Menüleiste für Datei, Bearbeiten, Ansicht, Werkzeuge, Hilfe.

Die Shell kennt nur Navigation, Fenstermanagement, globale Tastaturbefehle und Statusanzeige. Fachliche Aktionen werden an Presenter/Application delegiert.

## 6.2 Navigationsbereiche

V1 erhält folgende Hauptnavigation:

1. **Heute** – Dashboard und Aufmerksamkeitssicht.
2. **Bewerbungen** – Liste und Board.
3. **Stellen / Opportunities** – vorgemerkte berufliche Chancen und JobPostings.
4. **Unternehmen** – Unternehmen und Historie.
5. **Kontakte** – Recruiter/HR/Fachkontakte.
6. **Aufgaben** – Aufgaben, Next Actions und Wiedervorlagen.
7. **Interviews** – Termine und Gesprächsrunden.
8. **Dokumente** – Dokumentbibliothek und Versionen.
9. **Auswertungen** – Analytics.
10. **Einstellungen** – konfigurierbare Listenwerte, Anzeige, Backup/Export.

Eine globale Suche ist von jeder Hauptansicht erreichbar.

## 6.3 Detailnavigation

Komplexe Objekte werden als Detailarbeitsbereich in der Hauptshell geöffnet und nicht für jede Bearbeitung in ein neues Hauptfenster ausgelagert.

Beispiel **Bewerbungsakte**:

```text
Bewerbung: System Engineer Linux – Muster GmbH
┌───────────────────────────────────────────────────────────┐
│ Kopf: Status · Priorität · Next Action · Outcome          │
├───────────────────────────────────────────────────────────┤
│ Übersicht | Timeline | Kontakte | Interviews | Dokumente  │
│ Aufgaben  | Aussagen | Stellenanzeige | Notizen           │
└───────────────────────────────────────────────────────────┘
```

Kleine atomare Bearbeitungen, etwa Tag anlegen oder Statusliste bearbeiten, dürfen als modale Dialoge umgesetzt werden. Große Fachakten werden nicht in verschachtelten Modal-Dialogketten bearbeitet.

## 6.4 Präsentationsverträge

Jeder größere UI-Bereich erhält:

- `I...View` – schmaler View-Vertrag;
- `...Presenter` – Zustands- und Aktionskoordination;
- Display-/Row-Modelle – UI-freundliche immutable/semi-immutable Daten;
- Application Queries/Commands – eigentliche Use Cases.

Beispiel:

```text
IApplicationsListView
ApplicationsListPresenter
ApplicationListItemModel
SearchApplicationsQuery
ChangeApplicationStatusCommand
```

## 6.5 Designerfähigkeit

Der WinForms-Designer darf nicht durch DI oder Laufzeitabhängigkeiten unbenutzbar werden. Deshalb gilt:

- designer-generierter Code bleibt ausschließlich in `*.Designer.cs`;
- keine Geschäftslogik in `InitializeComponent()`;
- UserControls besitzen bei Bedarf einen designerfähigen parameterlosen Konstruktor;
- Laufzeitabhängigkeiten werden über Presenter-/Initialize-Methoden oder eine kontrollierte Factory verdrahtet;
- Design-Time-Code darf keine Datenbank öffnen;
- keine globalen Service-Locator-Aufrufe aus Forms.

## 6.6 UI-Zustände

Jede datenbasierte Hauptansicht unterstützt explizit mindestens:

- `Loading`;
- `Ready`;
- `Empty`;
- `Editing`/`Dirty`;
- `Saving`;
- `Error`;
- `Disabled`/Operation nicht verfügbar.

Ungespeicherte Änderungen werden bei Navigation oder Schließen nicht still verworfen. Der Benutzer erhält bei echten Dirty-Zuständen die Optionen **Speichern**, **Verwerfen**, **Abbrechen**.

## 6.7 Asynchronität

Datenbankabfragen, Dateikopie, Hashing, Backup, Restore, Import und Export dürfen den UI-Thread nicht blockieren.

Regeln:

- `async void` nur in echten UI-Eventhandlern;
- Presenter/Application Methoden bevorzugt `Task`/`Task<T>`;
- `CancellationToken` bei länger laufenden Operationen;
- UI-Aktualisierung nur über den WinForms-UI-Kontext;
- Mehrfachklick auf nicht parallele Operationen wird verhindert;
- Fortschritt wird gedrosselt und sinnvoll aggregiert.

---

# 7. UX-, Accessibility- und Darstellungsregeln

## 7.1 Grundprinzipien

Die Anwendung soll wie ein gepflegtes Windows-Arbeitswerkzeug wirken, nicht wie eine Weboberfläche in Desktopverkleidung. Standard-WinForms-Controls werden bevorzugt, wenn sie die Anforderung erfüllen.

Verbindliche UX-Regeln:

- zentrale Aktionen stehen an konsistenten Positionen;
- Löschen und Restore sind visuell und textlich destruktiv gekennzeichnet;
- Status wird nie nur über Farbe vermittelt;
- Pflichtfelder sind sparsam und begründet;
- Validierung erfolgt feldnah und zusätzlich fachlich im Application-/Domain-Layer;
- Fehlermeldungen nennen Ursache/Auswirkung, wenn bekannt, und nächste Handlung;
- Listen öffnen per Doppelklick/Enter die Detailakte;
- Kontextmenüs sind Zusatzweg, niemals einziger Weg zu einer Kernfunktion.

## 7.2 DPI und Skalierung

Die Anwendung wird auf 100 %, 125 %, 150 %, 175 % und 200 % Skalierung getestet. Layouts werden überwiegend mit `TableLayoutPanel`, `FlowLayoutPanel`, Dock/Anchor und AutoSize aufgebaut; starre Pixelpositionierung wird auf visuelle Sonderfälle beschränkt.

Kerninhalte dürfen bei 200 % nicht abgeschnitten oder unerreichbar sein. Dialoge benötigen Mindestgrößen und bei Bedarf Scrollbereiche.

## 7.3 Tastaturbedienung

Mindestens:

- `Ctrl+N` – kontextabhängig neuen Hauptdatensatz anlegen;
- `Ctrl+S` – speichern;
- `Ctrl+F` – globale/kontextbezogene Suche;
- `Ctrl+Shift+F` – Filterbereich fokussieren;
- `Esc` – Dialog/temporären Zustand verlassen;
- `Enter` – markierten Datensatz öffnen bzw. Standardaktion;
- `F5` – aktuelle Sicht neu laden;
- `Delete` – nur mit Bestätigung und nur wenn fachlich zulässig.

Tab-Reihenfolge wird explizit geprüft. Labels erhalten Mnemonics, wo dies nicht überladen wirkt.

## 7.4 Theme

V1 nutzt die von .NET 10 unterstützte WinForms-Systemfarblogik. Die Anwendung SOLL dem Windows-Hell-/Dunkelmodus folgen. Eigene Farbcodierung wird sparsam verwendet und muss kontrastfähig bleiben.

Keine fachliche Semantik darf ausschließlich von Theme-Farben abhängen.

---

# 8. Fachliches Domänenmodell

## 8.1 Aggregate und Verantwortlichkeiten

### Company

Repräsentiert ein Unternehmen als langfristiges Stammdatenobjekt. Es besitzt keine Bewerbungshistorie als eingebettete Liste; diese wird über Beziehungen/Queries ermittelt.

Kernfelder:

- `Id`;
- `Name`;
- `NormalizedName`;
- `WebsiteUrl`;
- `MainLocation`;
- `IndustryId?`;
- `CompanyTypeId?`;
- `Notes`;
- `ArchivedAt?`;
- `CreatedAtUtc`, `UpdatedAtUtc`.

### Contact

Eigenständiges Personenobjekt. Ein Kontakt kann mit einem Unternehmen und vielen Bewerbungen/Opportunities verknüpft sein.

Kernfelder:

- Namebestandteile / DisplayName;
- Rolle/Funktion;
- CompanyId optional;
- E-Mail, Telefon, Profil-URL;
- freie Notizen;
- letzter Kontakt und optional nächstes Follow-up als abgeleitete/gespeicherte Daten;
- Archivstatus.

### Opportunity

Repräsentiert die fachliche berufliche Chance unabhängig von einer konkreten veröffentlichten Anzeige und unabhängig von einer konkreten Bewerbung.

Kernfelder:

- CompanyId;
- Title;
- Location;
- EmploymentType;
- WorkModel;
- CompensationRange;
- Priority;
- CurrentState;
- Notes;
- ArchivedAt.

### JobPosting

Historisch unveränderlicher bzw. versionierter Snapshot einer Stellenanzeige.

Kernfelder:

- OpportunityId;
- SourceId;
- OriginalUrl;
- CapturedAtUtc;
- TitleAtCapture;
- RawTextSnapshot;
- Optional original format metadata;
- Hash des Snapshots.

Eigene Notizen werden **nicht** in `RawTextSnapshot` geschrieben.

### Application

Eine konkrete Bewerbung zu einer Opportunity.

Kernfelder:

- OpportunityId;
- AppliedOn (`DateOnly`);
- ApplicationChannel/Source;
- StatusId;
- Priority;
- CurrentNextActionId?;
- Outcome;
- OutcomeDate?;
- Notes;
- ArchivedAt.

Eine Opportunity kann mehrere Bewerbungen besitzen, etwa bei Wiederbewerbung.

### Activity

Historisches Ereignis. Aktivitäten sind grundsätzlich vergangenheitsorientiert und nicht mit Aufgaben gleichzusetzen.

Typen mindestens:

- ApplicationSubmitted;
- Email;
- PhoneCall;
- NetworkMessage;
- Meeting;
- Interview;
- DocumentSent;
- Offer;
- Rejection;
- StatusChanged;
- Note;
- CommitmentCreated/Changed;
- TaskCompleted.

### Communication

Strukturierte Kommunikationsdetails zu einer Activity. Dadurch bleibt eine E-Mail ein Timeline-Ereignis, kann aber zusätzliche Felder besitzen.

Felder:

- ActivityId;
- Channel;
- Direction;
- Subject;
- Summary;
- Body/Excerpt optional und bewusst manuell;
- ExternalReference optional.

### Task

Vom Benutzer zu erledigende Arbeit. Aufgaben besitzen Status, Priorität, Fälligkeit und können Checklistenpunkte enthalten.

### NextAction

Eigener fachlicher Typ. Er beschreibt den **aktuell wichtigsten nächsten Schritt** oder einen bewussten Wartezustand.

Typen:

- `UserAction`;
- `WaitUntil`;
- `FollowUp`;
- `PrepareInterview`;
- `ReviewOffer`;
- `Other`.

Eine aktive Bewerbung kann höchstens eine als `Current` markierte Next Action besitzen. Erledigte/ersetzte Next Actions bleiben historisch erhalten.

### Commitment

Versprechen/Zusage eines Dritten. Ein Commitment ist keine eigene Aufgabe.

Felder:

- Application/Opportunity Bezug;
- ContactId optional;
- Content;
- DueDate;
- Status (`Open`, `Fulfilled`, `NotFulfilled`, `Cancelled`, `Postponed`);
- SourceActivityId optional;
- OriginalDueDate / geänderte Fälligkeit über History.

### Interview

Eigenständige Gesprächsrunde mit mehreren Teilnehmern.

Felder:

- ApplicationId;
- RoundName;
- StartAt;
- Duration optional;
- Format;
- Location/MeetingLink;
- PreparationNotes;
- Questions;
- MeetingNotes;
- Learnings;
- PersonalRating optional;
- FollowUpNotes.

### Document und DocumentVersion

`Document` beschreibt die logische Unterlage („Lebenslauf IT 2026“). `DocumentVersion` ist der unveränderliche Dateistand.

Dadurch bleibt exakt rekonstruierbar, welche konkrete Datei versandt wurde.

### SourcedStatement

Quellenbezogene Aussage, zum Beispiel Remote-Regelung, Gehalt, Technologie, Teamgröße. Widersprüchliche Aussagen werden nicht überschrieben.

Felder:

- Subject/Topic;
- Value/Text;
- SourceType;
- ContactId?;
- SourceActivityId?;
- ObservedAt;
- Notes.

### Outcome

Abschlussergebnis einer Bewerbung. Mindestens:

- Accepted;
- OfferDeclined;
- EmployerRejected;
- Withdrawn;
- NoResponseClosed.

## 8.2 Fachliche Invarianten

1. Company, Contact, Opportunity, JobPosting und Application besitzen eigene IDs.
2. Ein JobPosting darf nicht allein durch Änderung der Opportunity nachträglich verändert werden.
3. Eine Application muss auf genau eine Opportunity zeigen.
4. Eine aktive Application darf maximal eine aktuelle Next Action besitzen.
5. Ein Statuswechsel erzeugt zusätzlich einen historischen Statuswechsel/Activity-Eintrag.
6. Archivierung ist reversibel; endgültiges Löschen ist separat.
7. Eine DocumentVersion ist nach Zuordnung zu einer versendeten Bewerbung inhaltlich unveränderlich.
8. Widersprüchliche SourcedStatements dürfen parallel existieren.
9. Ein Commitment ist nicht automatisch eine Task.
10. Ein Interview gehört zu genau einer Application und kann mehrere Contacts als Teilnehmer besitzen.
11. Ein Outcome schließt die Application fachlich ab, löscht aber keinerlei Historie.
12. Merge-Operationen dürfen referenzierte Datensätze nicht verwaisen lassen.

---

# 9. Persistenz- und Datenbankentwurf

## 9.1 SQLite-Betriebsmodell

Die Anwendung verwendet eine einzelne SQLite-Datenbank pro Benutzerprofil. SQLite wird mit Foreign Keys aktiviert. Der Datenbankzugriff erfolgt ausschließlich über Infrastructure/Application-Abstraktionen.

Empfohlene Pragmas nach Validierung:

- `foreign_keys = ON`;
- WAL-Journalmodus für robuste lokale Nebenläufigkeit;
- angemessenes `synchronous`-Level;
- `busy_timeout`, um kurzzeitige Sperren sauber abzufangen.

Die exakten Werte werden durch Integrationstests und ADR bestätigt. Ein Sicherheits-/Integritätsmerkmal darf nicht zugunsten minimaler Benchmarkgewinne abgeschaltet werden.

## 9.2 Datenpfade

Standardpfad:

```text
%LOCALAPPDATA%\SASD\Bewerbungsmanager\
├── data\
│   └── bewerbungsmanager.db
├── documents\
│   └── objects\...
├── backups\
├── logs\
├── diagnostics\
├── cache\
└── settings.json
```

Benutzerdaten werden nicht in `Program Files` gespeichert. Deinstallation entfernt diese Daten nicht automatisch.

## 9.3 Tabellenübersicht

| Tabelle | Zweck |
|---|---|
| `Companies` | Unternehmen |
| `Contacts` | Personen/Recruiter |
| `CompanyContacts` | optionale historische/mehrfache Zuordnung |
| `Opportunities` | berufliche Chancen |
| `JobPostings` | Stellenanzeigen-Snapshots |
| `Applications` | konkrete Bewerbungen |
| `ApplicationStatuses` | konfigurierbare Pipelinephasen |
| `ApplicationStatusHistory` | historische Statuswechsel |
| `Activities` | allgemeine Timeline-Ereignisse |
| `Communications` | strukturierte Kommunikationsdetails |
| `ActivityContacts` | Teilnehmer/Bezug von Aktivitäten |
| `Tasks` | Aufgaben |
| `TaskChecklistItems` | Checklistenpunkte |
| `NextActions` | nächster Schritt / Wartezustand |
| `Commitments` | Zusagen Dritter |
| `CommitmentHistory` | Fälligkeits-/Statusänderungen |
| `Interviews` | Interviewrunden |
| `InterviewParticipants` | n:m Interview-Kontakt |
| `InterviewQuestions` | konkrete Fragen/Vorbereitung |
| `Documents` | logische Dokumente |
| `DocumentVersions` | unveränderliche Dateiversionen |
| `ApplicationDocuments` | tatsächlich genutzte/versandte Versionen |
| `SourcedStatements` | quellenbezogene Aussagen |
| `Tags` | freie Tags |
| `EntityTags` bzw. typspezifische Tag-Links | Tag-Zuordnung |
| `Sources` | Job-/Bewerbungsquellen |
| `Industries`, `CompanyTypes`, `EmploymentTypes`, `WorkModels` | Lookupwerte |
| `SavedViews` | gespeicherte Filter |
| `UserSettings` | nur kleine strukturierte fachliche Einstellungen, soweit nicht JSON |
| `SchemaInfo` | Datenbankschema-/Migrationsinformation |

## 9.4 Schlüssel

Interne IDs werden als GUID/UUID (`Guid`) erzeugt. Sie sind unabhängig von Anzeigenamen und bleiben bei Merge/Migration stabil.

Warum GUID statt Auto-Increment:

- vereinfachter offener Export mit stabilen Referenzen;
- spätere Import-/Merge-Fähigkeit;
- keine fachliche Bedeutung der ID;
- sichere Erzeugung vor Persistenz.

In SQLite werden GUIDs konsistent in einem dokumentierten Format gespeichert. Die Entscheidung Text/BLOB wird im Persistenz-ADR festgeschrieben und nicht zwischen V1.x-Releases geändert.

## 9.5 Zeitmodell

- reine fachliche Tage: `DateOnly`, gespeichert als ISO-Datum;
- genaue Ereignisse: `DateTimeOffset` in der Anwendung, persistent normalisiert in UTC;
- UI zeigt Zeit gemäß Windows-/Benutzereinstellung;
- `CreatedAtUtc` und `UpdatedAtUtc` werden zentral gesetzt;
- historische Ereignisse dürfen nach Korrektur einen fachlichen Ereigniszeitpunkt besitzen, der vom Erstellzeitpunkt abweicht.

## 9.6 Geldbeträge

Geldwerte werden nicht als `double` gespeichert. Strukturierte Vergütung verwendet:

- Betrag als `long` in kleinster Währungseinheit oder dokumentierter Ganzzahleinheit;
- `CurrencyCode` nach ISO-4217;
- Zeitraum (`Year`, `Month`, `Hour`, `Day`);
- Minimum/Maximum;
- zusätzlicher Freitext für Sonderfälle.

## 9.7 Indizes

Mindestens Indizes auf:

- Company normalized name;
- Contact display name / e-mail;
- Opportunity title, company, active/archive state;
- Application status, application date, outcome, archive state;
- NextAction due date/status;
- Task due date/status;
- Commitment due date/status;
- Interview start;
- Activity event time;
- Source und Priority;
- viele Fremdschlüsselspalten.

Performance-Indizes werden anhand realer Query-Pläne verifiziert. Überindizierung wird vermieden.

## 9.8 Volltextsuche

Die globale Suche wird zweistufig umgesetzt:

1. strukturierte Suche über normalisierte Schlüsselspalten;
2. Volltextsuche für große Textfelder wie Stellenanzeigen, Notizen und Aktivitätsbeschreibungen.

Für SQLite darf FTS5 eingesetzt werden, sofern die im Release verwendete SQLite-Buildvariante dies reproduzierbar unterstützt. Andernfalls muss ein äquivalenter lokaler Suchindex bereitgestellt werden. Der Suchindex ist **ableitbar** und darf nie einzige Quelle fachlicher Daten sein.

## 9.9 Migrationen

EF-Core-Migrationen sind versioniert und Bestandteil des Repositories.

Regeln:

- kein `EnsureCreated()` für produktive V1-Datenbanken;
- jede Schemaänderung erhält Migration;
- destructive Migration nur mit explizitem Datenmigrationsschritt und Test;
- vor riskanter Migration automatische oder angebotene Sicherung;
- Upgrade-Test von mindestens der vorherigen freigegebenen V1.x-Version;
- Downgrade der Anwendung auf ein älteres Schema wird nicht automatisch unterstützt.

---

# 10. Dokument- und Dateispeicher

## 10.1 Grundprinzip

Dokumente, die einer Bewerbung als tatsächlich verwendet zugeordnet werden, werden **als verwaltete Kopie** in den Anwendungsdatenspeicher übernommen. Nur einen externen Dateipfad zu speichern reicht nicht aus, weil die Datei später geändert, verschoben oder gelöscht werden könnte.

## 10.2 Content-addressed Storage

Jede importierte Datei erhält einen SHA-256-Hash. Physische Dateien können unter einem hashbasierten Pfad abgelegt werden:

```text
documents/objects/ab/cd/<sha256>.bin
```

Dazu werden Metadaten gespeichert:

- ursprünglicher Dateiname;
- logischer Dokumenttitel;
- MIME/Extension;
- Dateigröße;
- SHA-256;
- Importzeitpunkt;
- ursprünglicher Pfad optional, nur als Hinweis;
- relative interne Ablage.

Identische Dateien können physisch dedupliziert werden, ohne fachliche Dokumentversionen zusammenzuwerfen.

## 10.3 Unveränderlichkeit

Sobald eine `DocumentVersion` als versendet/verwendet an einer Bewerbung referenziert wurde, ist ihr Dateiinhalt unveränderlich. Eine neue Datei erzeugt eine neue Version.

## 10.4 Öffnen externer Dateien

Dateien werden niemals automatisch ausgeführt. Öffnen erfolgt nur durch explizite Benutzeraktion über die Windows-Shell. Ungewöhnliche Dateitypen werden mit zusätzlicher Sicherheitsinformation behandelt.

## 10.5 Fehlende oder beschädigte Dateien

Fehlt eine verwaltete Datei oder stimmt ihr Hash nicht:

- bleibt die Datenbank vollständig zugänglich;
- die betroffene Version wird als fehlend/beschädigt markiert;
- Diagnose nennt Pfad/Hash, aber keine sensiblen Inhalte;
- Backup-/Integritätsprüfung meldet das Problem;
- andere Dokumente werden nicht blockiert.

---

# 11. Umsetzung der Hauptfunktionsbereiche

## 11.1 Dashboard „Heute“

### Zweck

Das Dashboard ist keine allgemeine Statistikseite, sondern die operative Tagessteuerung.

### Bereiche

1. **Überfällig** – Tasks, Next Actions, offene Commitments.
2. **Heute fällig** – heute ausstehende eigene Aktionen.
3. **Bevorstehende Interviews** – konfigurierbarer Zeitraum, Standard 14 Tage.
4. **Warten auf Rückmeldung** – aktuelle Wait-Next-Actions/Commitments.
5. **Ohne nächsten Schritt** – aktive Bewerbungen ohne Current Next Action.
6. **Kennzahlen kompakt** – aktive Bewerbungen, Interviews, Angebote, offene Aufgaben.

Jedes Element ist klickbar und führt in die passende Detailakte oder vorgefilterte Liste.

### Technische Umsetzung

Das Dashboard wird über eine spezialisierte `DashboardQuery` aufgebaut. Es lädt keine vollständigen Aggregate, sondern projektiert nur benötigte Felder. Die Abfrage ist read-only (`AsNoTracking`) und muss mit Referenzbestand innerhalb der Performancegrenze liegen.

## 11.2 Unternehmen

### Listenansicht

Spalten mindestens:

- Name;
- Standort;
- Branche;
- aktive Opportunities;
- aktive Bewerbungen;
- letzter Kontakt;
- Tags.

### Detailansicht

Tabs:

- Übersicht;
- Opportunities/Bewerbungen;
- Kontakte;
- Timeline;
- Recherche/Notizen.

### Dublettenerkennung

Beim Speichern prüft die Application-Schicht normalisierte Namen und Website-Domain. Ergebnis ist ein Hinweis, kein automatischer Merge.

### Merge

Merge zeigt vor Ausführung:

- beibehaltenes Unternehmen;
- zu übertragende Kontakte;
- Opportunities;
- Bewerbungen;
- Aktivitäten;
- Tags/Notizen;
- potenzielle Feldkonflikte.

Merge läuft in einer Datenbanktransaktion.

## 11.3 Kontakte / Recruiter-CRM

Kontaktliste mit Suche nach Name, Unternehmen, Rolle, E-Mail und Tags.

Kontaktakte zeigt:

- Kontaktdaten;
- Unternehmen;
- Rollen/Tags;
- letzte Interaktionen;
- verknüpfte Opportunities und Bewerbungen;
- offene Follow-ups;
- Notizen.

E-Mail und Telefon werden nicht automatisch kontaktiert. Eine anklickbare E-Mail-Adresse darf nur den lokal registrierten Handler öffnen, wenn der Benutzer dies bewusst auslöst.

## 11.4 Opportunities und Stellenanzeigen

### Opportunity erfassen

Schnellerfassungsdialog/-bereich mit:

- Unternehmen;
- Rollenbezeichnung;
- Standort;
- Beschäftigungsart;
- Arbeitsmodell;
- Gehalt;
- Quelle;
- Priorität;
- URL;
- Stellenbeschreibung;
- Tags.

Unternehmen kann inline gesucht und bei Bedarf neu angelegt werden.

### Snapshot

Der Benutzer fügt Text manuell ein oder importiert eine lokale Text-/HTML-Datei. V1 führt **kein automatisches Web-Scraping** aus. HTML wird als untrusted Input behandelt; für Anzeige wird aktiver Inhalt nicht ausgeführt. Primär wird der extrahierte/gespeicherte Text angezeigt.

Mehrere Snapshots werden zeitlich geordnet. Unterschiedsanzeige ist V1.x-Could und nicht Release-Gate.

## 11.5 Bewerbungsakte

Die Bewerbungsakte ist der zentrale Arbeitsbereich.

Kopfbereich:

- Unternehmen und Rolle;
- Status;
- Priorität;
- Bewerbungsdatum/-kanal;
- Next Action;
- Outcome;
- wichtige Warnungen (überfällig, fehlende Dokumente).

Tabs:

1. **Übersicht** – Kerndaten und kompakte offene Punkte.
2. **Timeline** – chronologische Ereignisse.
3. **Kontakte** – beteiligte Personen.
4. **Interviews** – Interviewrunden.
5. **Dokumente** – verwendete Versionen.
6. **Aufgaben** – Aufgaben/Checklisten.
7. **Aussagen** – quellenbezogene Informationen.
8. **Stellenanzeige** – Snapshots und Annotationen.
9. **Notizen** – freie interne Notizen.

### Statuswechsel

Ein Statuswechsel erfolgt über Application Use Case:

1. aktuellen Datensatz laden;
2. Übergang validieren;
3. neuen Status setzen;
4. `ApplicationStatusHistory` anlegen;
5. Timeline-Activity `StatusChanged` anlegen;
6. Transaktion committen;
7. UI aktualisieren.

Es gibt keine stillen direkten Statusupdates in der UI.

## 11.6 Board / Pipeline

Das Board zeigt primär aktive Bewerbungen. Jede Spalte entspricht einer sichtbaren Pipelinephase.

Karte enthält:

- Company;
- Role;
- Priority;
- aktuelle Next Action/Fälligkeit;
- Warte-/Überfällig-Indikator;
- Tags in begrenzter Zahl.

Statuswechsel erfolgt per Drag & Drop **und** über ein zugängliches Kontext-/Aktionsmenü. Drag & Drop ist nicht der einzige Bedienweg.

Vor Drop wird der Zielstatus validiert. Nach erfolgreicher Änderung wird die Karte verschoben; bei Fehler bleibt sie im Ursprungsstatus und der Benutzer erhält eine verständliche Meldung.

Um WinForms-Performance zu schützen, lädt die Boardansicht nur aktive/gefilterte Datensätze. Für sehr große historische Bestände ist die Listenansicht vorgesehen.

## 11.7 Timeline und Aktivitäten

Timeline wird rückwärts chronologisch dargestellt und unterstützt Filter nach Aktivitätstyp.

Darstellung eines Eintrags:

- Zeitpunkt;
- Typ-Icon + Text;
- Kurzbeschreibung;
- beteiligte Kontakte;
- optional Verknüpfung zu Interview, Dokument, Commitment;
- Quelle/Ersteller, soweit relevant.

Zukünftige Tasks/Termine werden in einem getrennten Abschnitt oder visuell eindeutig getrennt gezeigt. Sie werden nicht als bereits geschehene Aktivitäten ausgegeben.

## 11.8 Next Action und Wiedervorlage

Beim Speichern einer aktiven Bewerbung wird kein harter Zwang zu einer Next Action aufgebaut, weil das Lastenheft ausdrücklich Sichtbarkeit „ohne Next Action“ verlangt. Stattdessen:

- aktuelle Next Action kann direkt im Header gesetzt werden;
- fehlende Next Action erzeugt Aufmerksamkeit im Dashboard;
- Erledigen setzt `CompletedAt` und erzeugt bei Bedarf Timeline-Ereignis;
- Snooze erzeugt History/Änderungsereignis, damit die ursprüngliche Fälligkeit nachvollziehbar bleibt.

## 11.9 Commitments

Commitment-Editor verlangt:

- Inhalt;
- Fälligkeit;
- Bezug;
- optional Kontakt;
- optional Quelle/Activity.

Überfälligkeitslogik:

```text
Status == Open
AND DueDate < LocalToday
=> Overdue
```

Verschieben ändert die aktuelle Fälligkeit und erzeugt `CommitmentHistory` mit altem/neuem Datum.

Aus einem Commitment kann bewusst eine Follow-up-Task erzeugt werden; dies geschieht nie automatisch ohne Benutzeraktion.

## 11.10 Aufgaben und Checklisten

Aufgaben besitzen:

- Titel;
- Beschreibung;
- Status;
- Priorität;
- DueDate;
- Bezug zu Bewerbung/Opportunity/Company/Contact;
- Checklistenpunkte;
- Created/Completed timestamps.

Checklistenpunkte sind einfache Reihenfolge + Text + erledigt. Kein vollständiges Projektmanagementsystem.

## 11.11 Interviews

Intervieweditor mit:

- Runde/Bezeichnung;
- Datum/Uhrzeit;
- Dauer;
- Format;
- Ort/Meeting-Link;
- Teilnehmer;
- Vorbereitung;
- eigene Fragen;
- Gesprächsnotizen;
- Learnings;
- persönliche Bewertung;
- Follow-up.

Ein Interview kann vor dem Termin vorbereitende Tasks erzeugen und nach dem Termin ein Follow-up erzeugen, jeweils nur auf Benutzeraktion.

Das Interview erscheint:

- in der Bewerbungsakte;
- auf dem Dashboard;
- in der Kalenderansicht;
- als Timeline-Ereignis bzw. über eine eindeutige Timeline-Projektion.

## 11.12 Dokumente

Dokumentbibliothek unterscheidet:

- Lebenslauf;
- Anschreiben;
- Zeugnis;
- Zertifikat;
- Stellenanzeige/Anhang;
- E-Mail/Kommunikationsbeleg;
- Sonstiges.

Zu einer Bewerbung werden konkrete **DocumentVersions** mit Rolle markiert, zum Beispiel `CV sent`, `CoverLetter sent`, `Attachment sent`.

Die Oberfläche muss die Frage beantworten können:

> Welche exakte Datei wurde bei dieser Bewerbung verwendet?

## 11.13 Globale Suche

Globale Suche liefert gruppierte Treffer:

- Unternehmen;
- Kontakte;
- Opportunities;
- Bewerbungen;
- Aktivitäten/Notizen;
- Dokumentmetadaten.

Suchergebnisse zeigen Kontext und öffnen das zugehörige Objekt.

Suche beginnt nach kurzer Eingabeverzögerung (ca. 250–350 ms) oder explizit mit Enter, um unnötige Queries zu vermeiden.

## 11.14 Filter und gespeicherte Ansichten

Filtermodell ist ein Application-Datentyp, nicht in Controls versteckte Logik.

Filter mindestens:

- Status;
- Unternehmen;
- Quelle;
- Priorität;
- Tags;
- Zeitraum;
- aktiv/archiviert/alle.

Gespeicherte Ansichten speichern Filterkriterien und Sortierung, nicht die Ergebnisdatensätze.

## 11.15 Kalenderansicht

V1-Kalender ist eine **lokale interne Ansicht**, keine Google-/Outlook-Synchronisierung.

Angezeigt werden:

- Interviews;
- Task-Fälligkeiten;
- Next Actions;
- Wiedervorlagen;
- Commitments.

Monats- und Agendaansicht sind zulässig; die Agendaansicht ist für V1 wichtiger als ein komplexer Kalender-Designer.

## 11.16 Analytics

Analytics basiert ausschließlich auf lokalen Daten.

Definitionen werden dokumentiert, beispielsweise:

- **Response Rate** = Bewerbungen mit dokumentierter Arbeitgeber-/Recruiterreaktion / alle versendeten Bewerbungen im Filterzeitraum;
- **Interview Rate** = Bewerbungen mit mindestens einem Interview / versendete Bewerbungen;
- **Offer Rate** = Bewerbungen mit Angebot / versendete Bewerbungen;
- **Time to first response** = erste relevante eingehende Response-Activity minus Bewerbungsdatum/-zeitpunkt.

Jede Kennzahl zeigt Fallzahl `n`. Kleine Fallzahlen werden sichtbar gekennzeichnet, um Scheingenauigkeit zu vermeiden.

---

# 12. Import, Export, Backup und Restore

## 12.1 Offener Datenexport

Der offene Export dient Datenhoheit, nicht nur Backup.

Format:

```text
export-2026-08-24/
├── manifest.json
├── companies.csv
├── contacts.csv
├── opportunities.csv
├── job-postings.csv
├── applications.csv
├── activities.csv
├── communications.csv
├── tasks.csv
├── commitments.csv
├── interviews.csv
├── documents.csv
├── document-versions.csv
├── statements.csv
├── tags.csv
└── README.md
```

Alternativ/ergänzend darf ein maschinenlesbares JSON-Gesamtformat angeboten werden. CSV bleibt für tabellarische Kernobjekte Pflichtkandidat, weil es ohne proprietäre Software lesbar ist.

Export enthält stabile IDs, damit Beziehungen nachvollziehbar bleiben. Sensible Inhalte werden nicht automatisch ausgelassen, da es der eigene Datenexport ist; die UI weist jedoch darauf hin, dass die Exportdatei personenbezogene Daten enthält.

## 12.2 CSV-Import

V1-Should-Umsetzung:

1. Datei auswählen;
2. Encoding/Delimiter erkennen oder wählen;
3. Feldzuordnung anzeigen;
4. Vorschau;
5. Validierung/Dublettenhinweise;
6. Ergebniszahlen `neu / übersprungen / Konflikt / fehlerhaft`;
7. Import erst nach Bestätigung.

Import verwendet eine Staging-Repräsentation. Kein CSV-Datensatz schreibt unmittelbar beim Parsen in produktive Entities.

## 12.3 Komplettbackup

Eigenes Backupformat, z. B. Dateiendung `.sasdbm-backup` als ZIP-Container:

```text
backup/
├── manifest.json
├── database.db
├── documents/
└── integrity.json
```

`manifest.json` enthält mindestens:

- Produktname;
- App-Version;
- Schema-Version;
- Erstellungszeitpunkt UTC;
- Backupformat-Version;
- Anzahl zentraler Datensätze;
- Dateiliste.

`integrity.json` enthält SHA-256-Prüfsummen aller Backupbestandteile.

## 12.4 Backup-Konsistenz

Backup muss einen konsistenten Datenbankstand erzeugen. Das bloße Kopieren einer aktiven SQLite-Datei ist unzulässig, wenn dadurch WAL-/Transaktionszustände inkonsistent werden könnten. Die Infrastructure-Schicht verwendet eine SQLite-geeignete Backup-/Checkpoint-Strategie und testet Restore aus dem erzeugten Artefakt.

## 12.5 Verschlüsseltes Backup

REQ-SEC-010 ist ein Should. Umsetzung für V1.0 geplant:

- optionales Passwort beim Erstellen;
- moderne authentifizierte Verschlüsselung;
- Schlüsselableitung aus Passwort mit gespeichertem Salt und geeignetem KDF;
- keine Speicherung des Passworts;
- klare Warnung: verlorenes Passwort = Backup nicht wiederherstellbar.

Der konkrete Kryptografiecontainer wird in einem Security-ADR festgelegt und vor Release mit bekannten Testvektoren geprüft. Eigenentwickelte Kryptografiealgorithmen sind verboten.

## 12.6 Restore

Restore ist ein expliziter Wizard:

1. Backup auswählen;
2. Format/Integrität prüfen;
3. Produkt-/Schema-Kompatibilität prüfen;
4. Inhalt zusammenfassen;
5. **vorhandenen Bestand automatisch sichern** oder Benutzer bewusst darauf verzichten lassen;
6. Anwendung in Restore-Modus versetzen;
7. Datenbank und Dokumente in Staging-Pfad wiederherstellen;
8. Konsistenz-/Migrationsprüfung;
9. atomarer Wechsel auf wiederhergestellten Bestand;
10. Neustart bzw. erneutes Laden;
11. Ergebnisbericht.

Ein fehlgeschlagener Restore darf den vorherigen Bestand nicht zerstören.

---

# 13. Archivierung, Löschung und Merge

## 13.1 Archivierung

Archivierung setzt `ArchivedAt` und entfernt den Datensatz aus Standard-Aktivansichten. Sie löscht keine Historie.

Reaktivierung löscht/neutralisiert `ArchivedAt` und stellt den Vorgang wieder bereit.

## 13.2 Endgültiges Löschen

Endgültiges Löschen ist eine separate Aktion mit Impact Preview.

Beispiel Unternehmen:

```text
Muster GmbH löschen?
- 3 Opportunities
- 2 Bewerbungen
- 5 Kontakte mit Zuordnung
- 18 Aktivitäten
- 4 Dokumentzuordnungen
```

Die UI bietet je nach Objekt:

- Abbrechen;
- archivieren statt löschen;
- abhängige Datensätze umhängen;
- endgültig löschen, falls fachlich zulässig.

Keine pauschalen EF-Cascade-Deletes dürfen umfangreiche fachliche Historie ohne kontrollierten Use Case entfernen.

## 13.3 Merge

Company-/Contact-Merge wird als transaktionaler Application Use Case implementiert. Der Merge muss:

- Beziehungen umhängen;
- Konfliktfelder anzeigen;
- Tags vereinigen;
- keine Aktivitäten verlieren;
- Merge-Activity/Audit-Hinweis erzeugen;
- Quellobjekt anschließend archivieren oder definiert entfernen.

---

# 14. Einstellungen und Konfiguration

## 14.1 Benutzerpräferenzen

Lokale Einstellungen umfassen:

- Datums-/Zeitdarstellung;
- Standardquelle;
- Standardstatus für neue Bewerbungen;
- Dashboard-Zeitraum für Interviews;
- UI-Theme/Systemmodus;
- zuletzt verwendete Filter/Ansichten, soweit sinnvoll;
- Backup-Standardpfad.

## 14.2 Fachliche Lookupwerte

Datenbankgestützt konfigurierbar:

- Sources;
- Tags;
- ApplicationStatuses;
- WorkModels;
- EmploymentTypes;
- CompanyTypes;
- Industries.

Systemwerte besitzen stabile IDs oder Systemkennzeichen, damit Umbenennungen nicht Geschäftsregeln zerstören.

## 14.3 Pipeline-Konfiguration

Status besitzt:

- ID;
- Anzeigename;
- Reihenfolge;
- Phase category (`Preparation`, `Applied`, `Interview`, `Offer`, `Closed` etc.);
- Aktiv/Deaktiviert;
- optional Anzeigehinweis.

Bereits verwendete Statuswerte dürfen nicht physisch gelöscht werden, solange historische Daten darauf verweisen. Sie werden deaktiviert.

---

# 15. Fehlerbehandlung

## 15.1 Fehlerklassen

Die Anwendung unterscheidet:

1. **Validierungsfehler** – Benutzer kann Eingabe korrigieren.
2. **Fachliche Konflikte** – Operation ist in aktuellem Zustand nicht erlaubt.
3. **Ressourcenfehler** – Datei fehlt, Laufwerk voll, Datenbank gesperrt.
4. **Integrationsfehler** – Import/Export/Dateisystemfehler.
5. **unerwartete technische Fehler** – zentral behandelt und diagnostizierbar.

## 15.2 Fehlerfluss

```text
UI-Eingabe
  -> UI-Sofortvalidierung
  -> Application Command/Query
  -> Domain-Validierung
  -> Infrastructure
  -> Result / definierter Fehler
  -> Presenter
  -> sichere Benutzermeldung + Logging
```

Exceptions werden nicht als reguläres fachliches Steuersignal missbraucht. Erwartete Validierungs-/Konfliktfälle erhalten explizite Result-Typen oder definierte Exceptions an klaren Grenzen.

## 15.3 Globale Fehlergrenze

WinForms erhält eine zentrale Fehlerbehandlung für nicht erwartete UI-Thread-/Task-Fehler. Sie:

- protokolliert technische Details;
- zeigt eine sichere Fehlermeldung;
- bietet ggf. Diagnosebericht;
- verhindert soweit möglich Datenverlust;
- beendet bei nicht sicher fortsetzbarem Zustand die Anwendung kontrolliert.

„Fehler ignorieren und weiter“ ist bei Datenbank-/Integritätsfehlern nicht zulässig, wenn der Zustand unbekannt ist.

---

# 16. Logging und Diagnose

## 16.1 Logging-Ziele

Logging dient:

- Start-/Shutdowndiagnose;
- Migrationen;
- technische Fehler;
- Performance auffälliger Operationen;
- Backup/Restore/Import/Export-Ergebnis;
- Dateispeicherfehler.

Es dient **nicht** der vollständigen Protokollierung fachlicher Bewerbungsinhalte.

## 16.2 Datenschutz im Log

Standardmäßig nicht loggen:

- vollständige CV-Inhalte;
- komplette Stellenanzeigen;
- vollständige E-Mails;
- private Notizen;
- Anschriften;
- Telefonnummern/E-Mail-Adressen, wenn nicht für Fehlerdiagnose zwingend;
- Passwort/Backup-Schlüssel.

IDs dürfen zur Korrelation verwendet werden. Benutzerbezogene Texte werden gekürzt/redigiert oder gar nicht aufgenommen.

## 16.3 Logrotation

Lokale Logs werden größen-/zeitbasiert rotiert. Aufbewahrung ist begrenzt. Ein Diagnose-Export nimmt nur relevante Dateien in einem definierten Zeitraum auf.

## 16.4 Diagnosebericht

Erzeugbarer Diagnosebericht enthält:

- Anwendungsversion;
- .NET-/OS-Version;
- Architektur;
- Schema-Version;
- Datenbankdateigröße;
- Dokumentanzahl/fehlende Dateien als Zähler;
- letzte technische Fehler;
- Konfigurationsflags ohne sensible Werte.

Vor Erstellung erhält der Benutzer eine Inhaltsübersicht.

---

# 17. Security und Privacy by Design

## 17.1 Keine versteckten Netzaufrufe

Im V1-Kern werden keine HTTP-Clients für Produktfunktionen benötigt. Externe URLs werden nur nach Benutzeraktion über den Standardbrowser geöffnet.

Es gibt:

- keine Telemetrie;
- keinen Crash-Upload;
- keine Cloudkonfiguration;
- keine automatische Updateprüfung mit Übertragung fachlicher Daten;
- keine KI-API.

Falls eine Updateprüfung später ergänzt wird, wird sie separat opt-in/transparent spezifiziert.

## 17.2 Untrusted Input

Folgende Inputs sind untrusted:

- CSV;
- Stellenanzeigentext/HTML;
- URLs;
- importierte Dateien;
- Dateinamen;
- Backupcontainer.

Maßnahmen:

- Pfadnormalisierung;
- Verzeichnis-Traversal verhindern;
- Dateitypen nicht blind ausführen;
- HTML nicht in einem aktiven WebBrowser-Control rendern;
- Größenlimits/Streaming für große Dateien;
- ZIP-Entpacken nur in kontrollierten Staging-Pfad;
- Backup-Manifest und Hashes validieren.

## 17.3 Sensible Daten im Speicher

V1 speichert keine fremden Kontopasswörter. Backup-Passwörter werden nur für die aktuelle Operation im Speicher gehalten und nicht persistiert.

## 17.4 Dateiberechtigungen

Anwendungsdaten liegen im Benutzerprofil. Es werden keine unnötigen ACL-Erweiterungen vorgenommen. Die Anwendung benötigt keine Administratorrechte für normalen Betrieb.

## 17.5 Screen-Capture-Schutz

.NET 10 bietet WinForms-Unterstützung, Inhalte eines Formulars vor Bildschirmaufzeichnung zu verbergen. Für V1 wird **kein globaler Capture-Schutz erzwungen**, da er normale Nutzbarkeit und Support erschweren kann. Eine spätere optionale Privacy-Einstellung kann als ADR/V1.x bewertet werden.

---

# 18. Performance- und Skalierbarkeitskonzept

## 18.1 Referenzbestand

Pflichtbenchmark:

- 10.000 Opportunities/Bewerbungsvorgänge;
- 50.000 Activities;
- realistische Kontakte/Unternehmen;
- mindestens mehrere tausend Dokumentmetadaten;
- ausreichend Textdaten für Suche.

## 18.2 Query-Prinzipien

- Listen arbeiten mit projektierten DTOs, nicht vollständigen Aggregategraphs;
- `AsNoTracking` für reine Reads;
- serverseitige Filter/Sortierung in SQLite;
- Seitengröße typischerweise 100–250 Datensätze;
- Details werden bei Bedarf nachgeladen;
- keine `Include()`-Ketten für Dashboard/Listen, wenn Projektion genügt;
- Indizes werden mit Queryplan geprüft.

## 18.3 UI-Listen

`DataGridView` lädt nicht ungeprüft 10.000 komplexe Objekte samt Unterdaten. Paging oder inkrementelles Nachladen ist verbindlich, sobald Messungen zeigen, dass Vollmaterialisierung die 1-Sekunden-Grenze gefährdet.

## 18.4 Startzeit

Startpfad:

1. Host/Konfiguration;
2. Log initialisieren;
3. Datenpfade prüfen;
4. DB öffnen;
5. Schema prüfen/migrieren;
6. Shell anzeigen;
7. Dashboard asynchron laden.

Schwere Integritätsprüfungen laufen nicht bei jedem normalen Start vollständig, sondern gezielt über Wartungsfunktion oder nach erkannten Problemen.

## 18.5 Backup/Import

Lange Operationen:

- laufen asynchron;
- zeigen Phase und Fortschritt;
- schreiben zunächst in Staging;
- unterstützen Cancel nur bis zu einem sicheren Commit-Punkt;
- hinterlassen bei Cancel keine halben Datenbestände.

---

# 19. Nebenläufigkeit und Single-Instance

Version 1.0 wird als **Single-Instance-Anwendung pro Benutzerprofil** spezifiziert. Grund: eine lokale SQLite-Datenbank und verwaltete Dokumente sollen nicht durch zwei unbeabsichtigt parallel gestartete UI-Prozesse bearbeitet werden.

Die Single-Instance-Implementierung muss:

- parallele Starts erkennen;
- die vorhandene Instanz in den Vordergrund bringen;
- Startargumente nur nach Validierung weitergeben;
- keine dauerhaft verwaiste Sperre hinterlassen.

Dies ersetzt SQLite-Transaktionen nicht; die Persistenz bleibt auch gegen interne Nebenläufigkeit abgesichert.

---

# 20. Installation, Update, Deinstallation

## 20.1 Installation

Ziel: per-user, self-contained, x64.

Installer:

- legt Programmdateien in einen geeigneten Benutzer-/Anwendungspfad;
- erzeugt Startmenüeintrag;
- optional Desktop-Verknüpfung nach Auswahl;
- verändert keine fachlichen Datenpfade bei Upgrade;
- benötigt nach Möglichkeit keine Adminrechte.

Der konkrete Installer-Generator wird vor Implementierungsmeilenstein M5 per ADR festgelegt. Zulässig sind nur reproduzierbare, skript-/CI-fähige Werkzeuge.

## 20.2 Update

V1.0 enthält keinen zwingenden Auto-Updater. Update erfolgt über neues Installationspaket.

Beim ersten Start einer neuen Version:

1. Versions-/Schemaerkennung;
2. falls Migration nötig: Backup anbieten/erzeugen;
3. Migration;
4. Smoke-Check;
5. Hauptoberfläche.

## 20.3 Deinstallation

Deinstallation löscht standardmäßig nur Programmdateien. Persönliche Daten unter `%LOCALAPPDATA%\SASD\Bewerbungsmanager` bleiben erhalten.

Eine separate Funktion „Alle lokalen Bewerbungsmanager-Daten löschen“ befindet sich in der Anwendung bzw. wird dokumentiert und erfordert explizite Bestätigung.

---

# 21. Teststrategie

## 21.1 Testpyramide

### Domain Unit Tests

Testen ohne DB/UI:

- Status-/Outcome-Regeln;
- Next-Action-Invarianten;
- Commitment-Überfälligkeit;
- Geld-/Datums-ValueObjects;
- Archivierungsregeln;
- DocumentVersion-Unveränderlichkeit;
- Dublettenheuristiken, soweit fachlich.

### Application Tests

Mit Fakes/isolierter Persistenzgrenze:

- Use Cases;
- Validierung;
- Merge-Plan;
- Dashboard-Logik;
- Analytics-Definitionen;
- Importkonflikte;
- Berechtigungs-/Scope-Regeln des Produkts.

### Infrastructure Integration Tests

Mit echter temporärer SQLite-Datei:

- EF-Konfigurationen;
- Foreign Keys;
- Migrationen;
- Transaktionen;
- WAL/Backupverhalten;
- DocumentStore;
- Hashing;
- Export/Import;
- Restore.

SQLite-Integrationstests verwenden **nicht** ausschließlich EF InMemory, weil dieser die reale SQLite-Semantik nicht ausreichend abbildet.

### Presenter/Component Tests

Presenter werden gegen Fake Views getestet:

- Loading/Ready/Error;
- Dirty-State;
- Navigation;
- Statuswechsel;
- Fehlerdarstellung;
- Mehrfachaktionsschutz.

### UI-/Systemtests

Risikobasiert für:

- Start und Navigation;
- Opportunity anlegen;
- Bewerbung anlegen;
- Board-Statuswechsel;
- Interview erfassen;
- Dokumentversion zuordnen;
- Backup/Restore-Wizard;
- Löschen/Archivieren;
- DPI/Tastatur-Smoke.

## 21.2 Testdaten

Es wird ein synthetischer Referenzbestand gepflegt:

- keine realen Bewerbungsdaten;
- deterministisch generierbar;
- kleiner Smoke-Datensatz;
- großer Performance-Datensatz;
- Migrationsdatensätze pro freigegebener V1.x-Version.

## 21.3 Abnahmetests

Die 15 End-to-End-Abnahmefälle des Lastenhefts werden als benannte Systemtest-/Manual-Acceptance-Szenarien übernommen. AT-011 Backup/Restore, AT-014 Datenintegrität nach Update und AT-015 keine unerwartete Datenübertragung sind Release-Gates.

## 21.4 Performance-Tests

Messungen auf dokumentierter Referenzhardware:

- Dashboard laden;
- Bewerbungsliste filtern;
- globale Suche;
- Detailakte öffnen;
- Timeline laden;
- Analytics Basisansicht;
- Backup 1 GB/realistischer Bestand als Langoperation.

95. Perzentil der typischen Listen-/Filteraktionen <= 1 s; Standardsuche <= 2 s.

## 21.5 Security-Tests

Mindestens:

- path traversal bei Import/Backup;
- manipuliertes Backup;
- HTML/Script als JobPosting-Inhalt;
- ungewöhnliche Dateiendungen;
- beschädigte Dokumentdatei;
- Logprüfung auf sensible Inhalte;
- Netzwerkbeobachtung im vollständigen Offline-/Normalbetrieb;
- Restore mit inkompatiblem Schema;
- Zip-Bomb-/überdimensionierte Importdatei angemessen begrenzen.

---

# 22. Build, CI und Quality Gates

## 22.1 Reproduzierbarer Build

Repository enthält:

- `global.json`;
- `Directory.Build.props`;
- `Directory.Packages.props`;
- dokumentierte NuGet-Quellen;
- Buildskript, z. B. `scripts/build.ps1`;
- Testskript;
- Publishskript.

Kanonisch:

```text
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build
dotnet publish ...
```

## 22.2 Compiler-/Analyzer-Regeln

- Nullable aktiviert;
- Warnungen sichtbar;
- neue Warnungen im geänderten Code werden nicht pauschal unterdrückt;
- `.editorconfig` versioniert;
- Analyzer-Regeln zentral;
- `TreatWarningsAsErrors` risikobasiert im CI-Gate bzw. für Projektcode.

## 22.3 CI-Stufen

1. Repository-/Format-/Metadatenprüfung;
2. Restore;
3. Build;
4. Unit Tests;
5. Integration Tests;
6. Architekturtests;
7. Security-/Dependency-Checks;
8. Publish win-x64;
9. Packaging;
10. Artefakt-/Hash-Erzeugung.

Release-Build erfolgt aus sauberem Checkout.

---

# 23. Abhängigkeiten und Bibliotheksstrategie

## 23.1 Grundsatz

Externe Pakete werden nur aufgenommen, wenn sie einen klaren Nutzen besitzen. Vor Aufnahme werden geprüft:

- Lizenz;
- Maintainer-/Releasezustand;
- bekannte Sicherheitsprobleme;
- Transitivabhängigkeiten;
- .NET-10-Kompatibilität;
- Notwendigkeit.

## 23.2 Vorgesehene Kernpakete

Voraussichtlich:

- `Microsoft.EntityFrameworkCore`;
- `Microsoft.EntityFrameworkCore.Sqlite`;
- `Microsoft.EntityFrameworkCore.Design` nur Entwicklungs-/Migrationskontext;
- `Microsoft.Extensions.Hosting`;
- `Microsoft.Extensions.Logging`;
- xUnit + Test SDK.

Weitere Libraries, etwa CSV-Parser oder Kryptografiecontainer, benötigen dokumentierte Auswahl. Ein schweres UI-Control-Framework ist für V1 nicht vorgesehen, sofern Standard-WinForms-Controls ausreichen.

## 23.3 Keine Codeübernahme aus Referenzprodukten

Die Markt-/Open-Source-Recherche war Inspirationsquelle. V1 wird unabhängig implementiert. Code aus fremden Projekten darf nur nach Lizenz-, Copyright- und Architekturprüfung übernommen werden und muss dokumentiert sein.

---

# 24. Datenintegrität und Transaktionsgrenzen

Folgende Operationen müssen transaktional sein:

- Bewerbung + initialer Status/History;
- Statuswechsel + History + Activity;
- Company-/Contact-Merge;
- Archiv-/Reaktivierungsoperation mit abhängigen Änderungen;
- Commitment-Status/Fälligkeitsänderung + History;
- Dokumentmetadaten + Versionzuordnung, soweit DB-Anteil;
- Importbatch pro sinnvoller Einheit;
- migrationskritische Datenumformung.

Dateisystem + Datenbank können nicht in einer gemeinsamen ACID-Transaktion laufen. Deshalb werden Dateioperationen über **Staging + Commit + Cleanup** umgesetzt:

```text
1. Datei in Staging schreiben
2. Hash/Integrität prüfen
3. DB-Transaktion anlegen
4. Datei in finalen content-addressed Pfad verschieben
5. DB committen
6. bei Fehler kompensierend aufräumen
```

Für Backup/Restore gilt ein noch strengeres Stagingmodell.

---

# 25. Datenmigration und Rückwärtskompatibilität

## 25.1 Schema-Versionierung

Jedes Release dokumentiert:

- App-Version;
- EF-Migrationsstand;
- internes Backupformat;
- Exportformatversion.

## 25.2 V1.x-Kompatibilität

Neue V1.x-Versionen müssen Daten der jeweils unterstützten Vorgängerversion öffnen und migrieren können. Jede Migration wird gegen eingefrorene Referenzdaten getestet.

## 25.3 Backup-Kompatibilität

Ein V1.x-Release soll Backups älterer V1.x-Versionen lesen können, sofern keine dokumentierte Ausnahme besteht. Beim Restore wird erst in Staging migriert.

## 25.4 Exportstabilität

Offene Exportformate erhalten eine Formatversion. Neue Felder werden möglichst additiv ergänzt. Entfernte/umbenannte Felder werden in Release Notes und Exportdokumentation beschrieben.

---

# 26. Fachliche und technische Abnahme

Version 1.0 darf nur veröffentlicht werden, wenn mindestens:

- alle Must-Anforderungen des Lastenhefts erfüllt oder formal abweichend freigegeben sind;
- alle 15 End-to-End-Abnahmefälle bestanden sind;
- Datenbankmigration auf Referenzbestand bestanden ist;
- Backup/Restore auf frischer Installation vollständig bestanden ist;
- Offline-Abnahme bestanden ist;
- Log-/Netzwerkprüfung keine unerwartete Datenübertragung zeigt;
- Performancegrenzen für Referenzbestand erfüllt sind;
- Installer/Upgrade/Deinstallation geprüft sind;
- keine offenen kritischen/hohen Datenverlust- oder Security-Defekte bestehen;
- Third-Party-Lizenzen dokumentiert sind;
- Benutzerhilfe für Backup, Restore, Export und Datenlöschung vorhanden ist.

---

# 27. Implementierungsmeilensteine

## M0 – Technische Baseline

- Solution/Projekte;
- `global.json`, zentrale Build-/Packagekonfiguration;
- Generic Host;
- WinForms-Shell;
- SQLite/EF-Core-Baseline;
- Testprojekte;
- CI-Basis;
- `SASD-COMPLIANCE.md`.

**Exit:** Restore/Build/Test/Start reproduzierbar.

## M1 – Stammdaten und Kernakte

- Companies;
- Contacts;
- Opportunities;
- JobPosting Snapshots;
- Applications;
- Status/Outcome;
- Listen-/Detailansichten.

**Exit:** AT-001/AT-008 in Kernform.

## M2 – Verlauf und Tagessteuerung

- Activities;
- Communications;
- Timeline;
- Next Action;
- Commitments;
- Tasks;
- Dashboard.

**Exit:** AT-003/AT-004/AT-005.

## M3 – Interviews und Dokumente

- Interviewrunden/Teilnehmer;
- Vorbereitung/Learnings;
- Document/DocumentVersion;
- Managed File Store;
- Versandzuordnung.

**Exit:** AT-002/AT-006.

## M4 – Finden, Board und Verstehen

- Pipeline-Board;
- globale Suche;
- Filter/gespeicherte Ansichten;
- Kalenderansicht;
- SourcedStatements;
- Analytics.

**Exit:** AT-007/AT-009 und Performance-Baseline.

## M5 – Datenhoheit und Releasefähigkeit

- Export;
- CSV-Import;
- Backup/Restore;
- optional Backupverschlüsselung gemäß Should;
- Archiv/Löschen/Merge-Härtung;
- Diagnostics;
- Installer/Upgrade;
- Hilfe;
- vollständige Abnahme.

**Exit:** AT-010 bis AT-015 und alle Release Gates.

---

# 28. Bewusst nicht implementierte V1-Funktionen

Folgende Funktionen sind auch technisch **nicht** Bestandteil des V1-Kerns:

- automatische Stellensuche/Scraping;
- Auto-Apply oder automatisches Absenden von Bewerbungen;
- Browserextension;
- Gmail/IMAP/Exchange-Synchronisierung;
- Google-/Outlook-Kalendersynchronisierung;
- Cloud-Sync;
- Mehrbenutzerbetrieb;
- eigenes Benutzer-/Rollenmanagement;
- generative KI;
- CV-Scoring per LLM;
- automatisierte externe Kommunikation;
- Plugin-Marktplatz;
- Web-/Mobile-Client.

Die Architektur darf spätere Adapter ermöglichen, aber V1 darf dafür keine versteckten Runtime-/Cloudabhängigkeiten einführen.

---

# 29. Technische Risiken und Gegenmaßnahmen

| Risiko | Auswirkung | Gegenmaßnahme |
|---|---|---|
| WinForms-Forms wachsen zu groß | Wartbarkeit/Testbarkeit sinkt | MVP, UserControls, Application Use Cases, Architekturtests |
| SQLite-Sperren bei Paralleloperationen | Speichern schlägt sporadisch fehl | Single Instance, kurze Transaktionen, WAL, busy timeout, keine DB-Arbeit auf UI-Thread |
| Dokumentdatei und DB laufen auseinander | fehlende Anhänge | Managed Store, Hash, Staging, Integritätscheck |
| Restore beschädigt aktuellen Bestand | hoher Datenverlust | Staging-Restore, Vorabbackup, atomarer Wechsel |
| Zu viele Daten im DataGridView | schlechte UI-Reaktion | Projektion, Paging, Filter, Indizes |
| FTS-/Suchindex inkonsistent | falsche Suchergebnisse | Index ableitbar, Rebuild-Funktion, fachliche Daten bleiben Primärquelle |
| Nutzer verwechselt Archiv/Löschen | irreversibler Verlust | getrennte Aktionen, Impact Preview, Bestätigung |
| Logs enthalten Bewerbungsdaten | Datenschutzverletzung | strukturierte redigierte Logs, Tests |
| Statuskonfiguration bricht Historie | inkonsistente Timeline | stabile Status-IDs, Deaktivieren statt Löschen |
| EF-Migration verliert Daten | Releaseblocker | Referenzdaten, Upgrade-Test, Backup vor riskanter Migration |
| Theme/DPI zerstört Layout | schlechte Bedienbarkeit | Standardcontrols, LayoutPanels, 100–200%-Tests |
| Bibliothek wird unmaintained | Wartungsrisiko | wenige Dependencies, regelmäßige Prüfung |

---

# 30. Offene technische Entscheidungen / ADR-Bedarf

Folgende Details benötigen vor dem jeweiligen Implementierungsmeilenstein einen ADR, ohne dass sie die Grundarchitektur dieses Pflichtenhefts infrage stellen:

1. exakter Installer-Generator und Signierungsweg;
2. GUID-Speicherformat in SQLite;
3. finale WAL-/Synchronous-/Busy-Timeout-Konfiguration;
4. FTS5 vs. alternativer lokaler Volltextindex;
5. konkretes Backupverschlüsselungsformat/KDF;
6. konkrete UI-Automationstechnik für Systemtests;
7. endgültiger Mechanismus der Single-Instance-Kommunikation;
8. Paket für CSV-Parsing oder Eigenimplementierung mit `TextFieldParser`/Standardmitteln;
9. exakte Strategie für Release-Signierung und Prüfsummenpublikation.

Ein ADR darf eine Entscheidung dieses Pflichtenhefts nur ändern, wenn zugleich die technische Spezifikation aktualisiert wird.

---

# 31. Definition of Done für eine Funktion

Eine V1-Funktion gilt nicht allein deshalb als fertig, weil sie in der Oberfläche sichtbar ist. Für jede relevante Funktion gelten proportional:

- fachliche Anforderung/ID bekannt;
- UI-/Application-/Domain-Zuständigkeit sauber;
- Validierung implementiert;
- Fehlerpfad definiert;
- Persistenz/Migration berücksichtigt;
- Unit-/Integrationstest vorhanden, wenn Fach-/Datenlogik betroffen;
- keine neue Analyzerwarnung ohne Begründung;
- Logging ohne sensible Inhalte;
- Tastatur/DPI bei UI-Kernfunktion geprüft;
- Dokumentation/Release Note aktualisiert, wenn Benutzerverhalten geändert;
- Traceability zu Requirement/Test nachvollziehbar.

---

# 32. Verbindlicher technischer Pflichtenkatalog

Die nachfolgenden IDs sind die **technischen Pflichten** dieses Pflichtenhefts. Sie ergänzen die fachlichen `REQ-*`-IDs des Lastenhefts und sollen später direkt auf Implementierung, Tests, ADRs und Release-Evidenz verweisen können.

| ID | Technische Pflicht | Mindestnachweis |
|---|---|---|
| `PFL-BASE-001` | Die Produktivlösung MUSS C# und `net10.0-windows` verwenden. | Projektdateien und Release-Build prüfen. |
| `PFL-BASE-002` | Produktive V1.0-Releases MÜSSEN auf einer unterstützten .NET-10-LTS-Patchversion erzeugt werden. | CI-/Releaseprotokoll und Supportstatus prüfen. |
| `PFL-BASE-003` | Das Release MUSS für `win-x64` self-contained publiziert werden. | Publishartefakt auf sauberem Windows-11-System starten. |
| `PFL-BASE-004` | Nullable Reference Types MÜSSEN in allen neuen Produktprojekten aktiviert sein. | Projekt-/Directory.Build.props-Prüfung. |
| `PFL-BASE-005` | Der kanonische Build MUSS ohne nicht versionierte Visual-Studio-Einstellungen reproduzierbar sein. | Clean-Checkout-Build über Skript/CI. |
| `PFL-BASE-006` | Die Anwendung MUSS als Single-Instance pro Benutzerprofil betrieben werden. | Paralleler Starttest. |
| `PFL-BASE-007` | Die Anwendung MUSS ohne externes Konto und ohne Netzwerkverbindung start- und kernnutzbar sein. | Offline-Systemtest. |
| `PFL-BASE-008` | Produktionsdaten MÜSSEN im Benutzerprofil und nicht im Installationsverzeichnis liegen. | Dateisystemprüfung. |
| `PFL-BASE-009` | Deinstallation DARF Benutzerdaten nicht ohne separate ausdrückliche Entscheidung löschen. | Installer-/Uninstall-Test. |
| `PFL-BASE-010` | Preview-SDKs, Preview-Sprachfeatures und Preview-NuGet-Pakete DÜRFEN NICHT in V1.0 gelangen. | CI-Paket-/SDK-Prüfung. |
| `PFL-ARCH-001` | Die Solution MUSS die Projekte WinForms, Application, Domain und Infrastructure getrennt führen. | Solution-/Referenzprüfung. |
| `PFL-ARCH-002` | Domain DARF NICHT von WinForms, EF Core oder Infrastructure abhängen. | Architekturtest. |
| `PFL-ARCH-003` | Application DARF NICHT von WinForms abhängen. | Architekturtest. |
| `PFL-ARCH-004` | Forms und UserControls DÜRFEN NICHT direkt auf `DbContext` zugreifen. | Architekturtest/Code Review. |
| `PFL-ARCH-005` | Größere WinForms-Funktionsbereiche MÜSSEN Presenter oder gleichwertige Präsentationskomponenten besitzen. | Struktur-/Testprüfung. |
| `PFL-ARCH-006` | Presenter MÜSSEN Application Use Cases verwenden und DÜRFEN kein SQL enthalten. | Code Review/Architekturtest. |
| `PFL-ARCH-007` | Der Composition Root MUSS zentral in Startup/Program liegen. | Codeprüfung. |
| `PFL-ARCH-008` | Dependency Injection DARF NICHT über einen globalen Service Locator aus Views konsumiert werden. | Architekturtest/Review. |
| `PFL-ARCH-009` | Fachliche Operationen MÜSSEN ohne gestartete vollständige WinForms-Anwendung testbar sein. | Application-/Domain-Tests. |
| `PFL-ARCH-010` | Designer-generierter Code DARF NICHT mit Geschäftslogik vermischt werden. | Review/Analyzer-Konvention. |
| `PFL-ARCH-011` | Navigation MUSS über einen zentralen NavigationService/Coordinator erfolgen. | UI-Architekturtest/Review. |
| `PFL-ARCH-012` | Modale Dialoge MÜSSEN einen begrenzten atomaren Zweck besitzen. | UX-Review. |
| `PFL-ARCH-013` | Datei-, Backup- und Exportzugriffe MÜSSEN über Infrastructure-Services gekapselt sein. | Architekturtest. |
| `PFL-ARCH-014` | Zeitabhängige Fachlogik SOLL über eine injizierbare Clock-Abstraktion testbar sein. | Unit-Tests mit Fake Clock. |
| `PFL-ARCH-015` | Dubletten-, Merge- und Delete-Impact-Operationen MÜSSEN als Application Use Cases implementiert sein. | Application-Tests. |
| `PFL-UI-001` | Die `MainForm` MUSS als Shell fungieren und DARF keine umfangreiche Geschäftslogik enthalten. | Code Review. |
| `PFL-UI-002` | Das Dashboard MUSS nach Start ohne zusätzliche Navigation erreichbar sein. | UI-Systemtest. |
| `PFL-UI-003` | Jede Hauptliste MUSS Detailakten per Enter/Doppelklick öffnen können. | Keyboard/UI-Test. |
| `PFL-UI-004` | Statusinformationen DÜRFEN NICHT ausschließlich durch Farbe dargestellt werden. | Accessibility-Test. |
| `PFL-UI-005` | Kernansichten MÜSSEN bei 100–200 % Windows-Skalierung bedienbar bleiben. | DPI-Testmatrix. |
| `PFL-UI-006` | Hauptnavigation und Kerndatenerfassung SOLLEN vollständig per Tastatur möglich sein. | Keyboard-Test. |
| `PFL-UI-007` | Ungespeicherte Änderungen DÜRFEN bei Navigation/Schließen nicht still verloren gehen. | Dirty-State-Test. |
| `PFL-UI-008` | Längere Operationen MÜSSEN einen sichtbaren Busy-/Fortschrittszustand besitzen. | UI-Systemtest. |
| `PFL-UI-009` | Nicht parallele Operationen MÜSSEN gegen unbeabsichtigte Mehrfachausführung geschützt sein. | Presenter-/UI-Test. |
| `PFL-UI-010` | UI-Validierung MUSS feldnah erfolgen, DARF aber Domain-/Application-Validierung nicht ersetzen. | Unit-/UI-Test. |
| `PFL-UI-011` | Board-Statuswechsel MUSS neben Drag-and-drop einen tastatur-/menübasierten Weg besitzen. | Accessibility-/UI-Test. |
| `PFL-UI-012` | Destruktive Aktionen MÜSSEN textlich eindeutig von Archivierung und normalen Änderungen getrennt sein. | UX-/Systemtest. |
| `PFL-UI-013` | Externe Links MÜSSEN vor dem Öffnen als externe Navigation erkennbar sein. | UI-Test. |
| `PFL-UI-014` | Fehlermeldungen MÜSSEN Auswirkung und mögliche nächste Handlung nennen, soweit technisch bekannt. | Fehlerpfadtests. |
| `PFL-UI-015` | Die primäre UI-Sprache MUSS Deutsch sein; UI-Strings SOLLEN aus Ressourcen beziehbar sein. | Resource-/UI-Prüfung. |
| `PFL-DATA-001` | Die produktive Persistenz MUSS SQLite über EF Core verwenden. | Projekt-/Integrationstest. |
| `PFL-DATA-002` | SQLite Foreign Keys MÜSSEN aktiviert sein. | PRAGMA-/Integritätstest. |
| `PFL-DATA-003` | Produktive Schemaänderungen MÜSSEN über versionierte EF-Core-Migrationen erfolgen. | Migrationsordner/CI-Test. |
| `PFL-DATA-004` | `EnsureCreated()` DARF NICHT als produktive Upgrade-Strategie verwendet werden. | Code Review. |
| `PFL-DATA-005` | Kernobjekte MÜSSEN stabile interne GUID-Identitäten besitzen. | Schema-/Domain-Test. |
| `PFL-DATA-006` | Statusänderungen MÜSSEN Current Status, StatusHistory und Timeline konsistent aktualisieren. | Transaktions-/Integrationstest. |
| `PFL-DATA-007` | Archivierung MUSS reversibel sein und DARF Historie nicht löschen. | Integrationstest. |
| `PFL-DATA-008` | Endgültiges Löschen MUSS einen vorab ermittelten Impact berücksichtigen. | Application-/Systemtest. |
| `PFL-DATA-009` | Company-/Contact-Merge MUSS in einer Datenbanktransaktion erfolgen. | Integrationstest mit Failure Injection. |
| `PFL-DATA-010` | Historische JobPosting-Snapshots MÜSSEN getrennt von Benutzerannotation gespeichert werden. | Schema-/Integrationstest. |
| `PFL-DATA-011` | Widersprüchliche SourcedStatements MÜSSEN parallel persistierbar sein. | Domain-/Integrationstest. |
| `PFL-DATA-012` | Eine aktive Bewerbung DARF höchstens eine aktuelle Next Action besitzen. | DB-/Domain-Invariantentest. |
| `PFL-DATA-013` | Commitments MÜSSEN von Tasks getrennt persistiert werden. | Schema-/Domain-Test. |
| `PFL-DATA-014` | Interviewteilnehmer MÜSSEN als n:m-Beziehung zu Contacts persistiert werden. | Integrationstest. |
| `PFL-DATA-015` | Exakte Ereigniszeitpunkte MÜSSEN in UTC normalisiert gespeichert und lokal dargestellt werden. | Zeit-/Zeitzonentest. |
| `PFL-DATA-016` | Reine Fälligkeitstage MÜSSEN ohne unbeabsichtigte Zeitzonenverschiebung als fachliche Tage gespeichert werden. | DateOnly-Test. |
| `PFL-DATA-017` | Geldwerte DÜRFEN NICHT als binäre Fließkommazahl modelliert werden. | Domain-/Schema-Test. |
| `PFL-DATA-018` | Read-Listen MÜSSEN serverseitig filtern/sortieren und dürfen bei großen Beständen nicht unnötig Aggregate materialisieren. | Query-/Performance-Test. |
| `PFL-DATA-019` | Referenzielle Integrität MUSS auch nach Upgrade, Merge, Archivierung und Restore erhalten bleiben. | Integritäts-Suite. |
| `PFL-DATA-020` | Der Volltextindex, sofern eingesetzt, MUSS aus Primärdaten vollständig neu aufbaubar sein. | Rebuild-Test. |
| `PFL-DOC-001` | Eine für eine Bewerbung verwendete Datei MUSS als verwaltete DocumentVersion gespeichert werden. | AT-002/Systemtest. |
| `PFL-DOC-002` | DocumentVersions MÜSSEN einen SHA-256-Inhaltshash besitzen. | Hash-/Integrationstest. |
| `PFL-DOC-003` | Eine bereits versandbezogen referenzierte DocumentVersion DARF inhaltlich nicht überschrieben werden. | Domain-/Integrationstest. |
| `PFL-DOC-004` | Identische Binärdateien KÖNNEN physisch dedupliziert werden, fachliche Versionen MÜSSEN aber unterscheidbar bleiben. | DocumentStore-Test. |
| `PFL-DOC-005` | Dateispeicheroperationen MÜSSEN Staging und Fehler-Cleanup verwenden. | Failure-Injection-Test. |
| `PFL-DOC-006` | Fehlende Dokumentdateien DÜRFEN den restlichen Datenbestand nicht blockieren. | AT/Integrationstest. |
| `PFL-DOC-007` | Importierte Dokumente DÜRFEN NICHT automatisch ausgeführt werden. | Security-Systemtest. |
| `PFL-DOC-008` | Das Öffnen einer Datei MUSS eine explizite Benutzeraktion sein. | UI-Systemtest. |
| `PFL-DOC-009` | Dokumentmetadaten und Bewerbungszuordnung MÜSSEN im offenen Export nachvollziehbar sein. | Exporttest. |
| `PFL-DOC-010` | Backup MUSS alle verwalteten Dokumentdateien oder eindeutig dokumentierte externe Abhängigkeiten enthalten. | Restore-Systemtest. |
| `PFL-SEC-001` | Der V1-Kern DARF keine Telemetrie übertragen. | Netzwerktest. |
| `PFL-SEC-002` | Der V1-Kern DARF keine E-Mail-/Cloud-Credentials verlangen oder speichern. | UI-/Config-Prüfung. |
| `PFL-SEC-003` | HTML-Stellenanzeigentext DARF NICHT in einem aktiven Script-Kontext ausgeführt werden. | Securitytest. |
| `PFL-SEC-004` | Datei- und Backup-Pfade MÜSSEN normalisiert und gegen Traversal geprüft werden. | Negative Securitytests. |
| `PFL-SEC-005` | Logs DÜRFEN standardmäßig keine vollständigen Lebensläufe, E-Mails oder Stellenanzeigen enthalten. | Log-Content-Test. |
| `PFL-SEC-006` | Backupcontainer MÜSSEN vor Restore auf Format, Pfade und Integrität validiert werden. | Manipulations-/Traversal-Test. |
| `PFL-SEC-007` | Passwörter für verschlüsselte Backups DÜRFEN NICHT persistiert werden. | Code-/Speichertest. |
| `PFL-SEC-008` | Verschlüsselte Backups SOLLEN authentifizierte Verschlüsselung verwenden. | Kryptografie-/Roundtrip-Test. |
| `PFL-SEC-009` | Externe URLs DÜRFEN nur nach ausdrücklicher Benutzeraktion geöffnet werden. | Systemtest. |
| `PFL-SEC-010` | Die Anwendung SOLL ohne Administratorrechte im Normalbetrieb arbeiten. | Install-/Betriebstest. |
| `PFL-SEC-011` | Diagnoseexport MUSS vor Erstellung/Weitergabe seinen Inhalt transparent machen. | UI-/Privacy-Test. |
| `PFL-SEC-012` | Importe MÜSSEN Größen- und Strukturgrenzen gegen Ressourcenmissbrauch berücksichtigen. | Security-/Stress-Test. |
| `PFL-OPS-001` | Backup MUSS einen konsistenten SQLite-Datenbankstand enthalten. | Restore-Roundtrip-Test. |
| `PFL-OPS-002` | Backup MUSS Prüfsummen für enthaltene Komponenten besitzen. | Manipulationstest. |
| `PFL-OPS-003` | Restore MUSS zunächst in einem Staging-Bereich erfolgen. | Failure-Injection/Systemtest. |
| `PFL-OPS-004` | Ein fehlgeschlagener Restore DARF den vorherigen Datenbestand nicht zerstören. | Restore-Abbruch-/Fehlertest. |
| `PFL-OPS-005` | Vor riskanter Migration/Restore MUSS ein Backup angeboten oder automatisiert erstellt werden. | Upgrade-/Restore-Test. |
| `PFL-OPS-006` | Ein Update innerhalb V1.x DARF Benutzerdaten nicht löschen. | Upgrade-Systemtest. |
| `PFL-OPS-007` | Die Anwendung MUSS App- und Schema-Version diagnostizierbar machen. | About/Diagnostic-Test. |
| `PFL-OPS-008` | Logs MÜSSEN lokal rotiert und zeit-/größenbegrenzt aufbewahrt werden. | Logrotationstest. |
| `PFL-OPS-009` | Releaseartefakte MÜSSEN aus sauberem Checkout reproduzierbar erzeugt werden. | CI-Release-Nachweis. |
| `PFL-OPS-010` | Releaseartefakte MÜSSEN eine veröffentlichte kryptografische Prüfsumme besitzen. | Releaseprüfung. |
| `PFL-OPS-011` | Installer, Upgrade und Deinstallation MÜSSEN in einer sauberen Windows-11-Testumgebung geprüft werden. | Packaging-Systemtest. |
| `PFL-OPS-012` | Benutzerhilfe MUSS Backup, Restore, Export und endgültige Datenlöschung erklären. | Dokumentationsreview. |
| `PFL-TEST-001` | Domain-Kernregeln MÜSSEN automatisierte Unit Tests besitzen. | Testinventar/CI. |
| `PFL-TEST-002` | Persistenz und Migrationen MÜSSEN gegen echte temporäre SQLite-Dateien getestet werden. | Integrationtests. |
| `PFL-TEST-003` | Die Teststrategie DARF sich für SQLite-Verhalten nicht ausschließlich auf EF InMemory stützen. | Testreview. |
| `PFL-TEST-004` | Architekturgrenzen MÜSSEN automatisiert oder reproduzierbar geprüft werden. | ArchitectureTests. |
| `PFL-TEST-005` | Presenter kritischer Ansichten SOLLTEN ohne echte Forms testbar sein. | Presenter-Tests. |
| `PFL-TEST-006` | Alle 15 Lastenheft-Abnahmefälle MÜSSEN vor V1.0 bestanden sein. | Acceptance Report. |
| `PFL-TEST-007` | AT-011, AT-014 und AT-015 MÜSSEN Release-Gates sein. | Releasecheckliste/CI evidence. |
| `PFL-TEST-008` | Performance MUSS mit synthetischem Referenzbestand von 10.000 Vorgängen/50.000 Aktivitäten gemessen werden. | Benchmarkreport. |
| `PFL-TEST-009` | Typische Filter-/Listenaktionen MÜSSEN im 95. Perzentil <= 1 s liegen. | Performancebericht. |
| `PFL-TEST-010` | Globale Standardsuche MUSS im 95. Perzentil <= 2 s liegen. | Performancebericht. |
| `PFL-TEST-011` | Programmstart SOLL mit Referenzbestand <= 5 s bis bedienbare Shell/Dashboardzustand erreichen. | Startbenchmark. |
| `PFL-TEST-012` | Securitytests MÜSSEN mindestens Path Traversal, manipuliertes Backup, aktives HTML und Log-Leaks abdecken. | Security-Testreport. |
| `PFL-TEST-013` | Migrationsreferenzdaten MÜSSEN pro freigegebener V1.x-Basis aufbewahrt werden. | Testdata-Repository. |
| `PFL-TEST-014` | Produktive Testdaten DÜRFEN keine realen Bewerbungs-/Personendaten enthalten. | Testdatenreview. |
| `PFL-TEST-015` | Build, Unit-, Integration- und Architekturtests MÜSSEN im CI für Merge-/Releasepfade ausgeführt werden. | CI-Konfiguration. |

**Anzahl technischer Pflichten:** 109.

---

# 33. Quellen und technische Referenzen

## SASD Development Standard

- Repository: `Robin-Goerlach/SASD-Development-Standard`
- C#/.NET Profile, Approved 0.9.0
- Solution and Project Structure, Approved 0.9.0
- Error Handling, Approved 0.9.0
- Logging and Diagnostics, Approved 0.9.0
- Persistence, Approved 0.9.0
- .NET Testing, Approved 0.9.0
- Desktop Application Profile, Approved 0.9.0
- UI Architecture, Approved 0.9.0
- User Experience, Approved 0.9.0
- Application Lifecycle, Approved 0.9.0
- Windows Forms Guidance, informative 0.9.0

## Microsoft/.NET

- Official .NET Support Policy, Stand August 2026: .NET 10 LTS aktiv, Support bis November 2028.
- Windows Forms for .NET 10 documentation.
- Windows Forms / .NET Desktop SDK documentation.
- .NET on Windows support matrix.

---

# Anhang A – Technische Traceability des vollständigen Lastenhefts

Dieser Anhang wurde aus dem Eingangs-Lastenheft generiert. Er enthält **jede dort identifizierte Requirement-ID**. Dadurch ist nachvollziehbar, dass beim Übergang vom Lastenheft zum Pflichtenheft keine Anforderung allein durch Dokumentkürzung verloren gegangen ist.

Die Spalte „Umsetzungsbereich“ zeigt die primäre technische Heimat. Viele Anforderungen werden zusätzlich durch Querschnittstests abgesichert.

| Lastenheft-ID | Prio | Umsetzungsbereich | Schichten/Nachweis | Plan V1 | Kurzanforderung |
|---|---|---|---|---|---|
| `REQ-F-001` | Must | Bootstrap, lokale Konfiguration, Dashboard-Shell | UI/Application/Infrastructure | V1.0 Release-Gate | Die Anwendung MUSS ohne Benutzerkonto bei einem externen Dienst nutzbar sein. |
| `REQ-F-002` | Must | Bootstrap, lokale Konfiguration, Dashboard-Shell | UI/Application/Infrastructure | V1.0 Release-Gate | Die Anwendung MUSS einen lokalen Einzelbenutzerbetrieb unterstützen. |
| `REQ-F-003` | Must | Bootstrap, lokale Konfiguration, Dashboard-Shell | UI/Application/Infrastructure | V1.0 Release-Gate | Die Anwendung MUSS einen klar erkennbaren Start-/Dashboardbereich bereitstellen. |
| `REQ-F-004` | Should | Bootstrap, lokale Konfiguration, Dashboard-Shell | UI/Application/Infrastructure | V1.0 geplant; Verschiebung nur dokumentiert | Die Anwendung SOLL einen kurzen Erststart-Assistenten oder eine gleichwertige Einführung bieten. |
| `REQ-F-005` | Must | Bootstrap, lokale Konfiguration, Dashboard-Shell | UI/Application/Infrastructure | V1.0 Release-Gate | Der Nutzer MUSS Grundeinstellungen wie bevorzugte Datumsdarstellung, Standardstatus und Standardquellen verwalten können. |
| `REQ-F-006` | Could | Bootstrap, lokale Konfiguration, Dashboard-Shell | UI/Application/Infrastructure | V1.x-Kandidat, nicht release-blockierend | Die Anwendung KANN einen Demo-/Beispieldatenbestand für Lern- und Testzwecke anbieten. |
| `REQ-F-010` | Must | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Unternehmen MÜSSEN als eigenständige Datensätze verwaltet werden. |
| `REQ-F-011` | Must | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Zu einem Unternehmen MÜSSEN Name, Website, Hauptstandort und freie Notizen hinterlegt werden können. |
| `REQ-F-012` | Should | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Branche, Unternehmensart und weitere strukturierte Merkmale SOLLEN pflegbar sein. |
| `REQ-F-013` | Must | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Unternehmen MUSS mehrere Opportunities, Bewerbungen und Kontakte besitzen können. |
| `REQ-F-014` | Must | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Die Unternehmensakte MUSS frühere und aktuelle Bewerbungen zusammenhängend anzeigen. |
| `REQ-F-015` | Should | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Der Nutzer SOLL Unternehmensnotizen für Recherche, Kultur, Produkte, Technik und Gesprächsvorbereitung strukturieren können. |
| `REQ-F-016` | Should | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Offensichtliche Unternehmensdubletten SOLLEN vor oder nach dem Anlegen erkennbar sein. |
| `REQ-F-017` | Should | Unternehmensmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Unternehmen SOLLEN zusammengeführt werden können, ohne verknüpfte Bewerbungen oder Aktivitäten zu verlieren. |
| `REQ-F-020` | Must | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.0 Release-Gate | Kontakte MÜSSEN als eigenständige Personenobjekte verwaltet werden. |
| `REQ-F-021` | Must | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Kontakt MUSS mindestens Name, Rolle, Unternehmen, E-Mail, Telefon und Profil-/Weblink aufnehmen können. |
| `REQ-F-022` | Must | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Kontakt MUSS mit mehreren Bewerbungen und Opportunities verknüpft werden können. |
| `REQ-F-023` | Must | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.0 Release-Gate | Die Kontaktakte MUSS die zugehörigen Aktivitäten und letzten Interaktionen anzeigen. |
| `REQ-F-024` | Should | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Für Kontakte SOLLEN Tags beziehungsweise Gruppen verwaltet werden können. |
| `REQ-F-025` | Should | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Der Nutzer SOLL einen nächsten Kontakt- oder Follow-up-Zeitpunkt hinterlegen können. |
| `REQ-F-026` | Should | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Mögliche Kontaktdubletten SOLLEN erkennbar und kontrolliert zusammenführbar sein. |
| `REQ-F-027` | Could | Kontakt- und Recruiter-CRM | Domain/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Kontakte KÖNNEN mit einer persönlichen Beziehungs-/Relevanzeinschätzung versehen werden. |
| `REQ-F-030` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Eine berufliche Opportunity MUSS unabhängig von einer konkreten Stellenanzeigen-Version angelegt werden können. |
| `REQ-F-031` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Eine Opportunity MUSS einem Unternehmen zugeordnet werden können. |
| `REQ-F-032` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Eine Opportunity MUSS Stellenbezeichnung, Standort, Beschäftigungsart und Arbeitsmodell aufnehmen können. |
| `REQ-F-033` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Gehalts-/Vergütungsinformationen MÜSSEN strukturiert und zusätzlich als Freitext erfassbar sein. |
| `REQ-F-034` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Eine persönliche Priorität oder Interessensbewertung MUSS erfassbar sein. |
| `REQ-F-035` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Quellen einer Opportunity beziehungsweise Stellenanzeige MÜSSEN dokumentiert werden können. |
| `REQ-F-036` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Zur Stellenanzeige MUSS die Original-URL gespeichert werden können. |
| `REQ-F-037` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Der Text einer Stellenanzeige MUSS als lokaler Snapshot archiviert werden können. |
| `REQ-F-038` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Der Zeitpunkt der Erfassung beziehungsweise des Snapshots MUSS nachvollziehbar sein. |
| `REQ-F-039` | Should | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Eine Opportunity SOLL mehrere Versionen beziehungsweise Snapshots einer Ausschreibung verwalten können. |
| `REQ-F-040` | Could | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Unterschiede zwischen zwei gespeicherten Stellenanzeigen-Versionen KÖNNEN hervorgehoben werden. |
| `REQ-F-041` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Stellenbeschreibungen MÜSSEN zusätzlich frei annotierbar sein. |
| `REQ-F-042` | Should | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Relevante Skills, Technologien und Anforderungen SOLLEN strukturiert oder per Tags erfassbar sein. |
| `REQ-F-043` | Should | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Wichtige fachliche Aussagen SOLLEN mit Herkunft, Zeitpunkt und optionaler Person dokumentiert werden können. |
| `REQ-F-044` | Should | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Widersprüchliche Aussagen SOLLEN nebeneinander bestehen können, ohne ältere Informationen zwangsweise zu überschreiben. |
| `REQ-F-045` | Should | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Wiederveröffentlichungen derselben oder sehr ähnlicher Rolle SOLLEN als mögliche Dubletten erkennbar sein. |
| `REQ-F-046` | Must | Opportunity-, JobPosting- und Informationsmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Das bloße Merken einer Opportunity DARF NICHT automatisch als versendete Bewerbung gewertet werden. |
| `REQ-F-050` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Eine konkrete Bewerbung MUSS als eigener Datensatz innerhalb einer Opportunity geführt werden. |
| `REQ-F-051` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Eine Opportunity MUSS mehr als eine Bewerbung zulassen, falls der Nutzer sich zu einem späteren Zeitpunkt erneut bewirbt. |
| `REQ-F-052` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Bewerbungsdatum und Bewerbungsweg MÜSSEN erfassbar sein. |
| `REQ-F-053` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Jede Bewerbung MUSS einen fachlichen Status besitzen. |
| `REQ-F-054` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Version 1 MUSS mindestens Standardphasen für vorgemerkt, Vorbereitung, beworben, Interview, Angebot und abgeschlossen bereitstellen. |
| `REQ-F-055` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Abgeschlossene Ergebnisse MÜSSEN differenziert werden können. |
| `REQ-F-056` | Should | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Statusstufen SOLLEN durch den Nutzer erweitert oder angepasst werden können. |
| `REQ-F-057` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Statusänderungen MÜSSEN in der Timeline nachvollziehbar werden. |
| `REQ-F-058` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Die Bewerbungsakte MUSS alle verknüpften Kontakte, Aktivitäten, Aufgaben, Interviews, Dokumente und Commitments zugänglich machen. |
| `REQ-F-059` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Die Bewerbungsakte MUSS freie Notizen unterstützen. |
| `REQ-F-060` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Der Nutzer MUSS ein erwartetes oder bereits genanntes Gehalt und weitere Konditionen bewerbungsspezifisch dokumentieren können. |
| `REQ-F-061` | Should | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Eine Bewerbung SOLL eine frei definierbare persönliche Bewertung beziehungsweise Attraktivität besitzen können. |
| `REQ-F-062` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Archivierte oder abgeschlossene Bewerbungen MÜSSEN erhalten und weiterhin durchsuchbar bleiben. |
| `REQ-F-063` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Der Nutzer MUSS eine Bewerbung bewusst zurückziehen können. |
| `REQ-F-064` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Der Nutzer MUSS einen Vorgang duplizieren können, ohne historische Aktivitäten oder Bewerbungsereignisse versehentlich zu kopieren. |
| `REQ-F-065` | Should | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Die Anwendung SOLL vor offensichtlich doppelten aktiven Bewerbungen warnen. |
| `REQ-F-066` | Must | Bewerbungsakte und Bewerbungsworkflow | Domain/Application/UI/Persistence | V1.0 Release-Gate | Eine Bewerbung DARF NICHT automatisch versendet oder extern eingereicht werden. |
| `REQ-F-070` | Must | Pipeline-/Boardmodul | UI/Presentation/Application | V1.0 Release-Gate | Aktive Bewerbungen MÜSSEN in einer Pipeline-/Boardansicht darstellbar sein. |
| `REQ-F-071` | Must | Pipeline-/Boardmodul | UI/Presentation/Application | V1.0 Release-Gate | Ein Statuswechsel MUSS direkt aus der Boardansicht möglich sein. |
| `REQ-F-072` | Must | Pipeline-/Boardmodul | UI/Presentation/Application | V1.0 Release-Gate | Die Boardansicht MUSS wesentliche Kurzinfos anzeigen, ohne die Detailakte öffnen zu müssen. |
| `REQ-F-073` | Must | Pipeline-/Boardmodul | UI/Presentation/Application | V1.0 Release-Gate | Abgeschlossene Vorgänge MÜSSEN standardmäßig aus der aktiven Pipeline ausgeblendet, aber gezielt einblendbar sein. |
| `REQ-F-074` | Should | Pipeline-/Boardmodul | UI/Presentation/Application | V1.0 geplant; Verschiebung nur dokumentiert | Boardansicht SOLL nach Quelle, Unternehmen, Priorität oder Tags filterbar sein. |
| `REQ-F-075` | Should | Pipeline-/Boardmodul | UI/Presentation/Application | V1.0 geplant; Verschiebung nur dokumentiert | Der Nutzer SOLL die Reihenfolge innerhalb einer Statusphase nach relevanten Kriterien sortieren können. |
| `REQ-F-080` | Must | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Aktivitäten MÜSSEN getrennt von Status und Aufgaben verwaltet werden. |
| `REQ-F-081` | Must | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Mindestens Bewerbung, E-Mail, Telefonat, LinkedIn/Netzwerk, Meeting, Interview, Dokumentversand, Angebot, Absage, Statusänderung und freie Notiz MÜSSEN als Aktivität abbildbar sein. |
| `REQ-F-082` | Must | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Aktivitäten MÜSSEN Zeitpunkt, Typ, Beschreibung und relevante Verknüpfungen speichern können. |
| `REQ-F-083` | Must | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Die Bewerbungsakte MUSS eine chronologisch sortierte Timeline anzeigen. |
| `REQ-F-084` | Must | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Die Timeline MUSS zwischen bereits geschehenen Ereignissen und zukünftigen Aufgaben/Terminen unterscheiden. |
| `REQ-F-085` | Must | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Manuell angelegte Aktivitäten MÜSSEN korrigierbar sein. |
| `REQ-F-086` | Should | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Das Löschen einer Aktivität SOLL eine Bestätigung erfordern, wenn Verknüpfungen oder Folgeinformationen betroffen sind. |
| `REQ-F-087` | Must | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Kommunikation MUSS manuell protokolliert werden können, ohne dass ein externes Postfach verbunden ist. |
| `REQ-F-088` | Should | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Eine Kommunikation SOLL optional einen Dateianhang oder externen Referenzlink besitzen können. |
| `REQ-F-089` | Could | Aktivitäts-, Kommunikations- und Timeline-Modul | Domain/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Aktivitäten KÖNNEN über Vorlagen schneller erfasst werden. |
| `REQ-F-090` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Jede aktive Bewerbung MUSS eine nächste Aktion oder einen bewusst gesetzten Wartezustand besitzen können. |
| `REQ-F-091` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Aktive Bewerbungen ohne Next Action MÜSSEN im Dashboard erkennbar sein. |
| `REQ-F-092` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Next Actions MÜSSEN ein Fälligkeits- oder Wiedervorlagedatum besitzen können. |
| `REQ-F-093` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Erledigte Next Actions MÜSSEN in die Historie übergehen können. |
| `REQ-F-094` | Should | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Eine Next Action SOLL direkt in eine Aufgabe oder einen Termin überführt werden können. |
| `REQ-F-095` | Should | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Eine Wiedervorlage SOLL verschoben beziehungsweise „gesnoozed“ werden können. |
| `REQ-F-096` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Commitments anderer Personen MÜSSEN als eigener fachlicher Typ erfasst werden können. |
| `REQ-F-097` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Commitment MUSS mindestens Beteiligten, Inhalt, Fälligkeit, Status und Bezug zum Vorgang speichern können. |
| `REQ-F-098` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Commitment MUSS mit der auslösenden Aktivität oder Kommunikation verknüpft werden können. |
| `REQ-F-099` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Überfällige, nicht erfüllte Commitments MÜSSEN auf dem Dashboard erscheinen. |
| `REQ-F-100` | Must | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Commitment MUSS als erfüllt, nicht erfüllt, entfallen oder verschoben markiert werden können. |
| `REQ-F-101` | Should | Next Action, Wiedervorlage und Commitment | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Aus einem überfälligen Commitment SOLL eine eigene Follow-up-Aufgabe erzeugt werden können. |
| `REQ-F-105` | Must | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.0 Release-Gate | Aufgaben MÜSSEN unabhängig und mit Bezug zu Bewerbung, Opportunity, Unternehmen oder Kontakt angelegt werden können. |
| `REQ-F-106` | Must | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.0 Release-Gate | Aufgaben MÜSSEN Titel, Status, Priorität, Fälligkeit und Notiz unterstützen. |
| `REQ-F-107` | Must | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.0 Release-Gate | Aufgaben MÜSSEN als erledigt markiert werden können und danach historisch nachvollziehbar bleiben. |
| `REQ-F-108` | Should | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Aufgaben SOLLEN einfache Checklisten beziehungsweise Unterpunkte unterstützen. |
| `REQ-F-109` | Must | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.0 Release-Gate | Heute fällige und überfällige Aufgaben MÜSSEN im Dashboard erscheinen. |
| `REQ-F-110` | Should | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Aufgaben SOLLEN nach Priorität, Fälligkeit, Status und Bezug filterbar sein. |
| `REQ-F-111` | Could | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Wiederkehrende Aufgaben KÖNNEN innerhalb von V1.x unterstützt werden. |
| `REQ-F-112` | Could | Aufgaben und Checklisten | Domain/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Aufgaben- oder Checklisten-Vorlagen KÖNNEN bereitgestellt werden. |
| `REQ-F-120` | Must | Interviewmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Interviews MÜSSEN als eigenständige Ereignisse innerhalb einer Bewerbung verwaltet werden. |
| `REQ-F-121` | Must | Interviewmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Interview MUSS Datum, Uhrzeit, Format, Ort beziehungsweise Meeting-Link und Notizen aufnehmen können. |
| `REQ-F-122` | Must | Interviewmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Ein Interview MUSS mehrere Teilnehmer/Kontakte unterstützen. |
| `REQ-F-123` | Must | Interviewmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Interviewrunden MÜSSEN fachlich benennbar sein. |
| `REQ-F-124` | Must | Interviewmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Der Nutzer MUSS Vorbereitungspunkte und eigene Fragen zum Interview dokumentieren können. |
| `REQ-F-125` | Must | Interviewmodul | Domain/Application/UI/Persistence | V1.0 Release-Gate | Nach dem Interview MÜSSEN Gesprächsnotizen, Learnings und Follow-up festgehalten werden können. |
| `REQ-F-126` | Should | Interviewmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Der Nutzer SOLL das Interview nach eigenen Kriterien bewerten können. |
| `REQ-F-127` | Should | Interviewmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Wichtige Aussagen aus einem Interview SOLLEN als quellenbezogene Information in die Bewerbungsakte übernommen werden können. |
| `REQ-F-128` | Should | Interviewmodul | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Ein Interview SOLL automatisch beziehungsweise direkt eine vorbereitende Aufgabe und ein Follow-up anlegen können, wenn der Nutzer dies auslöst. |
| `REQ-F-129` | Could | Interviewmodul | Domain/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Eine persönliche, wiederverwendbare Fragenbibliothek KANN in V1.x verfügbar sein. |
| `REQ-F-130` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Die Anwendung MUSS Lebensläufe, Anschreiben, Zeugnisse und weitere Bewerbungsdokumente verwalten können. |
| `REQ-F-131` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Mehrere Versionen desselben Dokumenttyps MÜSSEN unterscheidbar bleiben. |
| `REQ-F-132` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Einer Bewerbung MÜSSEN genau die Dokumentversionen zugeordnet werden können, die tatsächlich verwendet beziehungsweise versendet wurden. |
| `REQ-F-133` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Dokumente MÜSSEN mit Titel, Kategorie, Version/Stand, Datum und optionalen Notizen versehen werden können. |
| `REQ-F-134` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Stellenanzeigen-Snapshots MÜSSEN ebenfalls als historische Bewerbungsunterlagen erhalten bleiben können. |
| `REQ-F-135` | Should | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Die Anwendung SOLL erkennen können, wenn exakt dieselbe Datei mehrfach hinzugefügt wird. |
| `REQ-F-136` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Fehlende oder extern verschobene Dokumentdateien DÜRFEN nicht zum Verlust der restlichen Bewerbungsakte führen. |
| `REQ-F-137` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Der Nutzer MUSS Dokumente aus ihrer Akte öffnen beziehungsweise in der vorgesehenen Standardanwendung aufrufen können. |
| `REQ-F-138` | Must | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 Release-Gate | Das Löschen eines verwendeten Dokuments MUSS vor dem Entfernen auf bestehende Bewerbungszuordnungen hinweisen. |
| `REQ-F-139` | Should | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Dokumente SOLLEN nach Kategorie, Verwendung, Unternehmen oder Bewerbung auffindbar sein. |
| `REQ-F-140` | Could | Dokument- und Versionsverwaltung | Infrastructure/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Die Anwendung KANN eine reine Metadaten-/Linkverwaltung zusätzlich zu verwalteten lokalen Kopien erlauben. |
| `REQ-F-150` | Must | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.0 Release-Gate | Das Dashboard MUSS heute fällige Aufgaben und Next Actions anzeigen. |
| `REQ-F-151` | Must | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.0 Release-Gate | Das Dashboard MUSS überfällige Aufgaben, Next Actions und Commitments hervorheben. |
| `REQ-F-152` | Must | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.0 Release-Gate | Das Dashboard MUSS bevorstehende Interviews anzeigen. |
| `REQ-F-153` | Must | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.0 Release-Gate | Das Dashboard MUSS aktive Bewerbungen ohne definierte Next Action anzeigen. |
| `REQ-F-154` | Must | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.0 Release-Gate | Das Dashboard MUSS Vorgänge anzeigen können, bei denen der Nutzer bewusst auf eine Reaktion wartet. |
| `REQ-F-155` | Should | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Das Dashboard SOLL kompakte Kennzahlen zu aktiven Bewerbungen, Interviews, Angeboten und offenen Aufgaben anzeigen. |
| `REQ-F-156` | Should | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Dashboardbereiche SOLLEN direkt in passende Detail- oder Filteransichten führen. |
| `REQ-F-157` | Could | Dashboard/Tagessteuerung | Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Der Nutzer KANN in V1.x Dashboardbereiche ein-/ausblenden oder anordnen. |
| `REQ-F-160` | Must | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 Release-Gate | Die Anwendung MUSS eine globale Suche über zentrale fachliche Objekte anbieten. |
| `REQ-F-161` | Must | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 Release-Gate | Bewerbungen und Opportunities MÜSSEN in einer tabellarischen beziehungsweise Listenansicht darstellbar sein. |
| `REQ-F-162` | Must | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 Release-Gate | Filter MÜSSEN mindestens Status, Unternehmen, Quelle, Priorität, Tags und Zeitraum unterstützen. |
| `REQ-F-163` | Should | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Mehrere Filter SOLLEN kombinierbar sein. |
| `REQ-F-164` | Should | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Häufig verwendete Filter SOLLEN als gespeicherte Ansicht abgelegt werden können. |
| `REQ-F-165` | Should | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Termine und fällige Aktionen SOLLEN in einer Kalenderansicht darstellbar sein. |
| `REQ-F-166` | Must | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 Release-Gate | Die Detailakte MUSS aus Such-, Listen-, Board- und Dashboardansicht erreichbar sein. |
| `REQ-F-167` | Should | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Listen SOLLEN nach relevanten Spalten sortierbar sein. |
| `REQ-F-168` | Must | Suche, Filter, Listen, Board und Kalender | Application/UI/Persistence | V1.0 Release-Gate | Tags MÜSSEN frei pflegbar und mehreren zentralen Objekten zuweisbar sein. |
| `REQ-F-170` | Must | Analytics | Application/UI/Persistence | V1.0 Release-Gate | Die Anwendung MUSS die Anzahl aktiver, abgeschlossener und insgesamt erfasster Bewerbungen auswerten können. |
| `REQ-F-171` | Must | Analytics | Application/UI/Persistence | V1.0 Release-Gate | Statusverteilung MUSS auswertbar sein. |
| `REQ-F-172` | Should | Analytics | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Response-, Interview- und Offer-Rate SOLLEN berechnet werden können. |
| `REQ-F-173` | Should | Analytics | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Zeit bis zur ersten Rückmeldung SOLL auswertbar sein, sofern ausreichende Daten vorliegen. |
| `REQ-F-174` | Should | Analytics | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Ergebnisse SOLLEN nach Quelle verglichen werden können. |
| `REQ-F-175` | Could | Analytics | Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Erfolgswerte verschiedener Dokumentversionen KÖNNEN in V1.x verglichen werden. |
| `REQ-F-176` | Must | Analytics | Application/UI/Persistence | V1.0 Release-Gate | Analytics DÜRFEN niedrige Fallzahlen nicht als scheinbar belastbare Erkenntnis darstellen. |
| `REQ-F-177` | Should | Analytics | Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Auswertungen SOLLEN nach Zeitraum filterbar sein. |
| `REQ-F-178` | Could | Analytics | Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Kennzahlen KÖNNEN als CSV oder vergleichbares offenes Format exportiert werden. |
| `REQ-F-180` | Must | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 Release-Gate | Der Nutzer MUSS den vollständigen fachlichen Datenbestand sichern können. |
| `REQ-F-181` | Must | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 Release-Gate | Ein Backup MUSS wiederherstellbar sein. |
| `REQ-F-182` | Must | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 Release-Gate | Vor einer Wiederherstellung MUSS die Anwendung einen erkennbaren Schutz gegen versehentliches Überschreiben des aktuellen Bestands bieten. |
| `REQ-F-183` | Must | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 Release-Gate | Der Nutzer MUSS seine zentralen Daten in einem offenen, dokumentierten Format exportieren können. |
| `REQ-F-184` | Should | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 geplant; Verschiebung nur dokumentiert | Dokumentmetadaten und Zuordnungen SOLLEN exportierbar sein. |
| `REQ-F-185` | Should | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 geplant; Verschiebung nur dokumentiert | Ein CSV-Import für einen pragmatischen Einstieg aus Tabellen SOLL unterstützt werden. |
| `REQ-F-186` | Must | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 Release-Gate | Ein Import DARF vorhandene manuell gepflegte Daten nicht stillschweigend überschreiben. |
| `REQ-F-187` | Should | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 geplant; Verschiebung nur dokumentiert | Vor größeren Importen SOLL eine Vorschau mit Anzahl neuer, geänderter und problematischer Datensätze möglich sein. |
| `REQ-F-188` | Must | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 Release-Gate | Backup- und Restore-Vorgänge MÜSSEN Erfolg oder Fehler eindeutig melden. |
| `REQ-F-189` | Should | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.0 geplant; Verschiebung nur dokumentiert | Backups SOLLEN eine prüfbare Integritätsinformation besitzen. |
| `REQ-F-190` | Could | Import, Export, Backup und Restore | Infrastructure/Application/UI | V1.x-Kandidat, nicht release-blockierend | V1.x KANN einen portablen Komplett-Export zur Migration auf einen anderen Rechner anbieten. |
| `REQ-F-200` | Must | Archivierung, Löschung und Datenpflege | Domain/Application/UI/Persistence | V1.0 Release-Gate | Abgeschlossene Vorgänge MÜSSEN archiviert werden können, ohne ihre Historie zu verlieren. |
| `REQ-F-201` | Must | Archivierung, Löschung und Datenpflege | Domain/Application/UI/Persistence | V1.0 Release-Gate | Der Nutzer MUSS archivierte Vorgänge wieder aktivieren können. |
| `REQ-F-202` | Must | Archivierung, Löschung und Datenpflege | Domain/Application/UI/Persistence | V1.0 Release-Gate | Endgültiges Löschen MUSS von Archivierung klar unterschieden sein. |
| `REQ-F-203` | Must | Archivierung, Löschung und Datenpflege | Domain/Application/UI/Persistence | V1.0 Release-Gate | Endgültiges Löschen verknüpfter Kernobjekte MUSS vor Datenfolgen warnen. |
| `REQ-F-204` | Should | Archivierung, Löschung und Datenpflege | Domain/Application/UI/Persistence | V1.0 geplant; Verschiebung nur dokumentiert | Der Nutzer SOLL veraltete Daten gezielt nach Alter, Status und Archivzustand auffinden können. |
| `REQ-F-205` | Could | Archivierung, Löschung und Datenpflege | Domain/Application/UI/Persistence | V1.x-Kandidat, nicht release-blockierend | Eine optionale Aufbewahrungs-/Bereinigungsunterstützung KANN in V1.x angeboten werden. |
| `REQ-F-210` | Must | Einstellungen und Anpassbarkeit | Application/UI/Infrastructure | V1.0 Release-Gate | Nutzerdefinierte Tags, Quellen und grundlegende Listenwerte MÜSSEN pflegbar sein. |
| `REQ-F-211` | Should | Einstellungen und Anpassbarkeit | Application/UI/Infrastructure | V1.0 geplant; Verschiebung nur dokumentiert | Pipelinephasen SOLLEN mit verständlichen Regeln konfigurierbar sein. |
| `REQ-F-212` | Should | Einstellungen und Anpassbarkeit | Application/UI/Infrastructure | V1.0 geplant; Verschiebung nur dokumentiert | Standardzeiträume für Dashboard und bevorstehende Interviews SOLLEN konfigurierbar sein. |
| `REQ-F-213` | Should | Einstellungen und Anpassbarkeit | Application/UI/Infrastructure | V1.0 geplant; Verschiebung nur dokumentiert | Datums-/Zeitdarstellung und grundlegende UI-Präferenzen SOLLEN anpassbar sein. |
| `REQ-F-214` | Could | Einstellungen und Anpassbarkeit | Application/UI/Infrastructure | V1.x-Kandidat, nicht release-blockierend | V1.x KANN benutzerdefinierte Zusatzfelder für ausgewählte Objekte unterstützen. |
| `REQ-Q-001` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Häufige Kernaktionen MÜSSEN mit wenigen, nachvollziehbaren Schritten erreichbar sein. |
| `REQ-Q-002` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Die Oberfläche MUSS konsistente Begriffe für Company, Kontakt, Opportunity, Stellenanzeige, Bewerbung, Aktivität, Aufgabe und Commitment verwenden. |
| `REQ-Q-003` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Fehlermeldungen MÜSSEN die Auswirkung und eine sinnvolle nächste Handlung nennen. |
| `REQ-Q-004` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Destruktive Aktionen MÜSSEN klar von normalen Bearbeitungsaktionen unterscheidbar sein. |
| `REQ-Q-005` | Should | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 geplant; Verschiebung nur dokumentiert | Die Anwendung SOLL weitgehend per Tastatur bedienbar sein. |
| `REQ-Q-006` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Information DARF NICHT ausschließlich über Farbe vermittelt werden. |
| `REQ-Q-007` | Should | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 geplant; Verschiebung nur dokumentiert | Die Oberfläche SOLL bei üblichen Windows-Skalierungen von 100 % bis 200 % ohne abgeschnittene Kerninhalte nutzbar bleiben. |
| `REQ-Q-008` | Should | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 geplant; Verschiebung nur dokumentiert | Relevante Bedienelemente SOLLEN zugängliche Namen/Labels für unterstützende Technologien besitzen. |
| `REQ-Q-010` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Die Hauptansichten MÜSSEN bei einem Referenzbestand von 10.000 Vorgängen und 50.000 Aktivitäten im Regelfall innerhalb von 1 Sekunde auf Benutzeraktionen reagieren. |
| `REQ-Q-011` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Eine globale Standardsuche MUSS bei Referenzbestand innerhalb von 2 Sekunden ein Ergebnis liefern. |
| `REQ-Q-012` | Should | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 geplant; Verschiebung nur dokumentiert | Programmstart mit Referenzbestand SOLL innerhalb von 5 Sekunden zur bedienbaren Hauptansicht führen. |
| `REQ-Q-013` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Längere Operationen wie Backup, Restore oder großer Import DÜRFEN die Anwendung nicht scheinbar eingefroren wirken lassen. |
| `REQ-Q-020` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Erfolgreich bestätigte Speichervorgänge DÜRFEN bei einem normalen Neustart nicht verloren gehen. |
| `REQ-Q-021` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Ein unerwarteter Programmabbruch DARF den gesamten Datenbestand nicht unbrauchbar machen. |
| `REQ-Q-022` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Backup/Restore MUSS fachliche Beziehungen erhalten. |
| `REQ-Q-023` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Datenmigrationen zwischen kompatiblen V1.x-Releases DÜRFEN vorhandene fachliche Daten nicht still verlieren. |
| `REQ-Q-024` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Fehlende externe Dokumentdateien DÜRFEN den restlichen Datenbestand nicht blockieren. |
| `REQ-Q-030` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Alle V1-Kernfunktionen MÜSSEN ohne Internetverbindung verfügbar sein. |
| `REQ-Q-031` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Fehlende Internetverbindung DARF lokale Funktionen nicht unnötig blockieren. |
| `REQ-Q-040` | Must | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 Release-Gate | Version 1 MUSS vollständig in deutscher Sprache bedienbar sein. |
| `REQ-Q-041` | Should | Qualität, UX, Performance und Zuverlässigkeit | alle Schichten/Test/Packaging | V1.0 geplant; Verschiebung nur dokumentiert | Texte und Fachbegriffe SOLLEN so strukturiert sein, dass eine spätere Lokalisierung möglich bleibt. |
| `REQ-SEC-001` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Personenbezogene Bewerbungsdaten MÜSSEN standardmäßig lokal verarbeitet werden. |
| `REQ-SEC-002` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Die Anwendung DARF ohne ausdrückliches Opt-in keine Telemetrie mit personenbezogenen oder inhaltlichen Bewerbungsdaten übertragen. |
| `REQ-SEC-003` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Externe Übertragungen oder Online-Aufrufe MÜSSEN für den Benutzer erkennbar und zweckgebunden sein. |
| `REQ-SEC-004` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Importierte Texte, URLs und Dateien MÜSSEN als nicht vertrauenswürdige Eingaben behandelt werden. |
| `REQ-SEC-005` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Das Anzeigen oder Importieren einer Stellenbeschreibung DARF eingebettete aktive Inhalte nicht unkontrolliert ausführen. |
| `REQ-SEC-006` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Importierte Dokumente DÜRFEN durch die Anwendung nicht automatisch ausgeführt werden. |
| `REQ-SEC-007` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Diagnosedaten und Logs MÜSSEN standardmäßig keine vollständigen Lebensläufe, E-Mail-Texte oder vergleichbar sensible Inhalte protokollieren. |
| `REQ-SEC-008` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Endgültiges Löschen personenbezogener Daten MUSS durch den Nutzer möglich sein, soweit keine verbleibenden Abhängigkeiten bewusst erhalten werden. |
| `REQ-SEC-009` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Datenexport MUSS dem Nutzer ermöglichen, seinen eigenen Datenbestand unabhängig vom Produkt zu sichern. |
| `REQ-SEC-010` | Should | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 geplant; Verschiebung nur dokumentiert | Backups mit sensiblen Daten SOLLEN optional geschützt beziehungsweise verschlüsselt werden können. |
| `REQ-SEC-011` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Die Anwendung DARF in Version 1 keine Zugangsdaten für E-Mail-/Cloud-Dienste verlangen, da diese Integrationen nicht zum Scope gehören. |
| `REQ-SEC-012` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Externe Links MÜSSEN als externe Navigation erkennbar sein. |
| `REQ-SEC-013` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Datei- und Backupoperationen MÜSSEN gegen unbeabsichtigtes Überschreiben beziehungsweise Pfadmissbrauch geschützt sein. |
| `REQ-SEC-014` | Should | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 geplant; Verschiebung nur dokumentiert | Datenschutzrelevante Standardwerte SOLLEN Datenminimierung fördern. |
| `REQ-SEC-015` | Must | Security & Privacy | UI/Application/Infrastructure/Logging/Test | V1.0 Release-Gate | Zukünftige optionale Online- oder KI-Funktionen DÜRFEN nicht vorausgesetzt werden, um lokale V1-Daten lesen oder verwalten zu können. |
| `REQ-DATA-001` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Zentrale fachliche Datensätze MÜSSEN stabile interne Identitäten besitzen. |
| `REQ-DATA-002` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Erstellungs- und Änderungszeitpunkte wesentlicher Datensätze MÜSSEN nachvollziehbar sein. |
| `REQ-DATA-003` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Beziehungen zwischen Unternehmen, Kontakten, Opportunities, Bewerbungen, Aktivitäten und Dokumenten MÜSSEN referenziell konsistent bleiben. |
| `REQ-DATA-004` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Eine Statusänderung DARF die historische Timeline nicht löschen oder ersetzen. |
| `REQ-DATA-005` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Änderungen an Stammdaten DÜRFEN historische, bereits verwendete Dokumentversionen nicht unkenntlich machen. |
| `REQ-DATA-006` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Originalsnapshots und eigene Notizen MÜSSEN fachlich getrennt sein. |
| `REQ-DATA-007` | Should | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 geplant; Verschiebung nur dokumentiert | Herkunft/Provenienz wichtiger Aussagen SOLL gespeichert werden können. |
| `REQ-DATA-008` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Konflikthafte Importdaten DÜRFEN manuell gepflegte Daten nicht still überschreiben. |
| `REQ-DATA-009` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Textdaten MÜSSEN Unicode-Zeichen zuverlässig speichern und exportieren können. |
| `REQ-DATA-010` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Datum und Uhrzeit MÜSSEN eindeutig genug gespeichert werden, dass zeitliche Reihenfolgen rekonstruierbar bleiben. |
| `REQ-DATA-011` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Exportformate MÜSSEN dokumentiert sein. |
| `REQ-DATA-012` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Ein Komplettbackup MUSS alle zur Wiederherstellung notwendigen Informationen enthalten oder deren externe Abhängigkeiten eindeutig dokumentieren. |
| `REQ-DATA-013` | Should | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 geplant; Verschiebung nur dokumentiert | Dokumentdateien SOLLEN eine Integritäts-/Identitätsinformation erhalten, um identische Dateien und Veränderungen zu erkennen. |
| `REQ-DATA-014` | Must | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 Release-Gate | Archivierung DARF keine versteckte Löschung fachlicher Historie bedeuten. |
| `REQ-DATA-015` | Should | Datenmodell, Integrität und Migration | Domain/Persistence/Infrastructure/Test | V1.0 geplant; Verschiebung nur dokumentiert | Eine spätere Erweiterung des Datenmodells innerhalb V1.x SOLL migrationsfähig sein. |
| `REQ-OPS-001` | Must | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 Release-Gate | Die Anwendung MUSS auf der vorgesehenen Zielplattform reproduzierbar installierbar sein. |
| `REQ-OPS-002` | Must | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 Release-Gate | Ein Update innerhalb der V1-Linie DARF vorhandene Nutzerdaten nicht löschen. |
| `REQ-OPS-003` | Must | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 Release-Gate | Deinstallation und Nutzerdatenlöschung MÜSSEN konzeptionell getrennt sein. |
| `REQ-OPS-004` | Must | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 Release-Gate | Die Anwendung MUSS eine verständliche Versionsinformation anzeigen. |
| `REQ-OPS-005` | Must | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 Release-Gate | Bei Fehlern MUSS eine Diagnosemöglichkeit existieren, die ohne Offenlegung unnötiger personenbezogener Inhalte genutzt werden kann. |
| `REQ-OPS-006` | Should | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 geplant; Verschiebung nur dokumentiert | Vor riskanten Datenmigrationen SOLL eine Sicherung empfohlen oder automatisiert angeboten werden. |
| `REQ-OPS-007` | Must | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 Release-Gate | Die Anwendung MUSS eine Benutzerhilfe für Backup, Restore, Export und Datenlöschung bereitstellen. |
| `REQ-OPS-008` | Should | Betrieb, Diagnose, Installation und Wartung | WinForms/Infrastructure/Packaging/Docs | V1.0 geplant; Verschiebung nur dokumentiert | Release Notes SOLLEN für V1.x Änderungen an Daten, Funktionen und bekannten Einschränkungen verständlich dokumentieren. |
| `REQ-CON-001` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Version 1 ist primär ein persönlicher Einzelbenutzer-Bewerbungsmanager, kein Unternehmens-ATS. |
| `REQ-CON-002` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Der Kernbetrieb ist local-first und offline-fähig. |
| `REQ-CON-003` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Keine Bewerbung wird automatisch abgesendet. |
| `REQ-CON-004` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Keine externe E-Mail-/Kalender-/Cloud-Integration ist Voraussetzung für Version 1. |
| `REQ-CON-005` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Personen-, Unternehmens-, Opportunity-, Stellenanzeigen- und Bewerbungsobjekte bleiben fachlich unterscheidbar. |
| `REQ-CON-006` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Status, Aktivität, Aufgabe, Next Action und Commitment bleiben fachlich unterscheidbar. |
| `REQ-CON-007` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Backup und Restore sind Release-Gate-Funktionen und dürfen nicht auf eine spätere Version verschoben werden. |
| `REQ-CON-008` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Datenschutz- und Datenintegritätsanforderungen dürfen nicht allein wegen fehlender Sichtbarkeit im UI niedrig priorisiert werden. |
| `REQ-CON-009` | Should | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 geplant; Verschiebung nur dokumentiert | Für die Umsetzung sollen die anwendbaren SASD-Profile `Core`, `C#/.NET` und `Desktop` geprüft werden. Die konkrete UI-Technologie bleibt Entscheidung des Pflichtenhefts/der Architektur. |
| `REQ-CON-010` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Die primäre Zielsprache von Version 1 ist Deutsch. |
| `REQ-CON-011` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Eine Windows-Desktop-Nutzung ist als primäre Betriebsannahme für Version 1 zu unterstützen. Die konkrete technische Umsetzung wird separat spezifiziert. |
| `REQ-CON-012` | Must | Architektur- und Produktconstraint | Gesamtarchitektur/Governance | V1.0 Release-Gate | Code oder Funktionen aus Referenzprodukten dürfen nicht ungeprüft übernommen werden; Lizenz- und Herkunftsfragen sind vor Wiederverwendung zu klären. |

---

# Anhang B – Technische Abnahmematrix der Lastenheft-End-to-End-Fälle

| Abnahmefall | Technische Hauptkomponenten | Automatisierbarer Anteil | Manueller Anteil |
|---|---|---|---|
| AT-001 Neue Stelle vollständig erfassen | Opportunity, Company, JobPosting, Tags, Persistence | Application-/Integrationtest | UI-Smoke/Usability |
| AT-002 Bewerbung mit Dokumentversionen | Application, DocumentStore, DocumentVersion, ApplicationDocuments | Integration/Systemtest | Öffnen/visuelle Prüfung |
| AT-003 Recruiter-Telefonat und Zusage | Contact, Activity, Communication, Commitment | Application/Systemtest | UI-Flow |
| AT-004 Überfällige Zusage | Commitment Query, Dashboard | Unit/Applicationtest mit kontrollierter Zeit | Dashboard-Smoke |
| AT-005 Bewerbung ohne Next Action | Application, NextAction, Dashboard | Applicationtest | Dashboard-Smoke |
| AT-006 Drei Interviewrunden | Interview, Participants, Timeline | Integration/Systemtest | UI-Prüfung |
| AT-007 Widersprüchliche Remote-Aussagen | SourcedStatement | Domain/Applicationtest | Detailansicht |
| AT-008 Mehrere Rollen bei Unternehmen | Company, Opportunity, Application | Integrationtest | Unternehmensakte |
| AT-009 Frühere Recruiter-Beziehung | Contact, Search, Timeline | Such-/Integrationstest | UI-Suche |
| AT-010 Abschluss und Archiv | Outcome, Archive, Queries | Domain/Integrationtest | UI-Archivierung |
| AT-011 Backup und Restore | BackupService, Integrity, Staging Restore | vollständiger Systemtest | Wizard-/Dateiauswahl-Smoke |
| AT-012 Offline-Betrieb | gesamte Kernanwendung | Systemtest in geblocktem Netzwerk | manueller Offline-Smoke |
| AT-013 Datenexport | ExportService, CSV/JSON | Integrationtest mit Roundtrip/Schema | Öffnen in Fremdtool |
| AT-014 Datenintegrität nach Update | EF Migrations, Reference Dataset | CI-Migrationstest | Release-Smoke |
| AT-015 Keine unerwartete externe Datenübertragung | Netzwerkfreiheit, Logging, LinkService | Netzwerk-/Securitytest | Browser-Link-Verhalten |

---

# Anhang C – UI-Ansichten und zugehörige Presenter

| UI-Bereich | View/UserControl | Presenter | Zentrale Application Use Cases |
|---|---|---|---|
| Shell | `MainForm` / `IMainView` | `MainPresenter` | Navigation, Startup status |
| Heute | `DashboardControl` | `DashboardPresenter` | `GetDashboardQuery` |
| Bewerbungen Liste | `ApplicationsListControl` | `ApplicationsListPresenter` | Search/filter/open/archive |
| Bewerbung Detail | `ApplicationDetailControl` | `ApplicationDetailPresenter` | Update, status, next action, outcome |
| Board | `PipelineBoardControl` | `PipelineBoardPresenter` | Load board, change status |
| Opportunities | `OpportunitiesControl` | `OpportunitiesPresenter` | CRUD, snapshots, duplicate check |
| Unternehmen | `CompaniesControl` | `CompaniesPresenter` | CRUD, merge, history |
| Kontakte | `ContactsControl` | `ContactsPresenter` | CRUD, merge, follow-up |
| Timeline | `TimelineControl` | `TimelinePresenter` | activities, communication |
| Tasks | `TasksControl` | `TasksPresenter` | CRUD, complete, checklist |
| Commitments | eingebettet + Liste | `CommitmentsPresenter` | create, postpone, fulfill, follow-up |
| Interviews | `InterviewsControl` | `InterviewsPresenter` | schedule, participants, notes |
| Documents | `DocumentsControl` | `DocumentsPresenter` | import, version, open, assign |
| Search | `GlobalSearchControl/Dialog` | `GlobalSearchPresenter` | global search |
| Calendar | `CalendarControl` | `CalendarPresenter` | local agenda/month query |
| Analytics | `AnalyticsControl` | `AnalyticsPresenter` | rates, distributions, source comparison |
| Settings | `SettingsControl` | `SettingsPresenter` | preferences/lookups/pipeline |
| Backup/Restore | Wizards/Dialogs | jeweilige Presenter | backup, validate, restore |
| Import/Export | Wizards/Dialogs | jeweilige Presenter | preview, import, export |

---

# Anhang D – Vorgeschlagene Kern-Commands und Queries

## Commands

- `CreateCompanyCommand`
- `UpdateCompanyCommand`
- `MergeCompaniesCommand`
- `CreateContactCommand`
- `UpdateContactCommand`
- `MergeContactsCommand`
- `CreateOpportunityCommand`
- `AddJobPostingSnapshotCommand`
- `CreateApplicationCommand`
- `ChangeApplicationStatusCommand`
- `SetApplicationOutcomeCommand`
- `ArchiveApplicationCommand`
- `ReactivateApplicationCommand`
- `CreateActivityCommand`
- `CreateCommunicationCommand`
- `SetNextActionCommand`
- `CompleteNextActionCommand`
- `SnoozeNextActionCommand`
- `CreateCommitmentCommand`
- `ChangeCommitmentStatusCommand`
- `PostponeCommitmentCommand`
- `CreateTaskCommand`
- `CompleteTaskCommand`
- `CreateInterviewCommand`
- `UpdateInterviewCommand`
- `ImportDocumentVersionCommand`
- `AssignDocumentVersionToApplicationCommand`
- `CreateSourcedStatementCommand`
- `ImportCsvCommand`
- `CreateBackupCommand`
- `RestoreBackupCommand`
- `DeleteEntityCommand` mit typisiertem Delete-Plan statt generischem Blind-Delete

## Queries

- `GetDashboardQuery`
- `SearchApplicationsQuery`
- `GetApplicationDetailQuery`
- `GetPipelineBoardQuery`
- `SearchOpportunitiesQuery`
- `GetOpportunityDetailQuery`
- `SearchCompaniesQuery`
- `GetCompanyDetailQuery`
- `SearchContactsQuery`
- `GetContactDetailQuery`
- `GetTimelineQuery`
- `SearchTasksQuery`
- `GetUpcomingInterviewsQuery`
- `GlobalSearchQuery`
- `GetCalendarAgendaQuery`
- `GetAnalyticsOverviewQuery`
- `GetSourceAnalyticsQuery`
- `GetDeleteImpactQuery`
- `ValidateBackupQuery`

---

# Anhang E – Freigabe

Dieses Pflichtenheft ist zur technischen Freigabe vorgesehen. Vor Beginn einer breiten Implementierung sollen insbesondere folgende Punkte bestätigt werden:

1. WinForms + .NET 10 LTS als verbindliche Plattformbasis;
2. modularer Monolith mit Domain/Application/Infrastructure/WinForms;
3. MVP/Presenter für komplexe WinForms-Bereiche;
4. SQLite + EF Core 10;
5. Managed Document Store mit unveränderlichen Versionen und SHA-256;
6. Single-Instance pro Benutzerprofil;
7. self-contained `win-x64` Release;
8. Windows 11 x64 als offizielle V1-Supportplattform;
9. Backup/Restore als Staging-/Integritäts-geprüfter Release-Gate;
10. keine externen Konten, Telemetrie oder Onlineintegrationen im V1-Kern.

Nach Freigabe bildet dieses Dokument gemeinsam mit dem Lastenheft die verbindliche Grundlage für Architekturentscheidungen, Implementierungsplanung und V1-Abnahme.
