using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SASD.Bewerbungsmanager.Application.Abstractions;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;

namespace SASD.Bewerbungsmanager.Infrastructure;

/// <summary>Registers SQLite and infrastructure adapters.</summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Adds the local SQLite persistence layer. A configured Database:Path overrides the default
    /// per-user LocalApplicationData location and is primarily useful for tests or diagnostics.
    /// </summary>
    public static IServiceCollection AddTrackerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var configuredPath = configuration["Database:Path"];
        var databasePath = string.IsNullOrWhiteSpace(configuredPath)
            ? AppDataPath.GetDefaultDatabasePath()
            : Path.GetFullPath(Environment.ExpandEnvironmentVariables(configuredPath));

        var directory = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        services.AddDbContextFactory<ApplicationTrackerDbContext>(options =>
            options.UseSqlite($"Data Source={databasePath};Foreign Keys=True"));
        services.AddSingleton<ITrackerDataStore, TrackerDataStore>();
        services.AddSingleton<IDocumentArchive, FileSystemDocumentArchive>();
        services.AddSingleton<IApplicationExportWriter, FileSystemApplicationExportWriter>();
        services.AddSingleton<ICommunicationHandoffReader, JsonCommunicationHandoffReader>();
        services.AddSingleton<IJobSourceReader, JsonJobSourceReader>();
        services.AddSingleton<IJobSourceReader, CsvJobSourceReader>();
        services.AddSingleton<DatabaseInitializer>();
        return services;
    }
}
