using MediatR;

namespace Sundarban.Contracts;

public record OrderPaidEvent(Guid OrderId, Guid CustomerId) : INotification;