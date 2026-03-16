using Moq;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Repositories;

namespace OrderManagement.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepository;
    private readonly ProductService _productService;

    public ProductServiceTests() 
    {
        _productRepository = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepository.Object);
    }

    [Fact]
    public async Task CreteAsync_ReturnsProduct()
    {
        //Arrange
        CreateProductRequest newProduct = new CreateProductRequest { Name = "Product", Price = (decimal)10.5 };
        _productRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        //Act
        ProductResponse result = await _productService.CreateAsync(newProduct);

        //Assert
        Assert.Equal(newProduct.Name, result.Name);
        Assert.Equal(newProduct.Price, result.Price);
    }

    [Fact]
    public async Task GetAllAsyncNoFilter_ReturnsAllProducts()
    {
        // Arrange
        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Price = (decimal)5.5 },
            new Product { Id = 2, Name = "Product2", Price = (decimal)3.4 },
            new Product { Id = 3, Name = "Product3", Price = (decimal)8.4 }
        };
        _productRepository.Setup(x => x.GetAllAsync(null)).ReturnsAsync(products);

        // Act
        IEnumerable<ProductResponse> result = await _productService.GetAllAsync(null);

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllAsyncWithFilter_ReturnsFilteredProducts()
    {
        // Arrange
        Product product = new Product { Id = 1, Name = "Product1", Price = (decimal)5.5 };

        List<Product> products = new List<Product>
        {
            product
        };
        _productRepository.Setup(r => r.GetAllAsync(product.Name)).ReturnsAsync(products);

        // Act
        IEnumerable<ProductResponse> result = await _productService.GetAllAsync(product.Name);

        // Assert
        Assert.Single(result);
        Assert.Equal(product.Name, result.First().Name);
    }

    [Fact]
    public async Task GetAllAsyncProductWithDiscount_ReturnsDiscountInResponse()
    {
        // Arrange
        Product product = new Product { Id = 1, Name = "Product1", Price = (decimal)5.5,
            Discount = new Discount { Percentage = 10, MinProductCount = 3 } };

        List<Product> products = new List<Product>
        {
            product
        };
        _productRepository.Setup(r => r.GetAllAsync(null)).ReturnsAsync(products);

        // Act
        IEnumerable<ProductResponse> result = await _productService.GetAllAsync(null);

        // Assert
        Assert.NotNull(result.First().Discount);
        Assert.Equal(10, result.First().Discount!.Percentage);
    }

    [Fact]
    public async Task ApplyDiscountAsync_ValidProduct_UpdatesDiscount()
    {
        // Arrange
        Product product = new Product { Id = 1, Name = "Product1", Price = 5 };
        DiscountRequest request = new DiscountRequest { Percentage = 10, MinCount = 3 };

        _productRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);
        _productRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        await _productService.ApplyDiscountAsync(1, request);

        // Assert
        _productRepository.Verify(x => x.UpdateAsync(It.Is<Product>(x =>
            x.Discount!.Percentage == 10 &&
            x.Discount.MinProductCount == 3)), Times.Once);
    }

    [Fact]
    public async Task ApplyDiscountAsync_ProductNotFound_ThrowsException()
    {
        // Arrange
        _productRepository.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _productService.ApplyDiscountAsync(99, new DiscountRequest()));
    }
}