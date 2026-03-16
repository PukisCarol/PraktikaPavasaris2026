using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services.Interfaces;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Retrieves all orders
    /// </summary>
    /// <returns>List of orders</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<OrderResponse> orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Creates a new order
    /// </summary>
    /// <param name="request">Order details with product list</param>
    /// <returns>Created order</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        OrderResponse order = await _orderService.CreateAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = order.Id }, order);
    }

    /// <summary>
    /// Retrieves invoice for a specific order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Order invoice with discount calculations</returns>
    [HttpGet("{id}/invoice")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoice(int id)
    {
        InvoiceResponse invoice = await _orderService.GetInvoiceAsync(id);
        return Ok(invoice);
    }
}