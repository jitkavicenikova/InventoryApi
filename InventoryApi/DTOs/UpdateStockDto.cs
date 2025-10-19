using InventoryApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

public class UpdateStockDto
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "QuantityChange must be 0 or greater.")]
    public int QuantityChange { get; set; }

    [Required]
    public MovementType MovementType { get; set; }
}
