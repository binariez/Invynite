using Invynite.Services.Productions.DTOs;

namespace Invynite.Services.Productions;

public interface IProductionOrderService
{
    Task<CreateProductionOrderResponse> Produce(CreateProductionOrderRequest request);
    Task<IEnumerable<GetProductionOrderResponse>> GetProductionOrderHistory();
    Task<GetProductionOrderResponse?> GetProductionOrderById(int orderId);
}
