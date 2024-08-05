using System.ComponentModel.DataAnnotations;


namespace Dai_2022.Models
{
    public class ProductResponseModel2
    {
        [Key]
        public int Id_Pr { get; set; }
        public string ProductName { get; set; }
        public string ProdutCount { get; set; }
        public string ProductCategory { get; set; }
        public string ContentType { get; set; }
        public byte[] ImageData { get; set; }


    }



}
