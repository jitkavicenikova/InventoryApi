using InventoryApi.Entities;
using InventoryApi.Enums;

namespace InventoryApi.Services;

public interface IStockMovementService
{
    Task CreateAsync(Stock stock, int quantityChange, MovementType type);
}
