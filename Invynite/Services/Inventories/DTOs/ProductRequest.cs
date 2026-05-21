using System.ComponentModel.DataAnnotations;

namespace Invynite.Services.Inventories.DTOs
{
    public record ProductRequest(
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Name must between 4-50 characters")]
        string Name,
        [RegularExpression(@"^[A-Z]{3}-[0-9]{4}", ErrorMessage = "SKU format must starts with 3 letters followed by dash and 4 numbers. Example: TBL-0001")]
        string SKU,
        [StringLength(25, MinimumLength = 3, ErrorMessage = "UOM must between 3-25 characters")]
        string UnitOfMeasure);
}