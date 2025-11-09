using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.RepositoryContracts;
using eCommerce.DataAccessLayer.Repositories;
using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AutoMapper;
using RabbitMQ.Client;
using eCommerce.BusinessLogicLayer.RabbitMQ;

namespace eCommerce.BusinessLogicLayer.Services;

public class ProductsService : IProductsService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductsRepository _productsRepository;
    private readonly IRabbitMQProducer _rabbitMQProducer;

    public ProductsService(IValidator<ProductAddRequest> productAddRequestValidator, IValidator<ProductUpdateRequest> productUpdateRequestValidator, IMapper mapper, IProductsRepository productsRepository, IRabbitMQProducer rabbitMQProducer)
    {
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productsRepository = productsRepository;
        _rabbitMQProducer = rabbitMQProducer;   
        
    }
    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }

        // Validate the product using FluentValidation  
        FluentValidation.Results.ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        // Check validation result  
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));//Error1, Error2, Error3 ...
            throw new ArgumentException(errors);
        }

        //Attempt to add the product to the repository
        Product productInput = _mapper.Map<Product>(productAddRequest);

        // Map ProductAddRequest into  product type (it invokes ProductsAddRequestToProductMappingProfile)

        Product? addedProduct = await _productsRepository.AddProduct(productInput);
        if (addedProduct == null)
        {
            return null;
        }

        // Map ProductAddRequest into  product type (it invokes ProductToProductResponseMappingProfile)  
        ProductResponse? addedproductResponse = _mapper.Map<ProductResponse>(addedProduct);


        return addedproductResponse;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? existingProduct = await _productsRepository.GetProductByCondiction(temp => temp.ProductID == productID);

        if (existingProduct == null)
        {
            return false;
        }

        // Attempt to delete the product from the repository
        bool isDeleted = await _productsRepository.DeleteProduct(productID);

        //publish message of product deletion
        if (isDeleted)
        {
            ProductDeletionMessage message = new ProductDeletionMessage(existingProduct.ProductID, existingProduct.ProductName);

            //string routingKey = "product.delete";// tipo direct

            //string routingKey = "product.#";//Topic
            //await _rabbitMQProducer.ProducerAsync<ProductDeletionMessage>(routingKey, message);
            var headers = new Dictionary<string, object>
            {
                { "x-match", "all" },
                { "event", "product.delete" },   //  "all" or "any"
                { "RowCount", 1 }
            }; //Headers exchange
            await _rabbitMQProducer.ProducerAsync<ProductDeletionMessage>(headers, message); //Fanout and header

        }
        return isDeleted;
    }
    public async Task<List<ProductResponse?>> GetProductsByCondicion(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product?> products = await _productsRepository.GetProductsByCondiction(conditionExpression);
        IEnumerable<ProductResponse> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);// Invoke ProductToProductResponseMappingProfile
        return productResponses.ToList();
    }

    public async Task<ProductResponse> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product? existingProduct = await _productsRepository.GetProductByCondiction(temp => temp.ProductID == productUpdateRequest.ProductID);
        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid Product ID");
        }
        // Validate the product using FluentValidation
        FluentValidation.Results.ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        // Check validation result
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));//Error1, Error2, Error3 ...
            throw new ArgumentException(errors);
        }

        // Map ProductUpdateRequest into  product type (it invokes ProductUpdateRequestToProductMappingProfile)
        Product product = _mapper.Map<Product>(productUpdateRequest);

        
        //bool isProductNameChanged = productUpdateRequest.ProductName != existingProduct.ProductName;

        //Check if product name is changed
        Product? updatedProduct = await _productsRepository.UpdateProduct(product);

        //Publish if product name is changed

        string routingKey = "product.update.name";
        //string routingKey = "product.update.*";//Topic
        var message = new ProductNameUpdateMessage(product.ProductID, product.ProductName);
        var headers = new Dictionary<string, object>
         {
                { "x-match", "all" },
                { "event", "product.update" },   //  "all" or "any"
                { "RowCount", 1 }
         }; //Headers exchange

        await _rabbitMQProducer.ProducerAsync<Product>(headers, product); //the is changed, show complet product details
        //await _rabbitMQProducer.ProducerAsync<ProductNameUpdateMessage>(message);// Fanout

        ProductResponse? updatedProductResponse = _mapper.Map<ProductResponse>(updatedProduct);// Map the updated product to existing product
        return updatedProductResponse;
    }
    public async Task<ProductResponse?> GetProductByCondicion(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await _productsRepository.GetProductByCondiction(conditionExpression);
        if (product == null)
        {
            return null;
        }
        ProductResponse productResponse = _mapper.Map<ProductResponse>(product);// Invoke ProductToProductResponseMappingProfile
        return productResponse;
    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await _productsRepository.GetProducts();
        IEnumerable<ProductResponse> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);// Invoke ProductToProductResponseMappingProfile
        return productResponses.ToList();
    }

    public async Task<ProductResponse?> GetProduct()
    {
       Product? product = (await _productsRepository.GetProducts()).FirstOrDefault();
        
       return product != null ? _mapper.Map<ProductResponse>(product) : null;
    }

}

