using InventoryApi.DTOs;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

/// <summary>
/// Controller responsible for managing stock items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StockController(IStockService service) : ControllerBase
{
    /// <summary>
    /// Retrieves detailed information about a stock item by its ID.
    /// </summary>
    /// <param name="id">The ID of the stock item to retrieve.</param>
    /// <returns>The <see cref="StockDetailDto"/> with the specified ID.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StockDetailDto>> GetDetailById(int id)
    {
        var stock = await service.GetDetailByIdAsync(id);
        return Ok(stock);
    }

    /// <summary>
    /// Creates a new stock item.
    /// </summary>
    /// <param name="createStockDto">The data for the stock item to create.</param>
    /// <returns>The created <see cref="StockDto"/>.</returns>
    [HttpPost]
    public async Task<ActionResult<StockDto>> Create([FromBody] CreateStockDto createStockDto)
    {
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
    public async Task<ActionResult<StockDto>> UpdateQuantity(int id, [FromBody] UpdateStockDto updateStockDto)
    {
        var stock = await service.UpdateQuantityAsync(id, updateStockDto);
        return Ok(stock);
    }

    /// <summary>
    /// Retrieves all stock items.
    /// </summary>
    /// <returns>A list of <see cref="StockDto"/>.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetAll()
    {
        var stocks = await service.GetAllAsync();
        return Ok(stocks);
    }
}