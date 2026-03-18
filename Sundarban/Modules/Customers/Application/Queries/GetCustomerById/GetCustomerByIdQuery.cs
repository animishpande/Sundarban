using MediatR;
using Sundarban.Modules.Customers.Application.DTOs;
using Sundarban.Modules.Customers.Domain;

namespace Sundarban.Modules.Customers.Application.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid CustomerId) : IRequest<Customer>;