using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto updateProductDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProductDto>> GetAllAsync();
}
