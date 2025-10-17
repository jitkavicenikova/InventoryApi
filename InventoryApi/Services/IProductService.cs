using InventoryApi.DTOs;
using InventoryApi.Entities;

namespace InventoryApi.Services;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
    Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<ProductDto>> GetAllAsync();

    Task<Product> GetEntityByIdOrThrow(int id);
}
