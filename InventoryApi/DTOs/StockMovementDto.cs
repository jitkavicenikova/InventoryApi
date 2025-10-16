using InventoryApi.Enums;

namespace InventoryApi.DTOs;

public class StockMovementDto
{
    public int Id { get; set; }
    public int QuantityChange { get; set; }
    public MovementType MovementType { get; set; }
    public DateTime Timestamp { get; set; }
}
