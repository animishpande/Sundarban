using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sundarban.Modules.Customers.Application.Commands;
using Sundarban.Modules.Customers.Application.DTOs;
using Sundarban.Modules.Customers.Application.Queries;
using Sundarban.Modules.Customers.Application.Queries.GetCustomerById;

namespace Sundarban.Modules.Customers.Presentation;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/customers", async (CreateCustomerCommand command, IMediator mediator) =>
        {
            var customerId = await mediator.Send(command);
            return Results.Ok(customerId);
        });

        app.MapGet("/customers", async (IMediator mediator) =>
        {
            var customers = await mediator.Send(new GetCustomerQuery());
            return Results.Ok(customers);
        });

        app.MapGet("/customers/{id}", async ([FromRoute] Guid id, IMediator mediator) =>
        {
            var customer = await mediator.Send(new GetCustomerByIdQuery(id));
            return Results.Ok(customer);
        });
    }
}