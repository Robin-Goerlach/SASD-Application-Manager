using SASD.Bewerbungsmanager.Domain;

namespace SASD.Bewerbungsmanager.Domain.Tests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Domain_must_not_reference_ui_or_entity_framework()
    {
        var references = typeof(DomainAssemblyMarker).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name ?? string.Empty)
            .ToArray();

        Assert.False(references.Any(name => name.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal)));
        Assert.DoesNotContain("System.Windows.Forms", references);
    }
}
