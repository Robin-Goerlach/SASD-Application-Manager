using SASD.Bewerbungsmanager.Infrastructure.Paths;

namespace SASD.Bewerbungsmanager.SystemTests;

public sealed class ApplicationPathsTests
{
    [Fact]
    public void Test_root_keeps_database_documents_logs_and_backups_together()
    {
        var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var paths = new ApplicationPaths(root);

        Assert.True(paths.DatabaseFile.StartsWith(Path.GetFullPath(root), StringComparison.OrdinalIgnoreCase));
        Assert.True(paths.DocumentStoreDirectory.StartsWith(Path.GetFullPath(root), StringComparison.OrdinalIgnoreCase));
        Assert.True(paths.LogDirectory.StartsWith(Path.GetFullPath(root), StringComparison.OrdinalIgnoreCase));
        Assert.True(paths.BackupDirectory.StartsWith(Path.GetFullPath(root), StringComparison.OrdinalIgnoreCase));
    }
}
