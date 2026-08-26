using SASD.Bewerbungsmanager.Application;

namespace SASD.Bewerbungsmanager.Application.Tests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Application_must_not_reference_infrastructure_or_winforms()
    {
        var references = typeof(ApplicationAssemblyMarker).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name ?? string.Empty)
            .ToArray();

        Assert.DoesNotContain("SASD.Bewerbungsmanager.Infrastructure", references);
        Assert.DoesNotContain("SASD.Bewerbungsmanager", references);
        Assert.DoesNotContain("System.Windows.Forms", references);
    }
}
