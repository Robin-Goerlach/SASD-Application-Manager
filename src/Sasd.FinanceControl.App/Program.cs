using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sasd.FinanceControl.Application.BankAccounts;
using Sasd.FinanceControl.Application.Banking;
using Sasd.FinanceControl.Application.Categories;
using Sasd.FinanceControl.Application.Documents;
using Sasd.FinanceControl.Application.Contracts;
using Sasd.FinanceControl.Application.Invoices;
using Sasd.FinanceControl.Application.Payments;
using Sasd.FinanceControl.Application.Orders;
using Sasd.FinanceControl.Application.Reconciliation;
using Sasd.FinanceControl.Infrastructure.Documents;
using Sasd.FinanceControl.Application.Persistence;
using Sasd.FinanceControl.Application.Suppliers;
using Sasd.FinanceControl.Application.Time;
using Sasd.FinanceControl.App.Configuration;
using Sasd.FinanceControl.App.Forms;
using Sasd.FinanceControl.App.Presentation;
using Sasd.FinanceControl.App.Services;
using Sasd.FinanceControl.App.Views;
using Sasd.FinanceControl.Infrastructure.Logging;
using Sasd.FinanceControl.Infrastructure.Persistence;
using Sasd.FinanceControl.Infrastructure.Persistence.Repositories;
using Sasd.FinanceControl.Infrastructure.Time;
using Sasd.FinanceControl.Import.Banking;

namespace Sasd.FinanceControl.App;

/// <summary>
/// Windows entry point and composition root for SASD Finance Control.
/// </summary>
internal static class Program
{
    private const string SingleInstanceMutexName = "Local\\SASD.FinanceControl.Application";

    /// <summary>
    /// Starts the desktop host, validates startup prerequisites, migrates the
    /// local database and composes all services implemented through Milestone 9.
    /// </summary>
    /// <returns>Zero for a normal shutdown; non-zero for startup failure.</returns>
    [STAThread]
    private static int Main()
    {
        ApplicationConfiguration.Initialize();
        System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

        ServiceProvider? serviceProvider = null;
        SingleInstanceGuard? singleInstanceGuard = null;

        try
        {
            var configuration = ConfigurationLoader.Load();
            var options = ConfigurationLoader.LoadOptions(configuration);
            var paths = ApplicationPaths.Create(options);
            paths.EnsureDirectoriesExist();

            if (options.SingleInstance)
            {
                singleInstanceGuard = new SingleInstanceGuard(SingleInstanceMutexName);

                if (!singleInstanceGuard.TryAcquire())
                {
                    MessageBox.Show(
                        "SASD Finance Control läuft bereits in dieser Windows-Sitzung.",
                        options.ApplicationName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return 0;
                }
            }

            var services = new ServiceCollection();
            ConfigureServices(services, options, paths);

            serviceProvider = services.BuildServiceProvider(
                new ServiceProviderOptions
                {
                    ValidateOnBuild = true,
                    ValidateScopes = true,
                });

            var logger = serviceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("Startup");

            logger.LogInformation(
                "Starting {ApplicationName} in {EnvironmentName}. DataRoot={DataRoot}; Database={DatabaseFile}",
                options.ApplicationName,
                options.EnvironmentName,
                paths.DataRoot,
                paths.DatabaseFilePath);

            // Database creation/migration happens before the main form appears.
            // If persistence cannot be made trustworthy, startup fails instead
            // of presenting a UI that would only fail later while saving data.
            serviceProvider
                .GetRequiredService<IDatabaseInitializer>()
                .InitializeAsync()
                .GetAwaiter()
                .GetResult();

            var globalExceptionHandler = serviceProvider.GetRequiredService<GlobalExceptionHandler>();
            globalExceptionHandler.Register();

            var mainForm = serviceProvider.GetRequiredService<MainForm>();
            var presenter = serviceProvider.GetRequiredService<MainPresenter>();
            presenter.Initialize();

            System.Windows.Forms.Application.Run(mainForm);

            logger.LogInformation("Application shutdown completed normally.");
            return 0;
        }
        catch (Exception exception)
        {
            // Startup is a deliberate top-level exception boundary. We try the
            // configured logger first; StartupFailureReporter remains usable
            // even when configuration, persistence or logging failed.
            TryLogStartupFailure(serviceProvider, exception);
            StartupFailureReporter.Report(exception);
            return 1;
        }
        finally
        {
            serviceProvider?.Dispose();
            singleInstanceGuard?.Dispose();
        }
    }

    private static void ConfigureServices(
        IServiceCollection services,
        FinanceControlOptions options,
        ApplicationPaths paths)
    {
        services.AddSingleton(options);
        services.AddSingleton(paths);

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(options.MinimumLogLevel);
            builder.AddProvider(new JsonFileLoggerProvider(paths.LogDirectory, options.MinimumLogLevel));
        });

        // Infrastructure implementations are selected only here in the
        // composition root. Application/Domain code remains independent from
        // the desktop host and from concrete infrastructure.
        services.AddSingleton<IApplicationClock, SystemApplicationClock>();
        services.AddSingleton(new SqliteConnectionFactory(paths.DatabaseFilePath));
        services.AddSingleton<IDatabaseInitializer, SqliteDatabaseInitializer>();
        services.AddSingleton<ISupplierRepository, SqliteSupplierRepository>();
        services.AddSingleton<ICategoryRepository, SqliteCategoryRepository>();
        services.AddSingleton<IBankAccountRepository, SqliteBankAccountRepository>();
        services.AddSingleton<IBankingRepository, SqliteBankingRepository>();
        services.AddSingleton<IPaymentAssignmentRepository, SqlitePaymentAssignmentRepository>();
        services.AddSingleton<IContractRepository, SqliteContractRepository>();
        services.AddSingleton<IInvoiceRepository, SqliteInvoiceRepository>();
        services.AddSingleton<IPurchaseOrderRepository, SqlitePurchaseOrderRepository>();
        services.AddSingleton<IProjectRepository, SqliteProjectRepository>();
        services.AddSingleton<ICostCenterRepository, SqliteCostCenterRepository>();
        services.AddSingleton<IReconciliationRepository, SqliteReconciliationRepository>();
        services.AddSingleton<IBankStatementFileImporter, CsvBankStatementFileImporter>();
        services.AddSingleton<IManualBankStatementSourceWriter, ManualBankStatementCsvWriter>();
        services.AddSingleton<IDocumentRepository, SqliteDocumentRepository>();
        services.AddSingleton<IDocumentStorage>(_ => new FileSystemDocumentStorage(paths.DocumentDirectory));

        services.AddSingleton<SupplierService>();
        services.AddSingleton<CategoryService>();
        services.AddSingleton<BankAccountService>();
        services.AddSingleton<BankingService>();
        services.AddSingleton<PaymentAssignmentService>();
        services.AddSingleton<ContractService>();
        services.AddSingleton<InvoiceService>();
        services.AddSingleton<PurchaseOrderService>();
        services.AddSingleton<ReconciliationService>();
        services.AddSingleton<DocumentArchiveService>();

        services.AddSingleton<IUserNotificationService, WinFormsUserNotificationService>();
        services.AddSingleton<GlobalExceptionHandler>();
        services.AddSingleton<IPageViewFactory, PageViewFactory>();

        services.AddSingleton<MainForm>();
        services.AddSingleton<IMainView>(provider => provider.GetRequiredService<MainForm>());
        services.AddSingleton<MainPresenter>();
    }

    private static void TryLogStartupFailure(
        ServiceProvider? serviceProvider,
        Exception exception)
    {
        if (serviceProvider is null)
        {
            return;
        }

        try
        {
            var logger = serviceProvider
                .GetService<ILoggerFactory>()?
                .CreateLogger("Startup");

            logger?.LogCritical(exception, "Application startup failed.");
        }
        catch (Exception loggingException)
        {
            // Avoid replacing the original startup failure with a secondary
            // logger failure. The fallback reporter will still persist the
            // original exception in the Windows temporary directory.
            System.Diagnostics.Debug.WriteLine(loggingException);
        }
    }
}
