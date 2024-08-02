namespace Domain.Image;

public class Image
{
    public int Id { get; set;}
    public byte[] ImageData { get; set;}

    public int ProductId           { get; set; }
    public Product.Product Product { get; set; }
}