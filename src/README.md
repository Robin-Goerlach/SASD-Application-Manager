# Source Layout

- `SASD.Bewerbungsmanager.Domain` – Fachmodell und Invarianten; keine EF-/WinForms-Abhängigkeiten.
- `SASD.Bewerbungsmanager.Application` – Use Cases, Commands/Queries und Ports.
- `SASD.Bewerbungsmanager.Infrastructure` – SQLite/EF Core, Dateisystem, Backup/Export/Diagnose-Adapter.
- `SASD.Bewerbungsmanager.WinForms` – Composition Root, Forms, Views und Presenter.

Neue Funktionen werden **vertikal** über diese Grenzen implementiert. Keine CRUD-Generator-Phase, die alle Entities vorab anlegt.
