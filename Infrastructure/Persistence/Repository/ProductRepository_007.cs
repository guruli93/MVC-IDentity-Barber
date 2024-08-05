using Application;
using Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class ProductRepository_007 : UFrepository<Product>, IProductRepository
    {
        private readonly DbContext.DbContext sQL_DbContext;
       

        public ProductRepository_007(DbContext.DbContext dbContext) : base(dbContext)
        {
            sQL_DbContext = dbContext;
           
        }

        public async Task<IEnumerable<Product>> GetAll_Productasync()
        {
          
        var products = await sQL_DbContext.Products.AsNoTracking()
       .Include(p => p.Image) 
       .ToListAsync();
            
            return products;
        }
        public async Task<IEnumerable<Product>> GetAll_Productasync(int page, int pageSize)
        {
            var products = await sQL_DbContext.Products.AsNoTracking()
                .Include(p => p.Image)
                .ToListAsync();

            return products;
        }
        /*
        public async Task<IEnumerable<Product>> GetAll_Productasync(int page, int pageSize)
        {
            var products = await sQL_DbContext.Products.AsNoTracking()
                .Include(p => p.Image)
                .Skip((page - 1) * pageSize) // გამოტოვეთ პროდუქტი, რაც შეესაბამება გვერდის ნომერს
                .Take(pageSize) // აირჩიეთ პროდუქტი, რაც შეესაბამება გვერდის ზომას
                .ToListAsync();

            return products;
        }
        */

        public async Task<IEnumerable<Product>> GetByCategory(string CategoryName)
        {
            var products = await sQL_DbContext.Products.AsNoTracking().
                Include(p => p.Image).
                Where(Name => Name.ProductCategory == CategoryName).ToListAsync();
            return products;
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            return  await sQL_DbContext.Products.AsNoTracking().CountAsync();
        }

        public async Task<Product> Get_Product_ById(int id)
        {
            var product = await sQL_DbContext.Products.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
            return product;
        }

        public async Task<Product> Get_Product_ByName(string name)
        {
            var product = await sQL_DbContext.Products
                .AsNoTracking()
                .Include(p => p.Image) 
                .FirstOrDefaultAsync(u => u.ProductName == name);
            return product;
        }



        public async Task<Product> ShowImage(int id)
        {
            var product = await sQL_DbContext.Products.AsNoTracking().
                Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);

            return product;

        }
    }
}
