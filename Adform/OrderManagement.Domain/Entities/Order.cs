namespace OrderManagement.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem> { };
}
