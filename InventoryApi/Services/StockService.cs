using AutoMapper;
using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

/// <summary>
/// Service for managing stocks, implements <see cref="IStockService"/>.
/// </summary>
public class StockService(InventoryDbContext context, IMapper mapper, IStockMovementService movementService) : IStockService
{
    public async Task<StockDetailDto> GetDetailByIdAsync(int id)
    {
        var stock = await context.Stocks
            .Include(s => s.StockMovements)
            .Include(s => s.Product)
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new KeyNotFoundException($"Stock with ID {id} not found.");

        return mapper.Map<StockDetailDto>(stock);
    }

    public async Task<StockDto> CreateAsync(CreateStockDto createStockDto)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == createStockDto.ProductId)
            ?? throw new KeyNotFoundException($"Product with id {createStockDto.ProductId} not found");
        var stock = new Stock
        {
            Quantity = createStockDto.Quantity,
            Unit = createStockDto.Unit,
            ProductId = createStockDto.ProductId,
            Product = product,
            IsDeleted = false
        };

        context.Stocks.Add(stock);
        await context.SaveChangesAsync();

        await movementService.CreateAsync(stock, createStockDto.Quantity, MovementType.Initial);

        return mapper.Map<StockDto>(stock);
    }

    public async Task<StockDto> UpdateQuantityAsync(int id, UpdateStockQuantityDto update)
    {
        var stock = await context.Stocks
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new KeyNotFoundException($"Stock with id {id} not found");

        UpdateStockQuantity(stock, update.QuantityChange, update.MovementType);
        await context.SaveChangesAsync();

        await movementService.CreateAsync(stock, update.QuantityChange, update.MovementType);

        return mapper.Map<StockDto>(stock);
    }

    public async Task<IEnumerable<StockDto>> GetAllAsync()
    {
        var stocks = await context.Stocks
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return stocks.Select(mapper.Map<StockDto>);
    }

    public async Task DeleteByProductIdAsync(int productId)
    {
        var stock = await context.Stocks
            .FirstOrDefaultAsync(s => s.ProductId == productId);

        if (stock == null)
        {
            return;
        }

        stock.IsDeleted = true;
        await context.SaveChangesAsync();
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