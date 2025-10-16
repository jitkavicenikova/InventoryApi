using InventoryApi.DTOs;
using InventoryApi.Entities;

namespace InventoryApi.Mappers;

public static class ProductMapper
{
    public static ProductDto ToDto(Product entity)
    {
        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Currency = entity.Currency,
            Sku = entity.Sku
        };
    }

    public static Product ToEntity(CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Currency = dto.Currency,
            Sku = dto.Sku,
            IsDeleted = false
        };
    }
}