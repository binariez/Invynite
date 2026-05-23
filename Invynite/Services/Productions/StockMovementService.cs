using Invynite.Domain.Entities.Inven;
using Invynite.Infrastructure.Data;
using Invynite.Services.Productions.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Linq.Expressions;

namespace Invynite.Services.Productions
{
    public class StockMovementService(AppDbContext context) : IStockMovementService
    {
        public async Task<IEnumerable<GetStockMovementResponse>> GetStockMovementHistory(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pagesize)
        {
            IQueryable<StockMovement> movementQuery = context.StockMovements
                .Include(m => m.Product)
                .Include(m => m.Material);

            // search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                movementQuery = movementQuery.Where(m => m.Material!.Name.Contains(searchTerm) || 
                                                m.Product!.Name.Contains(searchTerm)
                );
            }

            // sort
            Expression<Func<StockMovement, object>> keySelector = sortColumn?.ToLower() switch
            {
                "type" => movement => movement.MovementType,

                _ => movement => movement.Id
            };

            // order
            if(sortOrder?.ToLower() == "desc")
            {
                movementQuery = movementQuery.OrderByDescending(keySelector);
            }
            else
            {
                movementQuery = movementQuery.OrderBy(keySelector);
            }

            // pagination
            if(page > 0 && pagesize > 0)
            {
                movementQuery = movementQuery
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize);
            }

            // materialize result
            var result = await context.StockMovements
                .OrderByDescending(sm => sm.CreatedAt)
                .ThenBy(sm => sm.MovementType)
                .Select(sm => new GetStockMovementResponse(
                    sm.Id,
                    sm.ProductId != null ? "Product" : "Material",
                    sm.ProductId != null ? sm.Product!.Name : sm.Material!.Name,
                    sm.Quantity,
                    sm.MovementType.GetDisplayName(),
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
