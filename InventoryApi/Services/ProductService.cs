using AutoMapper;
using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

/// <summary>
/// Service for managing products, implements <see cref="IProductService"/>.
/// </summary>
public class ProductService(InventoryDbContext context, IMapper mapper, IStockService stockService) : IProductService
{
    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await GetEntityByIdOrThrow(id);

        return mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
    {
        CheckSkuUniqueness(createProductDto.Sku);

        var entity = mapper.Map<Product>(createProductDto);
        context.Products.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await GetEntityByIdOrThrow(id);

        if (product.Sku != updateProductDto.Sku)
        {
            CheckSkuUniqueness(updateProductDto.Sku, id);
        }

        product.Name = updateProductDto.Name;
        product.Price = updateProductDto.Price;
        product.Currency = updateProductDto.Currency;
        product.Sku = updateProductDto.Sku;

        await context.SaveChangesAsync();

        return mapper.Map<ProductDto>(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetEntityByIdOrThrow(id);

        await stockService.DeleteByProductIdAsync(id);

        product.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await context.Products.ToListAsync();

        return products.Select(mapper.Map<ProductDto>);
    }

    private async Task<Product> GetEntityByIdOrThrow(int id)
    {
        return await context.Products
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Product with id {id} not found");
    }

    private void CheckSkuUniqueness(string sku, int? excludeId = null)
    {
        if (context.Products.Any(p => p.Sku == sku
            && (!excludeId.HasValue || p.Id != excludeId.Value)))
        {
            throw new InvalidOperationException("SKU must be unique");
        }
    }
}
