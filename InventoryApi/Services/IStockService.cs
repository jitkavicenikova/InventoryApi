using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IStockService
{
    Task<StockDetailDto> GetDetailByIdAsync(int id);
    Task<StockDto> CreateAsync(CreateStockDto product);
    Task<StockDto> UpdateQuantityAsync(int id, UpdateStockDto update);
    Task<IEnumerable<StockDto>> GetAllAsync();

    Task DeleteByProductIdAsync(int productId);
}
