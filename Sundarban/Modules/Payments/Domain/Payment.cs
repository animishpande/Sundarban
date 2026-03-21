using Sundarban.Modules.Payments.Enums;

namespace Sundarban.Modules.Payments.Domain;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; private set; }
    
    private Payment() {}

    public static Payment Create(Guid orderId, decimal amount, PaymentStatus status)
    {
        if (amount <= 0) throw new ArgumentException("Amount needs to be greater than zero.");
        return new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }
}