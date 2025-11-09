using System;
using eCommerce.DataAccessLayer.Entities;
using System.Dynamic;
using System.Linq.Expressions;

namespace eCommerce.DataAccessLayer.RepositoryContracts;
/// <summary>
/// Represents a repository for managing products table in the database.
/// </summary>
/// 
public interface IProductsRepository
{
    // To do: add methods to interact with the database
    /// <summary>
    /// Retrivies all product asynchronously
    /// </summary>
    /// <returns>Returns all product from the table </returns>
    Task<IEnumerable<Product>> GetProducts();




    Task<IEnumerable<Product>> GetProductsAsync();



    /// <summary>
    /// Retrivies all product based on the specified condition asynchronously
    /// </summary>
    /// <param name="conditionExpression">The condition to filter products</param>
    /// <returns>returning a collection of matchimg product</returns>
    Task<IEnumerable<Product?>>GetProductsByCondiction(Expression<Func<Product, bool>> conditionExpression);


    /// <summary>
    /// Retrivies a single product based on the specified condition asynchronously
    /// </summary>
    /// <param name="conditionExpression">The condition to filter products</param>
    /// <returns>returning a single product or null if not found</returns>



    Task<Product?>GetProductByCondiction(Expression<Func<Product, bool>> conditionExpression);
    /// <summary>
    /// Add a new product into products table asynchronously
    /// </summary>
    /// <param name="product">The Product to be added</param>
    /// <returns>Returns the added product object or null if unsuccessful </returns>



    Task<Product?> AddProduct(Product product);

    /// <summary>
    /// update an exiting product in the products table asynchronously
    /// </summary>
    /// <param name="product">the product to be updated</param>
    /// <returns>Return the update product; or null if not found </returns>



    Task<Product?> UpdateProduct(Product product);

    /// <summary>
    /// Deletes the product asynchronously
    /// </summary>
    /// <param name="product">The product ID to be deleted</param>
    /// <returns>Detele true if the deletion is successfull, false otherwise</returns>




    Task<bool> DeleteProduct(Guid productID);


}

