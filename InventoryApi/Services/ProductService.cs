using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _context;

    public ProductService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await GetProductByIdOrThrowAsync(id);

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
        var product = await GetProductByIdOrThrowAsync(id);

        product.Name = updateProductDto.Name;
        product.Price = updateProductDto.Price;
        product.Currency = updateProductDto.Currency;
        product.Sku = updateProductDto.Sku;

        await _context.SaveChangesAsync();

        return ProductMapper.ToDto(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetProductByIdOrThrowAsync(id);

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

    private async Task<Product> GetProductByIdOrThrowAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Product with id {id} not found");
    }
}
