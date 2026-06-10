using Entities;

namespace RepositoryContracts
{
    public interface ICountriesRepository
    {
        //A különbség a Service-ben leírt metódushoz képest az, hogy itt Entityket használunk a DTO objektumok helyett.
        Task<Country> AddCountry(Country country);
        Task<List<Country>> GetAllCountries();
        Task<Country> GetCountryById(Guid id);
    }
}
