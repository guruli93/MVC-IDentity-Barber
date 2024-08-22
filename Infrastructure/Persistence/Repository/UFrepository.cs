﻿
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory; 

namespace Infrastructure.Persistence.Repository
{
    public class UFrepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly DbContext.DbContext _dbContext;
        private readonly IMemoryCache  _memoryCache; 

        public UFrepository(DbContext.DbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
             _memoryCache = memoryCache;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            // ქეშში შენახვა
            var cacheKey = $"{typeof(T).Name}_{entity.GetHashCode()}";
             _memoryCache.Set(cacheKey, entity, TimeSpan.FromHours(12));

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            // ქეშიდან ამოშლა
            var cacheKey = $"{typeof(T).Name}_{entity.GetHashCode()}";
             _memoryCache.Remove(cacheKey);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var cacheKey = $"{typeof(T).Name}_{id}";

            if (! _memoryCache.TryGetValue(cacheKey, out T entity))
            {
                entity = await _dbContext.Set<T>().FindAsync(id);

                if (entity != null)
                {
                     _memoryCache.Set(cacheKey, entity, TimeSpan.FromHours(12));
                }
            }

            return entity;
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            var cacheKey = $"{typeof(T).Name}_all";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<T> entities))
            {
                entities = await _dbContext.Set<T>().ToListAsync();
                 _memoryCache.Set(cacheKey, entities, TimeSpan.FromHours(12));
            }

            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

          
            var cacheKey = $"{typeof(T).Name}_{entity.GetHashCode()}";
             _memoryCache.Set(cacheKey, entity, TimeSpan.FromHours(12));

            return entity;
        }
    }
}


/*
using Application;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repository
{
    public class UFrepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly DbContext.DbContext _dbContext;

        public UFrepository(DbContext.DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
          
                await _dbContext.Set<T>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            
          
        }


        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;


        }

     
    }

}
*/
