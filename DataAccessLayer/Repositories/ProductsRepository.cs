
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace eCommerce.DataAccessLayer.Repositories;


public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProductsRepository> _logger;
    // To do: implement CRUD operations for the Products table
    public ProductsRepository(ApplicationDbContext dbContext, ILogger<ProductsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        
    }

    public async Task<Product?> AddProduct(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetProductByCondiction(Expression<Func<Product, bool>> conditionExpression)
    {
        _logger.LogInformation("Executing query to get product by condition: {Condition}", conditionExpression);
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(conditionExpression);
        _logger.LogInformation("Query executed. Product found: {Product}", product);
        return product;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondiction(Expression<Func<Product, bool>> conditionExpression)
    {
        return await _dbContext.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == product.ProductID);
        if (existingProduct == null)
        {
            return null;
        }
        existingProduct.ProductName = product.ProductName;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.QuantityInStock = product.QuantityInStock;
        existingProduct.Category = product.Category;

        await _dbContext.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productID);
        if (existingProduct == null)
        {
            return false;
        }
        _dbContext.Products.Remove(existingProduct);
        int affectedRowsCount = await _dbContext.SaveChangesAsync();
        return affectedRowsCount > 0;
    }

}
