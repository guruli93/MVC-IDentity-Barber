
using Domain;
using Domain.Product;

namespace Application
{
    public interface IProductRepository : IAsyncRepository<Product>
    {

        Task<Product> Get_Product_ByName(string name);
        Task<Product> Get_Product_ById(int id);
        Task<IEnumerable<Product>> GetAll_Productasync();
        Task<Product> ShowImage(int id);
     

    }
}
