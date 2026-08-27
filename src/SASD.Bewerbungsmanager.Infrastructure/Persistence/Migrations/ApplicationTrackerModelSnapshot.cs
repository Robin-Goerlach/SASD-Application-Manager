using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace SASD.Bewerbungsmanager.Infrastructure.Persistence.Migrations;

[DbContext(typeof(ApplicationTrackerDbContext))]
partial class ApplicationTrackerModelSnapshot : ModelSnapshot
{
    /// <inheritdoc />
    protected override void BuildModel(ModelBuilder modelBuilder)
        => BuildCurrentModel(modelBuilder);

    /// <summary>
    /// Builds the current persistence model used by the milestone migration metadata. Keeping one
    /// source of mapping truth is particularly valuable here because migrations are maintained in
    /// this coding chat without access to the dotnet-ef generator.
    /// </summary>
    internal static void BuildCurrentModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.11");
        ApplicationTrackerDbContext.ConfigureCurrentModel(modelBuilder);
    }
}
