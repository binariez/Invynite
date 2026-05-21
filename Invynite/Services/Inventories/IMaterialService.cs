using Invynite.Services.Inventories.DTOs;

namespace Invynite.Services.Inventories
{
    public interface IMaterialService
    {
        Task<SimpleMaterialResponse> CreateAsync(MaterialRequest request);
        Task<SimpleMaterialResponse> UpdateAsync(int matId, MaterialRequest request);
        Task DeleteAsync(int matId);
        Task<IEnumerable<SimpleMaterialResponse>> GetAllProductsAsync();
        Task<MaterialResponse> GetByIdAsync(int matId);
    }
}
