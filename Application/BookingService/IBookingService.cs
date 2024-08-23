namespace Application.BookingService
{
    public interface IBookingService
    {
        Task <IEnumerable<string>> GetBookingsByDate(DateTime date);
        Task<bool> Add_Reservation(ReqvestBookingViewModel model);
        Task<IEnumerable<ResponseBookingViewModel>> GetAllBookingsDate();
    }
}
