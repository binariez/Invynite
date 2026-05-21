using Invynite.Services.Inventories;
using Invynite.Services.Inventories.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers
{
    [ApiController]
    [Route("api/inventories/[controller]")]
    public class MaterialController(IMaterialService materialService) : ControllerBase
    {
        private readonly IMaterialService _materialService = materialService;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _materialService.GetAllProductsAsync());
        }

        [HttpGet]
        [Route("{matId:int}")]
        public async Task<IActionResult> GetProductById(int matId)
        {
            if (matId <= 0) throw new Exception("Id must be positive integer");

            return Ok(await _materialService.GetByIdAsync(matId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] MaterialRequest request)
        {
            var result = await _materialService.CreateAsync(request);

            return CreatedAtAction(nameof(GetProductById), new { matId = result.Id }, result);
        }

        [HttpPut]
        [Route("{matId:int}")]
        public async Task<IActionResult> UpdateProduct(int matId, [FromBody] MaterialRequest request)
        {
            if (matId <= 0) throw new Exception("Id must be positive integer");

            var result = await _materialService.UpdateAsync(matId, request);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{matId:int}")]
        public async Task<IActionResult> DeleteProduct(int matId)
        {
            if (matId <= 0) throw new Exception("Id must be positive integer");

            await _materialService.DeleteAsync(matId);

            return NoContent();
        }

        [HttpGet]
        [Route("low-stock-alert")]
        public async Task<IActionResult> GetLowStockMaterial()
        {
            return Ok(await _materialService.GetLowStockMaterials());
        }
    }
}
