using Microsoft.AspNetCore.Routing;
using System.Runtime.CompilerServices;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.BusinessLogicLayer.DTO;
using Microsoft.AspNetCore.Routing.Template;
using eCommerce.BusinessLogicLayer.Services;
using FluentValidation;
using FluentValidation.Results;
using eCommerce.DataAccessLayer.Entities;


namespace eCommerce.ProductsMicroService.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProuctAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //Get /api/products
        app.MapGet("/api/products", async (IProductsService productService) =>
        {
            List<ProductResponse?> products = await productService.GetProducts();

            return Results.Ok(products);
        });

        //Get /api/products/search/product-id/0000-0000-0000-0000
        app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) =>
        {
            //await Task.Delay(100);
            //throw new NotImplementedException();


            ProductResponse? product = await productsService.GetProductByCondicion(temp => temp.ProductID == ProductID);

            if (product == null)
                return Results.NotFound();
            else
                return Results.Ok(product);

        });

        //Get /api/products/search/product-id/{SearchString}
        // This endpoint searches products by ProductName (string) test     
        app.MapGet("/api/products/search/product-id/{SearchString}", async (IProductsService productsService, string SearchString) =>
        {
            List<ProductResponse?> productsByProductName = await productsService.GetProductsByCondicion(temp => temp.ProductName != null && temp.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            List<ProductResponse?> productsByCategory = await productsService.GetProductsByCondicion(temp => temp.Category != null && temp.Category.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            var products = productsByProductName.Union(productsByCategory);
            if (!products.Any())
                return Results.NotFound();


            return Results.Ok(products);
        });

        //Post /api/products
        app.MapPost("/api/products", async (IProductsService productsService, IValidator<ProductAddRequest> productAddRequestValidator, ProductAddRequest productAddRequest) =>
        {
            //Validate the ProductAddRequest object using Fluent Validation
            ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

            //Check the validation result
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                  .GroupBy(temp => temp.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }
            var addedProductResponse = await productsService.AddProduct(productAddRequest);
            if (addedProductResponse != null)
                return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
            else
                return Results.Problem("Error in adding product");
        });

        //PUT /api/products
        app.MapPut("/api/products", async (IProductsService productsService, IValidator<ProductUpdateRequest> productUpdateRequestValidator, ProductUpdateRequest productUpdateRequest) =>
        {
            //Validate the ProductUpdateRequest object using Fluent Validation
            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            //Check the validation result
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                  .GroupBy(temp => temp.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }


            var updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);
            if (updatedProductResponse != null)
                return Results.Ok(updatedProductResponse);
            else
                return Results.Problem("Error in updating product");
        });

        //DELETE /api/products/xxxxxxxxxxxxxxxxxxx
          
        app.MapDelete("/api/products/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) =>
        {
            bool isDeleted = await productsService.DeleteProduct(ProductID);
            if (!isDeleted)
            {
                return Results.Problem("Error in deleting product");
            }
            return Results.Ok(new { Message = "Product deleted successfully." });
        });
        



        return app;
    }
}
