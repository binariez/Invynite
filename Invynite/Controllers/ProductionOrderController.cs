using Invynite.Services.Productions;
using Invynite.Services.Productions.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductionOrderController(IProductionOrderService productionService) : ControllerBase
{
    private readonly IProductionOrderService _productionService = productionService;

    [HttpPost]
    [Route("production-orders")]
    public async Task<IActionResult> CreateProductionOrder([FromBody] CreateProductionOrderRequest request)
    {
        var result = await _productionService.Produce(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductionOrder()
    {
        return Ok(await _productionService.GetProductionOrderHistory());
    }

    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetProductionOrderById(int orderId)
    {
        return Ok(await _productionService.GetProductionOrderById(orderId));
    }
}
