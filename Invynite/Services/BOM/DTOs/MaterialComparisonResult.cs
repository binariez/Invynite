namespace Invynite.Services.BOM.DTOs;

public record MaterialComparisonResult(
        int MaterialId,
        string MaterialName,
        decimal Required,
        decimal InStock,
        decimal Shortage,
        bool IsSufficient);
