using Invynite.Domain.Entities.Purchasing;
using Invynite.Services.Procurement.DTOs;

namespace Invynite.Services.Procurement
{
    public interface IPurchaseOrderService
    {
        Task<CreatePurchaseOrderResponse> CreatePurchaseOrder(CreatePurchaseOrderRequest purchaseOrderRequest);

        //Task<ReceivePurchaseOrderResponse> ReceivePurchaseOrder(int purchaseOrderId, int warehouseId);

        Task<ReceivePurchaseOrderResponse> ReceivePurchaseOrder(int purchaseOrderId, int warehouseId);

        Task <IEnumerable<GetPurchaseOrderResponse>> GetPurchaseOrderHistory();
    }
}
