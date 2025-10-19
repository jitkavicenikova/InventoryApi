using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;

namespace InventoryApi.Infrastrucure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Stock, StockDto>().ReverseMap();
        CreateMap<Stock, StockDetailDto>().ReverseMap();
        CreateMap<CreateStockDto, Stock>();
        CreateMap<UpdateStockDto, Stock>();

        CreateMap<StockMovement, StockMovementDto>().ReverseMap();
    }
}