using MediatR;

namespace Sundarban.Modules.Orders.Application.Commands;

public record PlaceOrderCommand(Guid CustomerId, string Name, string Category, decimal Price) : IRequest<Guid>;