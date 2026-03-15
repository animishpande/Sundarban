namespace Sundarban.Contracts;

public interface ICustomerService
{
    Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken ct);
}