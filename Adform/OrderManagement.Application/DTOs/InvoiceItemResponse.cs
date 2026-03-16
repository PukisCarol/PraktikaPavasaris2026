namespace OrderManagement.Application.DTOs;

public class InvoiceItemResponse
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
    public int? DiscountPercentage { get; set; }
    public decimal Amount { get; set; }
}
