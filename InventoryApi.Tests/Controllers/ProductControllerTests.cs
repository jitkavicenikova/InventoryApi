using InventoryApi.Controllers;
using InventoryApi.DTOs;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryApi.Tests.Controllers;

/// <summary>
/// Unit tests for <see cref="ProductController"/>.
/// Tests cover typical controller actions and expected responses.
/// </summary>
public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductController(_mockService.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenProductExists()
    {
        var product = new ProductDto { Id = 1, Name = "Test", Currency = "CZK", Sku = "SKU", Price = 5 };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithProducts()
    {
        var products = new List<ProductDto> { new() { Id = 1, Name = "Test", Currency = "CZK", Sku = "SKU", Price = 5 } };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithEmptyList()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ProductDto>());

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var list = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Empty(list);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenProductCreated()
    {
        var createDto = new CreateProductDto { Name = "New", Currency = "CZK", Sku = "SKU" };
        var product = new ProductDto { Id = 1, Name = "New", Currency = "CZK", Sku = "SKU", Price = 5 };

        _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(product);

        var result = await _controller.Create(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(product, createdResult.Value);
        Assert.Equal(nameof(ProductController.GetById), createdResult.ActionName);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenProductUpdated()
    {
        var updateDto = new UpdateProductDto { Name = "Updated", Currency = "CZK", Sku = "SKU", Price = 5 };
        var product = new ProductDto { Id = 1, Name = "Updated", Currency = "CZK", Sku = "SKU", Price = 5 };

        _mockService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(product);

        var result = await _controller.Update(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenDeleted()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }
}
