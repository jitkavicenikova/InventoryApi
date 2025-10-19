using InventoryApi.Enums;

namespace InventoryApi.Entities;

/// <summary>
/// Entity representing a stock entry for a specific product in the inventory system.
/// </summary>
public class Stock
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
    public bool IsDeleted { get; set; }

    public int ProductId { get; set; }
    public required Product Product { get; set; }

    public List<StockMovement> StockMovements { get; set; } = [];
}
