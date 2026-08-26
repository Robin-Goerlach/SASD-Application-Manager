using Microsoft.Extensions.DependencyInjection;
using SASD.Bewerbungsmanager.WinForms.Presentation;
using MilestoneMainForm = SASD.Bewerbungsmanager.WinForms.Forms.MainForm;

namespace SASD.Bewerbungsmanager.WinForms;

/// <summary>
/// Registers the Windows Forms composition-root services for the current application shell.
/// Keeping these registrations in one method allows tests to validate the same DI graph that
/// production startup uses instead of duplicating service registrations in test code.
/// </summary>
public static class WinFormsServiceCollectionExtensions
{
    /// <summary>
    /// Adds the current Milestone-1 Windows Forms shell and presentation helpers.
    /// </summary>
    /// <param name="services">Service collection that receives the WinForms registrations.</param>
    /// <returns>The same service collection so registrations can be chained.</returns>
    public static IServiceCollection AddWinFormsPresentation(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<UiExceptionPresenter>();

        // Qualify the form deliberately. A legacy M0 shell still exists as
        // SASD.Bewerbungsmanager.WinForms.MainForm; registering the alias prevents that old type
        // from being selected by C# namespace precedence.
        services.AddSingleton<MilestoneMainForm>();

        return services;
    }
}
