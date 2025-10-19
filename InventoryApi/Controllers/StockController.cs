using InventoryApi.DTOs;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _service;
    public StockController(IStockService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StockDetailDto>> GetDetailById(int id)
    {
        try
        {
            var stock = await _service.GetDetailByIdAsync(id);
            return Ok(stock);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<StockDto>> Create([FromBody] CreateStockDto createStockDto)
    {
        try
        {
            var stock = await _service.CreateAsync(createStockDto);
            return CreatedAtAction(nameof(GetDetailById), new { id = stock.Id }, stock);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult<StockDto>> UpdateQuantity(int id, [FromBody] UpdateStockDto updateStockDto)
    {
        try
        {
            var stock = await _service.UpdateQuantityAsync(id, updateStockDto);
            return Ok(stock);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetAll()
    {
        var stocks = await _service.GetAllAsync();
        return Ok(stocks);
    }
}
