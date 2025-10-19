using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _context;
    private readonly IStockService _stockService;

    public ProductService(InventoryDbContext context, IStockService stockService)
    {
        _context = context;
        _stockService = stockService;
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await GetEntityByIdOrThrow(id);

        return ProductMapper.ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Sku == createProductDto.Sku && !p.IsDeleted);
        if (product != null)
        {
            throw new InvalidOperationException("SKU must be unique");
        }

        var entity = ProductMapper.ToEntity(createProductDto);
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();

        return ProductMapper.ToDto(entity);
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await GetEntityByIdOrThrow(id);

        product.Name = updateProductDto.Name;
        product.Price = updateProductDto.Price;
        product.Currency = updateProductDto.Currency;
        product.Sku = updateProductDto.Sku;

        await _context.SaveChangesAsync();

        return ProductMapper.ToDto(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetEntityByIdOrThrow(id);

        await _stockService.DeleteByProductIdAsync(id);

        product.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return products.Select(ProductMapper.ToDto);
    }

    public async Task<Product> GetEntityByIdOrThrow(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Product with id {id} not found");
    }
}
