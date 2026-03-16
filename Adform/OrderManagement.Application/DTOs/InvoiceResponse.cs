namespace OrderManagement.Application.DTOs;

public class InvoiceResponse
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<InvoiceItemResponse> Items { get; set; } = new();
}
