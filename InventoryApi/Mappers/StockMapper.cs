using InventoryApi.DTOs;
using InventoryApi.Entities;

namespace InventoryApi.Mappers;

public static class StockMapper
{
    public static StockDto ToDto(Stock stock)
    {
        return new StockDto
        {
            Id = stock.Id,
            ProductId = stock.ProductId,
            Quantity = stock.Quantity,
            Unit = stock.Unit
        };
    }

    public static StockDetailDto ToDetailDto(Stock stock)
    {
        return new StockDetailDto
        {
            Id = stock.Id,
            ProductId = stock.ProductId,
            Quantity = stock.Quantity,
            Unit = stock.Unit,
            Movements = stock.StockMovements
                .OrderByDescending(m => m.Timestamp)
                .Select(StockMovementMapper.ToDto)
                .ToList()
        };
    }
}