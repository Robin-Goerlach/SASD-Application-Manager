using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SASD.Bewerbungsmanager.Application;
using SASD.Bewerbungsmanager.Infrastructure;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;
using SASD.Bewerbungsmanager.WinForms.Presentation;
using WinFormsApplication = System.Windows.Forms.Application;
using MilestoneMainForm = SASD.Bewerbungsmanager.WinForms.Forms.MainForm;

namespace SASD.Bewerbungsmanager.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddApplicationServices();
        builder.Services.AddTrackerInfrastructure(builder.Configuration);
        builder.Services.AddWinFormsPresentation();

        using var host = builder.Build();

        // Keep initialization on the original STA thread. There is no WinForms synchronization
        // context before Application.Run, so an async Main could resume on an MTA thread.
        host.Start();
        try
        {
            host.Services.GetRequiredService<DatabaseInitializer>()
                .InitializeAsync()
                .GetAwaiter()
                .GetResult();

            InstallGlobalExceptionHandling(host.Services.GetRequiredService<UiExceptionPresenter>());

            // There is still a legacy M0 MainForm in the root WinForms namespace. Use an explicit
            // alias here so namespace lookup can never silently start that obsolete shell instead
            // of the Milestone-1 form in the Forms namespace.
            WinFormsApplication.Run(host.Services.GetRequiredService<MilestoneMainForm>());
        }
        finally
        {
            host.StopAsync().GetAwaiter().GetResult();
        }
    }

    private static void InstallGlobalExceptionHandling(UiExceptionPresenter presenter)
    {
        WinFormsApplication.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        WinFormsApplication.ThreadException += (_, args) => presenter.Show(args.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception exception)
            {
                presenter.Show(exception);
            }
        };
    }
}
