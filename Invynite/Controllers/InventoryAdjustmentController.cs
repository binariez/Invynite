using Invynite.Services.Inventories;
using Invynite.Services.Inventories.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryAdjustmentController(IInventoryAdjustmentService service) : ControllerBase
    {
        private readonly IInventoryAdjustmentService service = service;

        [HttpPost]
        public async Task<IActionResult> Adjust([FromBody] CreateInventoryAdjustmentRequest request)
        {
            var result = await service.CreateAdjustmentAsync(request);

            return CreatedAtAction(nameof(Adjust), result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAdjustmentsAsync();

            return Ok(result);
        }
    }
}
