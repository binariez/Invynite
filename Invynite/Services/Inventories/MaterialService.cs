using Invynite.Domain.Entities.Master;
using Invynite.Infrastructure.Data;
using Invynite.Middlewares.Exceptions;
using Invynite.Services.Inventories.DTOs;
using Invynite.Services.Inventories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Services.Inventories
{
    public class MaterialService(AppDbContext context) : IMaterialService
    {
        private readonly AppDbContext context = context;

        private async Task<decimal> GetCurrentStock(int matId)
        {
            var result = await context.Inventories
                .Where(i => i.MaterialId != null && i.MaterialId == matId)
                .GroupBy(i => i.MaterialId)
                .Select(g => new
                {
                    Quantity = g.Sum(i => i.Quantity)
                })
                .FirstOrDefaultAsync();

            if (result == null) return 0m;

            return result.Quantity;

        }

        public async Task<SimpleMaterialResponse> CreateAsync(MaterialRequest request)
        {
            var exists = context.Products.Any(p => p.Name.ToLower() == request.Name.ToLower());

            if (exists)
                throw new SameNameException("Material with the same name already exists");

            var entity = new Material
            {
                Name = request.Name,
                UnitOfMeasure = request.UnitOfMeasure
            };

            var result = await context.Materials.AddAsync(entity);

            await context.SaveChangesAsync();

            return new SimpleMaterialResponse(
                entity.Id,
                entity.Name,
                0,
                entity.UnitOfMeasure
                );
        }

        public async Task DeleteAsync(int matId)
        {
            decimal currentStock = await GetCurrentStock(matId);

            if (currentStock != 0)
                throw new StockNotZeroException("Can't delete when the current stock is not zero");

            var exists = await context.Materials.Where(p => p.Id == matId).ExecuteDeleteAsync();

            if (exists <= 1)
                throw new NotFoundException($"Material with id: {matId} does not exist");
        }

        public async Task<IEnumerable<SimpleMaterialResponse>> GetAllProductsAsync()
        {
            var materials = await context.Materials.ToListAsync();

            var ids = materials.Select(m => m.Id).ToList();

            var stockDictionary = await context.Inventories
                .Where(i => i.MaterialId != null && ids.Contains(i.MaterialId.Value))
                .GroupBy(i => i.MaterialId!.Value)
                .Select(g => new
                {
                    MaterialId = g.Key,
                    Quantity = g.Sum(i => i.Quantity)
                })
                .ToDictionaryAsync(i => i.MaterialId, i => i.Quantity);

            return materials.Select(r => new SimpleMaterialResponse(
                r.Id,
                r.Name,
                stockDictionary.GetValueOrDefault(r.Id, 0),
                r.UnitOfMeasure
                )
            );
        }

        public async Task<MaterialResponse> GetByIdAsync(int matId)
        {
            var result = await context.Materials.FindAsync(matId);

            if (result == null)
                throw new NotFoundException($"Material with id: {matId} does not exist");

            decimal currentStock = await GetCurrentStock(matId);

            IEnumerable<string> warehouses = await context.Inventories
                .Include(i => i.Warehouse)
                .Where(i => i.MaterialId != null && i.MaterialId == matId && i.Quantity != 0)
                .Select(i => i.Warehouse.Name)
                .ToListAsync();

            return new MaterialResponse(
                result.Id,
                result.Name,
                currentStock,
                result.UnitOfMeasure,
                warehouses
                );
        }

        public async Task<SimpleMaterialResponse> UpdateAsync(int matId, MaterialRequest request)
        {
            var existing = await context.Materials.FindAsync(matId);

            if (existing == null)
                throw new NotFoundException($"Material with id: {matId} does not exist");

            existing.Name = request.Name;
            existing.UnitOfMeasure = request.UnitOfMeasure;

            await context.SaveChangesAsync();

            decimal currentStock = await GetCurrentStock(matId);

            return new SimpleMaterialResponse(
                existing.Id,
                existing.Name,
                currentStock,
                existing.UnitOfMeasure
                );
        }

        public async Task<IEnumerable<LowStockDto>> GetLowStockMaterials()
        {
            var result = await context.Inventories
                .Where(i => i.MaterialId != null && i.Quantity <= 10)
                .GroupBy(i => new
                {
                    i.MaterialId,
                    i.Material!.Name
                })
                .Select(g => new LowStockDto(
                    g.Key.MaterialId!.Value,
                    g.Key.Name,
                    g.Select(i => new LowStockDetail(
                        i.Warehouse.Name,
                        i.Quantity
                    ))
                    .ToList()
                ))
                .ToListAsync();

            return result;
        }
    }
}
