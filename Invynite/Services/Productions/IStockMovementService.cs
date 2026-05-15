using Invynite.Services.Productions.DTOs;

namespace Invynite.Services.Productions;

public interface IStockMovementService
{
    Task<List<GetStockMovementResponse>> GetStockMovementHistory();
}
