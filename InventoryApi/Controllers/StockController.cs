using InventoryApi.DTOs;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

/// <summary>
/// Controller responsible for managing stock items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StockController(IStockService service, ILogger<StockController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieves detailed information about a stock item by its ID.
    /// </summary>
    /// <param name="id">The ID of the stock item to retrieve.</param>
    /// <returns>The <see cref="StockDetailDto"/> with the specified ID.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StockDetailDto>> GetDetailById(int id)
    {
        logger.LogInformation("Retrieving stock detail with ID {StockId}", id);
        var stock = await service.GetDetailByIdAsync(id);
        return Ok(stock);
    }

    /// <summary>
    /// Creates a new stock item.
    /// </summary>
    /// <param name="createStockDto">The data for the stock item to create.</param>
    /// <returns>The created <see cref="StockDto"/>.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StockDto>> Create([FromBody] CreateStockDto createStockDto)
    {
        logger.LogInformation("Creating a new stock item for Product ID {ProductId}", createStockDto.ProductId);
        var stock = await service.CreateAsync(createStockDto);
        return CreatedAtAction(nameof(GetDetailById), new { id = stock.Id }, stock);
    }

    /// <summary>
    /// Updates the quantity of an existing stock item by its ID.
    /// </summary>
    /// <param name="id">The ID of the stock item to update.</param>
    /// <param name="updateStockDto">The updated stock data.</param>
    /// <returns>The updated <see cref="StockDto"/>.</returns>
    [HttpPatch]
    [ProducesResponseType(typeof(StockDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StockDto>> UpdateQuantity(int id, [FromBody] UpdateStockQuantityDto updateStockDto)
    {
        logger.LogInformation("Updating stock quantity with ID {StockId}", id);
        var stock = await service.UpdateQuantityAsync(id, updateStockDto);
        return Ok(stock);
    }

    /// <summary>
    /// Retrieves all stock items.
    /// </summary>
    /// <returns>A list of <see cref="StockDto"/>.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StockDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetAll()
    {
        logger.LogInformation("Retrieving all stock items");
        var stocks = await service.GetAllAsync();
        return Ok(stocks);
    }
}