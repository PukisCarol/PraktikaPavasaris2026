namespace OrderManagement.Application.DTOs;

public class DiscountReportResponse
{
    public string ProductName { get; set; } = string.Empty;
    public int DiscountPercentage { get; set; }
    public int NumberOfOrders { get; set; }
    public decimal TotalAmount { get; set; }
}