using Invynite.Services.Productions.DTOs;

namespace Invynite.Services.Productions;

public interface IStockMovementService
{
    Task<IEnumerable<GetStockMovementResponse>> GetStockMovementHistory();
}
