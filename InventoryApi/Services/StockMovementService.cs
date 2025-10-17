using InventoryApi.Data;
using InventoryApi.Entities;
using InventoryApi.Enums;

namespace InventoryApi.Services;

public class StockMovementService : IStockMovementService
{
    private readonly InventoryDbContext _context;

    public StockMovementService(InventoryDbContext context)
    {
        _context = context;
    }

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

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();
    }
}
