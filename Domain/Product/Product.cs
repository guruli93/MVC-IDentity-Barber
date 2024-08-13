using System.ComponentModel.DataAnnotations;

namespace Domain.Product;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProdutCount { get; set; }
    public string ProductCategory { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Image.Image Image { get; set; }
}



