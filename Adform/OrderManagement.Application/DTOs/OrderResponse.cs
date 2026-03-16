namespace OrderManagement.Application.DTOs;

public class OrderResponse
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public List<OrderItemResponse> orderItems { get; set; } = new();
}
