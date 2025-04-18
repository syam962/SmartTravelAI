using Travel.Web.DTO;
using Travel.Web.Models;
using Travel.Web.Reposistory;

namespace Travel.Web.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightRoute>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId, bool includeConnections);
        Task<IEnumerable<Flight>> GetFlightsByCityAsync(int sourceCityId, int destinationCityId);
    }
    public class FlightService: IFlightService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlightService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FlightRoute>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId, bool includeConnections)
        {
            return await _unitOfWork.Flights.SearchFlightsAsync(startDate, endDate, sourceCityId, destinationCityId, includeConnections);
        }
        public async Task<IEnumerable<Flight>> GetFlightsByCityAsync(int sourceCityId, int destinationCityId)
        {
            return await _unitOfWork.Flights.GetFlightsByCityAsync(sourceCityId, destinationCityId);
        }

    }
}
