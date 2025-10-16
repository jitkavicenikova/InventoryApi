using InventoryApi.Enums;

namespace InventoryApi.DTOs;

public class StockMovementDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public MovementType MovementType { get; set; }
    public DateTime Timestamp { get; set; }
}
