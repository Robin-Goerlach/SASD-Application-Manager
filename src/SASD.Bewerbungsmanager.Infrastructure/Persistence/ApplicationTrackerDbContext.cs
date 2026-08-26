using Microsoft.EntityFrameworkCore;
using SASD.Bewerbungsmanager.Domain.Entities;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence;

/// <summary>
/// EF Core context for the local application-tracker database. The desktop application obtains
/// contexts through <see cref="IDbContextFactory{TContext}"/> so no context is retained for the
/// lifetime of the main form or shared across UI/background operations.
/// </summary>
public sealed class ApplicationTrackerDbContext(DbContextOptions<ApplicationTrackerDbContext> options) : DbContext(options)
{
    /// <summary>Gets the organizations table.</summary>
    public DbSet<Organization> Organizations => Set<Organization>();

    /// <summary>Gets the contacts table.</summary>
    public DbSet<Contact> Contacts => Set<Contact>();

    /// <summary>Gets the opportunities table.</summary>
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();

    /// <summary>Gets the source-links table.</summary>
    public DbSet<SourceLink> SourceLinks => Set<SourceLink>();

    /// <summary>Gets the applications table.</summary>
    public DbSet<JobApplication> Applications => Set<JobApplication>();

    /// <summary>Gets the application-status-history table.</summary>
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistory => Set<ApplicationStatusHistory>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        ConfigureOrganization(modelBuilder);
        ConfigureContact(modelBuilder);
        ConfigureOpportunity(modelBuilder);
        ConfigureSourceLink(modelBuilder);
        ConfigureApplication(modelBuilder);
        ConfigureApplicationStatusHistory(modelBuilder);
    }

    private static void ConfigureOrganization(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Organization>();
        entity.ToTable("organizations");
        entity.HasKey(item => item.Id);
        entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
        entity.Property(item => item.Type).HasConversion<string>().HasMaxLength(50).IsRequired();
        entity.Property(item => item.Website).HasMaxLength(2048);
        entity.Property(item => item.Notes).HasMaxLength(4000);
        entity.HasIndex(item => item.Name);
    }

    private static void ConfigureContact(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Contact>();
        entity.ToTable("contacts");
        entity.HasKey(item => item.Id);
        entity.Property(item => item.FullName).HasMaxLength(200).IsRequired();
        entity.Property(item => item.Role).HasMaxLength(200);
        entity.Property(item => item.Email).HasMaxLength(320);
        entity.Property(item => item.Phone).HasMaxLength(100);
        entity.Property(item => item.LinkedInUrl).HasMaxLength(2048);
        entity.Property(item => item.Notes).HasMaxLength(4000);
        entity.HasIndex(item => item.FullName);
        entity.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(item => item.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureOpportunity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Opportunity>();
        entity.ToTable("opportunities");
        entity.HasKey(item => item.Id);
        entity.Property(item => item.Title).HasMaxLength(250).IsRequired();
        entity.Property(item => item.DescriptionSnapshot).HasMaxLength(100_000).IsRequired();
        entity.Property(item => item.Location).HasMaxLength(250);
        entity.Property(item => item.RemoteText).HasMaxLength(250);
        entity.Property(item => item.SalaryText).HasMaxLength(250);
        entity.Property(item => item.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        entity.HasIndex(item => item.Status);
        entity.HasIndex(item => item.FoundAtUtc);
        entity.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(item => item.EmployerOrganizationId)
            .OnDelete(DeleteBehavior.SetNull);
        entity.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(item => item.IntermediaryOrganizationId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureSourceLink(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<SourceLink>();
        entity.ToTable("source_links");
        entity.HasKey(item => item.Id);
        entity.Property(item => item.Source).HasMaxLength(100).IsRequired();
        entity.Property(item => item.Url).HasMaxLength(2048).IsRequired();
        entity.Property(item => item.ExternalId).HasMaxLength(250);
        entity.HasIndex(item => item.OpportunityId);
        entity.HasOne<Opportunity>()
            .WithMany()
            .HasForeignKey(item => item.OpportunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureApplication(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<JobApplication>();
        entity.ToTable("applications");
        entity.HasKey(item => item.Id);
        entity.Property(item => item.Stage).HasConversion<string>().HasMaxLength(50).IsRequired();
        entity.Property(item => item.Channel).HasConversion<string>().HasMaxLength(50).IsRequired();
        entity.Property(item => item.SalaryExpectation).HasMaxLength(250);
        entity.Property(item => item.Outcome).HasMaxLength(2000);
        entity.HasIndex(item => item.Stage);
        entity.HasIndex(item => item.OpportunityId);
        entity.HasOne<Opportunity>()
            .WithMany()
            .HasForeignKey(item => item.OpportunityId)
            .OnDelete(DeleteBehavior.Restrict);

        // The public StatusHistory collection is intentionally read-only. EF writes directly to the
        // backing field when materializing a graph, preserving the domain API while keeping persistence simple.
        entity.Ignore(item => item.StatusHistory);
        entity.HasMany<ApplicationStatusHistory>("_statusHistory")
            .WithOne()
            .HasForeignKey(item => item.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure access mode on the navigation itself. The relationship builder exposes
        // foreign-key metadata, and EF Core 10 does not provide SetPropertyAccessMode on
        // IMutableForeignKey. The field-only navigation keeps the domain collection read-only.
        entity.Navigation("_statusHistory")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureApplicationStatusHistory(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ApplicationStatusHistory>();
        entity.ToTable("application_status_history");
        entity.HasKey(item => item.Id);
        entity.Property(item => item.Stage).HasConversion<string>().HasMaxLength(50).IsRequired();
        entity.Property(item => item.Note).HasMaxLength(2000);
        entity.HasIndex(item => new { item.ApplicationId, item.ChangedAtUtc });
    }
}
