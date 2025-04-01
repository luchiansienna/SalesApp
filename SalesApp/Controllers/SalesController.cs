using Microsoft.AspNetCore.Mvc;
using SalesApp.Services;
using SalesApp.DTOs;
using SalesApp.Services.Contracts;

namespace SalesApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ILogger<SalesController> logger, ISalesService salesService, IConfiguration configuration)
        {
            _salesService = salesService;
        }

        [HttpGet(Name = "GetSales")]
        public async Task<IActionResult> GetSales([FromQuery] SalesFilter filter, CancellationToken cancellationToken = default)
        {
            var salesData = await _salesService.GetSales(filter, cancellationToken);
            var allRowsCount = await _salesService.Count(filter, cancellationToken);
            return Ok(new SalesDataDTO() { Data = salesData, Count = allRowsCount });
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSalesSummary([FromQuery] SalesFilter filter, CancellationToken cancellationToken = default)
        {
            var salesData = await _salesService.GetSales(filter, cancellationToken: cancellationToken);

            var summary = new
            {
                TotalRevenue = salesData.Sum(s => s.UnitsSold * (double)s.SalePrice),
                TotalUnitsSold = salesData.Sum(s => s.UnitsSold),
                TotalManufacturingCost = salesData.Sum(s => s.UnitsSold * (double)s.ManufacturingPrice),
                TotalPerMonth = salesData.GroupBy(s => s.Date.Month)
                                        .Select(g => new { Month = g.Key, Total = g.Sum(s => s.UnitsSold) }),
                TotalProfit = salesData.Sum(s => s.UnitsSold * ((double)s.SalePrice - (double)s.ManufacturingPrice)),
                ProductsSold = salesData.GroupBy(s => s.Product)
                                        .Select(g => new { Product = g.Key, UnitsSold = g.Sum(s => s.UnitsSold) }),
            };

            return Ok(summary);
        }
        [HttpGet("types")]
        public async Task<IActionResult> GetSalesTypes(CancellationToken cancellationToken = default)
        {
            var salesData = await _salesService.GetSales(new SalesFilter(), cancellationToken: cancellationToken);
            var types = new { 
                Segments = salesData.Select(s => s.Segment.Trim()).Where(s => !string.IsNullOrEmpty(s)).Distinct(),
                Countries = salesData.Select(s => s.Country.Trim()).Where(s => !string.IsNullOrEmpty(s)).Distinct(),
                Products = salesData.Select(s => s.Product.Trim()).Where(s => !string.IsNullOrEmpty(s)).Distinct(),
                DiscountBands = salesData.Select(s => s.DiscountBand.Trim()).Where(s => !string.IsNullOrEmpty(s)).Distinct()
            };

            return Ok(types);
        }
    }
}
