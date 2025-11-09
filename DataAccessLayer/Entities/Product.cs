using System.ComponentModel.DataAnnotations;

namespace eCommerce.DataAccessLayer.Entities;

public class Product
{
    /// <summary>
    /// 
    /// </summary>
    [Key]
    public Guid ProductID { get; set; }
    public string? ProductName { get; set; }
    public string? Category { get; set; }
    public double UnitPrice { get; set; }
    public int QuantityInStock { get; set; }

    public static implicit operator bool(Product? v)
    {
        throw new NotImplementedException();
    }
}

