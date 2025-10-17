using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Enums;
using InventoryApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

public class StockService : IStockService
{
    private readonly InventoryDbContext _context;
    private readonly IProductService _productService;
    private readonly IStockMovementService _movementService;

    public StockService(InventoryDbContext context, IProductService productService, IStockMovementService movementService)
    {
        _context = context;
        _productService = productService;
        _movementService = movementService;
    }

    public async Task<StockDetailDto> GetDetailByIdAsync(int id)
    {
        var stock = await _context.Stocks
            .Include(s => s.StockMovements)
            .Include(s => s.Product)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted) 
            ?? throw new KeyNotFoundException($"Stock with ID {id} not found.");
        
        return StockMapper.ToDetailDto(stock);
    }

    public async Task<StockDto> CreateAsync(CreateStockDto createStockDto)
    {
        var product = await _productService.GetEntityByIdOrThrow(createStockDto.ProductId);
        var stock = new Stock
        {
            Quantity = createStockDto.Quantity,
            Unit = createStockDto.Unit,
            ProductId = createStockDto.ProductId,
            Product = product,
            IsDeleted = false
        };

        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        await _movementService.CreateAsync(stock.Id, createStockDto.Quantity, MovementType.Initial);

        return StockMapper.ToDto(stock);
    }

    public async Task UpdateQuantityAsync(int id, UpdateStockDto update)
    {
        var stock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new KeyNotFoundException($"Stock with id {id} not found");

        await _movementService.CreateAsync(id, update.QuantityChange, update.MovementType);

        stock.Quantity += update.MovementType == MovementType.Incoming
            ? update.QuantityChange
            : -update.QuantityChange;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var stock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new KeyNotFoundException($"Stock with id {id} not found");

        stock.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<StockDto>> GetAllAsync()
    {
        var stocks = await _context.Stocks
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return stocks.Select(StockMapper.ToDto);
    }
}