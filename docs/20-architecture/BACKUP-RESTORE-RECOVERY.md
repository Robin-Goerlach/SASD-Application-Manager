# Backup-, Restore- und Recovery-Konzept

> **Status:** Release-relevante Baseline  
> **Dokumentversion:** 1.0  
> **Stand:** 2026-08-24  
> **Geltungsbereich:** Produktversion 1.0  
> **Projekt:** SASD Bewerbungsmanager


## 1. Ziel

Ein Backup gilt nur dann als erfolgreich, wenn es **konsistent erzeugt, prüfbar und praktisch wiederherstellbar** ist. Export und Backup sind getrennte Funktionen.

## 2. Sicherungsumfang

Ein vollständiges V1-Backup enthält mindestens:

- konsistente SQLite-Datenbankkopie;
- verwalteten Dokumentstore;
- notwendige fachliche Konfiguration/Lookups;
- Backupmanifest mit Produkt-, Schema- und Formatversion;
- Hash-/Größeninformationen für enthaltene Komponenten.

Cache, Suchindex und temporäre Dateien müssen nicht gesichert werden, wenn sie rebuildable sind.

## 3. Konsistentes SQLite-Backup

Die laufende DB wird nicht blind per Dateikopie gesichert. Infrastructure verwendet eine SQLite-konforme Online-Backupstrategie bzw. einen nachweisbar konsistenten Snapshotweg.

## 4. Backupablauf

1. Zielpfad validieren und freien Speicher prüfen.
2. Operation-ID erzeugen.
3. konsistente DB-Kopie in Staging erzeugen.
4. Document-Store-Objekte erfassen/kopieren.
5. Manifest erzeugen.
6. Hashes prüfen.
7. optional nach ADR verschlüsseln/verpacken.
8. temporäres Paket atomar/final umbenennen.
9. Erfolg erst nach Abschluss aller Prüfungen melden.

Ein abgebrochener Vorgang darf kein scheinbar gültiges vollständiges Backup zurücklassen.

## 5. Backupformat

Format ist versioniert. Manifest enthält mindestens:

```json
{
  "formatVersion": 1,
  "applicationVersion": "1.0.0",
  "schemaVersion": "...",
  "createdAtUtc": "...",
  "database": {"sha256": "...", "size": 0},
  "documents": {"count": 0, "manifestSha256": "..."}
}
```

Das endgültige Container-/Verschlüsselungsformat wird per ADR geschlossen.

## 6. Restore-Prinzip

**Nie in-place auf den aktiven Bestand zurückschreiben.**

Restore ist eine Staging-Operation:

1. Paket lesen und Formatversion prüfen;
2. Authentizität/Hashes/Entschlüsselung prüfen;
3. in neues Staging-Verzeichnis extrahieren;
4. SQLite-Integritätscheck;
5. Schema-/Produktkompatibilität prüfen;
6. Dokumentmanifest prüfen;
7. optional Suchindex neu erzeugen;
8. erst danach kontrollierter Bestandswechsel;
9. alten Bestand bis zum erfolgreichen Smoke-Start als Fallback halten.

## 7. Generation Switch

Die konkrete Strategie (Generation-based Data Root oder robuste Rename/Replace-Kette) wird vor M5 per ADR festgelegt. Grundsatz: Nach einem Crash während der Umschaltung muss eindeutig erkennbar sein, welche Generation vollständig gültig ist.

## 8. Recovery ohne vollständigen Restore

### beschädigte `settings.json`

- Datei sichern/umbenennen;
- Defaults laden;
- Nutzer über zurückgesetzte Einstellungen informieren;
- keine DB löschen.

### beschädigter Suchindex

- Index verwerfen;
- aus Source of Truth neu aufbauen;
- Fachbetrieb ggf. eingeschränkt fortsetzen.

### fehlendes Dokumentobjekt

- DB-Referenz nicht still löschen;
- Integritätsfehler anzeigen;
- ggf. Restore/erneuten Import anbieten;
- Timeline/Historie erhalten.

### DB-Integritätsfehler

- Normalbetrieb stoppen;
- Recovery Mode anbieten;
- Diagnose/Backup des Istbestands ermöglichen;
- Restore aus verifiziertem Backup durchführen.

### fehlgeschlagene Migration

- keine wiederholten automatischen Destruktionsversuche;
- Pre-Migration-Backup/alte Generation erhalten;
- Anwendung mit verständlichem Recovery-Hinweis beenden oder Recovery Mode starten.

## 9. RPO/RTO als Produktziel

- **RPO:** durch Alter des letzten erfolgreichen Backups bestimmt; Anwendung soll Backupstatus sichtbar machen.
- **RTO-Ziel für Referenzbestand:** vollständiger Restore eines üblichen lokalen Bestands soll auf Referenzhardware in einer praktikablen Benutzersitzung möglich sein; für RC1 wird ein Zielwert von **≤ 15 Minuten** für den definierten Referenzbestand geprüft.

Das ist kein Hochverfügbarkeits-SLA, sondern ein überprüfbares Recoveryziel.

## 10. Restore-Testmatrix

- gültiges Backup → vollständiger Roundtrip;
- falscher Hash → Abbruch vor Umschaltung;
- beschädigte DB → Abbruch;
- fehlendes Dokumentobjekt → Abbruch oder klar definierter Fehler;
- zu wenig Speicher → sauberer Abbruch;
- falsches Passwort bei verschlüsseltem Backup → keine Teilwiederherstellung;
- alte kompatible Formatversion → Migrationstest;
- unbekannte neuere Formatversion → sichere Ablehnung;
- Crash vor Umschaltung → alter Bestand unverändert;
- Crash während Umschaltung → eindeutige Generation/recoverable.

## 11. Benutzerführung

Backup- und Restore-Dialoge zeigen klar:

- Quelle/Ziel;
- Zeitpunkt/Version;
- ob Verschlüsselung aktiv ist;
- ob bestehende Daten ersetzt werden;
- ob ein Sicherheitsbackup erzeugt wird;
- Abschluss mit prüfbarem Ergebnis statt nur „Datei geschrieben“.
