using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repository
{
    public class UFrepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<UFrepository<T>> _logger;

        public UFrepository(DbContext dbContext, IMemoryCache memoryCache, ILogger<UFrepository<T>> logger)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<T> AddAsync(T entity)
        {
            //_logger.LogInformation("Adding entity of type {EntityType}", typeof(T).Name);

            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            var cacheKey = GetCacheKey(entity);
            _memoryCache.Set(cacheKey, entity, TimeSpan.FromHours(12));

            _logger.LogInformation("Entity added to cache with key {CacheKey}", cacheKey);

            var allEntitiesCacheKey = $"{typeof(T).Name}_all";
            if (_memoryCache.TryGetValue(allEntitiesCacheKey, out List<T> allEntities))
            {
                allEntities.Add(entity);
                _memoryCache.Set(allEntitiesCacheKey, allEntities, TimeSpan.FromHours(12));
                _logger.LogInformation("Entity added to all entities cache.", cacheKey);
            }
            else
            {
                _memoryCache.Remove(allEntitiesCacheKey);
                _logger.LogWarning("All entities cache key not found. Cache cleared.");
            }

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                var cacheKey = GetCacheKey(entity);
                _memoryCache.Remove(cacheKey);
                _logger.LogInformation("Entity removed from cache with key {CacheKey}", cacheKey);
                var allEntitiesCacheKey = $"{typeof(T).Name}_all";
                _memoryCache.Remove(allEntitiesCacheKey);
                _logger.LogInformation("All entities cache removed with key {CacheKey}", allEntitiesCacheKey);

            }
        }


        public async Task<T> UpdateAsync(T entity)
        {
            _logger.LogInformation("Updating entity of type {EntityType}", typeof(T).Name);

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var cacheKey = GetCacheKey(entity);
            _memoryCache.Set(cacheKey, entity, TimeSpan.FromHours(12));

            _logger.LogInformation("Entity updated in cache with key {CacheKey}", cacheKey);

            return entity;
        }

        private string GetCacheKey(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                _logger.LogError("Type {EntityType} must have a property named 'Id'.", typeof(T).Name);
                throw new InvalidOperationException("Type must have a property named 'Id'.");
            }

            var idValue = idProperty.GetValue(entity);
            return $"{idValue}";
        }
    }
}
