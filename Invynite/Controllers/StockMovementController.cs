using Invynite.Services.Productions;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockMovementController(IStockMovementService stockMovementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStockMovementHistory()
    {
        return Ok(await stockMovementService.GetStockMovementHistory());
    }
}
