using MediatR;

namespace Sundarban.Modules.Customers.Application.Commands;

public record CreateCustomerCommand(string FirstName, string LastName, string Email) : IRequest<Guid>;