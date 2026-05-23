using Invynite.Services.BOM.DTOs;

namespace Invynite.Services.BOM;

public interface IBillOfMaterialService
{
    Task<BillOfMaterialItemResponse> GetMaterialsByProductIdAsync(int productId);
    Task<ProductMaterialResponse> CountMaterialsByProductIdAsync(int prodId, int quantity);
    Task<BillOfMaterialItemResponse> CreateBillOfMaterialAsync(CreateBomRequest request);
    Task<BillOfMaterialItemResponse> UpdateBillOfMaterialsAsync(int prodId, UpdateBomRequest request);
}