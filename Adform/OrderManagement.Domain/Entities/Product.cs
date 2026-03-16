namespace OrderManagement.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public Discount? Discount { get; set; }
}
