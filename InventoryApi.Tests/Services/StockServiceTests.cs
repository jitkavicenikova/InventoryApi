using AutoMapper;
using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Enums;
using InventoryApi.Infrastrucure.Mapping;
using InventoryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace InventoryApi.Tests.Services;

/// <summary>
/// Unit tests for <see cref="StockService"/>.
/// </summary>
public class StockServiceTests
{
    private readonly InventoryDbContext _context;
    private readonly Mock<IStockMovementService> _stockMovementService;
    private readonly StockService _service;
    private readonly Product product;

    public StockServiceTests()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new InventoryDbContext(options);
        _stockMovementService = new Mock<IStockMovementService>();

        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { });
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<MappingProfile>();
        var config = new MapperConfiguration(expression, loggerFactory);
        var mapper = new Mapper(config);
        _service = new StockService(_context,mapper, _stockMovementService.Object);
        product = new Product
        {
            Id = 1,
            Name = "Test",
            Sku = "SKU",
            Price = 1,
            Currency = "USD",
            IsDeleted = false
        };
    }

    [Fact]
    public async Task GetDetailByIdAsync_ShouldReturnDetailDto_WhenStockExists()
    {
        var stock = new Stock { Id = 1, Quantity = 5, Unit = Unit.Piece, IsDeleted = false, Product = product };
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        var result = await _service.GetDetailByIdAsync(1);

        Assert.Equal(5, result.Quantity);
        Assert.Equal(Unit.Piece, result.Unit);
    }

    [Fact]
    public async Task GetDetailByIdAsync_ShouldThrow_WhenStockNotFound()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetDetailByIdAsync(-1));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateStock_WhenProductExists()
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new CreateStockDto { ProductId = 1, Quantity = 10, Unit = Unit.Piece };
        var result = await _service.CreateAsync(dto);

        Assert.Equal(10, result.Quantity);
        Assert.Equal(Unit.Piece, result.Unit);
        _stockMovementService.Verify(m => m.CreateAsync(It.IsAny<Stock>(), 10, MovementType.Initial), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenProductDoesNotExist()
    {
        var dto = new CreateStockDto { ProductId = -1, Quantity = 10, Unit = Unit.Piece };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task UpdateQuantityAsync_ShouldIncreaseQuantity_ForIncomingMovement()
    {
        var stock = new Stock { Id = 1, Quantity = 5, Unit = Unit.Piece, IsDeleted = false, Product = product };
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        var dto = new UpdateStockQuantityDto { QuantityChange = 3, MovementType = MovementType.Incoming };
        var result = await _service.UpdateQuantityAsync(1, dto);

        Assert.Equal(8, result.Quantity);
        _stockMovementService.Verify(m => m.CreateAsync(stock, 3, MovementType.Incoming), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityAsync_ShouldDecreaseQuantity_ForOutgoingMovement()
    {
        var stock = new Stock { Id = 1, Quantity = 10, Unit = Unit.Piece, IsDeleted = false, Product = product };
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        var dto = new UpdateStockQuantityDto { QuantityChange = 4, MovementType = MovementType.Outgoing };
        var result = await _service.UpdateQuantityAsync(1, dto);

        Assert.Equal(6, result.Quantity);
        _stockMovementService.Verify(m => m.CreateAsync(stock, 4, MovementType.Outgoing), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityAsync_ShouldThrow_WhenOutgoingExceedsStock()
    {
        var stock = new Stock { Id = 1, Quantity = 2, Unit = Unit.Piece, IsDeleted = false, Product = product };
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        var dto = new UpdateStockQuantityDto { QuantityChange = 5, MovementType = MovementType.Outgoing };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateQuantityAsync(1, dto));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllNonDeletedStocks()
    {
        _context.Stocks.Add(new Stock { Id = 1, Quantity = 5, Unit = Unit.Piece, IsDeleted = false, Product = product });
        _context.Stocks.Add(new Stock { Id = 2, Quantity = 3, Unit = Unit.Kilogram, IsDeleted = true, Product = product });
        _context.Stocks.Add(new Stock { Id = 3, Quantity = 8, Unit = Unit.Box, IsDeleted = false, Product = product });
        await _context.SaveChangesAsync();

        var result = (await _service.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.Id == 1);
        Assert.Contains(result, s => s.Id == 3);
    }

    [Fact]
    public async Task DeleteByProductIdAsync_ShouldSetIsDeleted_WhenStockExists()
    {
        _context.Stocks.Add(new Stock { Id = 1, ProductId = 1, Quantity = 5, Unit = Unit.Piece, IsDeleted = false, Product = product });
        await _context.SaveChangesAsync();

        await _service.DeleteByProductIdAsync(1);

        var stock = await _context.Stocks.FindAsync(1);
        Assert.NotNull(stock);
        Assert.True(stock.IsDeleted);
    }

    [Fact]
    public async Task DeleteByProductIdAsync_ShouldDoNothing_WhenStockNotFound()
    {
        await _service.DeleteByProductIdAsync(-1);
    }
}
