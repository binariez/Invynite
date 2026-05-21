using Invynite.Services.Inventories.DTOs;

namespace Invynite.Services.Inventories
{
    public interface IProductService
    {
        Task<SimpleProductResponse> CreateAsync(ProductRequest request);
        Task<SimpleProductResponse> UpdateAsync(int prodId, ProductRequest request);
        Task DeleteAsync(int prodId);
        Task<IEnumerable<SimpleProductResponse>> GetAllProductsAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize);
        Task<ProductResponse> GetByIdAsync(int prodId);
    }
}