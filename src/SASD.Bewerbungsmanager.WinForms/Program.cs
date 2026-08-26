using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SASD.Bewerbungsmanager.Infrastructure.DependencyInjection;
using SASD.Bewerbungsmanager.Infrastructure.Persistence;
using SASD.Bewerbungsmanager.WinForms.Bootstrap;
using SASD.Bewerbungsmanager.WinForms.Presentation.MainShell;

namespace SASD.Bewerbungsmanager.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        using var singleInstance = SingleInstanceGuard.TryAcquire();
        if (!singleInstance.IsPrimaryInstance)
        {
            MessageBox.Show(
                "Der SASD Bewerbungsmanager läuft bereits.",
                "SASD Bewerbungsmanager",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        ApplicationConfiguration.Initialize();

        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddDebug();

        builder.Services.AddBewerbungsmanagerInfrastructure();
        builder.Services.AddSingleton<MainShellPresenter>();
        builder.Services.AddSingleton<MainForm>();

        using var host = builder.Build();
        host.Start();

        var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Application");
        Application.ThreadException += (_, eventArgs) =>
            logger.LogCritical(eventArgs.Exception, "Unhandled WinForms thread exception.");
        TaskScheduler.UnobservedTaskException += (_, eventArgs) =>
        {
            logger.LogError(eventArgs.Exception, "Unobserved task exception.");
            eventArgs.SetObserved();
        };
        AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
            logger.LogCritical(eventArgs.ExceptionObject as Exception, "Unhandled process exception.");

        try
        {
            host.Services.GetRequiredService<DatabaseInitializer>().Initialize();
            Application.Run(host.Services.GetRequiredService<MainForm>());
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "Application startup or message loop failed.");
            MessageBox.Show(
                "Die Anwendung konnte nicht sicher gestartet oder ausgeführt werden und wurde beendet.",
                "SASD Bewerbungsmanager",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            host.StopAsync().GetAwaiter().GetResult();
        }
    }
}
