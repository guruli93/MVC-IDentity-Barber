using Application;
using Domain.Bookingentity;
using Infrastructure.Persistence.DbContext_;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence.Repository.BookingRepository_
{
    public class CommandBookingRepository : ICommandBookingRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CommandBookingRepository(AppDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<bool> AddAsync(Booking entity)
        {
            _dbContext.Booking.Add(entity);
            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                
                var cacheKey = $"bookings_date_{entity.SelectedDate:yyyyMMdd}";
                _memoryCache.Remove(cacheKey);

                _memoryCache.Set($"Booking_{entity.Id}", entity, TimeSpan.FromHours(12));
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.Booking.FindAsync(id); 
            if (entity == null)
            {
                return false;
            }

            _dbContext.Booking.Remove(entity); 
            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                _memoryCache.Remove($"Booking_{id}"); 
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Booking entity)
        {
            _dbContext.Booking.Update(entity); 
            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                _memoryCache.Set($"Booking_{entity.Id}", entity, TimeSpan.FromHours(12)); 
                return true;
            }
            return false;
        }
    }
}
