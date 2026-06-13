using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    //Arra kell mindig törekedni hogy a lehető legkönnyebb legyen ez a réteg. Semmi mást ne takarjon, csak az adatbázisnak kiosztott feladatot.
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CountriesRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Country> AddCountry(Country country)
        {
            this._dbContext.Countries.Add(country);
            await this._dbContext.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await this._dbContext.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryById(Guid id)
        {
            return await this._dbContext.Countries.FirstOrDefaultAsync(country => country.CountryID == id);
        }

        public async Task<Country?> GetCountryByName(string name)
        {
            return await _dbContext.Countries.FirstOrDefaultAsync(country => country.CountryName == name);
        }
    }
}
