using AutoMapper;
using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

public class StockService : IStockService
{
    private readonly InventoryDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStockMovementService _movementService;

    public StockService(InventoryDbContext context, IMapper mapper, IStockMovementService movementService)
    {
        _context = context;
        _mapper = mapper;
        _movementService = movementService;
    }

    public async Task<StockDetailDto> GetDetailByIdAsync(int id)
    {
        var stock = await _context.Stocks
            .Include(s => s.StockMovements)
            .Include(s => s.Product)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted) 
            ?? throw new KeyNotFoundException($"Stock with ID {id} not found.");
        
        return _mapper.Map<StockDetailDto>(stock);
    }

    public async Task<StockDto> CreateAsync(CreateStockDto createStockDto)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == createStockDto.ProductId && !p.IsDeleted)
            ?? throw new KeyNotFoundException($"Product with id {createStockDto.ProductId} not found");
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

        await _movementService.CreateAsync(stock, createStockDto.Quantity, MovementType.Initial);

        return _mapper.Map<StockDto>(stock);
    }

    public async Task<StockDto> UpdateQuantityAsync(int id, UpdateStockDto update)
    {
        var stock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new KeyNotFoundException($"Stock with id {id} not found");

        await _movementService.CreateAsync(stock, update.QuantityChange, update.MovementType);

        UpdateStockQuantity(stock, update.QuantityChange, update.MovementType);
        await _context.SaveChangesAsync();

        return _mapper.Map<StockDto>(stock);
    }

    public async Task<IEnumerable<StockDto>> GetAllAsync()
    {
        var stocks = await _context.Stocks
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return stocks.Select(_mapper.Map<StockDto>);
    }

    public async Task DeleteByProductIdAsync(int productId)
    {
        var stock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.ProductId == productId);

        if (stock == null)
        {
            return;
        }

        stock.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    private static void UpdateStockQuantity(Stock stock, int quantityChange, MovementType type)
    {
        switch (type)
        {
            case MovementType.Initial:
            case MovementType.Incoming:
                stock.Quantity += quantityChange;
                break;
            case MovementType.Outgoing:
                if (stock.Quantity < quantityChange)
                {
                    throw new InvalidOperationException("Not enough items in stock for outgoing movement.");
                }
                stock.Quantity -= quantityChange;
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unsupported movement type: {type}");
        }
    }
}