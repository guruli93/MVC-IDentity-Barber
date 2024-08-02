using Application;
using Domain.Booking;
using Infrastructure.Persistence.DbContext;
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

            //var viewModel = new Booking
            //{

            //    ReservedTimes = reservedTimes,

            //};


            return reservedTimes;
        }


    }
   


    }


