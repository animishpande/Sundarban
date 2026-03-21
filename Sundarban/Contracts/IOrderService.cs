namespace Sundarban.Contracts;

public interface IOrderService
{
    Task<bool> OrderExistsAsync(Guid orderId, CancellationToken cancellationToken);
    Task<decimal> GetOrderAmountAsync(Guid orderId, CancellationToken cancellationToken);
    Task<bool> MarkOrderAsPaidAsync(Guid orderId, CancellationToken cancellationToken);
}