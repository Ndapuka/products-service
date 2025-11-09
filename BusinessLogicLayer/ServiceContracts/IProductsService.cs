
using eCommerce.DataAccessLayer.Entities;
using eCommerce.BusinessLogicLayer.DTO;
using System.Linq.Expressions;

namespace eCommerce.BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
    /// <summary>
    /// Retrieves the list of products from the product repository
    /// </summary>
    /// <returns>Returns list of productResponse obj</returns>
    Task<List<ProductResponse?>> GetProducts();

    /// <summary>
    /// Retrieves the list of products from the product repository based on the specified condition
    /// </summary>
    /// <param name="conditionExpression">Condition to check</param>
    /// <returns>Return matching products</returns>
    Task<List<ProductResponse?>> GetProductsByCondicion(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Return the single product based on the specified condition
    /// </summary>
    /// <param name="conditionExpression">Express that represents the condition to check </param>
    /// <returns>Return product or null</returns>
    Task<ProductResponse?> GetProductByCondicion(Expression<Func<Product, bool>> conditionExpression);
    /// <summary>
    /// Add a new product into the the table using product repository
    /// </summary>
    /// <param name="productAddRequest">Product to insert</param>
    /// <returns>Product after to insert or null if unsuccessful</returns>
    Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);


    /// <summary>
    /// Update an existing product based ProductID in product repository
    /// </summary>
    /// <param name="productUpdateRequest">Product data to update</param>
    /// <returns>Returns product obj after successfull updating; otherwise null</returns>
    Task<ProductResponse> UpdateProduct(ProductUpdateRequest productUpdateRequest);/// <summary>
    
    /// <summary>
    /// Deletes an existing product based on given ProductID in product repository
    /// </summary>
    /// <param name="productUpdateRequest">ProductID to search and delete</param>
    /// <returns>Returns true if the deletion is successfull deleting; otherwise false</returns>
    Task<bool> DeleteProduct(Guid productID);
}
    