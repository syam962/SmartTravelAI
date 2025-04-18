using Microsoft.SemanticKernel;
using System.ComponentModel;
using Travel.Web.Models;
using Travel.Web.Services;

public sealed class BookingsPlugin
{


    private readonly IFlightService _flightService;
    public BookingsPlugin(IFlightService flightService)

    {
        _flightService = flightService;


    }
    [KernelFunction("FlightLists")]
    [Description("List of flight between two cities.Need source and destination city id")]
    public async Task<List<Flight>> FlightList(
        [Description("source city Id")] int sourceCityId,
        [Description("Destination city Id")] int destinationCityId)
    {

        var res = await _flightService.GetFlightsByCityAsync(sourceCityId, destinationCityId);

        return res.ToList();
    }


    [KernelFunction("BookTable")]
    [Description("Books a new table at a restaurant")]
    public async Task<string> BookTableAsync(
        [Description("Name of the restaurant")] string restaurant,
        [Description("The time in UTC")] DateTime dateTime,
        [Description("Number of people in your party")] int partySize,
        [Description("Customer name")] string customerName,
        [Description("Customer email")] string customerEmail,
        [Description("Customer phone number")] string customerPhone
    )
    {
        Console.WriteLine($"System > Do you want to book a table at {restaurant} on {dateTime} for {partySize} people?");
        Console.WriteLine("System > Please confirm by typing 'yes' or 'no'.");
        Console.Write("User > ");



        return "Booking Successfull";
    }

    [KernelFunction]
    [Description("List reservations booking at a restaurant.")]
    public async Task<List<string>> ListReservationsAsync()
    {
        // Print the booking details to the console
        List<string> result = new List<string>();
        result.Add("Hotel Lemon tree");
        result.Add("Hotel Azad");


        return result;
    }

    [KernelFunction]
    [Description("Cancels a reservation at a restaurant.")]
    public async Task<string> CancelReservationAsync(
        [Description("The appointment ID to cancel")] string appointmentId,
        [Description("Name of the restaurant")] string restaurant,
        [Description("The date of the reservation")] string date,
        [Description("The time of the reservation")] string time,
        [Description("Number of people in your party")] int partySize)
    {
        // Print the booking details to the console


        return "Cancellation successful!";
    }
}