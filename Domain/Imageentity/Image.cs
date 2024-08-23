
using Domain.Productentity;

namespace Domain.Imageentity;

public class Image
{
     public int Id { get; set;}
    public byte[] ImageData { get; set;}

    public int ProductId   { get; set; }
    public Product Product { get; set; }
}