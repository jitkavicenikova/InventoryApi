using InventoryApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

/// <summary>
/// Data transfer object used to update the quantity of a stock item.
/// Contains the quantity change and the type of movement.
/// </summary>
public class UpdateStockDto
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "QuantityChange must be 0 or greater.")]
    public int QuantityChange { get; set; }

    [Required]
    public MovementType MovementType { get; set; }
}
