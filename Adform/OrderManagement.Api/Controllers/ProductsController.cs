using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services.Interfaces;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="request">Product name and price</param>
    /// <returns>Created product</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        ProductResponse product = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
    }

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <param name="name">Optional filter by product name</param>
    /// <returns>List of products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? name)
    {
        IEnumerable<ProductResponse> products = await _productService.GetAllAsync(name);
        return Ok(products);
    }

    /// <summary>
    /// Applies discount to a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="request">Discount percentage and minimum quantity</param>
    /// <returns>No content</returns>
    [HttpPost("{id}/discount")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApplyDiscount(int id, [FromBody] DiscountRequest request)
    {
        await _productService.ApplyDiscountAsync(id, request);
        return NoContent();
    }
}