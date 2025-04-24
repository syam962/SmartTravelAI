using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travel.Web.Models;
using Travel.Web.Models.DTOs;
using Travel.Web.Reposistory;

namespace Travel.Web.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<IEnumerable<State>> GetAllStatesAsync();
        Task<IEnumerable<CityDTO>> GetAllCitiesAsync();
    }
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _unitOfWork.Locations.GetAllCountriesAsync();
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            return await _unitOfWork.Locations.GetAllStatesAsync();
        }

        public async Task<IEnumerable<CityDTO>> GetAllCitiesAsync()
        {
            var cities = await _unitOfWork.Locations.GetAllCitiesAsync();
            return _mapper.Map<IEnumerable<CityDTO>>(cities);
        }
    }
}
