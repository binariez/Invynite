using Invynite.Services.BOM;
using Invynite.Services.BOM.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillOfMaterialController(IBillOfMaterialService bomService) : ControllerBase
{
    private readonly IBillOfMaterialService _bomService = bomService;

    [HttpGet]
    [Route("{prodId:int}")]
    public async Task<IActionResult> GetMaterialsByProductId(int prodId)
    {
        var result = await _bomService.GetMaterialsByProductIdAsync(prodId);

        return Ok(result);
    }

    [HttpGet]
    [Route("count/pid={prodId:int}&qty={quantity:int}")]
    public async Task<IActionResult> CountMaterialsByProductId(int prodId, int quantity)
    {
        var result = await _bomService.CountMaterialsByProductIdAsync(prodId, quantity);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBillOfMaterial([FromBody] CreateBomRequest request)
    {
        var result = await _bomService.CreateBillOfMaterialAsync(request);

        return CreatedAtAction(nameof(GetMaterialsByProductId), new { prodId = result.ProductId }, result);
    }
}
