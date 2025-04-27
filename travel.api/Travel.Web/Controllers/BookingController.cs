using Microsoft.AspNetCore.Mvc;
using Travel.Web.Models;
using Travel.Web.Services;

namespace Travel.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IFlightBookingService _flightBookingService;
        public BookingController(IFlightBookingService bookingService)
        {
            _flightBookingService = bookingService;
        }

        public IActionResult Index()
        {
            return View();
        }

     

        public class BookingRequest
        {
            public int SegmentID { get; set; }
        }
        public async Task<IActionResult> BookFlight([FromBody] BookingRequest request)
        {
            try
            {

                await _flightBookingService.CreateBookingAsync(Convert.ToInt32(request.SegmentID),1);
                return Json(new { success = true, message = "Booking created successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
