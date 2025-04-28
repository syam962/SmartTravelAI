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
            int userID = 0;
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userIdStr)
            {
                userID = Convert.ToInt32(userIdStr);
            }


            try
            {

                await _flightBookingService.CreateBookingAsync(Convert.ToInt32(request.SegmentID), userID);
                return Json(new { success = true, message = "Booking created successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
