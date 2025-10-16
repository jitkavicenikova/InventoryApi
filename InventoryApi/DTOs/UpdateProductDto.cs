namespace InventoryApi.DTOs;

public class UpdateProductDto
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required string Currency { get; set; }
    public required string Sku { get; set; }
}
