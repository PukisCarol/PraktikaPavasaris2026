namespace OrderManagement.Application.DTOs;

public class DiscountRequest
{
    public int Percentage { get; set; }
    public int MinCount { get; set; }
}