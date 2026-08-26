# Security-, Privacy- und Datenlebenszyklusplan

> **Status:** Security-Baseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Scope

V1 ist eine lokale Desktopanwendung ohne notwendigen Netzwerkdienst. Die Hauptschutzgüter sind Bewerbungs-, Kontakt-, Kommunikations- und Dokumentdaten sowie deren Integrität und Wiederherstellbarkeit.

## 2. Schutzbedarf

| Asset | Vertraulichkeit | Integrität | Verfügbarkeit |
|---|---:|---:|---:|
| Bewerbungsakte | hoch | hoch | mittel |
| Kontakte/Kommunikation | hoch | hoch | mittel |
| Dokumentversionen | hoch | hoch | mittel |
| SQLite-Datenbank | hoch | hoch | hoch |
| Backups/Exporte | hoch | hoch | mittel |
| Logs/Diagnose | mittel | mittel | niedrig |
| Suchindex/Cache | mittel | niedrig | niedrig |

## 3. Trust Boundaries

1. **Anwendungsprozess ↔ lokale Dateien** – Pfade und Dateien gelten nicht blind als vertrauenswürdig.
2. **Import ↔ externe Dateien/CSV/Archive** – vollständig untrusted input.
3. **Anwendung ↔ Windows Shell** – URLs/Dateien werden kontrolliert geöffnet, nicht interpretiert/ausgeführt.
4. **Backup ↔ externer Datenträger** – Backup kann das geschützte Benutzerprofil verlassen.
5. **spätere Integrationen ↔ Netzwerk** – außerhalb V1, benötigen neue Threat-Bewertung.

## 4. V1-Bedrohungen und Maßnahmen

| Risiko | Maßnahme |
|---|---|
| Pfadtraversal/unsichere Dateinamen | Pfade normalisieren, Storage Keys selbst erzeugen, keine Zielpfade aus Input übernehmen |
| manipulierte Dokumentdatei | SHA-256 und Integritätsprüfung |
| SQLite-Manipulation/Corruption | Constraints, Foreign Keys, Transactions, Integrity Check, Restorepfad |
| sensible Logs | Allowlist von Feldern, keine kompletten Bewerbungs-/Mailtexte, Redaction |
| Export/Backup-Leak | klare Warnung, definierte Verschlüsselungsoption, keine automatische Cloudübertragung |
| schädlicher CSV/HTML/Textinput | keine Scriptausführung, sichere Darstellung, Längenlimits, Encoding-Handling |
| Supply Chain | wenige Pakete, feste Quellen, Vulnerability-/Lizenzprüfung |
| destructive UI action | explizite Bestätigung, Preview/Referenzanalyse |

## 5. Netzwerk und Telemetrie

V1 startet, arbeitet und speichert ohne Internetzugang. Es gibt:

- keine versteckte Telemetrie;
- keine automatisch hochgeladenen Logs;
- keinen Cloud-Account-Zwang;
- keine automatische Übertragung von Bewerbungsdaten an KI-Dienste.

Spätere Netzwerkfunktionen müssen explizit aktiviert, dokumentiert und einzeln autorisiert werden.

## 6. Daten at Rest

Die Anwendung verlässt sich für den primären lokalen Bestand zunächst auf das Windows-Benutzerprofil und dessen Dateisystemberechtigungen. Sie behauptet keine eigene vollständige Datenbankverschlüsselung, solange diese nicht umgesetzt und getestet ist.

Backups sind ein anderes Risikoprofil, weil sie kopiert werden. Dafür ist vor M5 eine explizite Verschlüsselungsentscheidung vorgesehen.

## 7. Secrets

V1 benötigt im Kern keine API-Tokens oder Passwörter. Daher gibt es keinen Grund, Secrets im Repository, `settings.json` oder Logs zu speichern. Falls spätere Integrationen Secrets benötigen, ist Windows-geschützter Secret Storage/DPAPI oder ein gleichwertiger Ansatz neu zu bewerten.

## 8. Datenminimierung

- nur für Bewerbungsmanagement notwendige Daten erfassen;
- keine besonderen Kategorien personenbezogener Daten absichtlich modellieren;
- freie Notizen sollen nicht zu einem unkontrollierten Datenfriedhof werden;
- Diagnosepakete enthalten standardmäßig keine vollständigen Dokumentdateien;
- Testdaten sind synthetisch.

## 9. Datenlebenszyklus

### Erfassung

Daten entstehen manuell oder per kontrolliertem Import. Herkunft wird dort gespeichert, wo sie fachlich relevant ist.

### Nutzung

Nur lokale Anwendungskomponenten greifen entsprechend ihrer Aufgabe zu. UI erhält bevorzugt Read Models statt unbeschränktem direkten DB-Zugriff.

### Archivierung

Abgeschlossene Vorgänge können archiviert werden und bleiben auffindbar. Archivieren ist reversibel.

### Export

Export erzeugt bewusst transportierbare Daten. Benutzer wird darauf hingewiesen, dass Exportdateien nicht automatisch denselben Schutz wie der lokale Profilordner besitzen.

### Backup

Siehe `BACKUP-RESTORE-RECOVERY.md`.

### Löschung

Endgültiges Löschen:

1. Referenzen ermitteln;
2. Auswirkungen anzeigen;
3. explizit bestätigen;
4. transaktional strukturierte Daten löschen/anpassen;
5. unreferenzierte Binärobjekte erst danach bereinigen;
6. Fehler protokollieren, keine Halblöschung verschleiern.

## 10. Logging

Erlaubt: Operation-ID, Feature, Dauer, Fehlerklasse, technische Pfade soweit nötig und minimiert, Version, Schema-Version.  
Nicht erlaubt als Standard: komplette Anschreiben, CV-Inhalte, E-Mailtexte, Passwörter/Tokens, vollständige ungekürzte personenbezogene Freitexte.

## 11. Dependency-/Supply-Chain-Regeln

- NuGet nur aus dokumentierten Quellen;
- Paketbedarf begründen;
- keine Bibliothek nur für triviale Funktion einführen;
- transitive Vulnerabilities im Releasegate prüfen;
- Lizenzen und Third-Party Notices vor Public Release prüfen;
- reproduzierbarer Releasebuild.

## 12. Incident-/Recovery-Sicht

Bei vermuteter lokaler Kompromittierung:

1. Anwendung beenden;
2. betroffenen Datenbestand nicht überschreiben;
3. Diagnose/Backup nur auf vertrauenswürdiges Ziel;
4. Hashes/Logs sichern;
5. Hostsystem separat bewerten;
6. Restore nur auf vertrauenswürdiger Umgebung;
7. nach möglicher Datenexfiltration keine falsche „lokal = sicher“-Annahme treffen.

## 13. Security-Gate RC1

- keine offenen kritischen/hohen Dependency Findings ohne akzeptierte Ausnahme;
- Import-/Pfad-Negativtests grün;
- Diagnose ohne unerwünschte personenbezogene Inhalte geprüft;
- Backup-/Restore-Securitypfad geprüft;
- Netzaktivität im Offline-Test nicht erforderlich/unerwartet;
- Datenlöschung und Uninstall-Verhalten verifiziert.
