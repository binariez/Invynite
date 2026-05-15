using Invynite.Services.Productions.DTOs;

namespace Invynite.Services.Productions;

public interface IProductionOrderService
{
    Task<CreateProductionOrderResponse> Produce(CreateProductionOrderRequest request);
    Task<List<GetProductionOrderResponse>> GetProductionOrderHistory();
    Task<GetProductionOrderResponse?> GetProductionOrderById(int orderId);
}
