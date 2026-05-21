using System.ComponentModel.DataAnnotations;

namespace Invynite.Services.Inventories.DTOs
{
    public record MaterialRequest(
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Name must between 4-50 characters")]
        string Name,
        [StringLength(25, MinimumLength = 3, ErrorMessage = "UOM must between 3-25 characters")]
        string UnitOfMeasure);
}
