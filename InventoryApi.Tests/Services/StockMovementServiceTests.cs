using InventoryApi.Data;
using InventoryApi.Entities;
using InventoryApi.Enums;
using InventoryApi.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Tests.Services;

/// <summary>
/// Unit tests for <see cref="StockMovementService"/>.
/// </summary>
public class StockMovementServiceTests
{
    private readonly InventoryDbContext _context;
    private readonly StockMovementService _service;

    public StockMovementServiceTests()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new InventoryDbContext(options);
        _service = new StockMovementService(_context);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddStockMovement()
    {
        var stock = new Stock
        {
            Id = 1,
            Quantity = 10,
            Unit = Unit.Piece,
            IsDeleted = false,
            Product = new Product { Name = "Test", Sku = "SKU", Price = 10, Currency = "USD", IsDeleted = false }
        };
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        await _service.CreateAsync(stock, 5, MovementType.Incoming);

        var movement = _context.StockMovements.FirstOrDefault();
        Assert.NotNull(movement);
        Assert.Equal(stock.Id, movement.StockId);
        Assert.Equal(5, movement.QuantityChange);
        Assert.Equal(MovementType.Incoming, movement.MovementType);
        Assert.Equal(stock, movement.Stock);
        Assert.True((DateTime.UtcNow - movement.Timestamp).TotalSeconds < 5);
    }
}
