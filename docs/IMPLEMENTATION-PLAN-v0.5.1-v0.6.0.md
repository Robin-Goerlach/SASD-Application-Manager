# Umsetzungsplan v0.5.1 bis v0.6.0

> **Status:** Arbeits- und Übergabeplan
>
> **Stand:** 2026-09-13
>
> **Ausgangsbasis:** GitHub `main`, Commit `6ef5e67`
>
> **Aktueller Code-Stand:** v0.5.0 – Optionale Assistenz

## 1. Zweck

Dieser Plan zerlegt die Konsolidierung des SASD Bewerbungsmanagers und die Vorbereitung
Login-pflichtiger Jobportale in kleine, einzeln prüfbare Arbeitspakete. Er ist kein Nachweis dafür,
dass ein Arbeitspaket bereits umgesetzt oder getestet wurde.

Der Code im aktuellen Repository bleibt Source of Truth. Bei jedem Paket werden Dokumentation,
Implementierung, Migrationen und Tests erneut gegeneinander geprüft.

## 2. Verbindliche Grenzen

Der SASD Bewerbungsmanager:

- speichert keine Benutzernamen oder Passwörter für Jobportale;
- speichert keine Cookies, Session- oder Refresh-Tokens;
- speichert keine MFA-Secrets oder Passkeys;
- automatisiert keine Anmeldung und keine Bedienung eines Benutzerkontos;
- umgeht weder CAPTCHA noch andere Bot-Schutzmechanismen;
- führt kein automatisiertes Portal-Scraping und kein Auto-Apply aus.

Login, MFA, Passkeys, Cookies und Sessions verbleiben im normalen Standardbrowser. SASD öffnet nur
eine vom Benutzer gespeicherte Such- oder Anmeldeseite. Treffer gelangen anschließend über einen
bewussten Handoff – zunächst Zwischenablage, Datei oder E-Mail-Handoff – in die vorhandene
`JobLead`-Inbox.

## 3. Versionsgrenzen

### v0.5.1 – Stabilisierung

Nur technische Stabilisierung und Konsolidierung:

- Build-, Test- und CI-Baseline reparieren;
- tatsächliche Testausführung je Testprojekt absichern;
- nachweislich unbenutzte M0-Altlasten entfernen;
- Dokumentation an den implementierten Stand angleichen;
- kleinere Fehler mit unmittelbarer Auswirkung auf Zuverlässigkeit oder Datenkonsistenz beheben.

Keine neuen Portal-, Browser- oder KI-Integrationen und möglichst keine Schemaänderung.

### v0.6.0 – Authenticated Search Workflow

Ein kleiner, benutzergesteuerter Arbeitsablauf:

```text
fälliges SearchProfile
        -> Suche im Standardbrowser öffnen
        -> Benutzer authentifiziert sich bei Bedarf selbst
        -> Treffer per Clipboard/Datei/E-Mail-Handoff erfassen
        -> normalisierter JobLead
        -> menschliche Prüfung
        -> Opportunity
```

Die bestehende Grenze `externe Quelle -> JobLead -> Opportunity` bleibt erhalten.

## 4. Arbeitspakete und Checkpoints

| Paket | Version | Ergebnis | Status |
|---|---|---|---|
| A | v0.5.1 | reproduzierbare Ausgangsbasis und vollständiger Befund | abgeschlossen |
| B | v0.5.1 | CI und Test-Discovery zuverlässig | in Verifikation |
| C | v0.5.1 | Altlasten bereinigt, Dokumentation konsistent | in Bearbeitung |
| D | v0.5.1 | vollständige Verifikation und auslieferbares Stabilisierungspaket | offen |
| E | v0.6.0 | minimales SearchProfile-Datenmodell und Migration | offen |
| F | v0.6.0 | atomare JobLead-Promotion und erweiterbare URL-Kanonisierung | offen |
| G | v0.6.0 | Browser-/Clipboard-Workflow in der WinForms-Oberfläche | offen |
| H | v0.6.0 | Upgrade-, Workflow- und Releaseprüfung sowie Dokumentation | offen |

### Paket A – Ist-Zustand festhalten

Bereits festgestellte Baseline:

- `main` endet bei `6ef5e67` und enthält funktional v0.5.0;
- produktiver Kontext ist `ApplicationTrackerDbContext`;
- produktive Migrationskette reicht bis `202608270005_AssistantWorkspace`;
- 57 Testmethoden mit `[Fact]` oder `[Theory]` sind im Quelltext vorhanden;
- der letzte GitHub-Actions-Lauf baute Release mit 0 Warnungen und 0 Fehlern;
- der CI-Testschritt führte wegen `dotnet test --solution ...` und `MSB1001` keine Tests aus;
- die aktuelle Ausführungsumgebung besitzt kein .NET SDK und kann den Windows-Build nicht ersetzen;
- Backup/Restore, `.sasdbak`, Release-Gate und Test-Discovery-Gate sind in dieser Baseline nicht
  implementiert;
- historische M0-Kontexte, Migrationen, DI- und MainForm-Reste sind noch vorhanden.

### Paket B – CI und Test-Discovery

Scope:

1. ungültigen CI-Testaufruf korrigieren;
2. jedes der fünf Testprojekte separat ausführen oder Ergebnisdateien zuverlässig auswerten;
3. einen Fehler erzeugen, wenn ein Testprojekt keine Tests entdeckt;
4. lokales Verify-Skript und CI denselben Prüfpfad verwenden lassen;
5. Infrastructure-Test-Discovery unter Windows gezielt prüfen;
6. keine Analyzer-, Security- oder Defender-Prüfung pauschal deaktivieren.

Abnahme:

- Restore und Release-Build erfolgreich;
- alle fünf Testprojekte haben eine nachgewiesene Testzahl größer null;
- ein absichtlich leerer Testlauf würde das Gate fehlschlagen lassen;
- CI auf `main` ist grün.

### Paket C – Altlasten und Dokumentation

Scope:

1. alte M0-Dateien nur nach Referenzprüfung entfernen;
2. `README.md`, `PROJECT-STATUS.md`, `ROADMAP.md`, `CHANGELOG.md`, `AGENTS.md`,
   `KNOWN-ISSUES.md` und den Dokumentationsindex konsolidieren;
3. vollständige Migrationen und reale Module dokumentieren;
4. implementierte Funktionen nicht als Zukunft darstellen;
5. geplante Backup-/Restore- und Releasefunktionen nicht als fertig ausweisen;
6. Portal-Credential- und Browsergrenze in Security-/Architekturdokumentation festhalten.

Abnahme:

- eine repositoryweite Suche findet keine widersprüchlichen aktuellen Versionsangaben;
- historische Dokumente bleiben erkennbar historisch;
- kein M0-Produktionspfad kann versehentlich aufgelöst oder migriert werden;
- lokale Markdown-Links sowie XML-, JSON- und YAML-Dateien sind gültig.

### Paket D – v0.5.1 abschließen

Scope und Gate:

```powershell
dotnet clean .\SASD.Bewerbungsmanager.sln
dotnet restore .\SASD.Bewerbungsmanager.sln
dotnet build .\SASD.Bewerbungsmanager.sln -c Release --no-restore
powershell -ExecutionPolicy Bypass -File .\scripts\Verify-Tests.ps1
dotnet run --project .\src\SASD.Bewerbungsmanager.WinForms\SASD.Bewerbungsmanager.WinForms.csproj
```

Zusätzlich: Migration einer leeren temporären SQLite-Datenbank, Start-Smokecheck, Publish-Smokecheck,
`git diff --check`, Datenschutz-/Secret-Sichtung und ZIP-Integritätsprüfung.

Ergebnis: Delta-ZIP und Complete-ZIP für v0.5.1 samt SHA-256. Commit oder Push erfolgen nur nach
ausdrücklicher Freigabe.

### Paket E – SearchProfile-Modell

Vor der Änderung wird das vorhandene Modell erneut geprüft. Die minimale Zielrichtung ist:

- bisherige Daten und IDs erhalten;
- Zugriff (`AccessMode`) und Trefferübergabe (`CaptureMode`) getrennt abbilden;
- `PublicBrowser`, `AuthenticatedBrowser`, `EmailAlert`, `OfficialApi` und `ExternalAdapter` nur als
  fachliche Zugriffsarten verwenden;
- `Clipboard`, `FileHandoff`, `EmailHandoff`, `BrowserHelper` und `Api` nur als Capture-Arten
  verwenden;
- vorhandene Namen wie `Source` und `Url` nur mit belastbarem Migrationsgrund aufteilen oder
  umbenennen;
- `LoginUrl` optional halten; sie enthält niemals Anmeldedaten;
- eine additive EF-Core-Migration mit Upgrade-Test erstellen.

Abnahme umfasst öffentliche und authentifizierte Browserprofile, optionale Login-URL,
URL-Validierung, Fälligkeitsberechnung, „Heute geprüft“, inaktive Profile und Upgrade bestehender
Profile.

### Paket F – Promotion und URL-Kanonisierung

Scope:

1. Opportunity, optionalen SourceLink und JobLead-Status in einer lokalen SQLite-Transaktion
   persistieren;
2. erfolgreichen Roundtrip und erzwungenen Fehlerfall testen;
3. generische URL-Kanonisierung aus dem Service in eine klar benannte, testbare Komponente ziehen,
   sofern dies ohne leere Portaladapter gelingt;
4. Fragmente sowie bekannte Tracking-/Sessionparameter konservativ entfernen;
5. keine portalspezifischen Canonicalizer ohne reale Regeln einführen.

Abnahme: Erfolg erzeugt alle drei fachlichen Bestandteile; Fehler hinterlässt keinen partiellen
Zustand.

### Paket G – Browser- und Capture-Workflow

Scope:

1. `SearchProfile` zeigt Zugriffstyp und nächste Prüfung verständlich an;
2. „Suche öffnen“ verwendet ausschließlich den Standardbrowser;
3. „Treffer erfassen“ öffnet den vorhandenen Clipboard-Import mit `SearchProfileId` und Quelle
   vorbelegt;
4. „Heute geprüft“ aktualisiert die vorhandene Terminlogik;
5. verständliche Empty-/Fehlerzustände und deaktivierte Aktionen ohne Auswahl;
6. keine eingebettete Browserengine und keine Sessionübernahme.

### Paket H – v0.6.0 abschließen

Neben dem vollständigen Build-/Test-Gate:

- frische Datenbank bis zur neuesten Migration;
- Upgrade von der v0.5.1-Migration;
- SearchProfile-Browser- und Clipboard-Ablauf manuell unter Windows;
- kein Credential-/Cookie-/Token-Feld in Domain, Persistenz, Settings oder Logs;
- aktualisierte README, Status, Roadmap, Changelog, Upgrade-, Security- und Milestone-Dokumente;
- Delta-ZIP und Complete-ZIP samt SHA-256.

## 5. Arbeitsregel zwischen den Paketen

Nach jedem Paket:

1. Status und Befunde in diesem Dokument aktualisieren;
2. `git status --short`, `git diff --check` und `git diff --stat` prüfen;
3. keine fremden SASD-Projekte, Buildartefakte, Datenbanken, Backups oder personenbezogenen Daten
   übernehmen;
4. ausführbare Prüfungen mit exaktem Ergebnis dokumentieren;
5. nicht ausführbare Prüfungen ausdrücklich als ausstehend kennzeichnen;
6. erst dann das nächste Paket beginnen.

So kann ein neuer Chat oder eine unterbrochene Sitzung anhand des Repository-Diffs und dieser Datei
ohne Neubeginn fortsetzen.

## 6. Nicht Bestandteil dieser Folge

- Browsererweiterungen;
- Selenium, Playwright oder Puppeteer;
- automatische Portal-Logins oder Sessionübernahme;
- Credential Vault für Jobportale;
- Scraper für LinkedIn, StepStone oder andere Portale;
- CAPTCHA-/Anti-Bot-Umgehung;
- Cloud-Synchronisation;
- Auto-Apply;
- direkte KI-Providerintegration.

Ein späterer Browser Helper benötigt eine eigene Strategieentscheidung und einen nachgewiesenen
Bedarf. Er ist keine Voraussetzung für v0.6.0.
