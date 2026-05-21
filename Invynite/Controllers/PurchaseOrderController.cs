using Invynite.Services.Procurement;
using Invynite.Services.Procurement.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Invynite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderController(IPurchaseOrderService purchaseOrderService) : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService = purchaseOrderService;

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequest orderRequest)
        {
            var result = await _purchaseOrderService.CreatePurchaseOrder(orderRequest);

            return Ok(result);
        }

        [HttpPost("receive/pid={purchaseOrderId:int}&wid={warehouseId:int}")]
        public async Task<IActionResult> ReceivePurchaseOrder(int purchaseOrderId, int warehouseId)
        {
            var result = await _purchaseOrderService.ReceivePurchaseOrder(purchaseOrderId, warehouseId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrderHistory()
        {
            var result = await _purchaseOrderService.GetPurchaseOrderHistory();

            return Ok(result);
        }
    }
}
