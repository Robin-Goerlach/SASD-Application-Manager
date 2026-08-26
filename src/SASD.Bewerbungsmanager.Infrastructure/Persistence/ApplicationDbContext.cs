using Microsoft.EntityFrameworkCore;

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence boundary for the local SQLite database.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new database context.
    /// </summary>
    /// <param name="options">EF Core context options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets the operational metadata set used by the M0 baseline.</summary>
    public DbSet<SystemMetadataRecord> SystemMetadata => Set<SystemMetadataRecord>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        var metadata = modelBuilder.Entity<SystemMetadataRecord>();
        metadata.ToTable("SystemMetadata");
        metadata.HasKey(item => item.Id);
        metadata.Property(item => item.Key).HasMaxLength(200).IsRequired();
        metadata.Property(item => item.Value).HasMaxLength(2_000).IsRequired();
        metadata.Property(item => item.UpdatedAtUtc).IsRequired();
        metadata.HasIndex(item => item.Key).IsUnique();
    }
}
