


using Domain.Bookingentity;

namespace Application
{
    public interface IBookingRepository: ICommandBookingRepository
    {
        Task<IEnumerable<string>> GetBookingsByDate(DateTime date);
        Task<IEnumerable<Booking>> GetAllBookingsDate();

    }
}
