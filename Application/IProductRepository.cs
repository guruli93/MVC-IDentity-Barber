

using Domain.Product;

namespace Application
{
    public interface IProductRepository : IAsyncRepository<Product>
    {

        Task<Product> Get_Product_ByName(string name);
        Task<Product> Get_Product_ById(int id);
        Task<IEnumerable<Product>> GetAll_Productasync();
        //Task<IEnumerable<Product>> GetAll_Productasync(int page,int pegSize);
        Task<Product> ShowImage(int id);
        Task<IEnumerable<Product>> GetByCategory(string CategoryName);
        Task<int> GetTotalProductCountAsync();
    }
}
