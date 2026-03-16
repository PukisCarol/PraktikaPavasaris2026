using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Repositories;

namespace OrderManagement.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository; 
    }

    public async Task ApplyDiscountAsync(int productId, DiscountRequest request)
    {
        Product? product = await _repository.GetByIdAsync(productId);

        if (product == null)
        {
            throw new Exception("Product not found");
        }

        product.Discount = new Discount
        {
            Percentage = request.Percentage,
            MinProductCount = request.MinCount
        };

        await _repository.UpdateAsync(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        Product product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        await _repository.AddAsync(product);

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync(string? nameFilter)
    {
        IEnumerable<Product> products = await _repository.GetAllAsync(nameFilter);
        List<ProductResponse> result = new List<ProductResponse>();

        foreach (Product product in products)
        {
            ProductResponse response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            if (product.Discount != null)
            {
                response.Discount = new DiscountResponse
                {
                    Percentage = product.Discount.Percentage,
                    MinCount = product.Discount.MinProductCount
                };
            }

            result.Add(response);
        }

        return result;
    }
}
