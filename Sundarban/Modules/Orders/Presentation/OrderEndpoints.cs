using MediatR;
using Sundarban.Modules.Orders.Application.Commands;
using Sundarban.Modules.Orders.Application.Queries;

namespace Sundarban.Modules.Orders.Presentation;

public static class OrderEndpoints
{
    public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (PlaceOrderCommand command, IMediator mediator) =>
        {
            var orderId = await mediator.Send(command);
            return Results.Ok(orderId);
        });

        app.MapGet("/orders", async (IMediator mediator) =>
        {
            var orderList = await mediator.Send(new GetOrderQuery());
            return Results.Ok(orderList);
        });
    }
}