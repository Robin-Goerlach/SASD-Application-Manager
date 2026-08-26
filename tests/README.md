# Test Layout

- `Domain.Tests` – Invarianten und Domain-Architekturgrenzen.
- `Application.Tests` – Use Cases, Validierung und Application-Architekturgrenzen.
- `Infrastructure.Tests` – echte SQLite-Dateien, Migrationen, File Store und Recovery.
- `Presentation.Tests` – Presenter-/View-Verträge ohne produktive Datenbank.
- `SystemTests` – ausgewählte End-to-End-/Betriebsprüfungen auf Windows.

Providerabhängiges SQLite-Verhalten wird nicht ausschließlich mit EF InMemory getestet.
