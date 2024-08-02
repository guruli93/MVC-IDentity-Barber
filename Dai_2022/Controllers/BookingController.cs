
using Application;
using Application.BookingService;
using Microsoft.AspNetCore.Mvc;
namespace Dai.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;
        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> BookAppointment(ReqvestBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var reservedTimes = await _bookingService.GetBookingsByDate(model.SelectedDate);
                if (reservedTimes.Contains(model.SelectedTime))
                {
                    ModelState.AddModelError(string.Empty, "The selected time slot is already reserved.");
                    return View(model);
                }

                var result = await _bookingService.Add_Reservation(model);
                return RedirectToAction("ShowRezervation", "Services");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the reservation");

                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetBookings(DateTime date)
        {

            var reservedTimes = await _bookingService.GetBookingsByDate(date);

            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(reservedTimes);
                return Content(json, "application/json");

            }
            catch (Exception exX)
            {

                return StatusCode(500, "Internal server error");
            }

        }



    }
}
