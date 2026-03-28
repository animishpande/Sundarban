using MediatR;
using Sundarban.Modules.Orders.Application.DTOs;
using Sundarban.Modules.Orders.Domain;

namespace Sundarban.Modules.Orders.Application.Queries;

public record GetOrderQuery(Guid CustomerId) : IRequest<List<Order>>;