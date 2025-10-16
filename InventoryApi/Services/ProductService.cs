using InventoryApi.Data;
using InventoryApi.DTOs;
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

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        return product == null ? null : ProductMapper.ToDto(product);
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

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto updateProductDto)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (entity == null)
        {
            return null;
        }

        entity.Name = updateProductDto.Name;
        entity.Price = updateProductDto.Price;
        entity.Currency = updateProductDto.Currency;
        entity.Sku = updateProductDto.Sku;

        await _context.SaveChangesAsync();

        return ProductMapper.ToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return products.Select(ProductMapper.ToDto);
    }
}
