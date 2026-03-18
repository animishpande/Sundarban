namespace Sundarban.Modules.Orders.Application.DTOs;

public class PlaceOrderDto
{
    public Guid CustomerId  { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
}