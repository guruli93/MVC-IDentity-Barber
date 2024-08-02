



namespace Application.BookingService
{
    public interface IBookingService
    {
        Task <List<string>> GetBookingsByDate(DateTime date);
        Task<ResponseBookingViewModel> Add_Reservation(ReqvestBookingViewModel model);
        Task<IEnumerable<ResponseBookingViewModel>> GetAllBookingsDate();
    }
}
