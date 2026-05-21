using Invynite.Infrastructure.Data;
using Invynite.Middlewares.Exceptions;
using Invynite.Services.BOM.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Services.BOM;

public class BillOfMaterialService : IBillOfMaterialService
{
    private readonly AppDbContext _context;

    public BillOfMaterialService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BillOfMaterialItemResponse> GetMaterialsByProductIdAsync(int productId)
    {
        var materials = await _context.BillOfMaterials
            .Where(b => b.ProductId == productId)
            .Select(b => new BillOfMaterialItemResponse
            (
                b.ProductId,
                b.Product.Name,
                b.Id,
                b.BillOfMaterialItems.Select(i => new Dictionary<string, string>
                {
                    {"Material", i.Material.Name },
                    {"Required", $"{i.QuantityRequired} {i.Material.UnitOfMeasure}" }
                }).ToList()
            )).FirstOrDefaultAsync();

        if (materials == null)
            throw new NotFoundException($"No BOM found with product id: {productId}.");

        return materials;
    }

    public async Task<ProductMaterialResponse> CountMaterialsByProductIdAsync(int prodId, int quantity)
    {
        var result = await _context.BillOfMaterials
            .Where(bom => bom.ProductId == prodId)
            .Select(bom => new
            {
                ProductName = bom.Product.Name,
                Materials = bom.BillOfMaterialItems.Select(items => new
                {
                    items.MaterialId,
                    MaterialName = items.Material.Name,
                    QuantityRequired = items.QuantityRequired * quantity,
                    CurrentStock = _context.Inventories
                        .Where(inv => inv.MaterialId == items.MaterialId)
                        .Select(inv => (decimal?)inv.Quantity)
                        .FirstOrDefault() ?? 0
                })
            })
            .FirstOrDefaultAsync();

        if (result == null)
            throw new NotFoundException($"No BOM found with product id: {prodId}.");

        var requiredMaterials = result.Materials.Select(m => new MaterialComparisonResult(
            m.MaterialId,
            m.MaterialName,
            m.QuantityRequired,
            m.CurrentStock,
            Math.Max(0, m.QuantityRequired - m.CurrentStock),
            m.CurrentStock >= m.QuantityRequired
            )
        ).ToList();

        return new ProductMaterialResponse(
            prodId,
            result.ProductName,
            quantity,
            requiredMaterials
        );
    }
}
