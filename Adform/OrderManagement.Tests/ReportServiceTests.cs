using Moq;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Repositories;

namespace OrderManagement.Tests;

public class ReportServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly ReportService _reportService;

    public ReportServiceTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _reportService = new ReportService(_orderRepository.Object);
    }

    [Fact]
    public async Task GetDiscountReportAsyncNoOrders_ReturnsEmptyList()
    {
        // Arrange
        _orderRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Order>());

        // Act
        IEnumerable<DiscountReportResponse> result = await _reportService.GetDiscountReportAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDiscountReportAsyncCountLessThanMin_ReturnsEmptyList()
    {
        // Arrange
        Product product = new Product { Id = 1, Name = "Product1", Price = 10, Discount = new Discount { Percentage = 10, MinProductCount = 5 }};

        List<Order> orders = new List<Order>
        {
            new Order
            {
                Id = 1,
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, ProductCount = 3, Product = product }
                }
            }
        };

        _orderRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

        // Act
        IEnumerable<DiscountReportResponse> result = await _reportService.GetDiscountReportAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDiscountReportAsyncWithDiscount_ReturnsCorrectReport()
    {
        // Arrange
        Product product = new Product
        {
            Id = 1,
            Name = "Product1",
            Price = 10,
            Discount = new Discount { Percentage = 10, MinProductCount = 5 }
        };

        List<Order> orders = new List<Order>
    {
        new Order
        {
            Id = 1,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, ProductCount = 6, Product = product }
            }
        }
    };

        _orderRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        // Act
        IEnumerable<DiscountReportResponse> result = await _reportService.GetDiscountReportAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Product1", result.First().ProductName);
        Assert.Equal(10, result.First().DiscountPercentage);
        Assert.Equal(1, result.First().NumberOfOrders);
        Assert.Equal(54, result.First().TotalAmount);
    }

    [Fact]
    public async Task GetDiscountReportAsync_MultipleOrders_CountsCorrectly()
    {
        // Arrange
        Product product = new Product
        {
            Id = 1,
            Name = "Product1",
            Price = 10,
            Discount = new Discount { Percentage = 10, MinProductCount = 5 }
        };

        List<Order> orders = new List<Order>
    {
        new Order
        {
            Id = 1,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, ProductCount = 6, Product = product }
            }
        },
        new Order
        {
            Id = 2,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, ProductCount = 5, Product = product }
            }
        }
    };

        _orderRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        // Act
        IEnumerable<DiscountReportResponse> result = await _reportService.GetDiscountReportAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result.First().NumberOfOrders);
        Assert.Equal(99, result.First().TotalAmount);
    }

    [Fact]
    public async Task GetDiscountReportAsync_ProductWithoutDiscount_NotIncluded()
    {
        // Arrange
        Product productWithoutDiscount = new Product
        {
            Id = 2,
            Name = "Product1",
            Price = 5,
            Discount = null
        };

        List<Order> orders = new List<Order>
    {
        new Order
        {
            Id = 1,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 2, ProductCount = 10, Product = productWithoutDiscount }
            }
        }
    };

        _orderRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        // Act
        IEnumerable<DiscountReportResponse> result = await _reportService.GetDiscountReportAsync();

        // Assert
        Assert.Empty(result);
    }
}
