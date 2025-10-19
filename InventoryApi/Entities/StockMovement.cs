using InventoryApi.Enums;

namespace InventoryApi.Entities;

/// <summary>
/// Entity representing a single movement or change in stock quantity for a specific stock entry.
/// </summary>
public class StockMovement
{
    public int Id { get; set; }
    public int QuantityChange { get; set; }
    public MovementType MovementType { get; set; }
    public DateTime Timestamp { get; set; }

    public int StockId { get; set; }
    public required Stock Stock { get; set; }
}
