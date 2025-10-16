using InventoryApi.DTOs;
using InventoryApi.Enums;

namespace InventoryApi.Services;

public interface IStockMovementService
{
    Task CreateAsync(int stockId, int quantityChange, MovementType type);
}
