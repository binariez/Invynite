using Invynite.Services.Inventories;
using Invynite.Services.Inventories.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers
{
    [ApiController]
    [Route("api/inventories/[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            return Ok(await _productService.GetAllProductsAsync(searchTerm, sortColumn, sortOrder, page, pageSize));
        }

        [HttpGet]
        [Route("{prodId:int}")]
        public async Task<IActionResult> GetProductById(int prodId)
        {
            if (prodId <= 0) throw new Exception("Id must be positive integer");

            return Ok(await _productService.GetByIdAsync(prodId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
        {
            var result = await _productService.CreateAsync(request);

            return CreatedAtAction(nameof(GetProductById), new { prodId = result.Id }, result);
        }

        [HttpPut]
        [Route("{prodId:int}")]
        public async Task<IActionResult> UpdateProduct(int prodId, [FromBody] ProductRequest request)
        {
            if (prodId <= 0) throw new Exception("Id must be positive integer");

            var result = await _productService.UpdateAsync(prodId, request);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{prodId:int}")]
        public async Task<IActionResult> DeleteProduct(int prodId)
        {
            if (prodId <= 0) throw new Exception("Id must be positive integer");

            await _productService.DeleteAsync(prodId);

            return NoContent();
        }
    }
}
