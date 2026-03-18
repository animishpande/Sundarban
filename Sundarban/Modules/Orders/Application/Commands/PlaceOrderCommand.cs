using MediatR;
using Sundarban.Modules.Orders.Application.DTOs;

namespace Sundarban.Modules.Orders.Application.Commands;

public record PlaceOrderCommand(PlaceOrderDto PlaceOrderDto) : IRequest<Guid>;