using Microsoft.AspNetCore.Mvc;
using SalesApp.Domain;
using SalesApp.Services;
using SalesApp.DTOs;

namespace SalesApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<SalesController> _logger;
        private readonly IConfiguration _configuration;

        public SalesController(ILogger<SalesController> logger, ISalesService salesService, IConfiguration configuration)
        {
            _logger = logger;
            _salesService = salesService;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetSales")]
        public async Task<IActionResult> GetSales([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var salesCSVPath = _configuration["SalesCSVPath"];
            var salesData = _salesService.FetchFromCSVFile<Sale, SaleClassMap>(salesCSVPath, pageIndex, pageSize);
            var allRowsCount = _salesService.CountAllRecords<Sale, SaleClassMap>(salesCSVPath);
            return Ok(new SalesDataDTO() { Data = salesData, Count = allRowsCount });
        }

    }
}
