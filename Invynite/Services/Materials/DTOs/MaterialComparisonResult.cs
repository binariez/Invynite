namespace Invynite.Services.Materials.DTOs;

public record MaterialComparisonResult(
        int MaterialId,
        string MaterialName,
        decimal Required,
        decimal InStock,
        decimal Shortage,
        bool IsSufficient);
