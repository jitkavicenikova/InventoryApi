using InventoryApi.Enums;

namespace InventoryApi.DTOs;

public class CreateStockDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
}
