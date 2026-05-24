using Invynite.Domain.Entities.BOM;
using Invynite.Domain.Entities.Master;
using Invynite.Infrastructure.Data;
using Invynite.Middlewares.Exceptions;
using Invynite.Services.BOM.DTOs;
using Invynite.Services.Inventories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Services.BOM;

public class BillOfMaterialService(AppDbContext context) : IBillOfMaterialService
{
    private readonly AppDbContext context = context;

    public async Task<BillOfMaterialItemResponse> GetMaterialsByProductIdAsync(int productId)
    {
        var materials = await context.BillOfMaterials
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
        var result = await context.BillOfMaterials
            .Where(bom => bom.ProductId == prodId)
            .Select(bom => new
            {
                ProductName = bom.Product.Name,
                Materials = bom.BillOfMaterialItems.Select(items => new
                {
                    items.MaterialId,
                    MaterialName = items.Material.Name,
                    QuantityRequired = items.QuantityRequired * quantity,
                    CurrentStock = context.Inventories
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

    public async Task<BillOfMaterialItemResponse> CreateBillOfMaterialAsync(CreateBomRequest request)
    {
        if (context.BillOfMaterials.Any(b => b.ProductId == request.ProductId))
            throw new SameNameException("Product already has a bill of material");

        var bom = new BillOfMaterial { ProductId = request.ProductId };

        var bomItems = new List<BillOfMaterialItem>();

        foreach (var material in request.Recipes)
        {
            bomItems.Add(new BillOfMaterialItem
            {
                MaterialId = material.Key,
                QuantityRequired = material.Value,
                Material = null!
            });
        }

        bom.BillOfMaterialItems = bomItems;

        await context.BillOfMaterials.AddAsync(bom);

        await context.SaveChangesAsync();

        return await GetMaterialsByProductIdAsync(request.ProductId);
    }

    public async Task<BillOfMaterialItemResponse> UpdateBillOfMaterialsAsync(int prodId, UpdateBomRequest request)
    {
        var current = await context.BillOfMaterials
            .Include(b => b.BillOfMaterialItems)
            .FirstOrDefaultAsync(b => b.ProductId == prodId);

        if (current == null)
            throw new NotFoundException($"No BOM found with product id: {prodId}.");
        var materials = new List<BillOfMaterialItem>();

        foreach (var material in request.Recipes)
        {
            materials.Add(new BillOfMaterialItem
            {
                MaterialId = material.Key,
                QuantityRequired = material.Value,
                Material = null!
            });
        }

        current.BillOfMaterialItems = materials;

        await context.SaveChangesAsync();

        return await GetMaterialsByProductIdAsync (prodId);
    }

    public async Task DeleteBillOfMaterialAsync(int prodId)
    {
        try
        {
            await context.BillOfMaterials
            .Where(b => b.ProductId == prodId)
            .ExecuteDeleteAsync();
        }
        catch
        {
            throw new NotFoundException($"No BOM found with product id: {prodId}.");
        }
    }
}
