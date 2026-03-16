using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Repositories;

namespace OrderManagement.Application.Services;

public class ReportService : IReportService
{
    private readonly IOrderRepository _orderRepository;

    public ReportService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<DiscountReportResponse>> GetDiscountReportAsync()
    {
        IEnumerable<Order> orders = await _orderRepository.GetAllAsync();
        List<DiscountReportResponse> result = new List<DiscountReportResponse>();

        List<OrderItem> discountedItems = new List<OrderItem>();

        foreach (Order order in orders)
        {
            foreach (OrderItem item in order.Items)
            {
                if (item.Product.Discount != null && item.ProductCount >= item.Product.Discount.MinProductCount)
                {
                    discountedItems.Add(item);
                }
            }
        }

        IEnumerable<IGrouping<int, OrderItem>> grouped = discountedItems.GroupBy(i => i.ProductId);

        foreach (IGrouping<int, OrderItem> group in grouped)
        {
            Product product = group.First().Product;
            Discount discount = product.Discount!;
            decimal totalAmount = 0m;

            foreach (OrderItem? item in group)
            {
                int percentage = discount.Percentage;

                if (percentage > 100)
                    percentage = 100;

                totalAmount += product.Price * item.ProductCount * (1 - percentage / (decimal)100);
            }

            result.Add(new DiscountReportResponse
            {
                ProductName = product.Name,
                DiscountPercentage = discount.Percentage,
                NumberOfOrders = group.Count(),
                TotalAmount = totalAmount
            });
        }

        return result;
    }
}
