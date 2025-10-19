using InventoryApi.Enums;

namespace InventoryApi.DTOs;

/// <summary>
/// Data transfer object representing a stock item.
/// </summary>
public class StockDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
}
