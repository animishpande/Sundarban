using MediatR;
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

    public CreatePaymentHandler(PaymentsDbContext dbContext, IOrderService orderService)
    {
        _dbContext = dbContext;
        _orderService = orderService;
    }
    
    public async Task<Guid> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var orderExists = await _orderService.OrderExistsAsync(command.OrderId, cancellationToken);
        if (!orderExists)
        {
            throw new NotFoundException("Order", command.OrderId);
        }
        var amount = await _orderService.GetOrderAmountAsync(command.OrderId, cancellationToken);
        var payment = Payment.Create(command.OrderId, amount, PaymentStatus.Completed);
        _dbContext.Payments.Add(payment);
        bool updateOrderStatus = await _orderService.MarkOrderAsPaidAsync(command.OrderId, cancellationToken);
        if (!updateOrderStatus) 
            throw new Exception("Order Not Paid");
        await _dbContext.SaveChangesAsync(cancellationToken);
        return payment.Id;
    }
}