

using AutoMapper;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.BusinessLogicLayer.DTO;

namespace eCommerce.BusinessLogicLayer.Mappers;

public class ProductAddRequestToProductMappingProfile : Profile
{
    public ProductAddRequestToProductMappingProfile()
    {
        CreateMap<ProductAddRequest, Product>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));

        CreateMap<ProductAddRequest, Product>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        CreateMap<ProductAddRequest, Product>().ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

        CreateMap<ProductAddRequest, Product>().ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock));

        CreateMap<ProductAddRequest, Product>().ForMember(dest => dest.ProductID, opt => opt.Ignore());


    }
}
