namespace Invynite.Services.Inventories.DTOs;

public record ProductMaterialResponse(
    int ProductId,
    string ProductName,
    int Quantity,
    ICollection<MaterialComparisonResult> RequiredMaterials);
