using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
namespace Application.Models_DB
{
    public class ProductReqvestModel2
    {
       // [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Category Name is required")]
        [MaxLength(15)]
        [MinLength(3)]
        [DisplayName("ProductName")]
        public string ProductName { get; set; }

        [Required]
        //  [Range(1,50,ErrorMessage = "ProductCategoryName must")]
        [MaxLength(15)]
        [MinLength(3)]
        [DisplayName("ProductCategoryName")]
        public string ProductCategoryName { get; set; }

        [Required]
        [Range(1, 5000, ErrorMessage = "ProductCount must")]
        [DisplayName("ProductCount")]
        public string ProductCount { get; set; }
        public string ContentType { get; set; } = string.Empty;

        public IFormFile PhotoData { get; set; }


    }
}
