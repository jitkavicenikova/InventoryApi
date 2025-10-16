namespace InventoryApi.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Currency { get; set; }
    public required string Sku { get; set; }
}
