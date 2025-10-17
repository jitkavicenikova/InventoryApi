using InventoryApi.Enums;

namespace InventoryApi.Entities;

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
