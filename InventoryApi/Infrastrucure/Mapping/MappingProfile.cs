using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;

namespace InventoryApi.Infrastrucure.Mapping;

/// <summary>
/// AutoMapper profile that defines mappings between domain entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class
    /// and configures all object-object mappings for the application.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Stock, StockDto>().ReverseMap();
        CreateMap<Stock, StockDetailDto>().ReverseMap();
        CreateMap<CreateStockDto, Stock>();
        CreateMap<UpdateStockQuantityDto, Stock>();

        CreateMap<StockMovement, StockMovementDto>().ReverseMap();
    }
}