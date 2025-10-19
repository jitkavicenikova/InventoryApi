using InventoryApi.Controllers;
using InventoryApi.DTOs;
using InventoryApi.Enums;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryApi.Tests.Controllers;

/// <summary>
/// Unit tests for <see cref="StockController"/>.
/// Tests cover typical controller actions and expected responses.
/// </summary>
public class StockControllerTests
{
    private readonly Mock<IStockService> _mockService;
    private readonly StockController _controller;

    public StockControllerTests()
    {
        _mockService = new Mock<IStockService>();
        _controller = new StockController(_mockService.Object);
    }

    [Fact]
    public async Task GetDetailById_ShouldReturnOk_WhenStockExists()
    {
        var stock = new StockDetailDto { Id = 1, Quantity = 10 };
        _mockService.Setup(s => s.GetDetailByIdAsync(1)).ReturnsAsync(stock);

        var result = await _controller.GetDetailById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(stock, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithStocks()
    {
        var stocks = new List<StockDto> { new() { Id = 1, Quantity = 10 } };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(stocks);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(stocks, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithEmptyList()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var enumerable = Assert.IsAssignableFrom<IEnumerable<StockDto>>(okResult.Value);
        Assert.Empty(enumerable);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenStockCreated()
    {
        var createDto = new CreateStockDto { ProductId = 1, Quantity = 10, Unit = Unit.Piece };
        var stock = new StockDto { Id = 1, ProductId = 1, Quantity = 10 };

        _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(stock);

        var result = await _controller.Create(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(stock, createdResult.Value);
        Assert.Equal(nameof(StockController.GetDetailById), createdResult.ActionName);
    }

    [Fact]
    public async Task UpdateQuantity_ShouldReturnOk_WhenStockUpdated()
    {
        var updateDto = new UpdateStockQuantityDto { QuantityChange = 15, MovementType = Enums.MovementType.Incoming };
        var stock = new StockDto { Id = 1, ProductId = 1, Quantity = 15 };

        _mockService.Setup(s => s.UpdateQuantityAsync(1, updateDto)).ReturnsAsync(stock);

        var result = await _controller.UpdateQuantity(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(stock, okResult.Value);
    }
}