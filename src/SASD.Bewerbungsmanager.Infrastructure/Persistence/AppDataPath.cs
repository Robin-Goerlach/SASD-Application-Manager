namespace SASD.Bewerbungsmanager.Infrastructure.Persistence;

/// <summary>Resolves writable per-user paths without placing personal data inside the repository.</summary>
public static class AppDataPath
{
    /// <summary>Returns the default local SQLite database path and ensures its parent directory exists.</summary>
    public static string GetDefaultDatabasePath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (string.IsNullOrWhiteSpace(localAppData))
        {
            throw new InvalidOperationException("Das lokale Anwendungsdatenverzeichnis konnte nicht ermittelt werden.");
        }

        var directory = Path.Combine(localAppData, "SASD GmbH", "SASD Bewerbungsmanager");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "application-tracker.db");
    }
}
