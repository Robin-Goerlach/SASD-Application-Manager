using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SASD.Bewerbungsmanager.Infrastructure.Paths;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;

namespace SASD.Bewerbungsmanager.Infrastructure.DependencyInjection;

/// <summary>
/// Registers infrastructure adapters while keeping EF Core details out of the WinForms composition root.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the M0 infrastructure baseline.
    /// </summary>
    /// <param name="services">Application service collection.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddBewerbungsmanagerInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ApplicationPaths>();
        services.AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) =>
        {
            var paths = serviceProvider.GetRequiredService<ApplicationPaths>();
            paths.EnsureDirectoriesExist();
            options.UseSqlite(paths.ConnectionString);
        });
        services.AddSingleton<DatabaseInitializer>();

        return services;
    }
}
