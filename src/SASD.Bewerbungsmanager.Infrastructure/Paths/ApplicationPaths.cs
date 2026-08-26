using Microsoft.Data.Sqlite;

namespace SASD.Bewerbungsmanager.Infrastructure.Paths;

/// <summary>
/// Defines all writable local paths owned by the application.
/// </summary>
public sealed class ApplicationPaths
{
    /// <summary>
    /// Initializes paths below the current user's LocalApplicationData folder.
    /// </summary>
    public ApplicationPaths()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SASD",
            "Bewerbungsmanager"))
    {
    }

    /// <summary>
    /// Initializes the path model for a caller-provided root directory.
    /// This overload is intentionally public so integration tests can isolate their data.
    /// </summary>
    /// <param name="rootDirectory">Application data root.</param>
    public ApplicationPaths(string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);

        RootDirectory = Path.GetFullPath(rootDirectory);
        DataDirectory = Path.Combine(RootDirectory, "data");
        DocumentStoreDirectory = Path.Combine(RootDirectory, "documents");
        LogDirectory = Path.Combine(RootDirectory, "logs");
        BackupDirectory = Path.Combine(RootDirectory, "backups");
        DatabaseFile = Path.Combine(DataDirectory, "bewerbungsmanager.db");
    }

    /// <summary>Gets the root directory for application-owned local data.</summary>
    public string RootDirectory { get; }

    /// <summary>Gets the directory containing structured data.</summary>
    public string DataDirectory { get; }

    /// <summary>Gets the immutable managed document store root.</summary>
    public string DocumentStoreDirectory { get; }

    /// <summary>Gets the local log directory.</summary>
    public string LogDirectory { get; }

    /// <summary>Gets the default local backup directory.</summary>
    public string BackupDirectory { get; }

    /// <summary>Gets the SQLite database file path.</summary>
    public string DatabaseFile { get; }

    /// <summary>Gets the local SQLite connection string.</summary>
    public string ConnectionString
    {
        get
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = DatabaseFile,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Pooling = true,
                ForeignKeys = true,
            };

            return builder.ToString();
        }
    }

    /// <summary>
    /// Creates the directories required by the M0 application skeleton.
    /// </summary>
    public void EnsureDirectoriesExist()
    {
        Directory.CreateDirectory(RootDirectory);
        Directory.CreateDirectory(DataDirectory);
        Directory.CreateDirectory(DocumentStoreDirectory);
        Directory.CreateDirectory(LogDirectory);
        Directory.CreateDirectory(BackupDirectory);
    }
}
