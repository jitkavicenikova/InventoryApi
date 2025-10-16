using InventoryApi.DTOs;
using InventoryApi.Entities;

namespace InventoryApi.Mappers;

public class StockMovementMapper
{
    public static StockMovementDto ToDto(StockMovement movement)
    {
        return new StockMovementDto
        {
            Id = movement.Id,
            QuantityChange = movement.QuantityChange,
            MovementType = movement.MovementType,
            Timestamp = movement.Timestamp
        };
    }
}
