using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Routing.Constraints;

namespace eCommerce.BusinessLogicLayer.Mappers;

public class ProductUpdateRequestMappingProfile : Profile
{
    public ProductUpdateRequestMappingProfile()
    {
        CreateMap<ProductUpdateRequest, Product>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));

        CreateMap<ProductUpdateRequest, Product>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        CreateMap<ProductUpdateRequest, Product>().ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

        CreateMap<ProductUpdateRequest, Product>().ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock));

        CreateMap<ProductUpdateRequest, Product>().ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID));
        

    }
}
