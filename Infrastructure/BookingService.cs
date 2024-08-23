using Application;
using Application.BookingService;
using Domain.Bookingentity;

namespace Infrastructure
{
    public class BookingService : IBookingService
    { private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
            
        }

        public async Task<bool> Add_Reservation(ReqvestBookingViewModel model)
        {
            var Booking_ = new Booking
            {
                Id = model.Id,
                SelectedDate = model.SelectedDate,
                SelectedTime = model.SelectedTime,
                ReservedTimes = model.ReservedTimes

            };
            var AddDateReservation = await _bookingRepository.AddAsync(Booking_);


            return AddDateReservation;
        }

        public async Task<IEnumerable<ResponseBookingViewModel>> GetAllBookingsDate()
        {
           var  allReservationDate=await _bookingRepository.GetAllBookingsDate();
           var ResponseBookingViewModel= allReservationDate.Select(X=>new ResponseBookingViewModel
                {
               Id = X.Id,
               SelectedDate = X.SelectedDate,
               SelectedTime=X.SelectedTime,
               ReservedTimes = X.ReservedTimes  


            }).ToList();
            return ResponseBookingViewModel;
        }

        public async Task <IEnumerable<string>> GetBookingsByDate(DateTime date)
        {


            var BookingsByDate = await _bookingRepository.GetBookingsByDate(date);

            return BookingsByDate;

            //return new ResponseBookingViewModel
            //{
            //    Id = BookingsByDate.Id,
            //    SelectedDate = BookingsByDate.SelectedDate,
            //    SelectedTime = BookingsByDate.SelectedTime,
            //    ReservedTimes = BookingsByDate.ReservedTimes,
            //};


        }

      
    }
}
