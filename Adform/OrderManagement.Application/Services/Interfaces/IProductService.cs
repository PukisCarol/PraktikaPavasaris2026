using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync(string? nameFilter);
    Task<ProductResponse> CreateAsync(CreateProductRequest request);
    Task ApplyDiscountAsync(int productId, DiscountRequest request);
}
