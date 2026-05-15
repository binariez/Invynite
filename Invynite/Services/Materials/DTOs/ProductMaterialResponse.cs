namespace Invynite.Services.Materials.DTOs;

public record ProductMaterialResponse(
    int ProductId,
    string ProductName,
    int Quantity,
    ICollection<MaterialComparisonResult> RequiredMaterials);
