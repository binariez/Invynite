using Invynite.Services.Inventories;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers;

[ApiController]
[Route("api/inventories/[controller]")]
public class ProductController(IProductlService _productService) : ControllerBase
{
    private readonly IProductlService _productService = _productService;

    [HttpGet]
    [Route("{prodId:int}")]
    public async Task<IActionResult> GetMaterialsByProductId(int prodId)
    {
        var result = await _productService.GetMaterialsByProductIdAsync(prodId);

        return Ok(result);
    }

    [HttpGet]
    [Route("count/pid={prodId:int}&qty={quantity:int}")]
    public async Task<IActionResult> CountMaterialsByProductId(int prodId, int quantity)
    {
        var result = await _productService.CountMaterialsByProductIdAsync(prodId, quantity);

        return Ok(result);
    }
}
