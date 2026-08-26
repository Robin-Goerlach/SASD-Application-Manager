using Microsoft.EntityFrameworkCore;

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence;

/// <summary>
/// Applies versioned database migrations and records the initial schema baseline.
/// </summary>
public sealed class DatabaseInitializer
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    /// <summary>
    /// Initializes a new database initializer.
    /// </summary>
    /// <param name="contextFactory">Short-lived context factory.</param>
    public DatabaseInitializer(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
    }

    /// <summary>
    /// Initializes or upgrades the local database before the WinForms message loop starts.
    /// </summary>
    public void Initialize()
    {
        using var dbContext = _contextFactory.CreateDbContext();
        dbContext.Database.Migrate();

        const string baselineKey = "SchemaBaseline";
        if (dbContext.SystemMetadata.Any(item => item.Key == baselineKey))
        {
            return;
        }

        dbContext.SystemMetadata.Add(new SystemMetadataRecord
        {
            Key = baselineKey,
            Value = "M0",
            UpdatedAtUtc = DateTime.UtcNow,
        });
        dbContext.SaveChanges();
    }
}
