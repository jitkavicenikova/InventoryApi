using InventoryApi.DTOs;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController(IStockService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StockDetailDto>> GetDetailById(int id)
    {
        var stock = await service.GetDetailByIdAsync(id);
        return Ok(stock);
    }

    [HttpPost]
    public async Task<ActionResult<StockDto>> Create([FromBody] CreateStockDto createStockDto)
    {
        var stock = await service.CreateAsync(createStockDto);
        return CreatedAtAction(nameof(GetDetailById), new { id = stock.Id }, stock);
    }

    [HttpPatch]
    public async Task<ActionResult<StockDto>> UpdateQuantity(int id, [FromBody] UpdateStockDto updateStockDto)
    {
        var stock = await service.UpdateQuantityAsync(id, updateStockDto);
        return Ok(stock);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetAll()
    {
        var stocks = await service.GetAllAsync();
        return Ok(stocks);
    }
}
