
using Application.BookingService;
using Microsoft.AspNetCore.Mvc;
namespace Controllers
{
    public class ServicesController : Controller
    {
        private readonly IBookingService _bookingService;
       
        public ServicesController(IBookingService bookingService)
        {
            _bookingService = bookingService;
           
        }
        public IActionResult Services()
        {
            return View();
        }
      
        public async Task<IActionResult> ShowRezervation()
        {
            ViewBag.background_image = true;
            ViewBag.IsLoginPage = true;

          
            return View();
        }


    }

}
