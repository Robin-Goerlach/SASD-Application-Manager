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
using Sasd.FinanceControl.Application.Suppliers;
using Sasd.FinanceControl.App.Presentation;
using Sasd.FinanceControl.App.Services;
using Sasd.FinanceControl.App.Views.Documents;
using Sasd.FinanceControl.App.Views.Contracts;
using Sasd.FinanceControl.App.Views.Banking;
using Sasd.FinanceControl.App.Views.MasterData;
using Sasd.FinanceControl.App.Views.Payments;
using Sasd.FinanceControl.App.Views.Invoices;
using Sasd.FinanceControl.App.Views.Orders;
using Sasd.FinanceControl.App.Views.Reconciliation;

namespace Sasd.FinanceControl.App.Views;

/// <summary>Default factory for top-level WinForms views.</summary>
public sealed class PageViewFactory : IPageViewFactory
{
    private readonly SupplierService _supplierService;
    private readonly CategoryService _categoryService;
    private readonly BankAccountService _bankAccountService;
    private readonly BankingService _bankingService;
    private readonly PaymentAssignmentService _paymentAssignmentService;
    private readonly ContractService _contractService;
    private readonly InvoiceService _invoiceService;
    private readonly PurchaseOrderService _purchaseOrderService;
    private readonly ReconciliationService _reconciliationService;
    private readonly DocumentArchiveService _documentArchiveService;
    private readonly IUserNotificationService _notifications;
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>Initializes the factory and shared application services.</summary>
    public PageViewFactory(
        SupplierService supplierService,
        CategoryService categoryService,
        BankAccountService bankAccountService,
        BankingService bankingService,
        PaymentAssignmentService paymentAssignmentService,
        ContractService contractService,
        InvoiceService invoiceService,
        PurchaseOrderService purchaseOrderService,
        ReconciliationService reconciliationService,
        DocumentArchiveService documentArchiveService,
        IUserNotificationService notifications,
        ILoggerFactory loggerFactory)
    {
        _supplierService = supplierService;
        _categoryService = categoryService;
        _bankAccountService = bankAccountService;
        _bankingService = bankingService;
        _paymentAssignmentService = paymentAssignmentService;
        _contractService = contractService;
        _invoiceService = invoiceService;
        _purchaseOrderService = purchaseOrderService;
        _reconciliationService = reconciliationService;
        _documentArchiveService = documentArchiveService;
        _notifications = notifications;
        _loggerFactory = loggerFactory;
    }

    /// <inheritdoc />
    public Control Create(PageDescriptor page, ShellStatus? shellStatus)
    {
        ArgumentNullException.ThrowIfNull(page);

        return page.Target switch
        {
            NavigationTarget.Dashboard => new DashboardView(page, shellStatus),
            NavigationTarget.Suppliers => new SupplierMasterDataView(
                _supplierService,
                _notifications,
                _loggerFactory.CreateLogger<SupplierMasterDataView>()),
            NavigationTarget.Categories => new CategoryMasterDataView(
                _categoryService,
                _notifications,
                _loggerFactory.CreateLogger<CategoryMasterDataView>()),
            NavigationTarget.Banking => new BankingView(
                _bankingService,
                _bankAccountService,
                _notifications,
                _loggerFactory.CreateLogger<BankingView>(),
                _loggerFactory),
            NavigationTarget.Payments => new PaymentAssignmentView(
                _paymentAssignmentService,
                _bankAccountService,
                _supplierService,
                _categoryService,
                _notifications,
                _loggerFactory.CreateLogger<PaymentAssignmentView>()),
            NavigationTarget.Reconciliation => new ReconciliationView(
                _reconciliationService,
                _notifications,
                _loggerFactory.CreateLogger<ReconciliationView>()),
            NavigationTarget.Contracts => new ContractManagementView(
                _contractService,
                _supplierService,
                _documentArchiveService,
                _notifications,
                _loggerFactory.CreateLogger<ContractManagementView>()),
            NavigationTarget.Invoices => new InvoiceManagementView(
                _invoiceService,
                _reconciliationService,
                _supplierService,
                _documentArchiveService,
                _notifications,
                _loggerFactory.CreateLogger<InvoiceManagementView>()),
            NavigationTarget.Orders => new PurchaseOrderManagementView(
                _purchaseOrderService,
                _reconciliationService,
                _supplierService,
                _categoryService,
                _documentArchiveService,
                _notifications,
                _loggerFactory.CreateLogger<PurchaseOrderManagementView>()),
            NavigationTarget.Documents => new DocumentArchiveView(
                _documentArchiveService,
                _supplierService,
                _notifications,
                _loggerFactory.CreateLogger<DocumentArchiveView>()),
            _ => new PlaceholderView(page),
        };
    }
}
