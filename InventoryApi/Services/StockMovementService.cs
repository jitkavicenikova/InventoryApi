using InventoryApi.Data;
using InventoryApi.Entities;
using InventoryApi.Enums;

namespace InventoryApi.Services;

/// <summary>
/// Service for managing stock movements, implements <see cref="IStockMovementService"/>.
/// </summary>
public class StockMovementService(InventoryDbContext context) : IStockMovementService
{
    public async Task CreateAsync(Stock stock, int quantityChange, MovementType type)
    {
        var movement = new StockMovement
        {
            StockId = stock.Id,
            Stock = stock,
            QuantityChange = quantityChange,
            MovementType = type,
            Timestamp = DateTime.UtcNow
        };

        context.StockMovements.Add(movement);
        await context.SaveChangesAsync();
    }
}
