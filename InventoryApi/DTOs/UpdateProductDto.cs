using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

public class UpdateProductDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be 0 or greater.")]
    public required decimal Price { get; set; }

    [Required]
    public required string Currency { get; set; }

    [Required]
    public required string Sku { get; set; }
}
