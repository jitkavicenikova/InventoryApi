using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IStockService
{
    Task<StockDetailDto> GetDetailByIdAsync(string id);
    Task<StockDto> CreateAsync(CreateStockDto product);
    Task<bool> UpdateQuantityAsync(int stockId, UpdateStockDto update);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<StockDto>> GetAllAsync();
}
