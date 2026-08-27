using Sasd.FinanceControl.Domain.Entities;

namespace Sasd.FinanceControl.Application.Orders;

/// <summary>Persistence boundary for purchase orders and their invoice links.</summary>
public interface IPurchaseOrderRepository
{
    Task<string> ReserveNextOrderNumberAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default);
    Task UpdateAsync(PurchaseOrder order, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PurchaseOrder>> SearchAsync(PurchaseOrderSearchCriteria criteria, CancellationToken cancellationToken = default);
    Task AddInvoiceLinkAsync(PurchaseOrderInvoiceLink link, CancellationToken cancellationToken = default);
    Task UpdateInvoiceLinkAsync(PurchaseOrderInvoiceLink link, CancellationToken cancellationToken = default);
    Task<PurchaseOrderInvoiceLink?> GetInvoiceLinkAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PurchaseOrderInvoiceLink>> GetInvoiceLinksAsync(Guid orderId, bool includeVoided, CancellationToken cancellationToken = default);
    Task<bool> ActiveInvoiceLinkExistsAsync(Guid orderId, Guid invoiceId, CancellationToken cancellationToken = default);
}
