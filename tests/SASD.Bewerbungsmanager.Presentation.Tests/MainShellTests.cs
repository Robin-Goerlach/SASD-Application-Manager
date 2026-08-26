using System.Windows.Forms;

namespace SASD.Bewerbungsmanager.Presentation.Tests;

public sealed class MainShellTests
{
    [Fact]
    public void MainForm_is_a_windows_form()
    {
        Assert.True(typeof(Form).IsAssignableFrom(typeof(SASD.Bewerbungsmanager.WinForms.MainForm)));
    }
}
