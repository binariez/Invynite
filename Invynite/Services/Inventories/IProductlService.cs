using Invynite.Services.Inventories.DTOs;

namespace Invynite.Services.Inventories;

public interface IProductlService
{
    Task<BillOfMaterialItemResponse> GetMaterialsByProductIdAsync(int productId);
    Task<ProductMaterialResponse> CountMaterialsByProductIdAsync(int prodId, int quantity);
}
