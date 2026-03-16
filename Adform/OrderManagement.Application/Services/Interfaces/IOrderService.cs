using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderResponse>> GetAllAsync();
    Task<OrderResponse> CreateAsync(CreateOrderRequest request);
    Task<InvoiceResponse> GetInvoiceAsync(int orderId);
}
