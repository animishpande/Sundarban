using MediatR;

namespace Sundarban.Modules.Payments.Application.Commands;

public record CreatePaymentCommand(Guid OrderId) : IRequest<Guid>;