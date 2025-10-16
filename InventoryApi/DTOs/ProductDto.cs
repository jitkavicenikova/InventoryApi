namespace InventoryApi.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Currency { get; set; }
    public required string SKU { get; set; }
}
