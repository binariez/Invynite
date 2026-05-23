using Invynite.Services.Productions;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockMovementController(IStockMovementService stockMovementService) : ControllerBase
{
    private readonly IStockMovementService _stockMovementService = stockMovementService;

    [HttpGet]
    public async Task<IActionResult> GetStockMovementHistory(
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int page,
            [FromQuery] int pagesize)
    {
        return Ok(await _stockMovementService.GetStockMovementHistory
            (searchTerm, sortColumn, sortOrder, page, pagesize));
    }
}
