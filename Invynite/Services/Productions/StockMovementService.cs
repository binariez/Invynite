using Invynite.Infrastructure.Data;
using Invynite.Services.Productions.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Invynite.Services.Productions
{
    public class StockMovementService(AppDbContext context) : IStockMovementService
    {
        public async Task<List<GetStockMovementResponse>> GetStockMovementHistory()
        {
            var result = await context.StockMovements
                .OrderByDescending(sm => sm.CreatedAt)
                .ThenBy(sm => sm.MovementType)
                .Select(sm => new GetStockMovementResponse(
                    sm.Id,
                    sm.ProductId != null ? "Product" : "Material",
                    sm.ProductId != null ? sm.Product!.Name : sm.Material!.Name,
                    sm.Quantity,
                    sm.MovementType,
                    sm.Warehouse.Name,
                    sm.ReferenceId,
                    sm.CreatedAt
                    )
                )
                .ToListAsync();

            return result;
        }
    }
}
