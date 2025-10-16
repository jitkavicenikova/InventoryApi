using InventoryApi.Data;
using InventoryApi.Entities;
using InventoryApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

public class StockMovementService : IStockMovementService
{
    private readonly InventoryDbContext _context;

    public StockMovementService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(int stockId, int quantityChange, MovementType type)
    {
        var stock = await _context.Stocks
            .Include(s => s.StockMovements)
            .FirstOrDefaultAsync(s => s.Id == stockId)
            ?? throw new KeyNotFoundException($"Stock with ID {stockId} not found.");

        UpdateStockQuantity(stock, quantityChange, type);

        var movement = new StockMovement
        {
            StockId = stockId,
            Stock = stock,
            QuantityChange = quantityChange,
            MovementType = type,
            Timestamp = DateTime.UtcNow
        };

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();
    }

    private static void UpdateStockQuantity(Stock stock, int quantityChange, MovementType type)
    {
        if (type == MovementType.Incoming)
        {
            stock.Quantity += quantityChange;
        }
        else if (type == MovementType.Outgoing)
        {
            if (stock.Quantity < quantityChange)
                throw new InvalidOperationException("Not enough items in stock for outgoing movement.");

            stock.Quantity -= quantityChange;
        }
    }
}
