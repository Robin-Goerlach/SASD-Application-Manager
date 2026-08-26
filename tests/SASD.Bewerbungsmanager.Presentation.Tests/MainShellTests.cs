using Microsoft.Extensions.DependencyInjection;
using System.Windows.Forms;
using SASD.Bewerbungsmanager.WinForms;
using MilestoneMainForm = SASD.Bewerbungsmanager.WinForms.Forms.MainForm;
using LegacyMainForm = SASD.Bewerbungsmanager.WinForms.MainForm;

namespace SASD.Bewerbungsmanager.Presentation.Tests;

/// <summary>
/// Guards the WinForms composition root. In particular, these tests prevent the obsolete M0 shell
/// in the root WinForms namespace from silently shadowing the current Milestone-1 main form.
/// </summary>
public sealed class MainShellTests
{
    [Fact]
    public void Milestone_main_form_is_a_windows_form()
    {
        Assert.True(typeof(Form).IsAssignableFrom(typeof(MilestoneMainForm)));
    }

    [Fact]
    public void WinForms_composition_registers_current_shell_not_legacy_shell()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddWinFormsPresentation();

        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(MilestoneMainForm));
        Assert.DoesNotContain(services, descriptor => descriptor.ServiceType == typeof(LegacyMainForm));

        // ValidateOnBuild verifies constructor dependencies without constructing a Form on the
        // xUnit worker thread. This catches missing DI registrations before a real GUI start.
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        });
    }
}
