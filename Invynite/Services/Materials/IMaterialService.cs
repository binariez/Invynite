using Invynite.Services.Materials.DTOs;

namespace Invynite.Services.Materials
{
    public interface IMaterialService
    {
        Task<BillOfMaterialItemResponse?> GetMaterialsByProductIdAsync(int productId);
        Task<ProductMaterialResponse?> CountMaterialsByProductIdAsync(int prodId, int quantity);
    }
}
