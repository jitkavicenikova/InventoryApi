using InventoryApi.DTOs;

namespace InventoryApi.Services;

/// <summary>
/// Service interface for managing stock entities.
/// </summary>
public interface IStockService
{
    /// <summary>
    /// Retrieves detailed information for a stock by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the stock.</param>
    /// <returns>The <see cref="StockDetailDto"/> corresponding to the specified id.</returns>
    Task<StockDetailDto> GetDetailByIdAsync(int id);

    /// <summary>
    /// Creates a new stock record.
    /// </summary>
    /// <param name="product">The <see cref="CreateStockDto"/> containing stock creation data.</param>
    /// <returns>The created <see cref="StockDto"/>.</returns>
    Task<StockDto> CreateAsync(CreateStockDto product);

    /// <summary>
    /// Updates the quantity of an existing stock record.
    /// </summary>
    /// <param name="id">The identifier of the stock to update.</param>
    /// <param name="update">The <see cref="UpdateStockDto"/> containing the quantity change information.</param>
    /// <returns>The updated <see cref="StockDto"/>.</returns>
    Task<StockDto> UpdateQuantityAsync(int id, UpdateStockDto update);

    /// <summary>
    /// Retrieves all stock records that are not deleted.
    /// </summary>
    /// <returns>A collection of <see cref="StockDto"/> representing all stocks.</returns>
    Task<IEnumerable<StockDto>> GetAllAsync();

    /// <summary>
    /// Deletes stock associated with a specific product.
    /// </summary>
    /// <param name="productId">The identifier of the product whose stock should be deleted.</param>
    Task DeleteByProductIdAsync(int productId);
}
