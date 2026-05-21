namespace Invynite.Services.BOM.DTOs;

public record ProductMaterialResponse(
    int ProductId,
    string ProductName,
    int Quantity,
    ICollection<MaterialComparisonResult> RequiredMaterials);
