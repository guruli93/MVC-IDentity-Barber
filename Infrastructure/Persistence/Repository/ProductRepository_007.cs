using Application;
using Domain.Productentity;
using Infrastructure.Persistence.DbContext_;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repository
{
    public class ProductRepository_007 : UFrepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ProductRepository_007> _logger;

        public ProductRepository_007(AppDbContext dbContext, IMemoryCache memoryCache, ILogger<ProductRepository_007> logger)
            : base(dbContext, memoryCache, logger)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAll_Productasync()
        {
            var cacheKey = $"{typeof(Product).Name}_all";
            _logger.LogInformation("Retrieving all products from cache with key {CacheKey}", cacheKey);

            if (!_memoryCache.TryGetValue(cacheKey, out List<Product> products))
            {
                _logger.LogInformation("Cache miss. Retrieving products from database.");
                products = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .ToListAsync();

                _memoryCache.Set(cacheKey, products, TimeSpan.FromHours(12));
                _logger.LogInformation("Products cached with key {CacheKey}", cacheKey);
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetAll_WitchPagination(int page, int pageSize)
        {
           
               var products = await _dbContext.Products
                    .OrderBy(p => p.Id)
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


            return products;
        }

        public async Task<IEnumerable<Product>> GetByCategory(string CategoryName)
        {
            var cacheKey = $"products_category_{CategoryName}";
            _logger.LogInformation("Retrieving products by category from cache with key {CacheKey}", cacheKey);

            if (!_memoryCache.TryGetValue(cacheKey, out List<Product> products))
            {
                _logger.LogInformation("Cache miss. Retrieving products by category from database.");
                products = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .Where(p => p.ProductCategory == CategoryName)
                    .ToListAsync();

                _memoryCache.Set(cacheKey, products, TimeSpan.FromHours(12));
                _logger.LogInformation("Products by category cached with key {CacheKey}", cacheKey);
            }

            return products;
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            var cacheKey = "total_product_count";
            _logger.LogInformation("Retrieving total product count from cache with key {CacheKey}", cacheKey);

            if (!_memoryCache.TryGetValue(cacheKey, out int count))
            {
                _logger.LogInformation("Cache miss. Retrieving total product count from database.");
                count = await _dbContext.Products.AsNoTracking().CountAsync();

                _memoryCache.Set(cacheKey, count, TimeSpan.FromHours(12));
                _logger.LogInformation("Total product count cached with key {CacheKey}", cacheKey);
            }

            return count;
        }

        public async Task<Product> Get_Product_ById(int id)
        {
            var cacheKey = $"{typeof(Product).Name}_{id}";
            _logger.LogInformation("Retrieving product by ID from cache with key {CacheKey}", cacheKey);

            if (!_memoryCache.TryGetValue(cacheKey, out Product product))
            {
                _logger.LogInformation("Cache miss. Retrieving product by ID from database.");
                product = await _dbContext.Products
                    .Include(p => p.Image)
                    .FirstOrDefaultAsync(x => x.Id == id);

                _memoryCache.Set(cacheKey, product, TimeSpan.FromHours(12));
                _logger.LogInformation("Product by ID cached with key {CacheKey}", cacheKey);
            }

            return product;
        }

        public async Task<Product> Get_Product_ByName(string name)
        {
            var cacheKey = $"product_name_{name}";
            _logger.LogInformation("Retrieving product by name from cache with key {CacheKey}", cacheKey);

            if (!_memoryCache.TryGetValue(cacheKey, out Product product))
            {
                _logger.LogInformation("Cache miss. Retrieving product by name from database.");
                product = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .FirstOrDefaultAsync(u => u.ProductName == name);

                _memoryCache.Set(cacheKey, product, TimeSpan.FromHours(12));
                _logger.LogInformation("Product by name cached with key {CacheKey}", cacheKey);
            }

            return product;
        }

        public async Task<Product> ShowImage(int id)
        {
            var cacheKey = $"product_image_{id}";
            _logger.LogInformation("Retrieving product image from cache with key {CacheKey}", cacheKey);

            if (!_memoryCache.TryGetValue(cacheKey, out Product product))
            {
                _logger.LogInformation("Cache miss. Retrieving product image from database.");
                product = await _dbContext.Products
                    .AsNoTracking()
                    .Include(p => p.Image)
                    .FirstOrDefaultAsync(p => p.Id == id);

                _memoryCache.Set(cacheKey, product, TimeSpan.FromHours(12));
                _logger.LogInformation("Product image cached with key {CacheKey}", cacheKey);
            }

            return product;
        }

     
    }
}
