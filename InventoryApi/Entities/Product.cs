namespace InventoryApi.Entities;

/// <summary>
/// Entity representing a product in the inventory system.
/// </summary>
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Currency { get; set; }
    public required string Sku { get; set; }
    public required bool IsDeleted { get; set; } = false;
}
