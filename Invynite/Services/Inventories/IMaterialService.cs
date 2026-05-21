using Invynite.Services.Inventories.DTOs;

namespace Invynite.Services.Inventories
{
    public interface IMaterialService
    {
        Task<SimpleMaterialResponse> CreateAsync(MaterialRequest request);
        Task<SimpleMaterialResponse> UpdateAsync(int matId, MaterialRequest request);
        Task DeleteAsync(int matId);
        Task<IEnumerable<SimpleMaterialResponse>> GetAllProductsAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize);
        Task<MaterialResponse> GetByIdAsync(int matId);
        Task<IEnumerable<LowStockDto>> GetLowStockMaterials();
    }
}
