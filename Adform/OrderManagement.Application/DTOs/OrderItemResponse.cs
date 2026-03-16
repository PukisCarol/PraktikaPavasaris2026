namespace OrderManagement.Application.DTOs;

public class OrderItemResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Count { get; set; }
}
