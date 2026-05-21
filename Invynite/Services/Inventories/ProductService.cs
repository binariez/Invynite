using Invynite.Domain.Entities.Master;
using Invynite.Infrastructure.Data;
using Invynite.Middlewares.Exceptions;
using Invynite.Services.Inventories.DTOs;
using Invynite.Services.Inventories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Services.Inventories
{
    public class ProductService(AppDbContext context) : IProductService
    {
        private readonly AppDbContext context = context;

        private async Task<decimal> GetCurrentStock(int prodId)
        {
            var result = await context.Inventories
                .Where(i => i.ProductId != null && i.ProductId == prodId)
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    Quantity = g.Sum(i => i.Quantity)
                })
                .FirstOrDefaultAsync();

            if (result == null) return 0m;

            return result.Quantity;
        }

        public async Task<SimpleProductResponse> CreateAsync(ProductRequest request)
        {
            var exists = context.Products.Any(p => p.Name.ToLower() == request.Name.ToLower());

            if (exists)
                throw new SameNameException("Product with the same name already exists");

            var entity = new Product
            {
                Name = request.Name,
                SKU = request.SKU,
                UnitOfMeasure = request.UnitOfMeasure
            };

            var result = await context.Products.AddAsync(entity);

            await context.SaveChangesAsync();

            return new SimpleProductResponse(
                entity.Id,
                entity.Name,
                entity.SKU,
                0,
                entity.UnitOfMeasure
            );
        }

        public async Task DeleteAsync(int prodId)
        {
            decimal currentStock = await GetCurrentStock(prodId);

            if (currentStock != 0)
                throw new StockNotZeroException("Can't delete when the current stock is not zero");

            var exists = await context.Products.Where(p => p.Id == prodId).ExecuteDeleteAsync();

            if (exists <= 1)
                throw new NotFoundException($"Product with id: {prodId} does not exist");
        }

        public async Task<IEnumerable<SimpleProductResponse>> GetAllProductsAsync()
        {
            var products = await context.Products.ToListAsync();

            var ids = products.Select(r => r.Id).ToList();

            var stockDictionary = await context.Inventories
                .Where(i => i.ProductId != null && ids.Contains(i.ProductId.Value))
                .GroupBy(i => i.ProductId!.Value)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(i => i.Quantity)
                })
                .ToDictionaryAsync(x => x.ProductId, x => x.Quantity);

            return products.Select(r => new SimpleProductResponse(
                r.Id,
                r.Name,
                r.SKU,
                stockDictionary.GetValueOrDefault(r.Id, 0),
                r.UnitOfMeasure
                )
            );
        }

        public async Task<ProductResponse> GetByIdAsync(int prodId)
        {
            var result = await context.Products.FindAsync(prodId);

            if(result == null)
                throw new NotFoundException($"Product with id: {prodId} does not exist");

            decimal currentStock = await GetCurrentStock(prodId);

            var warehouses = await context.Inventories
                .Include(i => i.Warehouse)
                .Where(i => i.ProductId != null && i.ProductId == prodId && i.Quantity != 0)
                .ToDictionaryAsync(i => i.Warehouse.Name, i => i.Quantity);

            return new ProductResponse(
                result.Id,
                result.Name,
                result.SKU,
                currentStock,
                result.UnitOfMeasure,
                warehouses
            );
        }

        public async Task<SimpleProductResponse> UpdateAsync(int prodId, ProductRequest request)
        {
            var existing = await context.Products.FindAsync(prodId);

            if(existing == null)
                throw new NotFoundException($"Product with id: {prodId} does not exist");

            existing.Name = request.Name;
            existing.SKU = request.SKU;
            existing.UnitOfMeasure = request.UnitOfMeasure;

            await context.SaveChangesAsync();

            decimal currentStock = await GetCurrentStock(prodId);

            return new SimpleProductResponse(
                existing.Id,
                existing.Name,
                existing.SKU,
                currentStock,
                existing.UnitOfMeasure
            );
        }
    }
}
