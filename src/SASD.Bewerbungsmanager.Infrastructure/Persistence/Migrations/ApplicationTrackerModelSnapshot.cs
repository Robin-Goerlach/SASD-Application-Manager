using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SASD.Bewerbungsmanager.Domain.Entities;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;
using SASD.Bewerbungsmanager.Domain.Enums;

#nullable disable

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence.Migrations;

[DbContext(typeof(ApplicationTrackerDbContext))]
partial class ApplicationTrackerModelSnapshot : ModelSnapshot
{
    /// <inheritdoc />
    protected override void BuildModel(ModelBuilder modelBuilder) => BuildCurrentModel(modelBuilder);

    internal static void BuildCurrentModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.0");

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("organizations");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnType("TEXT");
            entity.Property(item => item.Name).HasColumnType("TEXT").HasMaxLength(200).IsRequired();
            entity.Property(item => item.Type).HasColumnType("TEXT").HasMaxLength(50).HasConversion<string>().IsRequired();
            entity.Property(item => item.Website).HasColumnType("TEXT").HasMaxLength(2048);
            entity.Property(item => item.Notes).HasColumnType("TEXT").HasMaxLength(4000);
            entity.Property(item => item.IsArchived).HasColumnType("INTEGER");
            entity.Property(item => item.CreatedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.UpdatedAtUtc).HasColumnType("TEXT");
            entity.HasIndex(item => item.Name);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("contacts");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnType("TEXT");
            entity.Property(item => item.OrganizationId).HasColumnType("TEXT");
            entity.Property(item => item.FullName).HasColumnType("TEXT").HasMaxLength(200).IsRequired();
            entity.Property(item => item.Role).HasColumnType("TEXT").HasMaxLength(200);
            entity.Property(item => item.Email).HasColumnType("TEXT").HasMaxLength(320);
            entity.Property(item => item.Phone).HasColumnType("TEXT").HasMaxLength(100);
            entity.Property(item => item.LinkedInUrl).HasColumnType("TEXT").HasMaxLength(2048);
            entity.Property(item => item.Notes).HasColumnType("TEXT").HasMaxLength(4000);
            entity.Property(item => item.IsArchived).HasColumnType("INTEGER");
            entity.Property(item => item.CreatedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.UpdatedAtUtc).HasColumnType("TEXT");
            entity.HasIndex(item => item.FullName);
            entity.HasIndex(item => item.OrganizationId);
            entity.HasOne<Organization>().WithMany().HasForeignKey(item => item.OrganizationId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Opportunity>(entity =>
        {
            entity.ToTable("opportunities");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnType("TEXT");
            entity.Property(item => item.EmployerOrganizationId).HasColumnType("TEXT");
            entity.Property(item => item.IntermediaryOrganizationId).HasColumnType("TEXT");
            entity.Property(item => item.Title).HasColumnType("TEXT").HasMaxLength(250).IsRequired();
            entity.Property(item => item.DescriptionSnapshot).HasColumnType("TEXT").HasMaxLength(100000).IsRequired();
            entity.Property(item => item.Location).HasColumnType("TEXT").HasMaxLength(250);
            entity.Property(item => item.RemoteText).HasColumnType("TEXT").HasMaxLength(250);
            entity.Property(item => item.SalaryText).HasColumnType("TEXT").HasMaxLength(250);
            entity.Property(item => item.Status).HasColumnType("TEXT").HasMaxLength(50).HasConversion<string>().IsRequired();
            entity.Property(item => item.FoundAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.PublishedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.DeadlineAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.CreatedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.UpdatedAtUtc).HasColumnType("TEXT");
            entity.HasIndex(item => item.EmployerOrganizationId);
            entity.HasIndex(item => item.IntermediaryOrganizationId);
            entity.HasIndex(item => item.Status);
            entity.HasIndex(item => item.FoundAtUtc);
            entity.HasOne<Organization>().WithMany().HasForeignKey(item => item.EmployerOrganizationId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne<Organization>().WithMany().HasForeignKey(item => item.IntermediaryOrganizationId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<SourceLink>(entity =>
        {
            entity.ToTable("source_links");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnType("TEXT");
            entity.Property(item => item.OpportunityId).HasColumnType("TEXT");
            entity.Property(item => item.Source).HasColumnType("TEXT").HasMaxLength(100).IsRequired();
            entity.Property(item => item.Url).HasColumnType("TEXT").HasMaxLength(2048).IsRequired();
            entity.Property(item => item.ExternalId).HasColumnType("TEXT").HasMaxLength(250);
            entity.Property(item => item.CapturedAtUtc).HasColumnType("TEXT");
            entity.HasIndex(item => item.OpportunityId);
            entity.HasOne<Opportunity>().WithMany().HasForeignKey(item => item.OpportunityId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.ToTable("applications");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnType("TEXT");
            entity.Property(item => item.OpportunityId).HasColumnType("TEXT");
            entity.Property(item => item.StartedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.SubmittedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.Stage).HasColumnType("TEXT").HasMaxLength(50).HasConversion<string>().IsRequired();
            entity.Property(item => item.Channel).HasColumnType("TEXT").HasMaxLength(50).HasConversion<string>().IsRequired();
            entity.Property(item => item.SalaryExpectation).HasColumnType("TEXT").HasMaxLength(250);
            entity.Property(item => item.ClosedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.Outcome).HasColumnType("TEXT").HasMaxLength(2000);
            entity.Property(item => item.CreatedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.UpdatedAtUtc).HasColumnType("TEXT");
            entity.Ignore(item => item.StatusHistory);
            entity.HasIndex(item => item.OpportunityId);
            entity.HasIndex(item => item.Stage);
            entity.HasOne<Opportunity>().WithMany().HasForeignKey(item => item.OpportunityId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany<ApplicationStatusHistory>("_statusHistory")
                .WithOne()
                .HasForeignKey(item => item.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Navigation("_statusHistory").UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<ApplicationStatusHistory>(entity =>
        {
            entity.ToTable("application_status_history");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnType("TEXT");
            entity.Property(item => item.ApplicationId).HasColumnType("TEXT");
            entity.Property(item => item.Stage).HasColumnType("TEXT").HasMaxLength(50).HasConversion<string>().IsRequired();
            entity.Property(item => item.ChangedAtUtc).HasColumnType("TEXT");
            entity.Property(item => item.Note).HasColumnType("TEXT").HasMaxLength(2000);
            entity.HasIndex(item => new { item.ApplicationId, item.ChangedAtUtc });
        });
    }
}
