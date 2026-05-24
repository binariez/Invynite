using Invynite.Services.Inventories.DTOs;

namespace Invynite.Services.Inventories;

public interface IInventoryAdjustmentService
{
    Task<CreateInventoryAdjustmentResponse> CreateAdjustmentAsync(CreateInventoryAdjustmentRequest request);
    Task<IEnumerable<CreateInventoryAdjustmentResponse>> GetAllAdjustmentsAsync();
}