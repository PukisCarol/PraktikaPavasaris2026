using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<DiscountReportResponse>> GetDiscountReportAsync();
}
