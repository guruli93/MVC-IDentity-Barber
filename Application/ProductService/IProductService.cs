using Dai_2022.Models;
using Dai_2022.Views.Product;
using Domain;
using Domain.Product;


namespace Application
{
    public interface IProductService
    {
        Task<ProductResponseModel2> Get_Product_ById(int id);
        Task<ProductResponseModel2> Get_Product_ByName(string name);
        Task<IEnumerable<ProductResponseModel2> > GetAll_Productasync();
        Task<ProductResponseModel2> Add_Product(ProductReqvestModel2 model);
        Task<ProductResponseModel2> Update_Product(ProductReqvestModel2 model, int id);
        Task Delete_Product(int id);
        Task<Product> ShowImage(int id);

        Task<IEnumerable<ProductResponseModel2>> GetByCategory(string CategoryName);
    }
}
