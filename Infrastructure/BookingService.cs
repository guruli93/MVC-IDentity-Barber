using Application;
using Application.BookingService;
using Dai_2022.Models;
using Domain.Booking;

namespace Infrastructure
{
    public class BookingService : IBookingService
    { private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
            
        }

        public async Task<ResponseBookingViewModel> Add_Reservation(ReqvestBookingViewModel model)
        {
            var Booking = new Booking
            {
                Id = model.Id,
                SelectedDate = model.SelectedDate,
                SelectedTime = model.SelectedTime,
                ReservedTimes = model.ReservedTimes

            };
            var AddDateReservation = await _bookingRepository.AddAsync(Booking);


            return new ResponseBookingViewModel
            {Id = model.Id,
                SelectedDate = AddDateReservation.SelectedDate,
                SelectedTime = AddDateReservation.SelectedTime,
                ReservedTimes = model.ReservedTimes
            };
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

        public async Task <List<string>> GetBookingsByDate(DateTime date)
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
