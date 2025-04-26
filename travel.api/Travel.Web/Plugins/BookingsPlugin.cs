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
        // Convert to UTC if necessary
        if (startDate == endDate)
        {
            endDate = startDate.AddDays(1);
        }
        var res = await _flightService.GetSegmentsWithinDateRangeAsync(startDate, endDate, flyingfromcity, flyingtoCity);

        return res.ToList();
    }

    [KernelFunction("BookFlight")]
    [Description("Book flight ticket for a given date,passenger count,SegmentId.SegmentId  can be get from FlightLists function")]
    public async Task<string> BookFlight(
     
     
      [Description("Segmnent ID. This is a required field")] int segmentID,
    
      [Description("Booking Date.This is a required field")] DateTime bookingDate,
      [Description("Number of Passengers.This is a required field")] int numberOfPassengers
      
    )
    {
       
        return "Please verify the booking details and confirm your booking.";

    }



}