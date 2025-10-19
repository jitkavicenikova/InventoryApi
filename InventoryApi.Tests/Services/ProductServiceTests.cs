using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InventoryApi.Tests.Services;

public class ProductServiceTests
{
    private readonly InventoryDbContext _dbContext;
    private readonly Mock<IStockService> _stockServiceMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new InventoryDbContext(options);
        _stockServiceMock = new Mock<IStockService>();
        _productService = new ProductService(_dbContext, _stockServiceMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
    {
        var product = new Product { Name = "Test", Sku = "SKU", Price = 10, Currency = "USD", IsDeleted = false };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var result = await _productService.GetByIdAsync(product.Id);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenNotExists()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _productService.GetByIdAsync(-1)
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateNewProduct_WhenSkuIsUnique()
    {
        var dto = new CreateProductDto
        {
            Name = "Test",
            Sku = "UNIQUE",
            Price = 10,
            Currency = "USD"
        };

        var created = await _productService.CreateAsync(dto);

        Assert.NotNull(created);
        Assert.Equal("UNIQUE", created.Sku);

        var productInDb = await _dbContext.Products.FirstOrDefaultAsync(p => p.Sku == "UNIQUE");
        Assert.NotNull(productInDb);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateNewProduct_WhenSkuExistsInDeletedProduct()
    {
        var existing = new Product { Name = "Old", Sku = "DUPLICATE", Price = 5, Currency = "USD", IsDeleted = true };
        _dbContext.Products.Add(existing);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateProductDto { Name = "New", Sku = "DUPLICATE", Price = 8, Currency = "USD" };

        var created = await _productService.CreateAsync(dto);

        Assert.NotNull(created);
        Assert.Equal("DUPLICATE", created.Sku);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenSkuExists()
    {
        var existing = new Product { Name = "Old", Sku = "DUPLICATE", Price = 5, Currency = "USD", IsDeleted = false };
        _dbContext.Products.Add(existing);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateProductDto { Name = "New", Sku = "DUPLICATE", Price = 8, Currency = "USD" };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _productService.CreateAsync(dto)
        );
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct()
    {
        var product = new Product { Name = "Old", Sku = "SKU", Price = 5, Currency = "USD", IsDeleted = false };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var update = new UpdateProductDto { Name = "Updated", Sku = "SKU2", Price = 9, Currency = "EUR" };

        var result = await _productService.UpdateAsync(product.Id, update);

        Assert.Equal("Updated", result.Name);
        var fromDb = await _dbContext.Products.FindAsync(product.Id);
        Assert.NotNull(fromDb);
        Assert.Equal("EUR", fromDb.Currency);
        Assert.Equal("SKU2", fromDb.Sku);
        Assert.Equal(9, fromDb.Price);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenSkuExists()
    {
        var productToUpdate = new Product { Name = "Old", Sku = "SKU", Price = 5, Currency = "USD", IsDeleted = false };
        var productSku = new Product { Name = "Exists", Sku = "SKU2", Price = 5, Currency = "USD", IsDeleted = false };
        _dbContext.Products.Add(productToUpdate);
        _dbContext.Products.Add(productSku);
        await _dbContext.SaveChangesAsync();

        var update = new UpdateProductDto { Name = "Updated", Sku = "SKU2", Price = 9, Currency = "EUR" };

        await Assert.ThrowsAsync<InvalidOperationException>(
           () => _productService.UpdateAsync(productToUpdate.Id, update)
       );
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenNotExists()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _productService.GetByIdAsync(-1)
        );
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsDeleted_AndCallStockService()
    {
        var product = new Product { Name = "ToDelete", Sku = "DEL", Price = 1, Currency = "USD", IsDeleted = false };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        await _productService.DeleteAsync(product.Id);

        var deleted = await _dbContext.Products.FindAsync(product.Id);
        Assert.NotNull(deleted);
        Assert.True(deleted.IsDeleted);
        _stockServiceMock.Verify(s => s.DeleteByProductIdAsync(product.Id), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyNotDeleted()
    {
        _dbContext.Products.Add(new Product { Name = "A", Sku = "1", Price = 1, Currency = "USD", IsDeleted = false });
        _dbContext.Products.Add(new Product { Name = "B", Sku = "2", Price = 1, Currency = "USD", IsDeleted = true });
        await _dbContext.SaveChangesAsync();

        var result = await _productService.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("A", result.First().Name);
    }
}
