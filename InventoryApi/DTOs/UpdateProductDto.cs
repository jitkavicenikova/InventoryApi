namespace InventoryApi.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
    public string SKU { get; set; } = null!;
}
