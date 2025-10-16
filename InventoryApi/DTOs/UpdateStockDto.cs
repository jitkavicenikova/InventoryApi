using InventoryApi.Enums;

namespace InventoryApi.DTOs;

public class UpdateStockDto
{
    public int QuantityChange { get; set; }
    public MovementType MovementType { get; set; }
}
