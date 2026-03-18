using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sundarban.Modules.Orders.Application.Commands;
using Sundarban.Modules.Orders.Application.DTOs;
using Sundarban.Modules.Orders.Application.Queries;

namespace Sundarban.Modules.Orders.Presentation;

public static class OrderEndpoints
{
    public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (PlaceOrderDto placeOrderDto, IMediator mediator) =>
        {
            var orderId = await mediator.Send(new PlaceOrderCommand(placeOrderDto));
            return Results.Ok(orderId);
        });

        app.MapGet("/orders/{id}", async ([FromRoute] Guid id, IMediator mediator) =>
        {
            var orderList = await mediator.Send(new GetOrderQuery(id));
            return Results.Ok(orderList);
        });
    }
}