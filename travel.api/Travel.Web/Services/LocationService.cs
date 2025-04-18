using System.Collections.Generic;
using System.Threading.Tasks;
using Travel.Web.Models;
using Travel.Web.Reposistory;

namespace Travel.Web.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<IEnumerable<State>> GetAllStatesAsync();
        Task<IEnumerable<City>> GetAllCitiesAsync();
    }
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _unitOfWork.Locations.GetAllCountriesAsync();
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            return await _unitOfWork.Locations.GetAllStatesAsync();
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await _unitOfWork.Locations.GetAllCitiesAsync();
        }
    }
}
