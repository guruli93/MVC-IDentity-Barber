using Application;
using Domain.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory; 

namespace Infrastructure.Persistence.Repository
{
    public class ProductRepository_007 : UFrepository<Product>, IProductRepository
    {
        private readonly DbContext.DbContext _dbContext;
        private readonly IMemoryCache _memoryCache; 

        public ProductRepository_007(DbContext.DbContext dbContext, IMemoryCache memoryCache)
            : base(dbContext, memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Product>> GetAll_Productasync()
        {
            var cacheKey = "all_products";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .ToListAsync();

                _memoryCache.Set(cacheKey, products, TimeSpan.FromHours(12));
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetAll_WitchPagination(int page, int pageSize)
        {
            var cacheKey = $"products_page_{page}_size_{pageSize}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _dbContext.Products
                    .OrderBy(p => p.Id)
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                _memoryCache.Set(cacheKey, products, TimeSpan.FromHours(12));
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetByCategory(string CategoryName)
        {
            var cacheKey = $"products_category_{CategoryName}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Product> products))
            {
                products = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .Where(Name => Name.ProductCategory == CategoryName)
                    .ToListAsync();

                _memoryCache.Set(cacheKey, products, TimeSpan.FromHours(12));
            }

            return products;
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            var cacheKey = "total_product_count";

            if (!_memoryCache.TryGetValue(cacheKey, out int count))
            {
                count = await _dbContext.Products.AsNoTracking().CountAsync();

                _memoryCache.Set(cacheKey, count, TimeSpan.FromHours(12));
            }

            return count;
        }

        public async Task<Product> Get_Product_ById(int id)
        {
            var cacheKey = $"product_{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out Product product))
            {
                product = await _dbContext.Products
                    .Include(p => p.Image)
                    .FirstOrDefaultAsync(x => x.Id == id);

                _memoryCache.Set(cacheKey, product, TimeSpan.FromHours(12));
            }

            return product;
        }

        public async Task<Product> Get_Product_ByName(string name)
        {
            var cacheKey = $"product_name_{name}";

            if (!_memoryCache.TryGetValue(cacheKey, out Product product))
            {
                product = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .FirstOrDefaultAsync(u => u.ProductName == name);

                _memoryCache.Set(cacheKey, product, TimeSpan.FromHours(12));
            }

            return product;
        }

        public async Task<Product> ShowImage(int id)
        {
            var cacheKey = $"product_image_{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out Product product))
            {
                product = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .FirstOrDefaultAsync(p => p.Id == id);

                _memoryCache.Set(cacheKey, product, TimeSpan.FromHours(12));
            }

            return product;
        }
    }
}



/*
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
          
        var products = await sQL_DbContext.Products.
        AsNoTracking()
       .Include(p => p.Image) 
       .ToListAsync();
            
            return products;
        }
        
        public async Task<IEnumerable<Product>> GetAll_WitchPagination(int page, int pageSize)
{
        var products = await sQL_DbContext.Products
       .OrderBy(p => p.Id).
        AsNoTracking()
       .Include(p => p.Image)
       .Skip((page - 1) * pageSize) 
       .Take(pageSize) 
       .ToListAsync();
        return products;
}

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
            var product = await sQL_DbContext.Products.Include(p => p.Image)
                .FirstOrDefaultAsync(x=>x.Id==id);
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
*/
