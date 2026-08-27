using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SASD.Bewerbungsmanager.Application;
using SASD.Bewerbungsmanager.Infrastructure;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;
using SASD.Bewerbungsmanager.WinForms.Presentation;
using WinFormsApplication = System.Windows.Forms.Application;
using OperationalMainForm = SASD.Bewerbungsmanager.WinForms.Forms.MainForm;

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

            // Always resolve the operational form by its explicit alias. Older repository overlays may still
            // contain the pre-Milestone-1 MainForm in the root namespace; the alias prevents that
            // obsolete shell from ever winning C# namespace lookup again.
            WinFormsApplication.Run(host.Services.GetRequiredService<OperationalMainForm>());
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
