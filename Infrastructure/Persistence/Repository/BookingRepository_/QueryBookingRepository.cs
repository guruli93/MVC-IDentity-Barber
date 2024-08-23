using Application;
using Domain.Bookingentity;
using Infrastructure.Persistence.DbContext_;
using Infrastructure.Persistence.Repository.BookingRepository_;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Domain.Productentity;

namespace Infrastructure.Persistence.Repository
{
    public class QueryBookingRepository : CommandBookingRepository, IBookingRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _memoryCache; 

        public QueryBookingRepository(AppDbContext  dbContext, IMemoryCache memoryCache): base(dbContext, memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }


        public async Task<IEnumerable<Booking>> GetAllBookingsDate()
        {
           
            var cacheKey = $"Booking_{new Booking().Id}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Booking> bookings))
            {
                bookings = await _dbContext.Booking.ToListAsync();


                _memoryCache.Set(cacheKey, bookings, TimeSpan.FromHours(12));
            }

            return bookings;
        }

        public async Task<IEnumerable<string>> GetBookingsByDate(DateTime date)
        {
            var cacheKey = $"bookings_date_{date:yyyyMMdd}";
          
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<string> reservedTimes))
            {
                reservedTimes = await _dbContext.Booking
                    .Where(b => b.SelectedDate.Date == date.Date)
                    .Select(b => b.SelectedTime)
                    .Distinct()
                    .ToListAsync();


                _memoryCache.Set(cacheKey, reservedTimes, TimeSpan.FromHours(12));
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

