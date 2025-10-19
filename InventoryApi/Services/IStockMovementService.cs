using InventoryApi.Entities;
using InventoryApi.Enums;

namespace InventoryApi.Services;

/// <summary>
/// Service interface for managing stock movements.
/// Provides functionality to record changes in stock quantity.
/// </summary>
public interface IStockMovementService
{
    /// <summary>
    /// Creates a new stock movement entry for a given stock.
    /// </summary>
    /// <param name="stock">The <see cref="Stock"/> entity for which the movement is recorded.</param>
    /// <param name="quantityChange">The quantity change associated with this movement.</param>
    /// <param name="type">The type of movement.</param>
    Task CreateAsync(Stock stock, int quantityChange, MovementType type);
}
