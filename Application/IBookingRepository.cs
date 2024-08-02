using Domain.Booking;


namespace Application
{
    public interface IBookingRepository: IAsyncRepository <Booking>
    {
        Task<List<string>> GetBookingsByDate(DateTime date);
        Task<IEnumerable<Booking>> GetAllBookingsDate();

    }
}
