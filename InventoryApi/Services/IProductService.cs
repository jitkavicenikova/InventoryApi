using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto product);
    Task<bool> UpdateAsync(int id, UpdateProductDto product);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProductDto>> GetAllAsync();
}
