using InventoryApi.Enums;

namespace InventoryApi.Entities;

public class StockMovement
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public MovementType MovementType { get; set; }
    public DateTime Timestamp { get; set; }

    public int StockId { get; set; }
    public Stock Stock { get; set; } = null!;
}
