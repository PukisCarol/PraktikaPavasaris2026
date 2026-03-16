using Moq;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Repositories;

namespace OrderManagement.Tests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRespository;
    private readonly Mock<IProductRepository> _productRepository;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderRespository = new Mock<IOrderRepository>();
        _productRepository = new Mock<IProductRepository>();
        _orderService = new OrderService(_orderRespository.Object, _productRepository.Object);
    }

    [Fact]
    public async Task GetInvoiceAsync_WithDiscount_AppliesDiscount()
    {
        // Arrange
        Product product = new Product
        {
            Id = 1,
            Name = "Product1",
            Price = 10,
            Discount = new Discount { Percentage = 10, MinProductCount = 5 }
        };

        Order order = new Order
        {
            Id = 1,
            CreationDate = DateTime.UtcNow,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, ProductCount = 6, Product = product }
            }
        };

        _orderRespository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);

        // Act
        InvoiceResponse invoice = await _orderService.GetInvoiceAsync(1);

        // Assert
        Assert.Equal(54, invoice.TotalAmount);
        Assert.Equal(10, invoice.Items[0].DiscountPercentage);
    }

    [Fact]
    public async Task GetInvoiceAsync_QuantityBelowMinimum_NoDiscountApplied()
    {
        // Arrange
        Product product = new Product
        {
            Id = 1,
            Name = "Product1",
            Price = 10,
            Discount = new Discount { Percentage = 10, MinProductCount = 5 }
        };

        Order order = new Order
        {
            Id = 1,
            CreationDate = DateTime.UtcNow,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, ProductCount = 3, Product = product }
            }
        };

        _orderRespository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);

        // Act
        InvoiceResponse invoice = await _orderService.GetInvoiceAsync(1);

        // Assert
        Assert.Equal(30, invoice.TotalAmount);
        Assert.Null(invoice.Items[0].DiscountPercentage); 
    }

    [Fact]
    public async Task GetInvoiceAsync_OrderNotFound_ThrowsException()
    {
        // Arrange
        _orderRespository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Order?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _orderService.GetInvoiceAsync(99));
    }
}