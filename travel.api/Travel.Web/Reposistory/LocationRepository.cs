using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travel.Web.Models;

namespace Travel.Web.Reposistory
{

    public interface ILocationRepository
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<IEnumerable<State>> GetAllStatesAsync();
        Task<IEnumerable<City>> GetAllCitiesAsync();
    }
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries
                .Include(c => c.States)
                .ToListAsync();
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            return await _context.States
                .Include(s => s.Cities)
                .Include(s => s.Country)
                .ToListAsync();
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await _context.Cities
                .Include(c => c.State)
                .ThenInclude(s => s.Country)
                .ToListAsync();
        }
    }
}
