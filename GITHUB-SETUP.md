# GitHub-Setup – SASD Bewerbungsmanager

## Empfohlener Repository-Name

`SASD-Bewerbungsmanager`

## Repository Description

Siehe `REPOSITORY-DESCRIPTION.txt`.

## Empfohlene Topics

`csharp`, `dotnet`, `dotnet10`, `winforms`, `sqlite`, `desktop`, `job-search`, `application-tracker`, `personal-crm`, `local-first`, `sasd`

## Initialer Import

```powershell
git init
git add .
git commit -m "chore: initialize SASD Bewerbungsmanager repository"
git branch -M main
git remote add origin git@github.com:Robin-Goerlach/SASD-Bewerbungsmanager.git
git push -u origin main
```

## Empfohlene GitHub-Einstellungen

1. Default branch: `main`.
2. Issues aktivieren.
3. Actions aktivieren.
4. Dependabot alerts/updates aktivieren.
5. Private vulnerability reporting aktivieren, falls das Repository öffentlich wird.
6. Branch Protection / Ruleset für `main` nach dem ersten grünen CI-Lauf:
   - Pull Request oder bewusst dokumentierter Maintainer-Override;
   - Statuscheck `build-test` erforderlich;
   - Force Push und Branch Delete sperren;
   - Conversations vor Merge auflösen.
7. Keine GitHub-Secrets anlegen, solange M0 keine externen Dienste benötigt.

## Wichtiger Lizenzhinweis

Die Produktlizenz ist noch **nicht entschieden**. Ein öffentlich sichtbares Repository ohne Lizenz gewährt Dritten nicht automatisch Open-Source-Nutzungsrechte. Vor RC1 ist die Entscheidung gemäß `docs/00-project/LICENSE-DECISION.md` zu schließen.
