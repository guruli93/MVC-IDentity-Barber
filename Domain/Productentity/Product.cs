
using Domain.Imageentity;

namespace Domain.Productentity;

public class Product
{
   
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProdutCount { get; set; }
    public string ProductCategory { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Image Image { get; set; }
}



