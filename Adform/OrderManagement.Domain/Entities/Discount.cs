namespace OrderManagement.Domain.Entities;

public class Discount
{
    public int Id { get; set; }
    public int Percentage { get; set; }
    public int MinProductCount { get; set; }
}
