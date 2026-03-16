using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services.Interfaces;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Retrieves discount report for all discounted products
    /// </summary>
    /// <remarks>
    /// Returns only products where discount was actually applied (quantity >= minimum quantity)
    /// </remarks>
    /// <returns>List of discounted products with order count and total amount</returns>
    [HttpGet("discounts")]
    [ProducesResponseType(typeof(IEnumerable<DiscountReportResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDiscountReport()
    {
        IEnumerable<DiscountReportResponse> report = await _reportService.GetDiscountReportAsync();
        return Ok(report);
    }
}