
using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.Repositories;
using eCommerce.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.ProductsService.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // To do: add data access layer services into the IoC container
        string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;


        string connectionString = connectionStringTemplate
            .Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
            .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"))
            .Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT"))
            .Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE"))
            .Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER"));

        

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(connectionString); //! is used to tell the compiler that the value is not null
        });

        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }

}

