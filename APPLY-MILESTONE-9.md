# SASD Finance Control – Milestone 9 anwenden

## Voraussetzung

Dieses Overlay ist für den erfolgreich gebauten und getesteten Milestone-8-Stand vorgesehen.
Referenzstand zum Zeitpunkt der Erstellung: Git-Commit `5a5c988` auf `main`.

Vor dem Entpacken sollte der lokale Arbeitsbaum sauber sein:

```powershell
git status
```

## Overlay anwenden

1. Repository sichern bzw. sicherstellen, dass Milestone 8 committed/gepusht ist.
2. `SASD-Finance-Control-Milestone-9-Overlay.zip` in das Root-Verzeichnis des Repositorys entpacken.
3. Vorhandene Dateien beim Entpacken überschreiben lassen.
4. Keine bestehenden Datenbankdateien manuell löschen. Migration 8 wird beim normalen Programmstart automatisch angewendet.

## Build und Tests

Vom Repository-Root aus:

```powershell
dotnet clean Sasd.FinanceControl.sln
dotnet restore Sasd.FinanceControl.sln
dotnet build Sasd.FinanceControl.sln -c Release --no-restore
dotnet test Sasd.FinanceControl.sln -c Release --no-build
```

Ziel: 0 Warnungen, 0 Fehler und alle Tests grün.

## Programm starten

```powershell
dotnet run --project .\src\Sasd.FinanceControl.App\Sasd.FinanceControl.App.csproj -c Release
```

## Manueller Smoke-Test für Milestone 9

1. Einen aktiven Lieferanten auswählen oder neu anlegen.
2. `Bestellungen` öffnen.
3. Eine neue Bestellung im Status `Entwurf` anlegen.
4. Zwei Positionen ergänzen, eine davon optional einer Finanzkategorie zuordnen.
5. Eine Position als Asset- oder Inventar-Kandidat markieren.
6. Bestellung speichern und Anwendung neu starten; Bestellung und Positionen müssen erhalten bleiben.
7. Bestellung auf `Bestellt` setzen und einen erwarteten Liefertermin pflegen.
8. Ein vorhandenes Archivdokument mit der Bestellung verknüpfen.
9. Eine Rechnung desselben Lieferanten und derselben Währung mit der Bestellung verknüpfen.
10. In der Bestellansicht prüfen, ob der aus Milestone 8 abgeleitete Zahlungsstatus der Rechnung angezeigt wird.
11. Den Rechnungslink mit Begründung stornieren und prüfen, dass der historische Link sichtbar bleibt.
12. Optional prüfen, dass eine Rechnung eines anderen Lieferanten bzw. einer anderen Währung nicht verknüpft werden kann.

## Fachliche Leitplanken

- Eine Bestellung ist keine Rechnung und kein Zahlungsnachweis.
- Tatsächliche Geldbewegungen bleiben ausschließlich durch unveränderliche Banktransaktionen belegt.
- Der Nachweisweg lautet: `Bestellung → Rechnung → Zahlungsallokation → Banktransaktion`.
- Bestell-/Rechnungsverknüpfungen werden nicht gelöscht, sondern nachvollziehbar storniert (Void).
- Asset- und Inventar-Kennzeichen sind in M9 nur Kandidatenmarker. Eine eigentliche Asset-/Lagerdomäne wird noch nicht vorgetäuscht.

## Falls etwas fehlschlägt

Bitte die vollständige Ausgabe von `dotnet build` und `dotnet test` zurückgeben. Bei einem Laufzeitfehler zusätzlich die aktuelle JSONL-Datei unter `%LOCALAPPDATA%\SASD\FinanceControl\logs\` bereitstellen.
