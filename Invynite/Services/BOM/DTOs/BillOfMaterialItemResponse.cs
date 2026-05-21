namespace Invynite.Services.BOM.DTOs;

public record BillOfMaterialItemResponse
(
    int ProductId,
    string ProductName,
    int BOMId,
    ICollection<Dictionary<string, string>> RequiredMaterials
);
