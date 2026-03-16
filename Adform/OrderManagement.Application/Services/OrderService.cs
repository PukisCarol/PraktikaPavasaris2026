using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Repositories;

namespace OrderManagement.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
    {
        Order order = new Order
        {
            CreationDate = DateTime.UtcNow,
            Items = new List<OrderItem>()
        };

        foreach (OrderItemRequest item in request.Items)
        {
            Product? product = await _productRepository.GetByIdAsync(item.ProductId);

            if (product == null)
            {
                throw new Exception($"Product with id {item.ProductId} not found");
            }

            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                ProductCount = item.Count
            });
        }

        await _orderRepository.AddAsync(order);

        Order? created = await _orderRepository.GetByIdAsync(order.Id);

        return new OrderResponse
        {
            Id = created!.Id,
            CreationDate = created.CreationDate,
            orderItems = created.Items.Select(i => new OrderItemResponse
            {
                ProductId = i.ProductId,
                ProductName = i.Product!.Name,
                Count = i.ProductCount
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderResponse>> GetAllAsync()
    {
        IEnumerable<Order> orders = await _orderRepository.GetAllAsync();
        List<OrderResponse> result = new List<OrderResponse>();

        foreach (Order order in orders)
        {
            List<OrderItemResponse> items = new List<OrderItemResponse>();

            foreach (OrderItem item in order.Items)
            {
                items.Add(new OrderItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Count = item.ProductCount
                });
            }

            result.Add(new OrderResponse
            {
                Id = order.Id,
                CreationDate = order.CreationDate,
                orderItems = items
            });
        }

        return result;
    }

    public async Task<InvoiceResponse> GetInvoiceAsync(int orderId)
    {
        Order? order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new Exception("Order not found"); 
        }

        List<InvoiceItemResponse> items = new List<InvoiceItemResponse>();

        foreach (OrderItem item in order.Items)
        {
            decimal price = item.Product.Price;
            int discountPercentage = 0;

            if (item.Product.Discount != null && item.ProductCount >= item.Product.Discount.MinProductCount)
            {
                discountPercentage = item.Product.Discount.Percentage;
            }

            if (discountPercentage > 100)
            {
                discountPercentage = 100;
            }

            decimal amount = price * item.ProductCount * (1 - discountPercentage / (decimal)100);

            InvoiceItemResponse invoiceItem = new InvoiceItemResponse
            {
                Name = item.Product.Name,
                Count = item.ProductCount,
                Amount = amount
            };

            if (discountPercentage > 0)
            {
                invoiceItem.DiscountPercentage = discountPercentage;
            }

            items.Add(invoiceItem);
        }

        return new InvoiceResponse
        {
            OrderId = order.Id,
            Items = items,
            TotalAmount = items.Sum(i => i.Amount)
        };
    }
}
