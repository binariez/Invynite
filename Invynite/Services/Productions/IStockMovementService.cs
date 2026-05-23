using Invynite.Services.Productions.DTOs;

namespace Invynite.Services.Productions;

public interface IStockMovementService
{
    Task<IEnumerable<GetStockMovementResponse>> GetStockMovementHistory(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int page,
        int pagesize);
}
