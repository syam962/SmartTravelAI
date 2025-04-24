using Microsoft.SemanticKernel;
using System.ComponentModel;
using Travel.Web.Models;
using Travel.Web.Models.DTOs;
using Travel.Web.Services;

public sealed class BookingsPlugin
{


    private readonly IFlightService _flightService;
    private readonly ILocationService _locationService;
    private readonly IFlightBookingService _flightBookingService;

    [KernelFunction("Cities")]
    [Description("list of all cities with cityid,cityname,stateid,timezone")]
    public async Task<List<CityDTO>> AllOperationalCities()

    {

        var res = await _locationService.GetAllCitiesAsync();
        List<CityDTO> result = new List<CityDTO>();
       

        return res.ToList();
    }
    public BookingsPlugin(IFlightService flightService, ILocationService location)

    {
        _flightService = flightService;
        _locationService = location;


    }
    [KernelFunction("FlightLists")]
    [Description("Get list of all flights available within a date range")]
    public async Task<List<FlightSegmentDTO>> FlightList(
        [Description("start date range")] DateTime startDate,
        [Description("end date range")] DateTime endDate,
        [Description("city flying from")] int flyingfromcity,
        [Description("city flying to")] int flyingtoCity)
    {
        startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
        var res = await _flightService.GetSegmentsWithinDateRangeAsync(startDate, endDate, flyingfromcity, flyingtoCity);

        return res.ToList();
    }

    [KernelFunction("BookFlight")]
    [Description("This fucntion will return booking UI so return as follows  {viewModel:flightBooking,Model:{functionresponse}}")]
    public async Task<BookingDTO> BookFlight(
      [Description("Booking ID")] int bookingID,
      [Description("User ID")] int userID,
      [Description("Flight ID")] int flightID,
      [Description("Return Flight ID (optional)")] int? returnFlightID,
      [Description("Booking Date")] DateTime bookingDate,
      [Description("Number of Passengers")] int numberOfPassengers,
      [Description("Trip Type")] string tripType,
      [Description("Class ID")] int classID
    )
    {
        bookingDate = DateTime.SpecifyKind(bookingDate, DateTimeKind.Utc);

        // Create a new booking object  
        var booking = new BookingDTO
        {
            BookingID = bookingID,
            UserID = userID,
            FlightID = flightID,
            ReturnFlightID = returnFlightID,
            BookingDate = bookingDate,
            NumberOfPassengers = numberOfPassengers,
            TripType = tripType,
            ClassID = classID
        };

        // Call the booking service to book the flight  
       // var bookedFlights = await _flightBookingService.CreateBookingAsync(booking);

        // Return success message if booking is successful  
        return booking;
    }



}