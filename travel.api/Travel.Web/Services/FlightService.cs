using AutoMapper;
using Travel.Web.DTO;
using Travel.Web.Models;
using Travel.Web.Models.DTOs;
using Travel.Web.Reposistory;

namespace Travel.Web.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightRoute>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId, bool includeConnections);
        Task<IEnumerable<FlightDTO>> GetFlightsByCityAsync(int sourceCityId, int destinationCityId);
        Task<IEnumerable<FlightSegmentDTO>> GetSegmentsWithinDateRangeAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId);
    }
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightRoute>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId, bool includeConnections)
        {
            return await _unitOfWork.Flights.SearchFlightsAsync(startDate, endDate, sourceCityId, destinationCityId, includeConnections);
        }

        public async Task<IEnumerable<FlightDTO>> GetFlightsByCityAsync(int sourceCityId, int destinationCityId)
        {
            var flights = await _unitOfWork.Flights.GetFlightsByCityAsync(sourceCityId, destinationCityId);
            return _mapper.Map<IEnumerable<FlightDTO>>(flights);
        }
        public async Task<IEnumerable<FlightSegmentDTO>> GetSegmentsWithinDateRangeAsync(DateTime startDate, DateTime endDate,int sourcecity,int destinationCity)
        {
            var segments = await _unitOfWork.FlightSegments.GetSegmentsWithinDateRangeAsync(startDate, endDate,sourcecity, destinationCity);
            return _mapper.Map<IEnumerable<FlightSegmentDTO>>(segments);
        }


    }
}
