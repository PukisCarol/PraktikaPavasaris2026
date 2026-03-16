using OrderManagement.Domain.Entities;

namespace OrderManagement.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync(string? nameFilter);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
}
