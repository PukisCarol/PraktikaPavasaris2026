namespace OrderManagement.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ProductCount { get; set; }
    public Product Product { get; set; } = null!;
}