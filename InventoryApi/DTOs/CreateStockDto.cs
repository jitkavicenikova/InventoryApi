using InventoryApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

/// <summary>
/// Data transfer object used to create a new stock item.
/// Contains all required fields for stock creation.
/// </summary>
public class CreateStockDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")]
    public int Quantity { get; set; }

    [Required]
    public Unit Unit { get; set; }
}
