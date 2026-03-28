using MediatR;
using Microsoft.EntityFrameworkCore;
using Sundarban.Contracts;
using Sundarban.Exceptions;
using Sundarban.Modules.Payments.Domain;
using Sundarban.Modules.Payments.Enums;
using Sundarban.Modules.Payments.Infrastructure;

namespace Sundarban.Modules.Payments.Application.Commands;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Guid>
{
    private readonly PaymentsDbContext _dbContext;
    private readonly IOrderService _orderService;
    private readonly IPublisher _publisher;

    public CreatePaymentHandler(PaymentsDbContext dbContext, IOrderService orderService, IPublisher publisher)
    {
        _dbContext = dbContext;
        _orderService = orderService;
        _publisher = publisher;
    }
    
    public async Task<Guid> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var orderExists = await _orderService.OrderExistsAsync(command.OrderId, cancellationToken);
        if (!orderExists)
            throw new NotFoundException("Order", command.OrderId);
        
        var paymentExists = await _dbContext.Payments.AnyAsync(p => p.OrderId == command.OrderId, cancellationToken);
        if (paymentExists)
            throw new PaymentException($"Payment with order {command.OrderId} already exists");
        
        var amount = await _orderService.GetOrderAmountAsync(command.OrderId, cancellationToken);
        var customerId = await _orderService.GetOrderCustomerIdAsync(command.OrderId, cancellationToken);

        var payment = Payment.Create(command.OrderId, amount, PaymentStatus.Completed);
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new OrderPaidEvent(command.OrderId, customerId), cancellationToken);
        return payment.Id;
    }
}