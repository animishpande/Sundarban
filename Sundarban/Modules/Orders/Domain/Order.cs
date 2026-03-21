using Sundarban.Modules.Orders.Enums;

namespace Sundarban.Modules.Orders.Domain;

public class Order
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public decimal Price { get; private set; } = 0;
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; private set; }
    
    private Order() {}

    public static Order Create(Guid customerId, string name, string category, decimal price)
    {
        if (price <= 0) throw new ArgumentException("Price needs to be greater than zero.");
        return new Order
        {
            Id = Guid.NewGuid(),
            Name = name,
            Category = category,
            Price = price,
            CustomerId = customerId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }
    public Order UpdateStatus(Order order, OrderStatus status)
    {
        var updatedOrder = new Order
        {
            Id = order.Id,
            Name = order.Name,
            Category = order.Category,
            Price = order.Price,
            CustomerId = order.CustomerId,
            Status = status,
            CreatedAt = order.CreatedAt
        };
        return updatedOrder;
    }
}