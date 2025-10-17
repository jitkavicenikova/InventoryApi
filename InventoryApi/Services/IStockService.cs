using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IStockService
{
    Task<StockDetailDto> GetDetailByIdAsync(int id);
    Task<StockDto> CreateAsync(CreateStockDto product);
    Task UpdateQuantityAsync(int id, UpdateStockDto update);
    Task DeleteAsync(int id);
    Task<IEnumerable<StockDto>> GetAllAsync();
}
