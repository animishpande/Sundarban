using MediatR;
using Sundarban.Modules.Orders.Domain;

namespace Sundarban.Modules.Orders.Application.Queries;

public record GetOrderQuery() : IRequest<List<Order>>;