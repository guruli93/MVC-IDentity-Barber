using Application;
using Domain.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory; 

namespace Infrastructure.Persistence.Repository
{
    public class BookingRepository : UFrepository<Booking>, IBookingRepository
    {
        private readonly DbContext.DbContext _dbContext;
        private readonly IMemoryCache _memoryCache; 

        public BookingRepository(DbContext.DbContext dbContext, IMemoryCache memoryCache)
            : base(dbContext, memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsDate()
        {
            var cacheKey = "all_bookings_date";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Booking> bookings))
            {
                bookings = await _dbContext.Booking.ToListAsync();

                // ქეშში შენახვა 5 წუთით
                _memoryCache.Set(cacheKey, bookings, TimeSpan.FromMinutes(5));
            }

            return bookings;
        }

        public async Task<List<string>> GetBookingsByDate(DateTime date)
        {
            var cacheKey = $"bookings_date_{date:yyyyMMdd}";

            if (!_memoryCache.TryGetValue(cacheKey, out List<string> reservedTimes))
            {
                reservedTimes = await _dbContext.Booking
                    .Where(b => b.SelectedDate.Date == date.Date)
                    .Select(b => b.SelectedTime)
                    .Distinct()
                    .ToListAsync();

                // ქეშში შენახვა 5 წუთით
                _memoryCache.Set(cacheKey, reservedTimes, TimeSpan.FromMinutes(5));
            }

            return reservedTimes;
        }
    }
}


/*

using Application;
using Domain.Booking;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class BookingRepository : UFrepository<Booking>, IBookingRepository
    {
        private readonly DbContext.DbContext _dbContext;

        public BookingRepository(DbContext.DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsDate()
        {
            var GetAllBookingsDate=await _dbContext.Booking.ToListAsync();
            return GetAllBookingsDate;

        }
        public async Task<List<string>> GetBookingsByDate(DateTime date)
        {
            var reservedTimes = await _dbContext.Booking
                .Where(b => b.SelectedDate.Date == date.Date)
                .Select(b => b.SelectedTime)
                .Distinct()
                .ToListAsync();

           

            return reservedTimes;
        }


    }
   


    }
*/

