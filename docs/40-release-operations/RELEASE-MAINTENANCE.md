# Release- und Wartungsplan

> **Status:** Betriebsbaseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Versionsstrategie

SemVer-orientiert:

- `MAJOR`: inkompatible Produkt-/Daten-/Verhaltensänderung;
- `MINOR`: rückwärtskompatible Funktion;
- `PATCH`: rückwärtskompatible Korrektur.

Vor 1.0 dürfen interne Builds `0.x` oder Milestone-/RC-Kennungen verwenden.

## 2. Releaseablauf

1. Scope Freeze für Kandidat.
2. Version setzen.
3. Releasebuild aus sauberem Checkout.
4. vollständige CI-/Quality Gates.
5. N-1→N Migrationstest.
6. Backup/Restore-Test.
7. Installer Clean/Upgrade/Uninstall.
8. AT-001…AT-015.
9. Dependency-/Lizenz-/Securityreview.
10. Release Notes/Known Issues.
11. Hash/Signing.
12. Release Record/Freigabe.
13. Artefakt veröffentlichen.

## 3. Supportstatus

Für V1 wird zunächst die jeweils aktuelle stabile 1.x-Version aktiv gepflegt. Kritische Datenintegritäts-/Securityfehler älterer 1.x-Stände werden nach Risiko bewertet; ein pauschaler Langzeitsupport mehrerer Linien ist nicht zugesichert.

## 4. Dependency-Wartung

| Bereich | Frequenz | Vorgehen |
|---|---|---|
| .NET SDK/Runtime | regelmäßig und bei Security Advisory | unterstützte LTS-Patches testen |
| NuGet direkte Pakete | monatlich bzw. Advisory-getrieben | Release Notes + Tests |
| transitive Schwachstellen | CI/Release | `dotnet list package --vulnerable --include-transitive` oder äquivalent |
| SQLite/EF Core | konservativ | Migration/Regression besonders prüfen |

Major-Upgrades werden nicht allein wegen Versionsneuheit vorgenommen.

## 5. Datenformatpflege

- jede Schemaänderung → Migration;
- Exportformat versionieren;
- Backupformat versionieren;
- Settingsformat versionieren, sobald inkompatible Änderungen möglich sind;
- ältere Daten niemals still verwerfen.

## 6. Diagnose und Support

Ein Supportfall soll mit folgendem Minimum bearbeitbar sein:

- Appversion;
- Schemaversion;
- Windowsversion;
- Operation-/Fehlerreferenz;
- bereinigtes Log/Diagnosepaket;
- Reproduktionsschritte.

Keine Aufforderung, vollständige private Bewerbungsordner ungefiltert zu versenden.

## 7. Backup-/Restore-Wartung

Restore ist ein dauerhaft getestetes Feature. Bei jeder Änderung an Schema, Document Store, Backupformat oder Datenroot werden passende Roundtriptests aktualisiert.

## 8. Incidents

Priorität:

1. Datenverlust/Datenkorruption;
2. Security/Privacy;
3. Start-/Migration-/Restoreblocker;
4. Kernworkflowblocker;
5. normale Funktions-/UX-Fehler.

Hotfixes umgehen keine Minimaltests für betroffene Datenpfade.

## 9. End of Life

Wenn das Projekt nicht mehr gepflegt werden kann, soll der letzte Stand mindestens:

- klaren Supportstatus nennen;
- offenen Export bereitstellen;
- Datenpfade und manuelle Sicherung dokumentieren;
- bekannte kritische Risiken nennen;
- keine falsche Updateerwartung erzeugen.

## 10. Reviewrhythmus

Review dieses Plans:

- vor 1.0 RC1;
- bei neuem Major/Runtimewechsel;
- bei Einführung externer Integrationen;
- bei Änderung des Supportmodells.
