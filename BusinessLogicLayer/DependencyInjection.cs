using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.Mappers;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using eCommerce.BusinessLogicLayer.Services;
using FluentValidation.AspNetCore;
using eCommerce.BusinessLogicLayer.Validators;
using FluentValidation;
using eCommerce.BusinessLogicLayer.RabbitMQ;

namespace eCommerce.ProductsService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        //TO DO: Add Business Logic Layer services into the IoC container
        services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddScoped<IProductsService, eCommerce.BusinessLogicLayer.Services.ProductsService>();
        services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

        return services;
    }
}


