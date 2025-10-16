using InventoryApi.Enums;

namespace InventoryApi.DTOs;

public class StockDetailDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Unit Unit { get; set; }
    public List<StockMovementDto> Movements { get; set; } = [];
}
