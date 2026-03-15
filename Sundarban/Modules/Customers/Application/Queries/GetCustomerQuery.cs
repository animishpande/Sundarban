using MediatR;
using Sundarban.Modules.Customers.Domain;

namespace Sundarban.Modules.Customers.Application.Queries;

public record GetCustomerQuery() : IRequest<List<Customer>>;