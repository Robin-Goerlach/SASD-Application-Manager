# Packaging Notes

**Erstellt:** 2026-08-24

Dieses Repository-ZIP wurde aus Lastenheft, Pflichtenheft, Architekturdokument und dem SASD-Dokumentationspaket abgeleitet.

## Durchgeführt

- Repositorystruktur und interne Pfade erzeugt;
- Screenshot nach `docs/images/dashboard-concept.png` eingebunden;
- README-Bildreferenz geprüft;
- XML/JSON/YAML/Markdown-Grundstruktur statisch geprüft;
- Solution-/Projekt-Referenzen und Dateiexistenz geprüft;
- ZIP-Integrität und SHA-256 nach Erstellung geprüft.

## Noch lokal/CI zu verifizieren

Die Packaging-Umgebung besitzt kein installiertes .NET SDK und erlaubt keinen direkten SDK-Download. Deshalb konnten `dotnet restore`, `dotnet build`, `dotnet test` und der reale Start der WinForms-Anwendung hier nicht ausgeführt werden.

Vorgesehene Verifikation auf Windows:

```powershell
.\scripts\verify.ps1
dotnet run --project src\SASD.Bewerbungsmanager.WinForms
```

Erst ein grüner Lauf ist der Build-/Testnachweis für M0.

## Test-Runner

Die Testprojekte verwenden xUnit v3 und den mit .NET 10 integrierten Microsoft Testing Platform (MTP)-Runner. Die Auswahl erfolgt repositoryweit über `global.json`; ein separates `Microsoft.NET.Test.Sdk` ist für diese Baseline nicht erforderlich.

Der Lösungstest wird mit `dotnet test --solution SASD.Bewerbungsmanager.sln` gestartet.
