using MediatR;
using Microsoft.EntityFrameworkCore;
using Sundarban.Modules.Payments.Application.Commands;

namespace Sundarban.Modules.Payments.Presentation;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/payments", async (CreatePaymentCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"payments/{id}", new { id });
        });
    }
}