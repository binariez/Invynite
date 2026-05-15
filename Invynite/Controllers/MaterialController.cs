using Invynite.Services.Materials;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialController : ControllerBase
{
    private readonly IMaterialService _materialService;

    public MaterialController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [HttpGet]
    [Route("{prodId:int}")]
    public async Task<IActionResult> GetMaterialsByProductId(int prodId)
    {
        var result = await _materialService.GetMaterialsByProductIdAsync(prodId);

        return Ok(result);
    }

    [HttpGet]
    [Route("count/pid={prodId:int}&qty={quantity:int}")]
    public async Task<IActionResult> CountMaterialsByProductId(int prodId, int quantity)
    {
        var result = await _materialService.CountMaterialsByProductIdAsync(prodId, quantity);

        return Ok(result);
    }
}
